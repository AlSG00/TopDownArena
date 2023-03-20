using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private StateIconVisibilityHandler _stateIcon;
    [SerializeField] private ShoulderLightController _shoulderLight;

    public float maxHealth;
    public float currentHealth;
    [HideInInspector] public float healtDecreaseByDebuff = 0f;

    // TODO: Заменить на SliderController
    public HealtBarScript healthBar;

    public bool isExhaused = false;
    public bool isHungry = false;
    public bool isDehydrated = false;
    public bool isTired = false;
    public bool isInsane = false;

    private float previousHealthValue = 0f;

    //private void Awake()
    //{
    //    //fullHpIndicationValue = health;
    //    //halfHpIndicationValue = health / 2;
    //    //quarterHpIndicationValue = health / 4;
    //}

    void Start()
    {
        _shoulderLight.Initialize(maxHealth);
        _shoulderLight.SetHealthColor(currentHealth);

        _stateIcon.Initialize(maxHealth, currentHealth);
        Transform hud = GameObject.Find("HUD").transform;
        Transform _hud = hud.transform.GetChild(0);
        //healthBar = _hud.GetChild(0).GetComponent<HealtBarScript>();

        if (currentHealth == 0)          
            currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
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

    // TODO: Перенести в FixedUpdate какой-нибудь
    private void TakeDamageByDebuff()
    {
        previousHealthValue = currentHealth;
        currentHealth -= healtDecreaseByDebuff;
        //_stateIcon.HandleStateIconVisibility(currentHealth, previousHealthValue);
        //_shoulderLight.SetHealthColor(currentHealth);
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    public void TakeDamage(float damage)
    {
        previousHealthValue = currentHealth;
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        _stateIcon.HandleStateIconVisibility(currentHealth, previousHealthValue);
        _shoulderLight.SetHealthColor(currentHealth);
    }

    public void Heal(float healing, bool instant)
    {
        previousHealthValue = currentHealth;
        currentHealth += healing;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;

        healthBar.SetHealth(currentHealth);
        _stateIcon.HandleStateIconVisibility(currentHealth, previousHealthValue);
        _shoulderLight.SetHealthColor(currentHealth);
    }

    public void CalculateDamageByDebuff()
    {

    }
}
