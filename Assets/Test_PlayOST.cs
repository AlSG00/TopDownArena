using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PlayOST : MonoBehaviour
{
    private bool isFinished = false;
    [SerializeField]
    private bool playOnce;
    [SerializeField]
    private AudioSource sound;
    [SerializeField]
    private AudioClip clip;
    [SerializeField]
    private float delay;

    //private bool isCoroutineRunning = false;

    private void OnTriggerEnter(Collider other)
    {
        if ((other.CompareTag("Player")) && (!playOnce || !isFinished))
        {
            if (sound.isPlaying)
            {
                StartCoroutine(Fade());
            }
            StartCoroutine(PlaySound());
        }
    }

    private IEnumerator PlaySound()
    {
        //if ((other.CompareTag("Player")) && (!playOnce || !isFinished))
        //{
        isFinished = true;
        yield return new WaitForSeconds(delay);
        sound.PlayOneShot(clip);
        //while (sound.isPlaying)
        //{
        //    isCoroutineRunning = true;
        //}
    }

    private IEnumerator Fade()
    {
        float timeToFade = 1.5f;
        float timeElapsed = 0.25f;
        float tempVolume = sound.volume;

        while (timeElapsed < timeToFade)
        {
            sound.volume = Mathf.Lerp(tempVolume, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        sound.Stop();
        sound.volume = tempVolume;
        //isCoroutineRunning = false;
    }
}
