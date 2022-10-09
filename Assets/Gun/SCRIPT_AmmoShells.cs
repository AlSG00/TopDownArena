using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_AmmoShells : MonoBehaviour
{
    class Shell
    {
        public GameObject shellObj;
    }

    List<Shell> shells = new List<Shell>();

    public GameObject shellPrefab;
    public Transform shellEjector;
    [SerializeField]
    float shellThrowingMinAngle_X;                 // Х-угол выброса гильзы
    [SerializeField]
    float shellThrowingMaxAngle_X;                 // Х-угол выброса гильзы
    [SerializeField]
    float shellThrowingMinAngle_Y;         // минимальный У-угол выброса гильзы
    [SerializeField]
    float shellThrowingMaxAngle_Y;         // максимальный У-угол выброса гильзы
    [SerializeField]
    float shellThrowingMinAngle_Z;                 // Х-угол выброса гильзы
    [SerializeField]
    float shellThrowingMaxAngle_Z;                 // Х-угол выброса гильзы

    //public int shellsLimit = 500;

    //private int shellPosition = 5;
    //private int nextShell;

    //private void Awake()
    //{
    //    nextShell = shellPosition;
    //}

    public void EjectShell()
    {
        NewShell();
    }

    public void NewShell()
    {
        Shell shell = new Shell();
        Transform shellEjectDirection = shellEjector;
        shellEjectDirection.transform.localEulerAngles = new Vector3(
            Random.Range(shellThrowingMinAngle_X, shellThrowingMaxAngle_X),
            Random.Range(shellThrowingMinAngle_Y, shellThrowingMaxAngle_Y),
            Random.Range(shellThrowingMinAngle_Z, shellThrowingMaxAngle_Z)
            );

        shell.shellObj = Instantiate(shellPrefab, shellEjectDirection.position, shellEjectDirection.rotation);
        shells.Add(shell);
    }

    //public void UpdateShells()
    //{
    //    DestroyShells();
    //}

    //private void DestroyShells()
    //{
    //    if (shells.Count >= shellsLimit)
    //    {
    //        shells.RemoveAt(nextShell);
    //        nextShell = NewNextShell();
    //    }
    //}

    //private int NewNextShell()
    //{
    //    shellPosition += shellPosition;
    //    Debug.Log(shellPosition % shellsLimit);
    //    return shellPosition % shellsLimit;
    //}
}
