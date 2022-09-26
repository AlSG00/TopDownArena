using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwapScene : MonoBehaviour
{
    [SerializeField] private string _targetLevel;
    [SerializeField] private string _targetSpawnPoint;
    [SerializeField] private Animator _fadeIn;

    private IEnumerator LoadScene() 
    {
        // AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_targetLevel);


        //var scene = GameManager.GetComponent<SceneTransitions>();
        //scene.targerSpawnPoint = _targetSpawnPoint;

        //SceneManager.LoadScene(_targetLevel);
        //LoadNewScene( );
        //gameObject.SetActive(false);
        _fadeIn.SetTrigger("Fading");

        yield return new WaitForSeconds(0.5f);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_targetLevel);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
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
