using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour 
{

	public GameObject creditScreen;

	// Use this for initialization
	void Start () 
	{
		PlayBgm();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void StartGame()
	{
		
	}

	public void Credits()
	{
		if(!creditScreen.activeSelf)
		{
			creditScreen.SetActive(true);
		}
		else if(creditScreen.activeSelf)
		{
			creditScreen.SetActive(false);
		}
	}

	public void ChangeScene(string name)
	{
		SceneManager.LoadScene(name);
	}

	void PlayBgm()
	{
		SoundManagerScript.Instance.PlayLoopingBGM(AudioClipID.BGM_MAIN_MENU);
	}
}
