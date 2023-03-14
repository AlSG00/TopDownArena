using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{


    public float health;
    public float currentHealth;
    [HideInInspector] public float healtDecreaseByDebuff = 0f;

    public HealtBarScript healthBar;

    public bool isExhaused = false;
    public bool isHungry = false;
    public bool isDehydrated = false;
    public bool isTired = false;
    public bool isInsane = false;
    
    void Start()
    {
        Transform hud = GameObject.Find("HUD").transform;
        Transform _hud = hud.transform.GetChild(0);
        healthBar = _hud.GetChild(0).GetComponent<HealtBarScript>();

        if (currentHealth == 0)          
            currentHealth = health;
        healthBar.SetMaxHealth(health);
        healthBar.SetHealth(currentHealth);
    }

    void Update()
    {
        CheckHealth();
        TakeDamageByDebuff();
    }

    //TODO: Может ли случиться так, что постоянная проверка хп действительно нужна?
    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void TakeDamageByDebuff()
    {
        currentHealth -= healtDecreaseByDebuff;
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    public void Heal(float healing, bool instant)
    {
        currentHealth += healing;
        if (currentHealth > health)
            currentHealth = health;

        healthBar.SetHealth(currentHealth);
    }

    public void CalculateDamageByDebuff()
    {

    }
}
