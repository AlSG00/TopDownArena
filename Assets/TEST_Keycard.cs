using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_Keycard : MonoBehaviour
{
    private bool _isMouseOver;
    public Texture2D icon;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.E) && _isMouseOver)
        {
            other.GetComponent<PlayerInventory>().AddItem(gameObject);
            Debug.Log("BLUE KEYCARD ADDED TO INVENTORY");
            Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
            Destroy(gameObject);
        }
    }

    private void OnMouseEnter()
    {
        _isMouseOver = true;
        Cursor.SetCursor(icon, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        _isMouseOver = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }
}
