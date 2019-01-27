using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoxLevelScript : MonoBehaviour 
{
	public GameObject key;
	public GameObject keyLock;
	public Text buttonText;
	public GameObject obstacle;


	 float buttonTime = 30.0f;
	public float timeRemaining;

	public bool startCountdown = false;
	public bool triggered = false;
	public bool keyObtained = false;

	void Awake()
	{
		key = GameObject.FindGameObjectWithTag("Key");
		keyLock = GameObject.Find("JailLock");
	}

	// Use this for initialization
	void Start () 
	{
		timeRemaining = buttonTime;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(triggered && Input.GetKeyDown(KeyCode.F))
		{
			startCountdown = true;
		}

		if(startCountdown)
		{
			timeRemaining -= 1.0f * Time.deltaTime;
			obstacle.SetActive(false);

			if(timeRemaining <= 0)
			{
				startCountdown = false;
				timeRemaining = buttonTime;
				obstacle.SetActive(true);
			}
				
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Key")
		{
			Destroy(key);
			keyObtained = true;

		}

		if(other.tag == "Button")
		{
			triggered = true;
			buttonText.GetComponent<Text>().enabled = true;
		}

		if(other.tag == "JailLock" && keyObtained)
		{
			keyLock.SetActive(false);
			Debug.Log("Destroyed");
		}
			
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.tag == "Button")
		{
			triggered = false;
			buttonText.GetComponent<Text>().enabled = false;

		}
	}

}
