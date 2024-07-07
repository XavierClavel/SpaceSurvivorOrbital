using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    //Exposed
    private RectTransform upgradesUI;
    private RectTransform planetsUI;
    [SerializeField] UpgradesDisplayManager upgradesManager;
    [SerializeField] PlanetSelectionManager planetManager;
    [SerializeField] private ButtonsPanelManager buttonsPanelManager;
    [SerializeField] private RbLbNavigator rbLbNavigator;
    private InputMaster controls;
    private string currentPanel = Vault.panel.ship.UpgradeSelection;
    private GameObject lastUpgradesSelected = null;
    
    //Static
    private static UIManager instance;
    private static float canvasHeight;

#region MonoBehaviourEvents
    private void Start()
    {
        instance = this;
        Canvas.ForceUpdateCanvases(); 
        canvasHeight = GetComponent<Canvas>().GetComponent<RectTransform>().rect.height;

        if (upgradesManager == null) return;
        
        upgradesUI = upgradesManager.getUITransform();
        planetsUI = planetManager.getUITransform();
        HideUpgradesUI();
        HidePlanetsUI();
        upgradesManager.Setup();
        planetManager.Setup();
        
        buttonsPanelManager.setActive(Vault.panel.ship.UpgradeSelection);

        controls = new InputMaster();
        controls.UI.ContextAction.performed += ctx => onContextAction();
        controls.UI.Enable();
    }

    private void onContextAction()
    {
        Debug.Log("context action");
        if (currentPanel == Vault.panel.ship.UpgradeSelection)
        {
            SwitchToPlanetSelection();
        } else SwitchToUpgradesSelection();
    }
    
    private void OnDestroy()
    {
        controls.Disable();
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
        currentPanel = Vault.panel.ship.PlanetSelection;
        rbLbNavigator.Lock();
        SoundManager.PlaySfx("Power_Switch");
        buttonsPanelManager.setActive(Vault.panel.ship.PlanetSelection);
        instance.upgradesManager.onPanelUnfocus();
        instance.upgradesUI.DOAnchorPosY(posAboveCamera, 1f).SetEase(Ease.InOutQuint);
        instance.planetsUI.DOAnchorPosY(0f, 1f).SetEase(Ease.InOutQuint);
        lastUpgradesSelected = EventSystem.current.currentSelectedGameObject;
        InputManager.setSelectedObject(planetManager.firstSelectedButton);
    }

    public void SwitchToUpgradesSelection()
    {
        currentPanel = Vault.panel.ship.UpgradeSelection;
        rbLbNavigator.Unlock();
        SoundManager.PlaySfx("Power_Switch");
        buttonsPanelManager.setActive(Vault.panel.ship.UpgradeSelection);
        instance.planetsUI.DOAnchorPosY(posBelowCamera, 1f).SetEase(Ease.InOutQuint);
        instance.upgradesUI.DOAnchorPosY(0f, 1f).SetEase(Ease.InOutQuint);
        InputManager.setSelectedObject(lastUpgradesSelected);
    }
    
    public void LoadShip()
    {
        SceneTransitionManager.TransitionToScene(gameScene.ship);
    }
    
#endregion

#region staticAPI

    public static float posAboveCamera => canvasHeight;
    public static float posBelowCamera => -canvasHeight;

    #endregion

}
