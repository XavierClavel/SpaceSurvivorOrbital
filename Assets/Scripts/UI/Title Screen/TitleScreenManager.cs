using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;

public class TitleScreenManager : MonoBehaviour
{
    //Exposed
    private RectTransform titleScreenUI;
    private RectTransform dataSelectorUI;
    [SerializeField] TitleScreen titleScreen;
    [SerializeField] DataSelector dataSelector;
    
    //Static
    private static TitleScreenManager instance;

    #region MonoBehaviourEvents
    private void Start()
    {
        instance = this;
        titleScreenUI = titleScreen.getUITransform();
        dataSelectorUI = dataSelector.getUITransform();
        HideDataSelector();
        titleScreen.Setup();
    }
    #endregion
    
    #region API

    public static void HideTitleScreen()
    {
        instance.titleScreenUI.anchoredPosition += Camera.main.scaledPixelHeight * Vector2.down;
    }

    public static void HideDataSelector()
    {
        instance.dataSelectorUI.anchoredPosition += Camera.main.scaledPixelHeight * Vector2.down;
    }

    public static void DisplayUpgradesUI()
    {
        instance.titleScreenUI.DOAnchorPosY(0f, 1f).SetEase(Ease.InOutQuint);
    }

    public void SwitchToDataSelection()
    {
        instance.titleScreenUI.DOAnchorPosY(posAboveCamera, 1f).SetEase(Ease.InOutQuint);
        instance.dataSelectorUI.DOAnchorPosY(0f, 1f).SetEase(Ease.InOutQuint);
    }

    public void SwitchToTitleScreen()
    {
        instance.dataSelectorUI.DOAnchorPosY(posBelowCamera, 1f).SetEase(Ease.InOutQuint);
        instance.titleScreenUI.DOAnchorPosY(0f, 1f).SetEase(Ease.InOutQuint);
    }
    
    #endregion

    #region staticAPI

    public static float posAboveCamera => Camera.main.scaledPixelHeight;
    public static float posBelowCamera => -Camera.main.scaledPixelHeight;

    #endregion

}

