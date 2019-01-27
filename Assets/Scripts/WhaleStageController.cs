﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleStageController : MonoBehaviour, IAnimal {

    public static bool completed = true;

    [Header("Trigger Range")]

    public float triggerRange;

    [Header("Speech Bubbles")]

    public GameObject speechBubblePrefab;
    public Vector2 speechBubblePosition;
    public Vector2 speechBubbleSize;
    public bool speechBubbleFlip;

    bool hasSpeechBubble;
    SpeechBubble speechBubble;

    [Header("Dialogue")]

    public int dialogueProgress;
    public string[] dialogue;
    public string[] completedDialogue;
    public bool hasDialogue;

    [Header("Whale")]

    public Transform whaleTransform;
    public float whaleAngle;
    public bool isLaunchingProjectile;

    [Header("Projectile")]

    public Transform projectileSpawnTransform;
    public GameObject projectilePrefab;
    public Vector2 spawnStartPoint;
    public Vector2 spawnEndPoint;

    [Header("Pile Of Nuts")]

    public Transform pileTransform;
    Collider2D pileCollider;
    Vector3 pileScale;

    public int nutsProgress;

    new Transform transform;
    new Rigidbody2D rigidbody;

    // --- IAnimal

    Vector2 IAnimal.position {
        get {
            return transform.position;
        }
    }

    float sqrRange;

    float IAnimal.sqrRange {
        get {
            return sqrRange;
        }
    }

    bool IAnimal.hasDialogue {
        get {
            return hasDialogue;
        }
    }

    public void OnTriggerDialogue () {

        if (!hasDialogue) return;

        if (hasSpeechBubble) {

            hasSpeechBubble = false;
            speechBubble.StartPoolingAnimation();

        }

        if (dialogueProgress < dialogue.Length) {

            speechBubble = SpeechBubble.GetFromPool(speechBubblePrefab);
            speechBubble.Initialize(transform,speechBubblePosition,speechBubbleSize,speechBubbleFlip);
            speechBubble.SetText(dialogue[dialogueProgress]);

            dialogueProgress++;
            hasSpeechBubble = true;

        } else {

            // begin minigame

            hasDialogue = false;
            hasSpeechBubble = true;

            speechBubble = SpeechBubble.GetFromPool(speechBubblePrefab);
            speechBubble.Initialize(transform,speechBubblePosition,speechBubbleSize,speechBubbleFlip);

        } 

    }

    // ---

    void Start () {

        sqrRange = triggerRange * triggerRange;

        AnimalSpeech.actives.Add(this);

        if (transform == null) transform = GetComponent<Transform>();
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();
        if (pileCollider == null) pileCollider = pileTransform.GetComponent<Collider2D>();

        // ---

        hasSpeechBubble = false;
        hasDialogue = true;

        pileScale = pileTransform.localScale;

        if (!completed) {
            pileCollider.enabled = false;
            pileTransform.localScale = Vector3.zero;
        } else dialogue = completedDialogue;

        nutsProgress = 0;

    }

    void OnDestroy () {

        AnimalSpeech.actives.Remove(this);

    }

    void Update () {

        if (!hasDialogue && hasSpeechBubble && !completed) {

            speechBubble.SetText("Help me collect my hazelnuts!\n<color=#FFFF00>" + nutsProgress + "/15</color>");

            if (!isLaunchingProjectile) StartCoroutine(WhaleLaunchProjectiles());

            if (nutsProgress >= 15) {

                completed = true;
                speechBubble.StartPoolingAnimation();
                speechBubble = SpeechBubble.GetFromPool(speechBubblePrefab);
                speechBubble.Initialize(transform,speechBubblePosition,speechBubbleSize,speechBubbleFlip);
                speechBubble.SetText("!");

                pileCollider.enabled = true;
                pileTransform.localScale = pileScale;

                hasDialogue = true;
                dialogue = completedDialogue;
                dialogueProgress = 0;

            }

        }

    }

    IEnumerator WhaleLaunchProjectiles () {

        float t = 0;
        isLaunchingProjectile = true;

        while (t < 1f) {

            t += Time.deltaTime;

            whaleAngle = Mathf.LerpAngle(whaleAngle,-45f,Time.deltaTime * 3f);
            whaleTransform.eulerAngles = new Vector3(0,0,whaleAngle);

            yield return null;

        }

        t = 0;

        while (t < 0.5f) {

            t += Time.deltaTime;

            whaleAngle = Mathf.LerpAngle(whaleAngle,10f,Time.deltaTime * 3f);
            whaleTransform.eulerAngles = new Vector3(0,0,whaleAngle);

            yield return null;

        }

        for (int i = 0; i < 5; i++) {

            WhaleStageProjectile wsp = WhaleStageProjectile.GetFromPool(projectilePrefab);
            wsp.Initialize(this,projectileSpawnTransform.position,20,Mathf.Deg2Rad * Random.Range(120f,170f));

        }

        t = 0;

        while (t < 1f) {

            t += Time.deltaTime;

            whaleAngle = Mathf.LerpAngle(whaleAngle,0,t);
            whaleTransform.eulerAngles = new Vector3(0,0,whaleAngle);

            yield return null;

        }

        isLaunchingProjectile = false;

    }

}
