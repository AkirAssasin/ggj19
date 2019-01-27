using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMScript : MonoBehaviour 
{

	// Use this for initialization
	void Start () 
	{
		SoundManagerScript.Instance.PlayLoopingBGM(AudioClipID.BGM_Level_1);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
