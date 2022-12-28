using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(PlayerHealth))]
public class SCRIPT_PlayerSatiety : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private SCRIPT_SatietyBar _satietyBar;
    
    [Header("Satiety parameters")]
    public float maxSatiety = 100f;
    public float currentSatiety = 100f;
    public float satietyDecreaseValue = 0.01f;
    public float healthDecreaseValue = 0.01f;

    private PlayerHealth _health;

    private void Awake()
    {
        _health = gameObject.GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        _satietyBar.SetMaxSatiety(maxSatiety);
    }

    private void FixedUpdate()
    {
        UpdateSatiety();
    }

    private void UpdateSatiety()
    {
        if (currentSatiety > 0)
        {
            currentSatiety -= satietyDecreaseValue;
        }
        else
        {
            currentSatiety = 0;
            //_health.healtDecreaseByDebuff
            _health.TakeDamage(healthDecreaseValue);
        }

        _satietyBar.SetSatiety(currentSatiety);
    }

    public void Eat(float satiety)
    {
        currentSatiety += satiety;
        if (currentSatiety > maxSatiety)
        {
            currentSatiety = maxSatiety;
        }
        _satietyBar.SetSatiety(currentSatiety);
        //StopAllCoroutines();
        //StartCoroutine(SatietyBarSmoothUpdate());
    }

    //private IEnumerator SatietyBarSmoothUpdate()
    //{
    //    while ()
    //}
}
