using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField]
    private float damage;
    [SerializeField]
    private float attackCooldown;
    private float _lastHitTime;
    private bool _isAttacking;

    private void FixedUpdate()
    {
        CountCooldown();
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (_isAttacking)
            {
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
            }
        }
    }

    private void CountCooldown()
    {
        if (_lastHitTime + attackCooldown <= Time.time)
        {
            _isAttacking = true;
            _lastHitTime = Time.time;
        }
        else
        {
            _isAttacking = false;
        }
    }
}
