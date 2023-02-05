using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTraits : MonoBehaviour
{
    public bool GodMode = false;
    public int health = 100;
    public int maxHealth = 100;
    public LocalPlayerControllerState controller;
    public UnityEvent onTakeDamage;


    void Start()
    {
        controller = GetComponent<LocalPlayerControllerState>();
    }

    public void TakeDamage(int h)
    {
        if (GodMode)
        {
            return;
        }
        health -= h;
        if (health <= 0)
        {
            controller.Die();
        }

        onTakeDamage?.Invoke();
    }
}
