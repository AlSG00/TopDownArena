using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_ExitGame : MonoBehaviour
{
    public void ExitGame()
    {
        Debug.Log("Exit");
        Application.Quit();
    }
}
