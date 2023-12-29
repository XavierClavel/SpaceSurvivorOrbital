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
    private RectTransform equipmentSelectorUI;
    [SerializeField] TitleScreen titleScreen;
    [SerializeField] GameObject dataSelector;
    [SerializeField] GameObject equipmentSelector;
    [SerializeField] private Canvas canvas;
    private static float canvasWidth;
    
    //Static
    private static TitleScreenManager instance;

    #region MonoBehaviourEvents
    private void Start()
    {
        instance = this;
        
        Canvas.ForceUpdateCanvases(); 
        canvasWidth = canvas.GetComponent<RectTransform>().rect.width;
        
        titleScreenUI = titleScreen.getUITransform();
        dataSelectorUI = dataSelector.GetComponent<RectTransform>();
        equipmentSelectorUI = equipmentSelector.GetComponent<RectTransform>();
        HideDataSelector();
        HideEquipmentSelector();
        titleScreen.Setup();
        
    }
    #endregion
    
    #region API

    public static void HideTitleScreen()
    {
        instance.titleScreenUI.anchoredPosition += canvasWidth * Vector2.left;
    }

    public static void HideDataSelector()
    {
        Debug.Log(canvasWidth);
        instance.dataSelectorUI.anchoredPosition += canvasWidth * Vector2.right;
    }

    public static void HideEquipmentSelector()
    {
        Debug.Log(canvasWidth);
        instance.equipmentSelectorUI.anchoredPosition += canvasWidth * Vector2.right;
    }

    public static void DisplayUpgradesUI()
    {
        instance.titleScreenUI.DOAnchorPosX(0f, 1f).SetEase(Ease.InOutQuint);
    }

    public void SwitchToDataSelection()
    {
        SoundManager.PlaySfx(transform, key: "Power_Switch");
        instance.titleScreenUI.DOAnchorPosX(posRightCamera, 1f).SetEase(Ease.InOutQuint);
        instance.dataSelectorUI.DOAnchorPosX(0f, 1f).SetEase(Ease.InOutQuint);
    }

    public void SwitchToDataSelectionFromEquipment()
    {
        SoundManager.PlaySfx(transform, key: "Power_Switch");
        instance.equipmentSelectorUI.DOAnchorPosX(posLeftCamera, 1f).SetEase(Ease.InOutQuint);
        instance.dataSelectorUI.DOAnchorPosX(0, 1f).SetEase(Ease.InOutQuint);
    }

    public void SwitchToTitleScreen()
    {
        SoundManager.PlaySfx(transform, key: "Power_Switch");
        instance.dataSelectorUI.DOAnchorPosX(posLeftCamera, 1f).SetEase(Ease.InOutQuint);
        instance.titleScreenUI.DOAnchorPosX(0f, 1f).SetEase(Ease.InOutQuint);
    }

    public void SwitchToEquipmentScreen()
    {
        SoundManager.PlaySfx(transform, key: "Power_Switch");
        instance.dataSelectorUI.DOAnchorPosX(posRightCamera, 1f).SetEase(Ease.InOutQuint);
        instance.equipmentSelectorUI.DOAnchorPosX(0f, 1f).SetEase(Ease.InOutQuint);
    }

    #endregion

    #region staticAPI

    public static float posLeftCamera => canvasWidth;
    public static float posRightCamera => -canvasWidth;

    #endregion

}

