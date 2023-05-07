using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemInfoWindowHandler : MonoBehaviour
{
    [SerializeField] private Image[] ItemInfoWindows;
    [SerializeField] private TextMeshProUGUI[] ItemInfoText;

    [SerializeField] private float appearingSpeed = 1f;
    [SerializeField] private float dissapearingSpeed = 1f;

    private bool isVisible = false;

    private void OnEnable()
    {
        SCRIPT_InventoryItem.OnShowItemInfo += SetVisibility;
    }

    private void OnDisable()
    {
        SCRIPT_InventoryItem.OnShowItemInfo -= SetVisibility;
    }

    private void Start()
    {
        HideUI();
    }

    private void SetVisibility(bool isVisible, SCRIPT_ItemData itemData)
    {
        if (isVisible)
        {
            ShowUI();
        }
        else
        {
            HideUI();
        }
    }

    private void DisableOnStart()
    {
        foreach (var textObject in ItemInfoText)
        {
            textObject.enabled = false;
        }

        foreach (var window in ItemInfoWindows)
        {
            window.enabled = false;
        }

        isVisible = false;
    }

    private void ShowUI()
    {
        StopAllCoroutines();

        foreach (var textObject in ItemInfoText)
        {
            StartCoroutine(ShowText(textObject));
        }
        
        foreach (var window in ItemInfoWindows)
        {
            StartCoroutine(ShowWindow(window));
        }

        isVisible = true;
    }

    private void HideUI()
    {
        StopAllCoroutines();

        foreach (var textObject in ItemInfoText)
        {
            StartCoroutine(HideText(textObject));
        }

        foreach (var window in ItemInfoWindows)
        {
            StartCoroutine(HideWindow(window));
        }

        isVisible = false;
    }

    private IEnumerator HideText(TextMeshProUGUI text)
    {
        while (text.color.a > 0)
        {
            text.color = new Color(
                text.color.r,
                text.color.g,
                text.color.b,
                text.color.a - dissapearingSpeed
                );

            yield return new WaitForFixedUpdate();
        }

        yield return text.color = new Color(
                text.color.r,
                text.color.g,
                text.color.b,
                0f
                );

        text.enabled = false;
    }

    private IEnumerator HideWindow(Image image)
    {
        while (image.color.a > 0)
        {
            image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
                image.color.a - dissapearingSpeed
                );

            yield return new WaitForFixedUpdate();
        }

        yield return image.color = new Color(
               image.color.r,
               image.color.g,
               image.color.b,
               0f
               );

        image.enabled = false;
    }

    private IEnumerator ShowText(TextMeshProUGUI text)
    {
        text.enabled = true;
        while (text.color.a < 1)
        {
            text.color = new Color(
                text.color.r,
                text.color.g,
                text.color.b,
                text.color.a + appearingSpeed
                );

            yield return new WaitForFixedUpdate();
        }

        yield return text.color = new Color(
            text.color.r,
            text.color.g,
            text.color.b,
            1f
            );
    }

    private IEnumerator ShowWindow(Image image)
    {
        image.enabled = true;
        while (image.color.a < 1)
        {
            image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
                image.color.a + appearingSpeed
                );

            yield return new WaitForFixedUpdate();
        }

        yield return image.color = new Color(
                image.color.r,
                image.color.g,
                image.color.b,
                1f
                );
    }
}
