using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public static Vector2 mousePosition;

	public Transform target;
	public float zDisplacement;
	public float lerpRate;
	private Transform _transform;

	private new Camera camera;

	public float halfwidth;
	public float halfheight;

	public Transform cursor;

	[Header("Focus")]
	public float playerToMouseFocus;
	public float centerToTargetFocus;

	public Vector3 deadFocus;
	public MainGameManager manager;

	void Start () {
		_transform = transform;
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		camera = GetComponent<Camera>();

		halfwidth = camera.ViewportToWorldPoint(new Vector3(1,0,0)).x - _transform.position.x;
		halfheight = camera.ViewportToWorldPoint(new Vector3(0,1,0)).y - _transform.position.y;

		Cursor.visible = false;

	}

	void FixedUpdate () {

		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		cursor.position = mousePosition;

		if (manager.gameState == 0) {

			_transform.position = Vector3.Lerp(_transform.position,deadFocus,Time.fixedDeltaTime * lerpRate);

		} else {
			
			Vector3 targetPosition = Vector3.Lerp(target.position,mousePosition,playerToMouseFocus);

			Vector3 idealPosition = new Vector3(targetPosition.x * centerToTargetFocus,targetPosition.y * centerToTargetFocus,zDisplacement);
			_transform.position = Vector3.Lerp(_transform.position,idealPosition,Time.fixedDeltaTime * lerpRate);

		}

	}

}
