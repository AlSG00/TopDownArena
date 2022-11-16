using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_TestBlackZones : MonoBehaviour
{
    public GameObject[] blackZone;
    private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            FadeArea();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            IncreaseArea();
        }
    }

    public void FadeArea()
    {
        StopAllCoroutines();
        for (int i = 0; i < blackZone.Length; i++)
        {
            Debug.Log($"Black zone: {i}");
            _renderer = blackZone[i].GetComponent<Renderer>();
            StartCoroutine(Fade(_renderer));
        }
    }

    public void IncreaseArea()
    {
        StopAllCoroutines();
        for (int i = 0; i < blackZone.Length; i++)
        {
            _renderer = blackZone[i].GetComponent<Renderer>();
            StartCoroutine(Increase(_renderer));
        }
    }

    private IEnumerator Fade(Renderer renderer)
    {
        while (renderer.material.color.a > 0)
        {
            yield return renderer.material.color = new Color(
                renderer.material.color.r, 
                renderer.material.color.g, 
                renderer.material.color.b, 
                renderer.material.color.a - 0.03f
                );
        }
        renderer.material.color = new Color(
            renderer.material.color.r, 
            renderer.material.color.g, 
            renderer.material.color.b, 
            0f
            );
    }

    private IEnumerator Increase(Renderer renderer)
    {
        while (renderer.material.color.a < 1)
        {
            yield return renderer.material.color = new Color(
                renderer.material.color.r, 
                renderer.material.color.g, 
                renderer.material.color.b, 
                renderer.material.color.a + 0.03f
                );
        }
        renderer.material.color = new Color(
            renderer.material.color.r, 
            renderer.material.color.g, 
            renderer.material.color.b, 
            1f
            );
    }
}
