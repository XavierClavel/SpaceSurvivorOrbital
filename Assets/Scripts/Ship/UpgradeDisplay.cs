using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI upgradeTitleDisplay;
    [SerializeField] TextMeshProUGUI upgradeTextDisplay;
    [SerializeField] TextMeshProUGUI upgradeCostDisplay;
    [SerializeField] TextMeshProUGUI upgradeChargeDisplay;
    [SerializeField] Button buyButton;
    static UpgradeDisplay instance;
    static UnityAction buyAction;
    static GameObject selectedButton;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        buyAction = delegate { };

        if (instance.upgradeTitleDisplay != null) upgradeTitleDisplay.SetText("");
        if (instance.upgradeTextDisplay != null) upgradeTextDisplay.SetText("");
        if (instance.upgradeTextDisplay != null) upgradeCostDisplay.SetText("");
        if (instance.upgradeTextDisplay != null) upgradeChargeDisplay.SetText("");
    }

    public static void DisplayUpgrade(string key)
    {
        if (instance.upgradeTitleDisplay != null) LocalizationManager.LocalizeTextField(key + Vault.key.ButtonTitle, instance.upgradeTitleDisplay);
        if (instance.upgradeTextDisplay != null) LocalizationManager.LocalizeTextField(key + Vault.key.ButtonDescription, instance.upgradeTextDisplay);
        if (instance.upgradeCostDisplay != null) LocalizationManager.LocalizeTextField(key + Vault.key.ButtonCost, instance.upgradeCostDisplay);
        if (instance.upgradeChargeDisplay != null) LocalizationManager.LocalizeTextField(key + Vault.key.ButtonCharge, instance.upgradeChargeDisplay);
    }

    public void OnClick()
    {
        buyAction.Invoke();
        if (selectedButton != null) EventSystem.current.SetSelectedGameObject(selectedButton);
    }
    
    public void SetupAction(UnityAction action)
    {
        buyAction = action;
    }

    public static void SetupBuyButton(UnityAction action, GameObject selected)
    {
        buyAction = action;
        selectedButton = selected;
    }
}
