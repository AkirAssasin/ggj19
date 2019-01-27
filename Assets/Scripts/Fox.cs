using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fox : MonoBehaviour, IAnimal {

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
    public bool hasDialogue;

    new Transform transform;

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

        } else hasDialogue = false;

    }

    // ---

    void Start () {

        sqrRange = triggerRange * triggerRange;

        AnimalSpeech.actives.Add(this);

        if (transform == null) transform = GetComponent<Transform>();
        // ---

        hasSpeechBubble = false;
        hasDialogue = true;
    }

    void OnDestroy () {

        AnimalSpeech.actives.Remove(this);

    }

}
