using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    float health;
    public float currentHealth;

    public HealtBarScript healthBar;
    // Start is called before the first frame update

    void Start()
    {
        if (currentHealth == 0)          
            currentHealth = health;
        healthBar.SetMaxHealth(health);
        healthBar.SetHealth(currentHealth);
    }

    void Update()
    {
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    public void Heal(float healing)
    {
        currentHealth += healing;
        if (currentHealth > health)
            currentHealth = health;

        healthBar.SetHealth(currentHealth);
    }
}
