using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleSoundScript : MonoBehaviour {

	public GameObject soundSource;
	AudioSource source;

	public bool canPlay;

	void Awake()
	{
		soundSource = GameObject.FindGameObjectWithTag("Source");
	}

	// Use this for initialization
	void Start () 
	{
		source = soundSource.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		ToggleSound();
	}

	void ToggleSound()
	{
		if(Input.GetKeyDown(KeyCode.F) && canPlay)
		{
			source.Play(0);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Source")
		{
			canPlay = true;
			Debug.Log("enter");
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.tag == "Source")
		{
			canPlay = false;
		}
	}
}
