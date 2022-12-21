using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float health;
    public float currentHealth;

    public HealtBarScript healthBar;

    private void OnEnable()
    {
        //SCRIPT_Medkit_Small.HealPlayer += Heal;
    }

    private void OnDisable()
    {
        
    }

    //public delegate void HealingAction();
    //public static event HealingAction Healing; 
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

    public void Heal(float healing, bool instant)
    {
        currentHealth += healing;
        if (currentHealth > health)
            currentHealth = health;

        healthBar.SetHealth(currentHealth);
    }
}
