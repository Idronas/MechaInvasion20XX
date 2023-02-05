using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemiesTextUI : MonoBehaviour
{
	private TextMeshProUGUI text;
	void Start()
	{
		//weaponManager = GameObject.Find("GunHolder")?.GetComponent<WeaponManager>();
		text = GetComponent<TextMeshProUGUI>();
	}

	void Update()
	{
		text.text = "ENEMIES: " + GameManager.GetEnemiesThisLevel();
	}
}