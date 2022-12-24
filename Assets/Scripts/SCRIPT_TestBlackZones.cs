using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SCRIPT_TestBlackZones : MonoBehaviour
{
    public GameObject[] blackZone;
    [Range(0, 1)] public float fadeSpeed = 0.3f;
    [Range(0, 1)] public float increaseSpeed = 0.3f;

    [HideInInspector] public bool inLineOfSight = false;
    [HideInInspector] public bool stayingInside = false;

    private Renderer _renderer;

    //private bool _isFaded = false;
    public float increaseDelay = 1f;
    [HideInInspector] public float increaseCooldown;

    private void FixedUpdate()
    {
        if (increaseCooldown + increaseDelay <= Time.time)
        {
            increaseCooldown = Time.time;
            inLineOfSight = false;

            if (!stayingInside && !inLineOfSight)
            {
                IncreaseArea();
            }
        }
        else if ((inLineOfSight || stayingInside))
        {
            FadeArea();
        }
    }

    private void Update()
    {
        //Debug.Log($"IncreaseCooldown: {increaseCooldown}");

        //if (increaseCooldown + increaseDelay <= Time.time)
        //{
        //    increaseCooldown = (float)Time.time;
        //    inLineOfSight = false;

        //        if (!stayingInside && !inLineOfSight)
        //        {
        //            IncreaseArea();
        //        }
        //}
        //else if ((inLineOfSight || stayingInside))
        //{
        //    FadeArea();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") /*&& !inLineOfSight*/)
        {
            increaseCooldown = Time.time;
            stayingInside = true;
            //_isFaded = true;
            //FadeArea();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            increaseCooldown = Time.time;
            stayingInside = false;
            //if (!inLineOfSight || !canSee)
            //{
            //    IncreaseArea();
            //}
        }
    }

    public void FadeArea()
    {
        //if (_lastCoroutine != null)
       // {
        //    StopCoroutine(_lastCoroutine);
       // }
        StopAllCoroutines();
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
                renderer.material.color.a - fadeSpeed
                );
        }
        yield return renderer.material.color = new Color(
            renderer.material.color.r, 
            renderer.material.color.g, 
            renderer.material.color.b, 
            0f
            );
    }

    private IEnumerator Increase(Renderer renderer)
    {
        yield return new WaitForSeconds(0.1f);
        if (!inLineOfSight)
        {
            while (renderer.material.color.a < 1)
            {
                yield return renderer.material.color = new Color(
                    renderer.material.color.r,
                    renderer.material.color.g,
                    renderer.material.color.b,
                    renderer.material.color.a + increaseSpeed
                    );
            }
            yield return renderer.material.color = new Color(
                renderer.material.color.r,
                renderer.material.color.g,
                renderer.material.color.b,
                1f
                );
        }
    }
}
