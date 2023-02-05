using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolumeSliderLogic : MonoBehaviour
{

	Slider slider;

	void Start()
	{
		slider = GetComponentInChildren<Slider>();


		slider.value = SettingsManager.Instance.MusicVolume;


	}
	
}
