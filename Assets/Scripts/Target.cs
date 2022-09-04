using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private float health;

    [SerializeField]
    private float armor;

    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = health;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }

    private void CheckHealth()
    {
        if (currentHealth <= 0)
        {
            // Сделать скриптик, чтобы была своя анимация смерти в зависимости от типа объекта
            // GameObject go = gameObject.GetComponent<Death>();

            Destroy(gameObject);

        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
