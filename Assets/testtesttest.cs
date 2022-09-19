using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testtesttest : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform player;
    Renderer render;

    void Start()
    {
        render = gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        render.sharedMaterial.SetVector("_PlayerPosition", player.position);
    }
}
