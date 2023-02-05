using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    private PlayerTraits traits;
	public TextMeshProUGUI text;
	public Image fill;

    void Start()
    {
        traits = LocalPlayerControllerState.Instance.gameObject.GetComponent<PlayerTraits>();
    }

    // Update is called once per frame
    void Update()
    {
		text.text = traits.health + " / " + traits.maxHealth;
		fill.fillAmount = (float)traits.health / 100f;
    }
}
