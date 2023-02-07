using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SCRIPT_ScannerImpactHandler : MonoBehaviour
{
    [Header("Image for scaned state")]
    public Image pickableImagePrefab;
    private Image _pickableImage;
    public float highlightTime = 5f;

    [SerializeField] private Transform iconPivot;

    private void Start()
    {
        if (iconPivot == null)
        {
            iconPivot = gameObject.transform;
        }
    }

    private void LateUpdate()
    {
        if (_pickableImage != null)
        {
            _pickableImage.rectTransform.position = Camera.main.WorldToScreenPoint(iconPivot.position);
        }
    }

    public void HighlightIconWithScanner()
    {
        if (_pickableImage == null)
        {
            if (iconPivot == null)
            {
                _pickableImage = Instantiate(pickableImagePrefab,
                                Camera.main.WorldToScreenPoint(gameObject.transform.position),
                                Quaternion.identity,
                                GameObject.Find("_HUD").transform)
                                .GetComponent<Image>();
            }
            else
            {
                _pickableImage = Instantiate(pickableImagePrefab,
                                Camera.main.WorldToScreenPoint(iconPivot.position),
                                Quaternion.identity,
                                GameObject.Find("_HUD").transform)
                                .GetComponent<Image>();
            }

            StopAllCoroutines();
            StartCoroutine(ShowHighlighteddIconRoutine());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(ShowHighlighteddIconRoutine());
        }
    }

    private IEnumerator ShowHighlighteddIconRoutine()
    {
        while (_pickableImage.color.a < 1)
        {
            yield return _pickableImage.color = new Color(
                _pickableImage.color.r,
                _pickableImage.color.g,
                _pickableImage.color.b,
                _pickableImage.color.a + 0.05f
                );
        }

        yield return StartCoroutine(RemoveHighlightedIconRoutine());
    }

    private IEnumerator RemoveHighlightedIconRoutine()
    {
        yield return _pickableImage.color = new Color(
                _pickableImage.color.r,
                _pickableImage.color.g,
                _pickableImage.color.b,
                1f
                );

        yield return new WaitForSeconds(highlightTime);

        while (_pickableImage.color.a > 0)
        {
            yield return _pickableImage.color = new Color(
                _pickableImage.color.r,
                _pickableImage.color.g,
                _pickableImage.color.b,
                _pickableImage.color.a - 0.01f
                );
        }

        yield return _pickableImage.color = new Color(
                _pickableImage.color.r,
                _pickableImage.color.g,
                _pickableImage.color.b,
                0f
                );

        Destroy(_pickableImage.gameObject);
    }

    private void OnDestroy()
    {
        if (_pickableImage != null)
        {
            Destroy(_pickableImage.gameObject);
        }
    }
}
