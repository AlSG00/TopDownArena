using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCRIPT_PlayerWakefulness : MonoBehaviour
{
    [SerializeField] private StateIconVisibilityHandler _stateIcon;


    [Header("References")]
    [SerializeField] private SCRIPT_SliderBarController _wakefulnessBar;
    [SerializeField] private SCRIPT_PlayerSatiety _satiety;
    [SerializeField] private SCRIPT_PlayerHydration _hydration;
    [SerializeField] private SCRIPT_PlayerStamina _stamina;
    [SerializeField] private SCRIPT_PlayerSanity _sanity;
    [SerializeField] private Image _blackScreen;
    [SerializeField] private AudioSource _ambientAudioSource;

    [SerializeField] private AudioClip[] _dreemAudioClip;

    public Animator sunAnimator;
    
    [Header("Wakefulness parameters")]
    public float maxWakefulness = 100f;
    public float currentWakefulness = 100f;
    public float wakefulnessDecreaseValue = 0.001f;

    [Header("Affection on player")]
    public float sanityDecreaseDebuff = 0.001f;

    public bool isTired = false;

    private float previousWakefulnessValue = 0f;

    private void Awake()
    {
        previousWakefulnessValue = currentWakefulness;
    }

    private void Start()
    {
        _wakefulnessBar.SetMaxValue(maxWakefulness);
        _stateIcon.Initialize(maxWakefulness, currentWakefulness);
    }

    private void FixedUpdate()
    {
        HandleWakefulness();
        HandleTirednessFlag();
    }

    private void HandleWakefulness()
    {
        previousWakefulnessValue = currentWakefulness;
        if (currentWakefulness > 0)
        {
            currentWakefulness -= wakefulnessDecreaseValue;
        }
        else
        {
            currentWakefulness = 0;
        }

        _wakefulnessBar.SetValue(currentWakefulness);
        _stateIcon.HandleStateIconVisibility(currentWakefulness, previousWakefulnessValue);
    }

    private void HandleTirednessFlag()
    {
        if (currentWakefulness > 0)
        {
            if (isTired)
            {
                isTired = false;
                _sanity.sanityDecreaseDebuff += sanityDecreaseDebuff;
            }
        }
        else
        {
            if (!isTired)
            {
                isTired = true;
                _sanity.sanityDecreaseDebuff -= sanityDecreaseDebuff;
            }
        }
    }

    // TODO: Это тестовая функция, переделать так, чтобы бодрость зависела
    // от количества времени, потраченного на сон
    public void Sleep()
    {
        // Затемнить экран
        // Сыграть несколько рандомных звуков
        // Восстановить бодрость
        // Порезать голод с жаждой???
        // Повысить рассудок???
        
        StopAllCoroutines();
        Debug.Log("Starting new coroutine");
        StartCoroutine(SleepRoutine());
    }

    private IEnumerator SleepRoutine()
    {
        Debug.Log("Shading screen...");
        while (_blackScreen.color.a < 1)
        {
           yield return _blackScreen.color = new Color(
           _blackScreen.color.r,
           _blackScreen.color.g,
           _blackScreen.color.b,
           _blackScreen.color.a + 0.03f
           );
        }

        yield return _blackScreen.color = new Color(
           _blackScreen.color.r,
           _blackScreen.color.g,
           _blackScreen.color.b,
           1f
           );

        Debug.Log("Waiting...");
        yield return new WaitForSeconds(3f);

        //_ambientAudioSource.PlayOneShot(_dreemAudioClip[0]);

        //while (_ambientAudioSource.isPlaying)
        //{
        //    yield return new WaitForSeconds(1f);
        //}



        //TODO: Добавить функции в нижеиспользуемые скрипты,
        //чтобы можно было добавлять или убавлять значение показателей,
        //не боясь вывалиться за допустимые значения

        Debug.Log("Setting stats...");
        _satiety.currentSatiety = 10f;
        _hydration.currentHydration = 10f;
        _sanity.currentSanity = _sanity.maxSanity;
        _stamina.currentStamina = _stamina.maxStamina;
        currentWakefulness = maxWakefulness;
        sunAnimator.Play("ANIM_Sun", 0, 0.01f);


        Debug.Log("Fading screen...");
        while (_blackScreen.color.a > 0)
        {
            yield return _blackScreen.color = new Color(
            _blackScreen.color.r,
            _blackScreen.color.g,
            _blackScreen.color.b,
            _blackScreen.color.a - 0.03f
            );
        }

        yield return _blackScreen.color = new Color(
           _blackScreen.color.r,
           _blackScreen.color.g,
           _blackScreen.color.b,
           0f
           );

        Debug.Log("Complete!");
        _stateIcon.HandleStateIconVisibility(currentWakefulness, previousWakefulnessValue);
    }
}
