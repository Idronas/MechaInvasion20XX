using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoUI : MonoBehaviour
{
	private WeaponManager weaponManager;
	private TextMeshProUGUI text;
	void Start()
	{
		weaponManager = LocalPlayerControllerState.Instance.GetComponentInChildren<WeaponManager>();
		text = GetComponent<TextMeshProUGUI>();
	}

	void Update()
	{
		if (weaponManager.activeweapon.reloading)
		{
			text.text = "Reloading";
			return;
		}

		text.text = "AMMO: " + weaponManager.activeweapon.currentAmmo.ToString() + " / " + weaponManager.activeweapon.maxAmmo.ToString();
	}
}
