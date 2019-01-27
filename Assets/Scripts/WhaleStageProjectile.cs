﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleStageProjectile : MonoBehaviour {

    public static List<WhaleStageProjectile> pool = new List<WhaleStageProjectile>();
    public bool inPool;

    public static WhaleStageProjectile GetFromPool (GameObject _prefab) {

        WhaleStageProjectile result;

        if (pool.Count > 0) {
            result = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
        } else {
            result = Instantiate(_prefab).GetComponent<WhaleStageProjectile>();
        }

        return result;

    }

    [Header("Variables")]

    public Vector3 velocity;
    public float terminalVelocity;
    public float gravity;
    public float xLoss;

    public float velocityToBreak;

    public int layerOnCollision;
    public int layerOnInitiation;

    bool overrideRigidbody;

    [Header("Particle")]

    public Sprite[] particleSprites;
    public GameObject particlePrefab;

    WhaleStageController stageController;

    new Transform transform;
    SpriteRenderer spriteRenderer;
    new Collider2D collider;
    new Rigidbody2D rigidbody;

    public void Initialize (WhaleStageController _stageController, Vector2 _position, float _speed, float _radian) {

        inPool = false;

        // ---

        if (transform == null) transform = GetComponent<Transform>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (collider == null) collider = GetComponent<Collider2D>();
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();

        // ---

        stageController = _stageController;

        spriteRenderer.enabled = true;
        collider.enabled = true;
        rigidbody.simulated = true;

        velocity = new Vector3(Mathf.Cos(_radian) * _speed,Mathf.Sin(_radian) * _speed);
        transform.position = _position;

        overrideRigidbody = true;
        rigidbody.gravityScale = 0;

        gameObject.layer = layerOnInitiation;

    }

    void Update () {

        if (inPool) return;
        float dt = Time.deltaTime;

        if (overrideRigidbody) {

            velocity.x -= velocity.x * xLoss * dt;
            rigidbody.velocity = velocity;

            if (velocity.y > -terminalVelocity) {
                velocity.y -= gravity * dt;
            }

        } else {

            if (transform.position.y < -10) Pool();

        }

        // spin if hazelnut

    }

    public void Pool () {

        if (inPool) return;

        spriteRenderer.enabled = false;
        collider.enabled = false;
        rigidbody.simulated = false;

        inPool = true;
        pool.Add(this);

        if (!overrideRigidbody) stageController.nutsProgress--;

    }

    void OnCollisionEnter2D (Collision2D collision) {

        if (overrideRigidbody) {

            overrideRigidbody = false;
            rigidbody.gravityScale = 1;

            gameObject.layer = layerOnCollision;

            stageController.nutsProgress++;

        }

        PlayerController pc = collision.gameObject.GetComponent<PlayerController>();
        if (pc != null) return;

        if (collision.gameObject.CompareTag("Level Border")) {

            for (int i = 0; i < particleSprites.Length; i++) {

                Particle p = Particle.GetFromPool(particlePrefab);
                p.Initialize(transform.position,transform.localScale,particleSprites[i],Random.Range(0f,10f),Random.value * 2 * Mathf.PI);

            }

            Pool();

        } else if (collision.relativeVelocity.magnitude > velocityToBreak) {

            for (int i = 0; i < particleSprites.Length; i++) {

                Particle p = Particle.GetFromPool(particlePrefab);
                p.Initialize(transform.position,transform.localScale,particleSprites[i],Random.Range(0f,10f),Random.value * 2 * Mathf.PI);

            }

            Pool();
        
        }

    }

}
