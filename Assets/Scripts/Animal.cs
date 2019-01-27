using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public static class AnimalSpeech {

    public static List<IAnimal> actives = new List<IAnimal>();

    public static bool TriggerAnimalSpeech (Vector2 _origin) {

        for (int i = 0; i < actives.Count; i++) {

            if (!actives[i].hasDialogue) continue;

            if ((actives[i].position - _origin).sqrMagnitude <= actives[i].sqrRange) {
                actives[i].OnTriggerDialogue();
                return true;
            }

        }

        return false;

    }

}

public interface IAnimal {

    Vector2 position { get; }
    float sqrRange { get; }
    bool hasDialogue { get; }

    void OnTriggerDialogue();

}