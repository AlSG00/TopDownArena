using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    [Tooltip("???")]
    public ToolTip tooltip;

    private Renderer renderer;


    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        //text.gameObject.SetActive(false);
        tooltip.SetDefaultTooltip();
        //WaitForSeconds wait = new WaitForSeconds(0.3);
    }


    Ray ray;
    RaycastHit hit;

    void FixedUpdate()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            tooltip.SetTooltip($"{hit.collider.name}");
            tooltip.transform.position = Input.mousePosition;
            Debug.Log(hit.collider.name);
        }
    }

    private void OnMouseEnter()
    {
     //   Debug.Log(ga);
        ////  Debug.Log($"speed {pivot.rotationSpeed}");
        ////  Debug.Log($"acceleration {acceleration_temp}");
        //renderer.material.color = Color.red;
        //while (pivot.rotationSpeed > 0 && acceleration_temp > 0)
        //{
        //    //   WaitForEndOfFrame;
        //    pivot.rotationSpeed -= acceleration_temp;
        //    acceleration_temp -= 0.01f;
        //    Debug.Log($"speed {pivot.rotationSpeed}");
        //    Debug.Log($"acceleration {acceleration_temp}");
        //}
        ////acceleration = 0;
        //pivot.rotationSpeed = 0;
        ////   Debug.Log(pivot.rotationSpeed);
    }

    //private void OnMouseExit()
    //{
    //    Debug.Log("Uncheck");
    //    renderer.material.color = Color.white;
    //    //acceleration_temp = 0.01f;
    //    //while (pivot.rotationSpeed < speed)
    //    //{
    //    //    pivot.rotationSpeed += acceleration_temp;
    //    //    acceleration_temp *= 2;
    //    //    Debug.Log(pivot.rotationSpeed);
    //    //    Debug.Log($"acceleration {acceleration_temp}");
    //    //}

    //    pivot.rotationSpeed = speed;
    //    //   acceleration_temp = acceleration;
    //    speed = 0;
    //    //   Debug.Log(pivot.rotationSpeed);
    //}

    private void OnMouseDown()
    {
        // Debug.Log("Íא ובאכמ סובו םאזלט");

        //if (!text.gameObject.activeSelf)
        //    text.gameObject.SetActive(true);
        //else
        //    text.gameObject.SetActive(false);


    }
}
