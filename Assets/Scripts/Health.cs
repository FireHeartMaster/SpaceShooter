using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] float maxHealth = 30f;

    [HideInInspector] public float currentHealth;

    [HideInInspector] public bool canTakeDamage = true;
    [HideInInspector] public bool isAlive = true;

    [SerializeField] float dodgeInvincibilityDelay = 0.5f;
    float timeSinceLastDodgeInvincibility;
    bool isInvincibleDueToDodge = false;

    [SerializeField] [Range(0f, 1f)] float alphaValueInvincibilityDesign = 0.5f;

    [Space]
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Slider healthSlider;

    private void Awake()
    {
        currentHealth = maxHealth;

        timeSinceLastDodgeInvincibility = dodgeInvincibilityDelay;

        if(healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
        }
    }


    private void Update()
    {
        if (isInvincibleDueToDodge)
        {
            timeSinceLastDodgeInvincibility += Time.deltaTime;

            if(timeSinceLastDodgeInvincibility >= dodgeInvincibilityDelay)
            {
                isInvincibleDueToDodge = false;
                canTakeDamage = true;
                ResetPlayerInvincibilityDesignDueToDodgeBackToNormal();
            }
        }

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
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

    public void PlayerDodgeInvincibility()
    {
        isInvincibleDueToDodge = true;
        timeSinceLastDodgeInvincibility = 0f;
        canTakeDamage = false;
        SetPlayerInvincibilityDesignDueToDodge();
    }

    void SetPlayerInvincibilityDesignDueToDodge()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alphaValueInvincibilityDesign);
    }

    void ResetPlayerInvincibilityDesignDueToDodgeBackToNormal()
    {
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }
}
