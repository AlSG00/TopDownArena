using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SCRIPT_TEST_LookAtBlackZone : MonoBehaviour
{
    public LayerMask activeLayers;
    [Range(0, 360)] public int viewAngle = 180;
    public int raysCount = 10;
    private RaycastHit[] _hits;
    private float angle;
    public SCRIPT_TestBlackZones blackZone;
    SCRIPT_TestBlackZones currentBlackZone;

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
    }

    private void LateUpdate()
    {
        GenerateRaycasts();
    }

    private void GenerateRaycasts()
    {
        for (int i = 0; i < _hits.Length; i++)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward) * 10, out _hits[i]/*, activeLayers)*/))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward) * 10, Color.green);
                if (_hits[i].collider.CompareTag("Black"))
                {
                    blackZone = _hits[i].collider.GetComponent<SCRIPT_TestBlackZones>();
                    if (!blackZone.inLineOfSight)
                    {
                        blackZone.inLineOfSight = true;
                        blackZone.increaseCooldown = Time.time;
                    }
                }
            }

            angle += _angleStep;
        }
        angle = _minAngle;
    }
}
