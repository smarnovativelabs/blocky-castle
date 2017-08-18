using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour 
{
	
	public GameObject instructoinsPanel;
	public GameObject playButton;
	public GameObject level1Button;
	public GameObject level2Button;

	public GameObject iapPanel;
	// Use this for initialization
	void Start ()
	{

	}


	public void Button_Click(Button btn)
	{
		if (btn.name == "play")
		{
			playButton.SetActive (false);
			level1Button.SetActive (true);
			level2Button.SetActive (true);
		}
		else if (btn.name == "howTo")
		{
			instructoinsPanel.SetActive (true);
		}
		else if (btn.name == "back")
		{
			instructoinsPanel.SetActive (false);
		}
		else if (btn.name == "level1")
		{
			PlayerPrefs.SetInt ("level", 0);
			SceneManager.LoadScene ("game");
		}
		else if (btn.name == "level2")
		{
			PlayerPrefs.SetInt ("level", 1);
			SceneManager.LoadScene ("game");
		}
		else if (btn.name == "removeAds")
		{
			iapPanel.SetActive (true);
		}
		else if (btn.name == "backIAP")
		{
			iapPanel.SetActive (false);
		}


	}


	public void OnPurchaseScuccessful()
	{
		iapPanel.SetActive (false);
		PlayerPrefs.SetInt ("isPurchased", 1);
	}

}
