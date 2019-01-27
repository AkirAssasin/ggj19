using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {

    public static List<Particle> pool = new List<Particle>();
    public bool inPool;

    public static Particle GetFromPool (GameObject _prefab) {

        Particle result;

        if (pool.Count > 0) {
            result = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
        } else {
            result = Instantiate(_prefab).GetComponent<Particle>();
        }

        return result;

    }

    [Header("Variables")]

    public Vector3 velocity;
    public float gravity;
    public float life;
    float progress;

    new Transform transform;
    SpriteRenderer spriteRenderer;

    public void Initialize (Vector3 _position, Vector3 _scale, Sprite _sprite, float _speed, float _radian) {

        inPool = false;

        // ---

        if (transform == null) transform = GetComponent<Transform>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();

        // ---

        spriteRenderer.sprite = _sprite;
        spriteRenderer.color = new Color(1,1,1,1);
        spriteRenderer.flipX = Random.value > 0.5f;
        spriteRenderer.flipY = Random.value > 0.5f;
        spriteRenderer.enabled = true;

        velocity = new Vector3(Mathf.Cos(_radian) * _speed,Mathf.Sin(_radian) * _speed);
        transform.position = _position;
        transform.localScale = _scale;

        transform.eulerAngles = new Vector3(0,0,Random.value * 360f);

        progress = 0;

    }

    void Update () {

        if (inPool) return;
        float dt = Time.deltaTime;

        transform.position += velocity * dt;
        velocity.y -= gravity * dt;

        progress += dt / life;

        spriteRenderer.color = new Color(1,1,1,1 - progress);

        if (progress >= 1) Pool();

        // spin if hazelnut

    }

    public void Pool () {

        if (inPool) return;

        spriteRenderer.enabled = false;

        inPool = true;
        pool.Add(this);

    }

}
