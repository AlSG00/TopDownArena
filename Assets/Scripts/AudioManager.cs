using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioManager instance;
    public AudioSource track01, track02;
    private bool isPlayingTrack01;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    //private void Start()
    //{
    //    isPlayingTrack01 = true;
    //    swapTrack
    //}

    public void SwapTrack(AudioClip clip)
    {

    }

    private IEnumerator FadeTrack(AudioClip newClip)
    {
        float timeToFade = 0.25f;
        float timeElapsed = 0.25f;

        if (isPlayingTrack01)
        {
            track02.clip = newClip;
            track02.Play();

            while (timeElapsed < timeToFade)
            {
                track02.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                track01.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track01.Stop();
        }
        else
        {
            track01.clip = newClip;
            track01.Play();

            while (timeElapsed < timeToFade)
            {
                track01.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                track02.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }

            track02.Stop();
        }
    }

   // public void ReturnToDefault
}
