using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class WinUIManager : MonoBehaviour
{
	// Start is called before the first frame update
	public AudioSource sfxAudioSource;
	public AudioMixer audioMixer;
	public AudioClip hover;

	void Start() {
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	public void HoverButton()
	{
		sfxAudioSource.PlayOneShot(hover);
	}
	public void ReturnToMainMenu()
	{
		GameManager.ReturnToMainMenu();
	}
}
