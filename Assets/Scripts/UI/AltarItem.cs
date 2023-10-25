
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AltarItem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI titleField;

    [SerializeField] private TextMeshProUGUI descriptionField;

    [SerializeField] private Image icon;
    [SerializeField] private Button button;

    public void Setup(string key)
    {
        LocalizationManager.LocalizeTextField(key + Vault.key.ButtonTitle, titleField);
        LocalizationManager.LocalizeTextField(key + Vault.key.ButtonDescription, descriptionField);
        icon.sprite = ScriptableObjectManager.dictKeyToPowerHandler[key].getIcon();
        UnityAction action = delegate
        {
            AcquirePower(key);
        };
        button.onClick.AddListener(action);
    }
    
    
    static void AcquirePower(string key)
    {
        PowerHandler powerHandler = ScriptableObjectManager.dictKeyToPowerHandler[key];
        PlayerManager.AcquirePower(powerHandler);
        powerHandler.Activate();
        ObjectManager.HideAltarUI();
    }

}
