using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStoredItem
{
    public GameObject item { get; set; }
    public Vector2 positionOnGrid { get; set; }
}
