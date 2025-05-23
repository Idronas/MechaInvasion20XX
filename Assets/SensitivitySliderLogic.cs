using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SensitivitySliderLogic : MonoBehaviour
{
	Slider slider;
	TMP_InputField text;
	void Start()
	{
		slider = GetComponentInChildren<Slider>();
		text = GetComponentInChildren<TMP_InputField>();

		slider.value = SettingsManager.Instance.lookSensitivity;
		text.text = SettingsManager.Instance.lookSensitivity.ToString();

	}

	void Update() {
		text.text = slider.value.ToString("F2");
	}

}
