using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour 
{

	public GameObject gameoverPanel;
	public GameObject levelCompletePanel;
	public GameObject pausePanel;
	public AudioClip finalClip;
	Player player; 
	public bool isGameOver;
	public static GameManager o;


	// Use this for initialization
	void Start () 
	{
		if (o == null)
			o = this;
		else
			Destroy (gameObject);

		isGameOver = false;
		player = GameObject.Find ("character").GetComponent<Player> ();


	}


	public void UI_Manager(Button btn)
	{
		if (btn.name == "pause")
		{
//			Time.timeScale = 0;
			pausePanel.SetActive (true);
			player.enabled = false;
		}
		else if (btn.name == "resume") 
		{
//			Time.timeScale = 1;
			pausePanel.SetActive (false);
			player.enabled = true;
		}
		else if (btn.name == "home") 
		{
			SceneManager.LoadScene ("home");
		}
		else if (btn.name == "replay") 
		{
			SceneManager.LoadScene ("game");
		}
		else if (btn.name == "next") 
		{
			int level = PlayerPrefs.GetInt ("level");
			level = 1 - level;
			PlayerPrefs.SetInt ("level", level);
			SceneManager.LoadScene ("game");
		}
	}


	public void GameOver()
	{
		isGameOver = true;
		gameoverPanel.SetActive (true);
		Camera.main.GetComponentInParent<CameraFollow> ().enabled = false;
		player.enabled = false;

		ShowAd ();
	}

	void ShowAd()
	{
		int isPurchased = PlayerPrefs.GetInt ("isPurchased");
		if (isPurchased != 1) 
		{
			int ad = PlayerPrefs.GetInt ("ad");
			if (ad % 2 == 0) 
			{
				AdMob.o.ShowVideo ();
			}
			ad++;
			PlayerPrefs.SetInt ("ad", ad);
		}
	}

//	public void PlayLevelCompleteClip()
//	{
//		GetComponent<AudioSource> ().Stop ();
////		GetComponent<AudioSource> ().loop = false;
//		GetComponent<AudioSource> ().PlayOneShot (finalClip);
//	}
	public void LevelComplete()
	{
		levelCompletePanel.SetActive (true);
		ShowAd ();
	}


	public void Restart()
	{
		SceneManager.LoadScene ("game");	
	}
}
