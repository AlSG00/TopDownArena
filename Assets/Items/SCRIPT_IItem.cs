using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SCRIPT_IItem
{
    //public GameObject prefab { public get; set; }
    public bool isUsable { get; set; }
    public void Use() { }
}
