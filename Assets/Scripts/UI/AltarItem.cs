
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
    private string key;
    
    // Start is called before the first frame update
    void Start()
    {
        List<PowerHandler> powersRemaining = ScriptableObjectManager.dictKeyToPowerHandler.Values.ToList().Difference(PlayerManager.powers);
        PowerHandler power = powersRemaining.getRandom();
        Setup(power.getKey());
    }

    void Setup(string key)
    {
        this.key = key;
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
