using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpgradeDisplay : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI upgradeTitleDisplay;
    [SerializeField] TextMeshProUGUI upgradeTextDisplay;
    static UpgradeDisplay instance;


    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        upgradeTextDisplay.SetText("");
        upgradeTitleDisplay.SetText("");
    }

    public static void DisplayUpgrade(string key)
    {
        LocalizationManager.LocalizeTextField(key + Vault.key.ButtonTitle, instance.upgradeTitleDisplay);
        LocalizationManager.LocalizeTextField(key + Vault.key.ButtonDescription, instance.upgradeTextDisplay);
    }
}
