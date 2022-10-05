using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_UnlockDoor : MonoBehaviour
{
    private bool _isMouseOver;
    public GameObject requiredItem;
    public Material unlocked;
    public Material locked;
    public Light glowing;
    public Texture2D openWithKeycardIcon;
    public Color closed;
    public Color opened;
    public GameObject targetDoor;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.E) && _isMouseOver)
        {
            if (other.GetComponent<PlayerInventory>().items.Contains(requiredItem))
            {
                other.GetComponent<PlayerInventory>().RemoveItem(gameObject);
                Unlock();
                Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
                Debug.Log("DOOR OPENED");
                Debug.Log($"{requiredItem} REMOVED FROM YOUR INVENTORY");
            }
            else
            {
                Debug.Log($"{requiredItem} REQUIRED");
            }
        }
    }

    private void Unlock()
    {
        gameObject.GetComponent<MeshRenderer>().material = unlocked;
        glowing.color = opened;
        targetDoor.SetActive(true);
    }

    private void OnMouseEnter()
    {
        _isMouseOver = true;
        Cursor.SetCursor(openWithKeycardIcon, Vector2.zero, CursorMode.ForceSoftware);
    }

    private void OnMouseExit()
    {
        _isMouseOver = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.ForceSoftware);
    }
}
