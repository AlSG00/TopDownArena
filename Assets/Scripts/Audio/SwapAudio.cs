using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapAudio : MonoBehaviour
{
    //  public CheckPass cp;
    public SwapAudio pair;
    public bool isPassed;
    public AudioClip sound;
    public AudioSource AmbientAudioSource;
    //public AudioSource NextAmbientAudioSource;
    
    //public float volumeAfterFade;
    public float timeToFade;
    private float timeElapsed;
    public float targetVolume;
    public float currentVolume;

    private void Start()
    {
       // currentVolume = PrevAmbientAudioSource.volume;
       // targetVolume = NextAmbientAudioSource.volume;
        //Debug.Log(targetVolume);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isPassed)
        {
                isPassed = true;
            pair.isPassed = false;
            currentVolume = pair.targetVolume;
            //FadeTrack(sound);
            //  }
            //currentVolume = pair.targetVolume;
          // StopAllCoroutines();
            
            StartCoroutine(FadeTrack(sound));
        }
        //if (!cp.isPassed)
        //{
        //    Debug.Log("true");
        // if (!isPassed)
        //  {
        //    isPassed = true;

        //   FadeTrack();
        //  }
        //}
        //else
        //    Debug.Log("false");
    }

    //private IEnumerator FadeTrack()
    //{
    //    //var temp = NextAmbientAudioSource.clip;

    //    AmbientAudioSource.clip = sound;
    //    AmbientAudioSource.Play();

    //    while (timeElapsed < timeToFade)
    //    {
    //        AmbientAudioSource.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
    //        pair.AmbientAudioSource.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
    //        timeElapsed += Time.deltaTime;
    //        yield return null;
    //    }

    //  //  PrevAmbientAudioSource.clip = temp;
    //    pair.AmbientAudioSource.Stop();
    //    //pair.isPassed = false;
    //}

    private IEnumerator FadeTrack(AudioClip newClip)
    {
        float tempTimeElapsed = timeElapsed;
        float tempTimeToFade = timeToFade;

        AmbientAudioSource.clip = newClip;
        AmbientAudioSource.Play();
        timeElapsed = 0;
        AmbientAudioSource.volume = 0;
        while (tempTimeElapsed < tempTimeToFade)
        {
            AmbientAudioSource.volume = Mathf.Lerp(0, targetVolume, tempTimeElapsed / tempTimeToFade);
            pair.AmbientAudioSource.volume = Mathf.Lerp(currentVolume, 0, tempTimeElapsed / tempTimeToFade);
            tempTimeElapsed += Time.deltaTime;
            yield return null;
        }

        pair.AmbientAudioSource.Stop();
    }
}
