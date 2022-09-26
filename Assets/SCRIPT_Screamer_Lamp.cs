using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_Screamer_Lamp : MonoBehaviour
{
    [SerializeField]
    private AudioSource _lampSoundSource;
    [SerializeField]
    private AudioClip _lampSoundEffect;
    [SerializeField]
    private ParticleSystem particle;
    [SerializeField]
    private GameObject _targetLight;
    private bool _isFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!_isFinished)
            {
                _targetLight.SetActive(false);
                particle.Play();
                _lampSoundSource.PlayOneShot(_lampSoundEffect);
                _isFinished = true;
            }
        }
    }
}
