using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public Transform followTarget;
	
    public float zDisplacement;
	public float lerpRate;

    public Vector2 lowerBound;
    public Vector2 upperBound;

    public Vector2 position;

    // ---

    new Transform transform;
    new Camera camera;

    void Start () {
		
        if (transform == null) transform = GetComponent<Transform>();
        if (camera == null) camera = GetComponent<Camera>();

        if (followTarget == null) followTarget = GameObject.FindGameObjectWithTag("Player").transform;

        position = transform.position;
        // Cursor.visible = false;

	}

	void FixedUpdate () {

        float dt = Time.fixedDeltaTime;

        Vector2 camSize = (Vector2)camera.ViewportToWorldPoint(new Vector3(1,1,0)) - position;

        Vector2 targetPosition = new Vector2(Mathf.Clamp(followTarget.position.x,lowerBound.x + camSize.x,upperBound.x - camSize.x),
                                             Mathf.Clamp(followTarget.position.y,lowerBound.y + camSize.y,upperBound.y - camSize.y));

        position = Vector2.Lerp(position,targetPosition,lerpRate * dt);

        position.x = Mathf.Clamp(position.x,lowerBound.x + camSize.x,upperBound.x - camSize.x);
        position.y = Mathf.Clamp(position.y,lowerBound.y + camSize.y,upperBound.y - camSize.y);

        transform.position = new Vector3(position.x,position.y,zDisplacement);

	}

}
