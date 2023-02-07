using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SwapScenesInteractive : MonoBehaviour
{
    [SerializeField]
    private Text _toolbar;

    [SerializeField]
    private string _targetLevel;

    [SerializeField]
    private string _targetSpawnPoint;

    public Animator fadeEffect;

    private bool hasCoroutine = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _toolbar = GameObject.Find("HUD").transform.GetChild(0).transform.GetChild(2).transform.GetChild(0).GetComponent<Text>();
            _toolbar.text = "[E]";
        //    StartCoroutine(Start());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && Input.GetKey(KeyCode.E))
        {
            fadeEffect = GameObject.Find("HUD").transform.GetChild(0).transform.GetChild(3).GetComponent<Animator>();

            other.GetComponent<nextSpawn>().TargetSpawn = _targetSpawnPoint;
            
            if (_targetLevel == "Titles")
            {
                other.transform.GetChild(0).gameObject.SetActive(false);
               // GameObject.Find("HUD").transform.GetChild(0).gameObject.SetActive(false);
               // GameObject.Find("PlayerCamera").transform.GetChild(0).gameObject.SetActive(false);
            }

            if (!hasCoroutine)
            {
                hasCoroutine = true;
                StartCoroutine(Start());
            }
        }
    }

    private IEnumerator Start()
    {
        fadeEffect.SetTrigger("FadeIn");
        float ntime = 0;
        while (ntime < 1f)
        {
            AnimatorStateInfo asi = fadeEffect.GetCurrentAnimatorStateInfo(0);
            ntime = asi.normalizedTime;
            yield return new WaitForEndOfFrame();
        }

        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(_targetLevel);

        while (!loadAsync.isDone)
        {
            yield return null;
        }

       // SceneManager.LoadScene(_targetLevel);
    }

    private void OnTriggerExit(Collider other)
    {
        _toolbar.text = "";
    }
}
