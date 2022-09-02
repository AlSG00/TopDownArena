using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeealth : MonoBehaviour
{
    [SerializeField]
    private float HP;
    private float currentHP;


    // Start is called before the first frame update
    void Start()
    {
        currentHP = HP;
    }

    // Update is called once per frame
    void Update()
    {
        CheckHealth();
    }

    void CheckHealth()
    {
        if(currentHP <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHP -= damage;
    }

    //public void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "Bullet")
    //    {
    //        collision.gameObject.GetComponent<Projectile>.
    //    }
    //}
}
