using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

public class PauseMenuManager : MonoBehaviour
{
	public Canvas[] canvases;
	public Canvas Pause;
	public AudioSource sfxAudioSource;
	public AudioMixer audioMixer;
	public AudioClip hover;
	public AudioClip enter;
	GameObject player;
	public PlayerInput playerInput;
	private InputAction pause;
	bool isPaused = false;

	void Start()
	{
		
		canvases = GetComponentsInChildren<Canvas>();
		foreach (Canvas c in canvases)
		{
			if (c.gameObject.name != "InGameUI") c.gameObject.SetActive(false);
		}
		
		player = GameObject.Find("Player");
		playerInput = player.GetComponent<PlayerInput>();
		pause = playerInput.actions["Pause"];

	}

	void Update() {
		if (pause.triggered) {
			ChangePanel(Pause);
			PauseGame();
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;

		}
	}

	public void ChangePanel(Canvas c)
	{
		c.gameObject.SetActive(true);
		sfxAudioSource.PlayOneShot(enter);
		foreach (Canvas d in canvases)
		{
			if (d.gameObject.name != c.gameObject.name)
			{
				d.gameObject.SetActive(false);
			}
		}
	}

	public void ChangeSensitivity(Slider s)
	{
		SettingsManager.lookSensitivity = s.value;
	}

	public void ChangeSFXVolume(Slider s)
	{
		SettingsManager.SFXVolume = s.value;
		audioMixer.SetFloat("SFXVolume", Mathf.Log(s.value) * 20);
	}
	public void ChangeMusicVolume(Slider s)
	{
		SettingsManager.MusicVolume = s.value;
		audioMixer.SetFloat("MusicVolume", Mathf.Log(s.value) * 20);
	}

	public void PauseGame() {
		isPaused = !isPaused;
	}

}
