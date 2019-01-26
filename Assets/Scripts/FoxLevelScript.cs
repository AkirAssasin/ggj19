using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxLevelScript : MonoBehaviour 
{
	public GameObject key;
	public GameObject keyLock;

	void Awake()
	{
		key = GameObject.FindGameObjectWithTag("Key");
		keyLock = GameObject.Find("KeyLock");
	}

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.tag == "Key")
		{
			Destroy(key);
			Destroy(keyLock);
		}
	}
}
