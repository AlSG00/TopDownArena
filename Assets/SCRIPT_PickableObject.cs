using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_PickableObject : MonoBehaviour
{
    public bool canPick = false;
    SCRIPT_InventoryController inventory;

    private void Start()
    {
        inventory = GameObject.Find("_PlayerCamera").GetComponent<SCRIPT_InventoryController>();
    }

    public void Pick()
    {
        inventory.InsertItemIntoInventory(gameObject);
        Destroy(gameObject);
    }
}
