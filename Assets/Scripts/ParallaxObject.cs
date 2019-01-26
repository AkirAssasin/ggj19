using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObject : MonoBehaviour {

    [Header("Position")]

    public Vector2 position;
    public float distance;

    [Header("Display")]

    public CameraController target;
    new Transform transform;

    void Start () {

        if (transform == null) transform = GetComponent<Transform>();
        transform.position = Vector2.LerpUnclamped(position,target.position,distance);

    }

    void Update () {

        transform.position = Vector2.LerpUnclamped(position,target.position,distance);

    }

}
