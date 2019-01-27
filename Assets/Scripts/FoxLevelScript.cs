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
	public GameObject jailCell;


	 float buttonTime = 30.0f;
	public float timeRemaining;

	public bool startCountdown = false;
	public bool triggered = false;
	public bool keyObtained = false;

	void Awake()
	{
		key = GameObject.FindGameObjectWithTag("Key");
		keyLock = GameObject.Find("JailLock");
		jailCell = GameObject.Find("JailCell");
	}

	// Use this for initialization
	void Start () 
	{
		timeRemaining = buttonTime;
		SoundManagerScript.Instance.PlayLoopingBGM(AudioClipID.BGM_FoxLevel);
	}
	
	// Update is called once per frame
	void Update () 
	{
		CellKeyDoor();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Key")
		{
			Destroy(key);
			keyObtained = true;
			SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_PickUp);

		}

		if(other.tag == "Button")
		{
			triggered = true;
			buttonText.GetComponent<Text>().enabled = true;
		}

		if(other.tag == "JailLock" && keyObtained)
		{
			SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_LockOpen);
			keyLock.SetActive(false);
			jailCell.GetComponent<BoxCollider2D>().enabled = false;

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

	void CellKeyDoor()
	{
		if(triggered && Input.GetKeyDown(KeyCode.F))
		{
			startCountdown = true;
			SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_LockOpen);
		}

		if(startCountdown)
		{
			timeRemaining -= 1.0f * Time.deltaTime;
			obstacle.SetActive(false);

			if(timeRemaining <= 0)
			{
				SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_LockOpen);
				startCountdown = false;
				timeRemaining = buttonTime;
				obstacle.SetActive(true);
			}

		}
	}

}
