using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Scope : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private Camera camera;

    // Update is called once per frame
    void Update()
    {
        //Ray ray = camera.ScreenPointToRay();
        Vector3 scopePosition = camera.ScreenToWorldPoint(Input.mousePosition.normalized);
        Debug.Log(scopePosition);
        scopePosition.z = 0f;
        transform.position = scopePosition;
    }
}
