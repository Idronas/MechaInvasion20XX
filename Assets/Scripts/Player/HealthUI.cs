using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    private PlayerTraits traits;
    public Image fill;

    void Start()
    {
        traits = GameObject.Find("Player")?.GetComponent<PlayerTraits>();
    }

    // Update is called once per frame
    void Update()
    {
        fill.fillAmount = (float)traits.health / 100f;

    }
}
