using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class PlayerController : MonoBehaviour {

    [Header("Movement")]

    public float horizontalSpeed;

    public int maxJumpCount;
    public float jumpInitialSpeed;
    public float jumpDeceleration;
    public float terminalVelocity;

    int currentJumpCount;
    float verticalSpeed;
    bool leftGround;

    [Header("Speech")]

    public GameObject speechBubblePrefab;
    public float speechBubbleDuration;

    bool hasSpeechBubble;
    float speechBubbleProgress;
    SpeechBubble speechBubble;

    new Transform transform;
    new Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
		
        if (transform == null) transform = GetComponent<Transform>();
        if (rigidbody == null) rigidbody = GetComponent<Rigidbody2D>();

        currentJumpCount = 1;
        hasSpeechBubble = false;


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

        if (hasSpeechBubble) {

            speechBubbleProgress += dt;

            if (speechBubbleProgress >= speechBubbleDuration) {

                hasSpeechBubble = false;
                speechBubble.StartPoolingAnimation();
                speechBubble = null;

            }

        } else if (Input.GetKeyDown(KeyCode.E)) {

            speechBubble = SpeechBubble.GetFromPool(speechBubblePrefab);
            speechBubble.Initialize(transform,new Vector3(0,1,0),new Vector2(1.3f,1f));
            speechBubble.SetText("<sprite=0>?");

            hasSpeechBubble = true;
            speechBubbleProgress = 0;


        }

    }

}
