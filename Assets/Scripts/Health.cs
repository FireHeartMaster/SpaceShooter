using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 30f;

    [HideInInspector] public float currentHealth;

    [HideInInspector] public bool canTakeDamage = true;
    [HideInInspector] public bool isAlive = true;


    private void Awake()
    {
        currentHealth = maxHealth;
    }
    

    public void TakeDamage(float damageAmount)
    {
        if (!canTakeDamage) return;

        currentHealth -= damageAmount;

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log(gameObject.name + " is dead");
        canTakeDamage = false;
        isAlive = false;
    }
}
