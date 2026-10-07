using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    public static HealthDisplay instance;
    public PlayerHealth playerHealth;

    public GameObject heartPrefab;
    private List<Image> hearts = new List<Image>();

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        instance = this;
    }
    private void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        
        for(int i = 0; i < playerHealth.maxHealth; i++)
        {
            GameObject heart = Instantiate(heartPrefab, this.transform);
            hearts.Add(heart.GetComponent<Image>());
        }
        UpdateHeart();
    }
    public void UpdateHeart()
    {
        int health = playerHealth.health;

        foreach(Image heart in hearts)
        {
            if(health > 0)
            {
                heart.fillAmount = 1f;
                health --;
            }
            else
            {
                heart.fillAmount = 0f;
            }
            
        }
    }
}
