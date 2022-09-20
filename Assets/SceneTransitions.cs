using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitions : MonoBehaviour
{
    public string targerSpawnPoint;
    private SwapScenesInteractive nextScene;
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var spawnPoint = GameObject.Find(targerSpawnPoint);
        var player = GameObject.Find("Player");

        player.transform.position = spawnPoint.transform.position;
    }
}
