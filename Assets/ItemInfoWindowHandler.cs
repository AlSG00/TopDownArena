using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ItemInfoWindowHandler : MonoBehaviour
{
    [SerializeField] private Image[] ItemInfoWindows;
    [SerializeField] private TextMeshProUGUI[] ItemInfoText;

    [SerializeField] private float appearingSpeed = 1f;
    [SerializeField] private float dissapearingSpeed = 1f;

    private bool _isVisible = false;

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

    public void SetVisibility(bool isVisible, SCRIPT_InventoryItem item)
    {
        if (isVisible == _isVisible)
        {
            return;
        }

        string[] parsedUiInfo = item.itemData.uiInfo.text.Split('#');
        ItemInfoText[0].text = parsedUiInfo[0];
        ItemInfoText[1].text = parsedUiInfo[1];
        ItemInfoText[2].text = parsedUiInfo[2];

        PlaceWindow(item);
        
        if (isVisible)
        {
            ShowUI();
        }
        else
        {
            HideUI();
        }
    }

    private void PlaceWindow(SCRIPT_InventoryItem item)
    {
        if (item.isRotated)
        {
            ItemInfoWindows[0].rectTransform.position = new Vector2(
            item.itemRectTransform.position.x + item.itemRectTransform.sizeDelta.y / 2,
            item.itemRectTransform.position.y + item.itemRectTransform.sizeDelta.x / 2
            );
        }
        else
        {
            ItemInfoWindows[0].rectTransform.position = new Vector2(
            item.itemRectTransform.position.x + item.itemRectTransform.sizeDelta.x / 2,
            item.itemRectTransform.position.y + item.itemRectTransform.sizeDelta.y / 2
            );
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

        _isVisible = false;
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

        _isVisible = true;
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

        _isVisible = false;
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
