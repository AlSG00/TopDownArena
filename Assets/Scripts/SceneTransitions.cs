using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    private string _targerSpawnPointName;
    private Transform _player;
    private GameObject _targerSpawnPoint;
    private Transform _fade;

    private void Awake()
    {
        _player = GameObject.Find("Player").transform.GetChild(0);
        _fade = GameObject.Find("HUD").transform.GetChild(0).transform.GetChild(3);
        _fade.GetComponent<Animator>().SetTrigger("FadeOut");

        string spawn = _player.GetComponent<nextSpawn>().TargetSpawn;
        if (spawn != "")
        {
            _targerSpawnPoint = GameObject.Find(spawn);
            _player.transform.position = _targerSpawnPoint.transform.position;
        }
    }
}
