using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //Exposed
    private RectTransform upgradesUI;
    private RectTransform planetsUI;
    [SerializeField] UpgradesDisplayManager upgradesManager;
    [SerializeField] PlanetSelectionManager planetManager;
    
    //Static
    private static UIManager instance;
    private static float canvasHeight;

#region MonoBehaviourEvents
    private void Start()
    {
        instance = this;
        
        Canvas.ForceUpdateCanvases(); 
        canvasHeight = GetComponent<Canvas>().GetComponent<RectTransform>().rect.height;
        
        upgradesUI = upgradesManager.getUITransform();
        planetsUI = planetManager.getUITransform();
        HideUpgradesUI();
        HidePlanetsUI();
        upgradesManager.Setup();
        planetManager.Setup();
    }
#endregion

#region API

    public static void HideUpgradesUI()
    {
        instance.upgradesUI.anchoredPosition += canvasHeight * Vector2.down;
    }

    public static void HidePlanetsUI()
    {
        instance.planetsUI.anchoredPosition += canvasHeight * Vector2.down;
    }

    public static void DisplayUpgradesUI()
    {
        instance.upgradesUI.DOAnchorPosY(0f, 1f).SetEase(Ease.InOutQuint);
    }

    public void SwitchToPlanetSelection()
    {
        instance.upgradesManager.onPanelUnfocus();
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

    public static float posAboveCamera => canvasHeight;
    public static float posBelowCamera => -canvasHeight;

    #endregion

}
