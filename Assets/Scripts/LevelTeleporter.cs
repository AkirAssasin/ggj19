using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTeleporter : MonoBehaviour {

    public static bool hasTeleported;
    public static Vector2 nextDestination;
    public int sceneID;
    public Vector2 destinationInScene;

    public void OnCollisionEnter2D (Collision2D collision) {

        if (collision.gameObject.GetComponent<PlayerController>() == null) return;

        hasTeleported = true;
        nextDestination = destinationInScene;

        Particle.pool.Clear();
        WhaleStageProjectile.pool.Clear();
        SpeechBubble.pool.Clear();

        SceneManager.LoadScene(sceneID);

    }

}
