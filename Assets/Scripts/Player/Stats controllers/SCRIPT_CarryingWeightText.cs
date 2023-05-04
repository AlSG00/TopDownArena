using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SCRIPT_CarryingWeightText : MonoBehaviour
{
    public TextMeshProUGUI carryingWeightInfo;
    public TextMeshProUGUI carryingWeightInventory;

    public Image backgroundImage;

    public float uiDissapearingDelay = 3f;
    public float uiAppearingSpeed = 1f;
    public float uiDissapearingSpeed = 1f;

    public bool isVisible = false;

    private void Awake()
    {
        carryingWeightInfo.text = "";
    }

    public void SetWeightText(float currentWeight, float maxWeight)
    {
        carryingWeightInfo.text = ($"{currentWeight}/{maxWeight}");
        carryingWeightInventory.text = carryingWeightInfo.text;
    }

    public void ShowUI()
    {
        StopAllCoroutines();
        StartCoroutine(ShowText());
        StartCoroutine(ShowBackground());
    }

    public void HideUI()
    {
        StopAllCoroutines();
        StartCoroutine(HideText());
        StartCoroutine(HideBackground());
    }

    private IEnumerator HideText()
    {
        while (carryingWeightInfo.color.a > 0 || backgroundImage.color.a > 0)
        {
            yield return carryingWeightInfo.color = new Color(
                carryingWeightInfo.color.r,
                carryingWeightInfo.color.g,
                carryingWeightInfo.color.b,
                carryingWeightInfo.color.a - uiDissapearingSpeed
                );
        }

        yield return carryingWeightInfo.color = new Color(
                carryingWeightInfo.color.r,
                carryingWeightInfo.color.g,
                carryingWeightInfo.color.b,
                0f
                );

        isVisible = false;
    }

    private IEnumerator HideBackground()
    {
        while (carryingWeightInfo.color.a > 0 || backgroundImage.color.a > 0)
        {
            yield return backgroundImage.color = new Color(
                backgroundImage.color.r,
                backgroundImage.color.g,
                backgroundImage.color.b,
                backgroundImage.color.a - uiDissapearingSpeed
                );
        }

        yield return backgroundImage.color = new Color(
               backgroundImage.color.r,
               backgroundImage.color.g,
               backgroundImage.color.b,
               0f
               );
    }

    private IEnumerator ShowText()
    {
        while (carryingWeightInfo.color.a < 1 || backgroundImage.color.a < 1)
        {
            yield return carryingWeightInfo.color = new Color(
                carryingWeightInfo.color.r,
                carryingWeightInfo.color.g,
                carryingWeightInfo.color.b,
                carryingWeightInfo.color.a + uiAppearingSpeed
                );
        }

        yield return carryingWeightInfo.color = new Color(
                carryingWeightInfo.color.r,
                carryingWeightInfo.color.g,
                carryingWeightInfo.color.b,
                1f
                );

        isVisible = true;
    }

    private IEnumerator ShowBackground()
    {
        while (carryingWeightInfo.color.a < 1 || backgroundImage.color.a < 1)
        {
            yield return backgroundImage.color = new Color(
                backgroundImage.color.r,
                backgroundImage.color.g,
                backgroundImage.color.b,
                backgroundImage.color.a + uiAppearingSpeed
                );
        }

        yield return backgroundImage.color = new Color(
                backgroundImage.color.r,
                backgroundImage.color.g,
                backgroundImage.color.b,
                1f
                );
    }
}
