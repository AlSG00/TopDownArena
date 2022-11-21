using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SCRIPT_TEST_LookAtBlackZone : MonoBehaviour
{
    [Range(0, 360)] public int viewAngle = 180;
    public int raysCount = 10;
    private RaycastHit[] _hits;
    private float angle;
    public List<SCRIPT_TestBlackZones> blackZone;
    SCRIPT_TestBlackZones currentBlackZone;
    //public bool alreadyFading = false;

    private float _minAngle;
    private float _maxAngle;
    private float _angleStep;

    private void Awake()
    {
        _hits = new RaycastHit[raysCount];
        _minAngle = -1 * (viewAngle / 2);
        _maxAngle = viewAngle / 2;
        _angleStep = viewAngle / _hits.Length;
        angle = _minAngle;
        blackZone = new List<SCRIPT_TestBlackZones>();
        //Debug.Log($"_hits: {_hits}");
        //Debug.Log($"_minAngle: {_minAngle}");
        //Debug.Log($"_maxAngle: {_maxAngle}");
        //Debug.Log($"_angleStep: {_angleStep}");
    }

    private void Update()
    {
        GenerateRaycasts();

    }

    private void GenerateRaycasts()
    {
        for (int i = 0; i < _hits.Length; i++)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward) * 10, out _hits[i]))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward) * 10, Color.green);
                if (_hits[i].collider.CompareTag("Black"))
                {
                    currentBlackZone = null;
                    currentBlackZone = blackZone.Find(x => x == _hits[i].collider.GetComponent<SCRIPT_TestBlackZones>());
                    if (currentBlackZone == null)
                    {
                        currentBlackZone = _hits[i].collider.GetComponent<SCRIPT_TestBlackZones>();
                        blackZone.Add(currentBlackZone);
                    }

                    if (currentBlackZone.raycastHits == null ||
                        currentBlackZone.raycastHits.Length != _hits.Length)
                    {
                        currentBlackZone.raycastHits = new bool[_hits.Length];
                    }

                    currentBlackZone.raycastHits[i] = true;
                    if (currentBlackZone.raycastHits.Count(x => x == true) <= 1)
                    {
                        currentBlackZone.inLineOfSight = true;
                        if (currentBlackZone.canSee)
                        {
                            currentBlackZone.FadeArea();
                        }
                    }

                    //for (int j = 0; j < blackZone.Count; j++)
                    //{
                    //    if (j == i)
                    //    {
                    //        continue;
                    //    }
                    //    blackZone[j].raycastHits[i] = false;
                    //}
                    
                    //if (!currentBlackZone.raycastHits.Contains(true))
                    //{
                    //    Debug.Log("3 - fading");
                    //    currentBlackZone.inLineOfSight = true;
                    //    currentBlackZone.raycastHits[i] = true;
                    //    if (currentBlackZone.canSee)
                    //    {
                    //        currentBlackZone.FadeArea();
                    //    }
                    //}
                    //if (blackZone.raycastHits == null ||
                    //    blackZone.raycastHits.Length != _hits.Length)
                    //{
                    //    blackZone.raycastHits = new bool[_hits.Length];
                    //}

                    //if (!blackZone.raycastHits.Contains(true))
                    //{
                    //    blackZone.inLineOfSight = true;
                    //    blackZone.raycastHits[i] = true;
                    //    if (blackZone.canSee)
                    //    {
                    //        blackZone.FadeArea();
                    //    }
                    //}
                    //}
                    //break;
                }
                else
                {
                    if (currentBlackZone != _hits[i].collider.GetComponent<SCRIPT_TestBlackZones>())
                    {
                        currentBlackZone.raycastHits[i] = false;
                        if (currentBlackZone.raycastHits.All(x => x == false))
                        {
                            currentBlackZone.inLineOfSight = false;
                            if (!currentBlackZone.stayingInside && !currentBlackZone.inLineOfSight)
                            {
                                currentBlackZone.IncreaseArea();
                                //blackZone.Remove(currentBlackZone);
                                currentBlackZone = null;
                            }
                        }
                    }

                    //if (blackZone != null)
                    //{
                    //    blackZone.raycastHits[i] = false;
                    //    if (blackZone.raycastHits.All(x => x == false))
                    //    {
                    //        blackZone.inLineOfSight = false;
                    //        if (!blackZone.stayingInside && !blackZone.inLineOfSight)
                    //        {
                    //            blackZone.IncreaseArea();
                    //            blackZone = null;
                    //        }
                    //    }
                    //}
                }
            }
            else
            {
                if (blackZone != null)
                {
                    currentBlackZone.raycastHits[i] = false;
                    if (currentBlackZone.raycastHits.Count(x => x == true) <= 1)
                    {
                        currentBlackZone.inLineOfSight = false;
                        if (!currentBlackZone.stayingInside && !currentBlackZone.inLineOfSight)
                        {
                            currentBlackZone.IncreaseArea();
                            blackZone.Remove(currentBlackZone);
                            currentBlackZone = null;
                        }
                    }
                }
            }
            angle += _angleStep;
        }
        angle = _minAngle;
    }
}
