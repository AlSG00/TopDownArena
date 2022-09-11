using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField]
    private Light _light;

    [SerializeField]
    private float minIntensity;

    [SerializeField]
    private float maxIntensity;

    [SerializeField]
    private float frequency;

    private float flickerTime;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Flicker();
    }

    private void Flicker()
    {
        if (flickerTime + frequency <= Time.time)
        {
            flickerTime = Time.time;
            GetComponent<Light>().intensity = Random.Range(minIntensity, maxIntensity);
        }        
    }
}
