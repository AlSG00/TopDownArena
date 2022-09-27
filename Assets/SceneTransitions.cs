using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    private string _targerSpawnPointName;
    private GameObject _player;
    private GameObject _targerSpawnPoint;
    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    var spawnPoint = GameObject.Find(targerSpawnPoint);
    //    var player = GameObject.Find("Player");

    //    player.transform.position = spawnPoint.transform.position;
    //}

    private void Start()
    {
        _player = GameObject.Find("ENTY_NextSpawn");
        _targerSpawnPoint = GameObject.Find(_player.GetComponent<nextSpawn>().TargetSpawn);
        _player.transform.position = _targerSpawnPoint.transform.position;
    }
}
