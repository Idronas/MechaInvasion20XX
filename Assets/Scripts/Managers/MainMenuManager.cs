using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuManager : MonoBehaviour
{
	public Canvas[] canvases;
	public AudioSource sfxAudioSource;
	public AudioMixer audioMixer;
	public AudioSource musicAudioSource;
	public AudioClip mainMenuMusic;
	public AudioClip hover;
	public AudioClip enter;
	public GameObject egg;


	void Start()
	{
		musicAudioSource?.PlayOneShot(mainMenuMusic);

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
		sfxAudioSource.PlayOneShot(enter);
		foreach(Canvas d in canvases) {
			if (d.gameObject.name != c.gameObject.name) {
				d.gameObject.SetActive(false);
			}
		}
	}

	public void PlayGame() {
		if (SettingsManager.DebugMode) {
			SceneManager.LoadScene(0);
			return;
		}
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
		sfxAudioSource.PlayOneShot(enter);
	}

	public void ChangeSensitivity(Slider s) {
		SettingsManager.lookSensitivity = s.value;
	}

	public void ChangeSFXVolume(Slider s) {
		SettingsManager.SFXVolume = s.value;
		audioMixer.SetFloat("SFXVolume", Mathf.Log(s.value) * 20);

	}
	public void ChangeMusicVolume(Slider s) {
		SettingsManager.MusicVolume = s.value;
		audioMixer.SetFloat("MusicVolume", Mathf.Log(s.value) * 20);
	}

	public void QuitGame() {
		Application.Quit();
		sfxAudioSource.PlayOneShot(enter);
	}

	public void DebugModeEnable() {
		SettingsManager.DebugMode = true;
		Debug.Log(SettingsManager.DebugMode);
		egg.SetActive(true);

	}

	public void HoverButton() {
		sfxAudioSource.PlayOneShot(hover);
	}

}

