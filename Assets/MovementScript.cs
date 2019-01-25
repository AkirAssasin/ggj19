using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementScript : MonoBehaviour 
{

	[Range(1 , 10)]
	public float jumpVelocity;

	public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;

	Rigidbody2D rb;

	public float speed = 5.0f;
	public GameObject player;

	void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		PlayerMovement();
		PlayerJump();

		if(rb.velocity.y < 0)
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
		}
		else if(rb.velocity.y > 0 && !Input.GetButton("Jump")) 
		{
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier  - 1) * Time.deltaTime;
		}
	}

	void PlayerMovement()
	{
		if(Input.GetKey(KeyCode.D))
		{
			player.transform.Translate(speed*Time.deltaTime, 0 ,0);
		}
		else if(Input.GetKey(KeyCode.A))
		{
			player.transform.Translate(-(speed*Time.deltaTime), 0 ,0);
		}

	}

	void PlayerJump()
	{
		if(Input.GetButtonDown("Jump"))
		{
			GetComponent<Rigidbody2D>().velocity = Vector2.up * jumpVelocity;
		}
	}
}
