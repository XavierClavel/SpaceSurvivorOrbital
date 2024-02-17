
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AltarItem : MonoBehaviour
{
    [SerializeField] private StringLocalizer titleField;
    [SerializeField] private StringLocalizer descriptionField;

    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    public void Setup(string key)
    {
        titleField.setKey(key + Vault.key.ButtonTitle);
        descriptionField.setKey(key + Vault.key.ButtonDescription);
        icon.sprite = ScriptableObjectManager.dictKeyToPowerHandler[key].getIcon();
        setAction(delegate
        {
            AcquirePower(key);
            UpgradesDisplayManager.addNewPanel(key);
        });
    }

    public void setAction(UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }
    
    
    static void AcquirePower(string key)
    {
        PowerHandler powerHandler = ScriptableObjectManager.dictKeyToPowerHandler[key];
        PlayerManager.AcquirePower(powerHandler);
        powerHandler.Activate();
        ObjectManager.HideAltarUI();
        ObjectManager.altar.DepleteAltar();
    }

}
