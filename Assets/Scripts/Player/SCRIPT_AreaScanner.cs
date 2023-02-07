using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCRIPT_AreaScanner : MonoBehaviour
{
    [SerializeField] private Transform _scannedArea;

    public float scanRadius = 5f;

    private Vector3 _scaleChange = new Vector3 (0.25f, 0, 0.25f);
    private Vector3 _startSize;
    private Vector3 _finalSize;

    public float highlightTime = 5f;
    public float scannerUseCooldown = 5f;
    private float scannerLastUse;

    public AudioSource _playerAudioSource;
    public AudioClip scanSound;
    public AudioClip scanReadySound;
    public AudioClip scanNotReadySound;

    private void Start()
    {
        _startSize = new Vector3(0.1f, _scannedArea.localScale.y, 0.1f);
        _finalSize = new Vector3(scanRadius, _scannedArea.localScale.y, scanRadius);
        _scannedArea.gameObject.SetActive(false);
        _playerAudioSource = GameObject.Find("PlayerAudioSource").GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        { 
            if (scannerLastUse + scannerUseCooldown <= Time.time)
            {
                scannerLastUse = Time.time;
                ScanArea();
            } 
            else if (scanNotReadySound != null)
            {
                _playerAudioSource.PlayOneShot(scanNotReadySound);
            }
        }
    }

    private void ScanArea()
    {
        Debug.Log("Scanning...");
        StopAllCoroutines();
        StartCoroutine(ScanRoutine());
    }

    private IEnumerator ScanRoutine()
    {
        _scannedArea.gameObject.SetActive(true);

        if (scanSound != null)
        {
            _playerAudioSource.PlayOneShot(scanSound);
        }
        
        while (_scannedArea.localScale.x < scanRadius)
        {
            yield return _scannedArea.localScale += _scaleChange;
        }

        yield return _scannedArea.localScale = _finalSize;

        yield return new WaitForSecondsRealtime(0.1f);

        yield return _scannedArea.localScale = _startSize;

        _scannedArea.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(scannerUseCooldown);

        if (scanReadySound != null)
        {
            _playerAudioSource.PlayOneShot(scanReadySound);
        }
    }
}
