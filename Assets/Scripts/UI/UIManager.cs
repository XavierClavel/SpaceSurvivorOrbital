using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] RectTransform upgradesUI;
    [SerializeField] RectTransform planetsUI;

    [SerializeField] PanelSelector upgradesManager;
    [SerializeField] PlanetSelectionManager planetManager;

    private static UIManager instance;

    private void Start()
    {
        instance = this;
        HideUpgradesUI();
        HidePlanetsUI();
        upgradesManager.Setup();
        planetManager.Setup();
    }

    public static void HideUpgradesUI()
    {
        instance.upgradesUI.anchoredPosition += Camera.main.scaledPixelHeight * Vector2.down;
    }

    public static void HidePlanetsUI()
    {
        instance.planetsUI.anchoredPosition += Camera.main.scaledPixelHeight * Vector2.down;
    }

    public static void DisplayUpgradesUI()
    {
        Debug.Log(">:)");
        instance.upgradesUI.DOAnchorPosY(0f, 1f).SetEase(Ease.InOutQuint);
    }

    public void SwitchToPlanetSelection()
    {
        instance.upgradesUI.DOAnchorPosY(Camera.main.scaledPixelHeight, 1f).SetEase(Ease.InOutQuint);
        instance.planetsUI.DOAnchorPosY(0f, 1f).SetEase(Ease.InOutQuint);
    }

    public void SwitchToUpgradesSelection()
    {
        instance.planetsUI.DOAnchorPosY(-Camera.main.scaledPixelHeight, 1f).SetEase(Ease.InOutQuint);
        instance.upgradesUI.DOAnchorPosY(0f, 1f).SetEase(Ease.InOutQuint);
    }
}
