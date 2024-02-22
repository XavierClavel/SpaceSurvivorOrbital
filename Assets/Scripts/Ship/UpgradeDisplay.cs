using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UpgradeDisplay : MonoBehaviour
{
    [SerializeField] StringLocalizer upgradeTitleDisplay;
    [SerializeField] StringLocalizer upgradeTextDisplay;
    [SerializeField] private Image upgradeIconDisplay;
    [SerializeField] Button buyButton;
    static UpgradeDisplay instance;
    static UnityAction buyAction;
    static GameObject selectedButton;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        buyAction = delegate { };
    }

    public static void DisplayUpgrade(string key, string target)
    {
        instance.buyButton.gameObject.SetActive(
            NodeManager.dictKeyToButton[key]. status is skillButtonStatus.unlocked
                );
        if (instance.upgradeTitleDisplay != null) instance.upgradeTitleDisplay.setKey(key + Vault.key.ButtonTitle);
        if (instance.upgradeTextDisplay != null) instance.upgradeTextDisplay.setKey(key + Vault.key.ButtonDescription);
        if (instance.upgradeIconDisplay != null)
            instance.upgradeIconDisplay.sprite = ScriptableObjectManager.getIcon(target);
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

    public static void Buy()
    {
        buyAction.Invoke();
    }
}
