using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Rename it to some "ShellGenerator"
public class SCRIPT_AmmoShells : MonoBehaviour
{
    // TODO: Refactor it
    class Shell
    {
        public GameObject shellObj;

    }

    List<Shell> shells = new List<Shell>();

    public GameObject shellPrefab;
    public Transform shellEjector;


    //[SerializeField]
    //float shellThrowingMinAngle_X;                 // Х-угол выброса гильзы
    //[SerializeField]
    //float shellThrowingMaxAngle_X;                 // Х-угол выброса гильзы
    //[SerializeField]
    //float shellThrowingMinAngle_Y;         // минимальный У-угол выброса гильзы
    //[SerializeField]
    //float shellThrowingMaxAngle_Y;         // максимальный У-угол выброса гильзы
    //[SerializeField]
    //float shellThrowingMinAngle_Z;                 // Х-угол выброса гильзы
    //[SerializeField]
    //float shellThrowingMaxAngle_Z;                 // Х-угол выброса гильзы

    [SerializeField] private Vector3Int _minThrowAngle;
    [SerializeField] private Vector3Int _maxThrowAngle;

    public void EjectShell()
    {
        NewShell();
    }

    public void NewShell()
    {
        Shell shell = new Shell();
        Transform shellEjectDirection = shellEjector;

        //shellEjectDirection.transform.localEulerAngles = new Vector3(
        //    Random.Range(shellThrowingMinAngle_X, shellThrowingMaxAngle_X),
        //    Random.Range(shellThrowingMinAngle_Y, shellThrowingMaxAngle_Y),
        //    Random.Range(shellThrowingMinAngle_Z, shellThrowingMaxAngle_Z)
        //    );

        shellEjectDirection.transform.localEulerAngles = new Vector3(
            Random.Range(_minThrowAngle.x, _maxThrowAngle.x),
            Random.Range(_minThrowAngle.y, _maxThrowAngle.y),
            Random.Range(_minThrowAngle.z, _maxThrowAngle.z)
            );

        shell.shellObj = Instantiate(shellPrefab, shellEjectDirection.position, shellEjectDirection.rotation);
        shells.Add(shell);
    }
}
