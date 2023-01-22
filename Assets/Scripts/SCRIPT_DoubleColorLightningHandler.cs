using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_DoubleColorLightningHandler : MonoBehaviour
{
    [SerializeField] private Light[] _lightOrigin;
    //[SerializeField] private GameObject[] _lightMesh;

    public Color DaylightningColor;
    //public Color DaylightningMeshColor;
    public Color NightlightningColor;
   // public Color NightlightningMeshColor;

    public float daylightIntensity = 1f;
    public float nightlightIntensity = 1f;
    public float intensityIncreasingSpeed = 0.1f;
    public float intensityDecreasingSpeed = 0.1f;
    public float activatingDelay = 0f;
    public float deactivatingDelay = 0f;

    private void OnEnable()
    {
        SCRIPT_SunRotation.DayStarted += SetDayLightning;
        SCRIPT_SunRotation.NightStarted += SetNightLightning;
    }

    private void OnDisable()
    {
        SCRIPT_SunRotation.DayStarted -= SetDayLightning;
        SCRIPT_SunRotation.NightStarted -= SetNightLightning;
    }

    private void Start()
    {
        //if (_lightOrigin.intensity != 0 &&
        //    _lightOrigin.intensity != targetIntensity)
        //{
        //    targetIntensity = _lightOrigin.intensity;
        //}
    }

    private void SetDayLightning()
    {
        if (_lightOrigin != null &&
            _lightOrigin.Length != 0)
        {
            for (int i = 0; i < _lightOrigin.Length; i++)
            {
                StartCoroutine(SwapLightColorRoutine(DaylightningColor, _lightOrigin[i], daylightIntensity));
            }
        }
        //for (int i = 0; i < _lightMesh.Length; i++)
        //{
        //    StartCoroutine(SwapMeshColorRoutine(DaylightningMeshColor, _lightMesh[i]));
        //}
    }

    private void SetNightLightning()
    {
        if (_lightOrigin != null &&
            _lightOrigin.Length != 0)
        {
            for (int i = 0; i < _lightOrigin.Length; i++)
            {
                StartCoroutine(SwapLightColorRoutine(NightlightningColor, _lightOrigin[i], nightlightIntensity));
            }
        }
        //for (int i = 0; i < _lightMesh.Length; i++)
        //{
        //    StartCoroutine(SwapMeshColorRoutine(NightlightningMeshColor, _lightMesh[i]));
        //}
    }

    private IEnumerator SwapLightColorRoutine(Color lightColor, Light lightOrigin, float targetIntensity)
    {
        yield return new WaitForSeconds(deactivatingDelay);

        while (lightOrigin.intensity > 0)
        {
            yield return lightOrigin.intensity -= intensityDecreasingSpeed;
        }

        yield return lightOrigin.intensity = 0;

        lightOrigin.color = lightColor;

        yield return new WaitForSeconds(activatingDelay);

        while (lightOrigin.intensity < targetIntensity)
        {
            yield return lightOrigin.intensity += intensityIncreasingSpeed;
        }

        yield return lightOrigin.intensity = targetIntensity;
    }

    //private IEnumerator SwapMeshColorRoutine(Color lightColor, GameObject lightMesh)
    //{
    //    yield return new WaitForSeconds(deactivatingDelay);

    //    var mat = lightMesh.GetComponent<Renderer>().sharedMaterial;
    //    mat.EnableKeyword("_EMISSION");

    //    DynamicGI.UpdateEnvironment();

    //    while (mat.color.a > 0)
    //    {
    //        yield return mat.color = new Color(
    //            mat.color.r,
    //            mat.color.g,
    //            mat.color.b,
    //            mat.color.a - intensityDecreasing
    //            );
    //    }

    //    mat.color = new Color(
    //        lightColor.r,
    //        lightColor.g,
    //        lightColor.b,
    //        0f
    //        );
    //    mat.SetColor("_EmissionColor", lightColor);

    //    yield return new WaitForSeconds(activatingDelay);
    //    DynamicGI.UpdateEnvironment();
    //    while (mat.color.a < 1)
    //    {
    //        yield return mat.color = new Color(
    //            mat.color.r,
    //            mat.color.g,
    //            mat.color.b,
    //            mat.color.a + intensityIncreasing
    //            );
    //    }

        

    //    //mat.color = new Color(
    //    //   lightColor.r,
    //    //   lightColor.g,
    //    //   lightColor.b,
    //    //   1f
    //    //   );
    //}
}
