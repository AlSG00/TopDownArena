using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_ActivateTriggers : MonoBehaviour
{
    public List<GameObject> trigger;

    private void OnDestroy()
    {
        if (trigger != null && trigger.Count > 0)
        {
            for (int i = 0; i < trigger.Count; i++)
            {
                trigger[i].SetActive(true);
            }
        }
    }
}
