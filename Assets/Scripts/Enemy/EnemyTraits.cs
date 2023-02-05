using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyTraits : MonoBehaviour
{
    public int health;
    private GenericEnemyController controller;
    public UnityEvent onTakeDamage;

    void Start()
    {
        controller = GetComponent<GenericEnemyController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0) controller.Die();
        controller.aggroed = true;

        onTakeDamage?.Invoke();
    }
}
