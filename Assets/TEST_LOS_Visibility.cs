using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_LOS_Visibility : MonoBehaviour
{
    public GameObject parent;
    private void LateUpdate()
    {
        Debug.Log(parent.GetComponent<Renderer>().enabled);
        GetComponent<Renderer>().enabled = parent.GetComponent<Renderer>().enabled;
        //if (GetComponentInParent<Renderer>().enabled)
        //    GetComponent<Renderer>().enabled = true;
        //else
        //    GetComponent<Renderer>().enabled = false;
    }
}
