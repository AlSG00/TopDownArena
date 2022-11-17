using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_TestBlackZones : MonoBehaviour
{
    public GameObject[] blackZone;
    //[HideInInspector]
    public bool inLineOfSight = false;
    public bool stayingInside = false;
    public bool[] raycastHits;
    private Renderer _renderer;
    private MaterialPropertyBlock _propertyBlock;
    private SCRIPT_TEST_LookAtBlackZone PlayerRaycastLook;

    public bool alreadyFading = false;

    private void Start()
    {
        _propertyBlock = new MaterialPropertyBlock();
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (raycastHits != null)
        {
            string temp = "";
            for (int i = 0; i < raycastHits.Length; i++)
            {
                temp += $"{raycastHits[i]} | ";
            }
            Debug.Log(temp);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") /*&& !inLineOfSight*/)
        {
            stayingInside = true;
            FadeArea();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player" /*&& !inLineOfSight*/)
        {
            stayingInside = false;
            if (!inLineOfSight)
            {
                IncreaseArea();
            }
        }
    }

    public void FadeArea()
    {
        StopAllCoroutines();
        alreadyFading = true;
        for (int i = 0; i < blackZone.Length; i++)
        {
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

        yield return alreadyFading = false;
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
