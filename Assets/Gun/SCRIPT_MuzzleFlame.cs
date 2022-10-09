using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_MuzzleFlame : MonoBehaviour
{
    [SerializeField]
    Light lightOrigin;                        // »сточник света дл€ вспышки от выстрела
    [SerializeField]
    float lightIntensity;               // »нтенсивность вспышки от выстрела
    [SerializeField]
    float fadingSpeed;                  // —корость затухани€ вспышки от выстрела
    [SerializeField]
    float lightRange;                   // ƒальность свечени€ вспышки от выстрела

    public ParticleSystem[] muzzleFlash;

    private void Awake()
    {
        lightOrigin.intensity = 0;
        lightOrigin.range = lightRange;
    }

    public void FadeFlame()
    {
        lightOrigin.intensity -= fadingSpeed;
    }

    public void LightFlame()
    {
        for (int i = 0; i < muzzleFlash.Length; i++)
        {
            muzzleFlash[i].Emit(1);
        }
        lightOrigin.intensity = lightIntensity;
    }
}
