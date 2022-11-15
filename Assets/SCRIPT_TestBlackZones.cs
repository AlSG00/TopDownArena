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
            StopAllCoroutines();
            //_renderer.GetPropertyBlock(_propertyBlock);
            //_propertyBlock.SetColor("Color", Color.red);
            Debug.Log("Entered");
            //_renderer.material.color = Color.red;
            for (int i = 0; i < blackZone.Length; i++)
            {
                _renderer = blackZone[i].GetComponent<Renderer>();
                StartCoroutine(FadeZone(_renderer));
            }
            
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Debug.Log("Leaved");
            //_renderer.material.color = Color.black;
            for (int i = 0; i < blackZone.Length; i++)
            {
                _renderer = blackZone[i].GetComponent<Renderer>();
                _renderer.material.color = new Color(0f, 0f, 0f, 1f);
            }
        }
    }

    private IEnumerator FadeZone(Renderer renderer)
    {
        while (renderer.material.color.a > 0)
        {
            yield return renderer.material.color = new Color(0f, 0f, 0f, renderer.material.color.a - 0.03f);
        }
        renderer.material.color = new Color(0f, 0f, 0f, 0f);
    }
}
