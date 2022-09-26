using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_LOS_Visibility : MonoBehaviour
{
    public GameObject parent;
    private void LateUpdate()
    {
        GetComponent<Renderer>().enabled = parent.GetComponent<Renderer>().enabled;
    }
}
