using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    [SerializeField]
    private Light light;

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
        light = GetComponent<Light>();
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
            light.intensity = Random.Range(minIntensity, maxIntensity);
        }        
    }
}
