using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    public GameObject playerPrefab;

    public void Activate()
    {
        Debug.Log("Activated");
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Parking");
        GameObject player = GameObject.Find("Player");
        GameObject spawnPoint = GameObject.Find("StartPoint");

        if (player == null)
        {
            Instantiate(playerPrefab, spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
        else
        {
            player.transform.position = spawnPoint.transform.position;
        }
    }
}
