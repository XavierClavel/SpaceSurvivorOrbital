using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TitleScreenManager : MonoBehaviour
{
    //Exposed
    private RectTransform titleScreenUI;
    private RectTransform dataSelectorUI;
    private RectTransform equipmentSelectorUI;
    private RectTransform bossSelectorUI;
    private RectTransform creditsUI;

    [SerializeField] TitleScreen titleScreen;
    [SerializeField] GameObject dataSelector;
    [SerializeField] GameObject equipmentSelector;
    [SerializeField] GameObject bossSelector;
    [SerializeField] GameObject credits;
    [SerializeField] private Canvas canvas;
    [SerializeField] private OptionsManager optionsManager;

    [SerializeField] private GameObject titleScreenFirstSelected;
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
        bossSelectorUI = bossSelector.GetComponent<RectTransform>();
        creditsUI = credits.GetComponent<RectTransform>();
        HideDataSelector();
        HideEquipmentSelector();
        HideBossSelector();
        HideCredits();
        titleScreen.Setup();
        
        optionsManager.LoadOptions();
        InputManager.setSelectedObject(titleScreenFirstSelected);
        RbLbNavigator.instance.Disable();
    }
    #endregion
    
    #region API

    public static void HideTitleScreen()
    {
        instance.titleScreenUI.anchoredPosition += canvasWidth * Vector2.left;
    }
    public static void HideCredits()
    {
        instance.creditsUI.anchoredPosition += canvasWidth * Vector2.right;
    }

    public static void HideDataSelector()
    {
        instance.dataSelectorUI.anchoredPosition += canvasWidth * Vector2.right;
    }

    public static void HideEquipmentSelector()
    {
        instance.equipmentSelectorUI.anchoredPosition += canvasWidth * Vector2.right;
    }
    public static void HideBossSelector()
    {
        instance.bossSelectorUI.anchoredPosition += canvasWidth * Vector2.right;
    }

    public static void DisplayUpgradesUI()
    {
        instance.titleScreenUI.DOAnchorPosX(0f, 1f).SetEase(Ease.InOutQuint);
    }

    public void SwitchToDataSelection()
    {
        Steamworks.SteamUserStats.SetAchievement(Vault.achievement.Test);
        SoundManager.PlaySfx("Power_Switch");
        instance.titleScreenUI.DOAnchorPosX(posRightCamera, 1f)
            .SetEase(Ease.InOutQuint);
        instance.dataSelectorUI.DOAnchorPosX(0f, 1f).SetEase(Ease.InOutQuint);
        InputManager.setSelectedObject(
            DataSelector.instance.dictKeyToButton[ScriptableObjectManager.dictKeyToCharacterHandler.Keys.ToList()[0]].gameObject);
        RbLbNavigator.instance.Enable();
    }
    public void SwitchToCredits()
    {
        SoundManager.PlaySfx("Power_Switch");
        instance.titleScreenUI.DOAnchorPosX(posRightCamera, 1f)
            .SetEase(Ease.InOutQuint);
        instance.creditsUI.DOAnchorPosX(0f, 1f).SetEase(Ease.InOutQuint);
        RbLbNavigator.instance.Enable();
    }

    public void SwitchToDataSelectionFromEquipment()
    {
        SoundManager.PlaySfx("Power_Switch");
        instance.equipmentSelectorUI.DOAnchorPosX(posLeftCamera, 1f).SetEase(Ease.InOutQuint);
        instance.dataSelectorUI.DOAnchorPosX(0, 1f).SetEase(Ease.InOutQuint);
        InputManager.setSelectedObject(
            DataSelector.instance.dictKeyToButton[ScriptableObjectManager.dictKeyToCharacterHandler.Keys.ToList()[0]].gameObject);
    }

    public void SwitchToTitleScreen()
    {
        SoundManager.PlaySfx("Power_Switch");
        instance.dataSelectorUI.DOAnchorPosX(posLeftCamera, 1f).SetEase(Ease.InOutQuint);
        instance.titleScreenUI.DOAnchorPosX(0f, 1f).SetEase(Ease.InOutQuint);
        InputManager.setSelectedObject(titleScreenFirstSelected);
        RbLbNavigator.instance.Disable();
    }
    public void SwitchToTitleScreenFromCredits()
    {
        SoundManager.PlaySfx("Power_Switch");
        instance.creditsUI.DOAnchorPosX(posLeftCamera, 1f).SetEase(Ease.InOutQuint);
        instance.titleScreenUI.DOAnchorPosX(0f, 1f).SetEase(Ease.InOutQuint);
        InputManager.setSelectedObject(titleScreenFirstSelected);
        RbLbNavigator.instance.Disable();
    }

    public void SwitchToEquipmentScreen()
    {
        SoundManager.PlaySfx("Power_Switch");
        instance.dataSelectorUI.DOAnchorPosX(posRightCamera, 1f).SetEase(Ease.InOutQuint);
        instance.equipmentSelectorUI.DOAnchorPosX(0f, 1f).SetEase(Ease.InOutQuint);
        InputManager.setSelectedObject(
            DataSelector.instance.dictKeyToButton[ScriptableObjectManager.dictKeyToEquipmentHandler.Keys.ToList()[0]].gameObject);
    }
    public void SwitchToBossScreen()
    {
        SoundManager.PlaySfx("Power_Switch");
        instance.equipmentSelectorUI.DOAnchorPosX(posRightCamera, 1f).SetEase(Ease.InOutQuint);
        instance.bossSelectorUI.DOAnchorPosX(0f, 1f).SetEase(Ease.InOutQuint);
        InputManager.setSelectedObject(
            DataSelector.instance.dictKeyToButton[ScriptableObjectManager.dictKeyToEquipmentHandler.Keys.ToList()[0]].gameObject);
    }

    public void SwitchToEquipmentFromBoss()
    {
        SoundManager.PlaySfx("Power_Switch");
        instance.bossSelectorUI.DOAnchorPosX(posLeftCamera, 1f).SetEase(Ease.InOutQuint);
        instance.equipmentSelectorUI.DOAnchorPosX(0, 1f).SetEase(Ease.InOutQuint);
        InputManager.setSelectedObject(
            DataSelector.instance.dictKeyToButton[ScriptableObjectManager.dictKeyToCharacterHandler.Keys.ToList()[0]].gameObject);
    }

    public void Quit()
    {
        Application.Quit();
    }

    #endregion

    #region staticAPI

    public static float posLeftCamera => canvasWidth;
    public static float posRightCamera => -canvasWidth;

    #endregion

}

