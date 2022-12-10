using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SCRIPT_TestBlackZones : MonoBehaviour
{
    public GameObject[] blackZone;
    [Range(0, 1)] public float fadeSpeed = 0.3f;
    [Range(0, 1)] public float increaseSpeed = 0.3f;
    //public bool canSee = true;

    [HideInInspector] public bool inLineOfSight = false;
    [HideInInspector] public bool stayingInside = false;
    /*[HideInInspector]*/ //public bool[] raycastHits;

    private Renderer _renderer;
    //private MaterialPropertyBlock _propertyBlock;
    //private SCRIPT_TEST_LookAtBlackZone PlayerRaycastLook;
    //private Coroutine _lastCoroutine;
    //public bool inLOS = false;
    private bool _isFaded = false;
    public float increaseDelay = 1f;
    [HideInInspector] public float increaseCooldown;

    //private _inArea = false;
   // public float increaseCooldownTemp;


    private void Awake()
    {
        //_propertyBlock = new MaterialPropertyBlock();
        
        //_renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        //if (raycastHits != null)
        //{
        //    string temp = "";
        //    for (int i = 0; i < raycastHits.Length; i++)
        //    {
        //        temp += $"{raycastHits[i]} | ";
        //    }
        //    Debug.Log(temp);
        //}


        if (increaseCooldown + increaseDelay <= Time.time)
        {
            increaseCooldown = Time.time;
            inLineOfSight = false;

            //if (increaseCooldown + increaseDelay <= Time.time)
           // {
                if (!stayingInside && !inLineOfSight)
                {
                    IncreaseArea();
                }
           // }
        }
        else if ((inLineOfSight || stayingInside))
        {
            FadeArea();
            //_isFaded = true;
        }


        //if (!inLineOfSight && !stayingInside)
        //{
        //    if (!stayingInside)
        //    {
        //        if (increaseCooldown + increaseDelay <= Time.time)
        //        {
        //            IncreaseArea();
        //        }
        //    }
        //    else
        //    {
        //        increaseCooldown = Time.time;
        //    }
        //}
        //else
        //{
        //    //increaseCooldown = Time.time;
        //    FadeArea();
        //}
        //if (raycastHits.All(x => x == false) && raycastHits.Count(x => x == true) <= 1 &&
        //    !stayingInside)
        //{
        //    IncreaseArea();
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") /*&& !inLineOfSight*/)
        {
            increaseCooldown = Time.time;
            stayingInside = true;
            _isFaded = true;
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
