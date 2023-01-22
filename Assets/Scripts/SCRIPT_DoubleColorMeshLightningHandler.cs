using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_DoubleColorMeshLightningHandler : MonoBehaviour
{
    [SerializeField] private GameObject[] _lightMesh;

    public Color DaylightningMeshColor;
    public Color NightlightningMeshColor;
    
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

    private void SetDayLightning()
    {
        if (_lightMesh != null &&
            _lightMesh.Length != 0)
        {
            for (int i = 0; i < _lightMesh.Length; i++)
            {
                StartCoroutine(SwapMeshColorRoutine(DaylightningMeshColor, _lightMesh[i]));
            }
        }
    }

    private void SetNightLightning()
    {
        if (_lightMesh != null &&
            _lightMesh.Length != 0)
        {
            for (int i = 0; i < _lightMesh.Length; i++)
            {
                StartCoroutine(SwapMeshColorRoutine(NightlightningMeshColor, _lightMesh[i]));
            }
        }
    }

    private IEnumerator SwapMeshColorRoutine(Color lightColor, GameObject lightMesh)
    {
        yield return new WaitForSeconds(deactivatingDelay);

        var mat = lightMesh.GetComponent<Renderer>().sharedMaterial;
        mat.EnableKeyword("_EMISSION");

        while (mat.color.a > 0)
        {
            yield return mat.color = new Color(
                mat.color.r,
                mat.color.g,
                mat.color.b,
                mat.color.a - intensityDecreasingSpeed
                );
        }

        yield return mat.color = new Color(
            lightColor.r,
            lightColor.g,
            lightColor.b,
            0f
            );

        mat.SetColor("_EmissionColor", lightColor * 3);

        yield return new WaitForSeconds(activatingDelay);

        while (mat.color.a < 1)
        {
            yield return mat.color = new Color(
                mat.color.r,
                mat.color.g,
                mat.color.b,
                mat.color.a + intensityIncreasingSpeed
                );
        }

        yield return mat.color = new Color(
            lightColor.r,
            lightColor.g,
            lightColor.b,
            1f
            );
    }
}
