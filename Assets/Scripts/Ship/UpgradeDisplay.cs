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
    [SerializeField] Button buyButton;
    static UpgradeDisplay instance;
    static UnityAction buyAction;
    static GameObject selectedButton;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        buyAction = delegate { };

        upgradeTextDisplay.SetText("");
        upgradeTitleDisplay.SetText("");
    }

    public static void DisplayUpgrade(string key)
    {
        LocalizationManager.LocalizeTextField(key + Vault.key.ButtonTitle, instance.upgradeTitleDisplay);
        LocalizationManager.LocalizeTextField(key + Vault.key.ButtonDescription, instance.upgradeTextDisplay);
    }

    public void OnClick()
    {
        buyAction.Invoke();
        EventSystem.current.SetSelectedGameObject(selectedButton);
    }

    public static void SetupBuyButton(UnityAction action, GameObject selected)
    {
        buyAction = action;
        selectedButton = selected;
    }
}
