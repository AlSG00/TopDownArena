using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwapScenesInteractive : MonoBehaviour
{
    //[SerializeField]
    //private GameObject _player;
    [SerializeField]
    private GameObject _entityNextSpawn;

    [SerializeField]
    private Text _toolbar;

    [SerializeField]
    private string _targetLevel;

    [SerializeField]
    private string _targetSpawnPoint;

    //[SerializeField]
    //private GameObject GameManager;

    private void OnTriggerEnter(Collider other)
    {
        _toolbar.text = "[E]";
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player" && Input.GetKey(KeyCode.E))
        {
            //var scene = GameManager.GetComponent<SceneTransitions>();
            //scene.targerSpawnPoint = _targetSpawnPoint;
            _entityNextSpawn.GetComponent<nextSpawn>().TargetSpawn = _targetSpawnPoint;
            //other.GetComponent<nextSpawn>().TargetSpawn = _targetSpawnPoint;
            
            //SceneManager.LoadScene(_targetLevel);
            //LoadNewScene();
            //gameObject.SetActive(false);

            //yield return new WaitForSeconds(0.5f);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_targetLevel);
            //// fadeToBlack.SetTrigger("FadeIn");
            ////   pState.movingToNextLevel = false;
            //// Wait until the asynchronous scene fully loads

            //while (!asyncLoad.isDone)
            //{
            //    yield return null;
            //}

            //if (_targetSpawnPoint == "Red")
            //{
            //    _player.transform.position = GameObject.FindGameObjectWithTag("Red").transform.position;
            //}
            /*if*/
            //{
            //    GameObject spawnpoint = GameObject.Find(_targetSpawnPoint);
            //  Debug.Log(spawnpoint);
            //     _player.transform.position = spawnpoint.transform.position;
            //  _player.transform.position = GameObject.FindGameObjectWithTag("Blue").transform.position;
            //}
        }
    }

    

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
    //    var spawnPoint = GameObject.Find("testSpawn");
    //    var player = GameObject.Find("Player");

    //    player.transform.position = spawnPoint.transform.position;
    //}


    //private void LoadNewScene()
    //{
    //    SceneManager.LoadScene(_targetLevel);
    //    GameObject spawnpoint = GameObject.Find(_targetSpawnPoint);
    //    _player.transform.position = spawnpoint.transform.position;
    //}

    //private void OnLevelWasLoaded(int level)
    //{
    //    GameObject spawnpoint = GameObject.Find(_targetSpawnPoint);
    //    _player.transform.position = spawnpoint.transform.position;

    //}

    private void OnTriggerExit(Collider other)
    {
        _toolbar.text = "";
    }
}
