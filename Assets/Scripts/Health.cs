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

    [Space]
    [Header("Death")]
    [SerializeField] float timeToDisapearAfterDying = 3f;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject gameOverScreen;

    [Space]
    [Header("DamageDelay")]
    [SerializeField] float timeBetweenDamages = 2f;
    float timeSinceLastDamage;

    //Pool Recycling
    Factory factory;

    Animator animator;

    private void Awake()
    {
        currentHealth = maxHealth;

        timeSinceLastDodgeInvincibility = dodgeInvincibilityDelay;

        if(healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
        }

        timeSinceLastDamage = timeBetweenDamages;

        canTakeDamage = true;

        animator = GetComponent<Animator>();
        animator.enabled = false;

    }

    private void Start()
    {
        factory = Factory.m_Factory;
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

        timeSinceLastDamage += Time.deltaTime;
    }

    public void TakeDamage(float damageAmount)
    {
        if (!canTakeDamage) return;

        //if (timeSinceLastDamage < timeBetweenDamages)
        //{
        //    return;
        //}
        //else
        //{
        //    timeSinceLastDamage = 0f;
        //}

        currentHealth -= damageAmount;

        if(currentHealth > 0)
        {
            animator.enabled = true;
            animator.Play("TookDamageAnimation", -1, 0f);
        }

        //Debug.Log("currentHealth: " + currentHealth);
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        //Debug.Log(gameObject.name + " is dead");
        canTakeDamage = false;
        isAlive = false;
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);
        //GameObject explosionGameObject = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        GameObject explosionGameObject = factory.CreateExplosion(transform.position, Quaternion.identity);
        factory.CallIntentToDeactivateExplosion(explosionGameObject, false);
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }
        //animator.enabled = false;

        Destroy(gameObject, timeToDisapearAfterDying);
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
;