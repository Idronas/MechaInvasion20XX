using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthUI : MonoBehaviour
{
   private PlayerTraits traits;
   private TextMeshProUGUI text;
   void Start()
   {
      traits = GameObject.Find("Player")?.GetComponent<PlayerTraits>();
      text = GetComponent<TextMeshProUGUI>();
   }	

   // Update is called once per frame
   void Update()
   {
      text.text = traits.health.ToString();
   }
}
