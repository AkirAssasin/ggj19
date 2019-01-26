using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxObject : MonoBehaviour {

    [Header("Position")]

    Vector2 position;
    public float distance;

    [Header("Display")]

    public CameraController target;
    new Transform transform;

    void Start () {

        if (transform == null) transform = GetComponent<Transform>();
        position = transform.position;
        transform.position = Vector2.LerpUnclamped(position,target.position,distance);

    }

    void Update () {

        transform.position = Vector2.LerpUnclamped(position,target.position,distance);

    }

}
