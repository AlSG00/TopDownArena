using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SCRIPT_TEST_LookAtBlackZone : MonoBehaviour
{
    [Range(0, 360)]
    public int viewAngle = 180;

    public int raysCount = 10;

    private RaycastHit[] _hits;
    
    private float angle;

    SCRIPT_TestBlackZones blackZone;

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

        Debug.Log($"_hits: {_hits}");
        Debug.Log($"_minAngle: {_minAngle}");
        Debug.Log($"_maxAngle: {_maxAngle}");
        Debug.Log($"_angleStep: {_angleStep}");
    }

    private void Update()
    {
        for (int i = 0; i < _hits.Length; i++)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward) * 10, out _hits[i]))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward) * 10, Color.green);
                if (_hits[i].collider.CompareTag("Black"))
                {
                    blackZone = _hits[i].collider.GetComponent<SCRIPT_TestBlackZones>();

                    //if (blackZone.canSee)
                    //{
                    if (blackZone.raycastHits == null ||
                        blackZone.raycastHits.Length != _hits.Length)
                    {
                        blackZone.raycastHits = new bool[_hits.Length];
                    }

                    if (!blackZone.raycastHits.Contains(true))
                    {
                        blackZone.inLineOfSight = true;
                        blackZone.raycastHits[i] = true;
                        if (blackZone.canSee)
                        {
                            blackZone.FadeArea();
                        }
                    }
                    //}
                }
                else
                {
                    if (blackZone != null)
                    {
                        blackZone.raycastHits[i] = false;
                        if (blackZone.raycastHits.All(x => x == false))
                        {
                            Debug.LogWarning("increase");
                            blackZone.inLineOfSight = false;
                            if (!blackZone.stayingInside && !blackZone.inLineOfSight)
                            {
                                blackZone.IncreaseArea();
                                blackZone = null;
                            }
                        }
                    }
                }

            }
            else
            {
                if (blackZone != null)
                {
                    blackZone.raycastHits[i] = false;
                    if (blackZone.raycastHits.All(x => x == false))
                    {
                        Debug.LogWarning("increase");
                        blackZone.inLineOfSight = false;
                        if (!blackZone.stayingInside && !blackZone.inLineOfSight)
                        {
                            blackZone.IncreaseArea();
                            blackZone = null;
                        }
                    }
                }
            }
            angle += _angleStep;   
        }
        angle = _minAngle;
    }
}
