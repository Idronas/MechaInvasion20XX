using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	public Canvas[] canvases;
	public GameObject egg;

	void Start()
	{
		canvases = GetComponentsInChildren<Canvas>();
		foreach (Canvas c in canvases) {
			if (c.gameObject.name != "MainMenu") c.gameObject.SetActive(false);
		}
	}
	
	public void Test(string s) {
		Debug.Log(s);
	}

	public void ChangePanel(Canvas c) {
		c.gameObject.SetActive(true);
		foreach(Canvas d in canvases) {
			if (d.gameObject.name != c.gameObject.name) {
				d.gameObject.SetActive(false);
			}
		}
	}

	public void PlayGame() {
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
	}

	public void ChangeSensitivity(float f) {
		SettingsManager.lookSensitivity = f;
	}

	public void QuitGame() {
		Application.Quit();
	}

	public void DebugModeEnable() {
		SettingsManager.DebugMode = !SettingsManager.DebugMode;
		egg.SetActive(true);

	}

}

