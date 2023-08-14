using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItem : MonoBehaviour, SCRIPT_IItem
{
    public bool isUsable { get; set; }

    public void Use()
    {
        Debug.Log("<color=yellow>Using weapon...</color>");

        // Pseudocode
        // Call a menu that will suggest a few places for equipping a weapon
        // Hide the menu by second right-click or moving cursor away from the menu or weapon icon 

    }
}
