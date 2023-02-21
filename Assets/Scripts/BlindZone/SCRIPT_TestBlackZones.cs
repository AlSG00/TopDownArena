using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SCRIPT_TestBlackZones : MonoBehaviour
{
    [SerializeField] private GameObject[] _blackZones;

    [Header("Black zone parameters")]
    public float increaseDelay = 1f;
    [Range(0, 1)] public float fadeSpeed = 0.3f;
    [Range(0, 1)] public float increaseSpeed = 0.3f;

    [HideInInspector] public float increaseCooldown;
    [HideInInspector] public bool inLineOfSight = false;
    [HideInInspector] public bool stayingInside = false;
    private Renderer _renderer;

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            increaseCooldown = Time.time;
            stayingInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            increaseCooldown = Time.time;
            stayingInside = false;
        }
    }

    public void FadeArea()
    {
        StopAllCoroutines();
        for (int i = 0; i < _blackZones.Length; i++)
        {
            _renderer = _blackZones[i].GetComponent<Renderer>();
            StartCoroutine(Fade(_renderer));
        }
    }

    public void IncreaseArea()
    {
        StopAllCoroutines();
        for (int i = 0; i < _blackZones.Length; i++)
        {
            _renderer = _blackZones[i].GetComponent<Renderer>();
            StartCoroutine(Increase(_renderer));
        }
    }

    private IEnumerator Fade(Renderer renderer)
    {
        renderer.gameObject.layer = 2;
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
            renderer.gameObject.layer = 17;
        }
    }
}
