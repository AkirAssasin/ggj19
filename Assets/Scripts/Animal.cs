using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class Animal : MonoBehaviour {

    public static List<Animal> actives = new List<Animal>();

    public static void TriggerAnimalSpeech (Vector2 _origin, float _range) {

        _range *= _range;

        for (int i = 0; i < actives.Count; i++) {

            if ((actives[i].position - _origin).sqrMagnitude <= _range) actives[i].TriggerSpeech();

        }

    }

    [Header("Speech")]

    public GameObject speechBubblePrefab;
    public Vector2 speechBubblePosition;
    public Vector2 speechBubbleSize;

    public string[] dialogue;

    bool hasSpeechBubble;
    SpeechBubble speechBubble;

    public Vector2 position;

    new Transform transform;
    new Rigidbody2D rigidbody;

    // Use this for initialization
    void Start () {

        actives.Add(this);

        if (transform == null) transform = GetComponent<Transform>();
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();

        // ---

        hasSpeechBubble = false;
        position = transform.position;

    }

    void OnDestroy () {

        actives.Remove(this);

    }

    public void TriggerSpeech () {

        if (hasSpeechBubble) {

            hasSpeechBubble = false;
            speechBubble.StartPoolingAnimation();
            speechBubble = null;

        } else {

            speechBubble = SpeechBubble.GetFromPool(speechBubblePrefab);
            speechBubble.Initialize(transform,speechBubblePosition,speechBubbleSize);
            speechBubble.SetText(dialogue[Random.Range(0,dialogue.Length)]);

            hasSpeechBubble = true;


        }


    }

}