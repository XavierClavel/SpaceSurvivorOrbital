using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //Exposed
    [SerializeField] RectTransform upgradesUI;
    [SerializeField] RectTransform planetsUI;
    [SerializeField] UpgradesDisplayManager upgradesManager;
    [SerializeField] PlanetSelectionManager planetManager;
    
    //Static
    private static UIManager instance;

#region MonoBehaviourEvents
    private void Start()
    {
        instance = this;
        HideUpgradesUI();
        HidePlanetsUI();
        upgradesManager.Setup();
        planetManager.Setup();
    }
#endregion

#region API

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
        instance.upgradesManager.DeactivatePanelsNotDisplayed();
        instance.upgradesUI.DOAnchorPosY(posAboveCamera, 1f).SetEase(Ease.InOutQuint);
        instance.planetsUI.DOAnchorPosY(0f, 1f).SetEase(Ease.InOutQuint);
    }

    public void SwitchToUpgradesSelection()
    {
        instance.planetsUI.DOAnchorPosY(posBelowCamera, 1f).SetEase(Ease.InOutQuint);
        instance.upgradesUI.DOAnchorPosY(0f, 1f).SetEase(Ease.InOutQuint);
    }
    
#endregion

#region staticAPI

    public static float posAboveCamera => Camera.main.scaledPixelHeight;
    public static float posBelowCamera => -Camera.main.scaledPixelHeight;

    #endregion

}
