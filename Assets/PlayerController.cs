using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerController : MonoBehaviour {

    public float horizontalSpeed;

    public int maxJumpCount;
    int currentJumpCount;

    public float jumpInitialSpeed;
    public float jumpDeceleration;
    public float terminalVelocity;
    float verticalSpeed;

    bool leftGround;

    new Transform transform;
    new Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
		
        if (transform == null) transform = GetComponent<Transform>();
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();

        currentJumpCount = 1;

	}
	
	// Update is called once per frame
	void Update () {

        float dt = Time.deltaTime;

        Vector2 bottomLeft = transform.position + new Vector3(-transform.localScale.x/2,-transform.localScale.y / 2);
        Vector2 bottomRight = transform.position + new Vector3(transform.localScale.x / 2,-transform.localScale.y / 2);

        RaycastHit2D touchGroundLinecast = Physics2D.Linecast(bottomLeft,bottomRight);

        if (touchGroundLinecast) {

            if (leftGround && currentJumpCount > 0) {

                currentJumpCount = 0;
                verticalSpeed = 0;
            }

            leftGround = false;

        } else {

            leftGround = true;
            if (currentJumpCount <= 0) currentJumpCount = 1;

        }

        if (currentJumpCount < maxJumpCount && Input.GetKeyDown(KeyCode.Space)) {

            currentJumpCount++;
            verticalSpeed = jumpInitialSpeed;

        }

        if (currentJumpCount > 0 && verticalSpeed > -terminalVelocity) {

            verticalSpeed -= jumpDeceleration * dt;
            if (verticalSpeed < -terminalVelocity) verticalSpeed = -terminalVelocity;

        }

        float hsp = Input.GetAxisRaw("Horizontal") * horizontalSpeed;

        rigidbody.velocity = new Vector2(hsp,verticalSpeed);

    }

}
