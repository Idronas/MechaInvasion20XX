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
	public Canvas HUD;
	public AudioSource sfxAudioSource;
	public AudioMixer audioMixer;
	public AudioClip hover;
	public AudioClip enter;
	GameObject player;
	public PlayerInput playerInput;
	private InputAction pause;
	

	void Start()
	{
		GameManager.isPaused = false;
		canvases = GetComponentsInChildren<Canvas>();
		foreach (Canvas c in canvases)
		{
			if (c.gameObject.name != "HUD") c.gameObject.SetActive(false);
		}
		player = GameObject.Find("Player");
		playerInput = player.GetComponent<PlayerInput>();
		pause = playerInput.actions["Pause"];

	}

	void Update()
	{
		if (pause.triggered)
		{
			if (!GameManager.isPaused) { PauseGame(); return; }

			if (GameManager.isPaused) { UnPauseGame(); return; }

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

	public void PauseGame()
	{
		GameManager.isPaused = !GameManager.isPaused;
		ChangePanel(Pause);
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.None;
		Time.timeScale = 0;
	}

	public void UnPauseGame()
	{
		GameManager.isPaused = !GameManager.isPaused;
		ChangePanel(HUD);
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Time.timeScale = 1f;
	}

	public void ReturnToMainMenu() {
		Time.timeScale = 1f;
		GameManager.ReturnToMainMenu();
	}
	
	public void HoverButton()
	{
		sfxAudioSource.PlayOneShot(hover);
	}

	public void NextLevel() {
		GameManager.NextLevel();
	}

}
