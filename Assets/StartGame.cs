using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    //public GameObject playerPrefab;
    public Animator fadeEffect;

    public void Activate()
    {
        Transform Player = GameObject.Find("Player").transform;
        Transform HUD = GameObject.Find("HUD").transform;
        Transform PlayerCamera = GameObject.Find("PlayerCamera").transform;

        Player.transform.GetChild(0).gameObject.SetActive(true);
        HUD.transform.GetChild(0).gameObject.SetActive(true);
        PlayerCamera.transform.GetChild(0).gameObject.SetActive(true);

        Player.transform.GetChild(0).gameObject.GetComponent<nextSpawn>().TargetSpawn = "StartPoint";
        fadeEffect = HUD.transform.GetChild(0).transform.GetChild(3).GetComponent<Animator>();
        StartCoroutine(Start());
    }

    private IEnumerator Start()
    {
        //fadeEffect.SetBool("isFaded", true);
        fadeEffect.SetTrigger("FadeIn");
        float ntime = 0;
        while (ntime < 1f)
        {
            AnimatorStateInfo asi = fadeEffect.GetCurrentAnimatorStateInfo(0);
            ntime = asi.normalizedTime;
            yield return new WaitForEndOfFrame();
        }
        SceneManager.LoadScene("Parking");
    }
}
