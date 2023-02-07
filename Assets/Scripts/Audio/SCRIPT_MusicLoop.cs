using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_MusicLoop : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioClip musicStart;

    // Start is called before the first frame update
    void Start()
    {
        musicSource.PlayOneShot(musicStart);
        musicSource.PlayScheduled(AudioSettings.dspTime + musicStart.length);
    }
}
