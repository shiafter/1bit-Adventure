using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth instance;
    public HealthDisplay healthDisplay;

    private SpriteRenderer playerSprite;

    public int maxHealth = 3;
    public int currentHealth;
    private bool immune = false;
    public int health { get { return currentHealth; } }

    public static event Action OnPlayerDead;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    void Start()
    {
        healthDisplay = FindObjectOfType<HealthDisplay>();
        playerSprite = GetComponent<SpriteRenderer>();
        InitHealth();

        GameController.OnLevelReset += InitHealth;
    }
    private void OnDestroy()
    {
        GameController.OnLevelReset -= InitHealth;
    }
    void InitHealth()
    {
        currentHealth = maxHealth;
        healthDisplay.UpdateHeart();
        Debug.Log("Reset Health");
    }
    public void TakeDamage(int amount)
    {
        if(immune) return;

        currentHealth -= amount;
        SoundEffectManager.Play("Hurt");

        StartCoroutine(FlashRed());
        StartCoroutine(ImmuneToDamage());

        healthDisplay.UpdateHeart();

        if (currentHealth <= 0)
        {
            OnPlayerDead.Invoke();
            SoundEffectManager.Play("GameOver");
        }
    }
    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        healthDisplay.UpdateHeart();
    }
    private IEnumerator FlashRed()
    {
        playerSprite.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        playerSprite.color = Color.white;
    }
    private IEnumerator ImmuneToDamage()
    {
        immune = true;
        yield return new WaitForSeconds(1f);
        immune = false;
    }
}
