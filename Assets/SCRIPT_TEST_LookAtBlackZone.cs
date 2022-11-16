using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_TEST_LookAtBlackZone : MonoBehaviour
{
    RaycastHit hit;
    RaycastHit hit2;
    RaycastHit hit3;
    RaycastHit hit4;
    RaycastHit hit5;

    RaycastHit[] hits = new RaycastHit[10];
    //Vector3 step = new Vector3(0, 1, 0);
    //// Spread angle of raycasts.
    //  Quaternion spreadAngle = Quaternion.AngleAxis(-60.0f, new Vector3(0, 1, 0));
    //Quaternion spreadAnglePositive = Quaternion.AngleAxis(60.0f, new Vector3(0, 1, 0));
    private float angle = -90;
    //Quaternion spreadAngleNegative2 = Quaternion.AngleAxis(-30.0f, new Vector3(0, 1, 0));
    //Quaternion spreadAnglePositive2 = Quaternion.AngleAxis(30.0f, new Vector3(0, 1, 0));

    private void Update()
    {
        //if (Physics.Raycast(transform.position, transform.forward, out hit))
        //{
        //        Debug.DrawRay(transform.position, transform.forward, Color.green); 
        //}

        //if (Physics.Raycast(transform.position, transform.TransformDirection(spreadAngleNegative * Vector3.forward), out hit2))
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(spreadAngleNegative * Vector3.forward), Color.blue);
        //}

        for (int i = 0; i < hits.Length; i++)
        {
            if (Physics.Raycast(transform.position, transform.TransformDirection(Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward) * 10, out hits[i]))
            {
                Debug.DrawRay(transform.position, transform.TransformDirection(Quaternion.AngleAxis(angle, Vector3.up) * Vector3.forward) * 10, Color.green);
                if (hits[i].collider.CompareTag("Black"))
                {
                    //Debug.Log($"Hit {i}");
                    //hits[i].collider.GetComponent<SCRIPT_TestBlackZones>().FadeArea();
                }
                
                
            }
            angle += 18f;   
        }
        angle = -90f;
        //Debug.DrawLine(transform.position /*+ step*/, transform.TransformDirection(spreadAngleNegative * Vector3.forward) * 6, Color.blue);
        //Debug.DrawLine(transform.position /*+ step*/, transform.TransformDirection(spreadAnglePositive * Vector3.forward) * 6, Color.blue);
        //Debug.DrawLine(transform.position /*- step*/, transform.TransformDirection(spreadAngleNegative2 * Vector3.forward) * 6, Color.yellow);
        //Debug.DrawLine(transform.position /*- step*/, transform.TransformDirection(spreadAnglePositive2 * Vector3.forward) * 6, Color.yellow);


        //if (Physics.Raycast(transform.position /*+ step*/, transform.TransformDirection(Vector3.forward), out hit))
        //{
        //  //  if (hit.distance < 6 && hit.transform.tag == "Player")
        //  //  {
        //  //      Debug.LogWarning("it came from 1");
        //        //Debug.DrawLine(transform.position, );
        //  //  }
        //}

        //else if (Physics.Raycast(transform.position /*+ step*/, transform.TransformDirection(spreadAngleNegative * Vector3.forward), out hit2))
        //{
        //  //  if (hit2.distance < 10 && hit2.transform.tag == "Player")
        //  //  {
        //  //      Debug.LogWarning("it came from 2");
        //  //  }
        //}

        //else if (Physics.Raycast(transform.position /*+ step*/, transform.TransformDirection(spreadAnglePositive * Vector3.forward), out hit3))
        //{
        //    //if (hit3.distance < 10 && hit3.transform.tag == "Player")
        //   // {
        //   //     Debug.LogWarning("it came from 3");
        //   // }
        //}

        //else if (Physics.Raycast(transform.position /*+ step*/, transform.TransformDirection(spreadAngleNegative2 * Vector3.forward), out hit4))
        //{
        //    //if (hit4.distance < 10 && hit4.transform.tag == "Player")
        //   // {
        //   //     Debug.LogWarning("it came from 4");
        //  // }
        //}

        //else if (Physics.Raycast(transform.position /*+ step*/, transform.TransformDirection(spreadAnglePositive2 * Vector3.forward), out hit5))
        //{
        //   // if (hit5.distance < 10 && hit5.transform.tag == "Player")
        //   // {
        //   //     Debug.LogWarning("it came from 5");
        //   // }
        //}
    }
}
