using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour
{
    public Text tootltip;

    public void SetDefaultTooltip()
    {

        tootltip.text = "";
    }

    public void SetTooltip(string message)
    {
        tootltip.text = message;
    }
}
