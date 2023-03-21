using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(PlayerHealth))]
public class SCRIPT_PlayerSatiety : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private SCRIPT_SatietyBar _satietyBar;
    [SerializeField] private StateIconVisibilityHandler _stateIcon;


    [Header("Satiety parameters")]
    public float maxSatiety = 100f;
    public float currentSatiety = 100f;
    public float satietyDecreaseValue = 0.01f;
    public float healthDecreaseValue = 0.01f;

    private PlayerHealth _health;

    private float previousSatietyValue = 0f;

    private void Awake()
    {
        _health = gameObject.GetComponent<PlayerHealth>();

        previousSatietyValue = currentSatiety;
    }

    private void Start()
    {
        // TODO: возможно, во все характеристики придется поставить setCurrentValue или типа того
        _satietyBar.SetMaxSatiety(maxSatiety);
        _stateIcon.Initialize(maxSatiety, currentSatiety);
    }

    private void FixedUpdate()
    {
        UpdateSatiety();
    }

    private void UpdateSatiety()
    {
        previousSatietyValue = currentSatiety;
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
        _stateIcon.HandleStateIconVisibility(currentSatiety, previousSatietyValue);
    }

    public void Eat(float satiety)
    {
        previousSatietyValue = currentSatiety;
        currentSatiety += satiety;
        if (currentSatiety > maxSatiety)
        {
            currentSatiety = maxSatiety;
        }
        _satietyBar.SetSatiety(currentSatiety);
        _stateIcon.HandleStateIconVisibility(currentSatiety, previousSatietyValue);
    }
}
