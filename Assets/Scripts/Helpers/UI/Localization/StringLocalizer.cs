using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using TMPro;

public class StringLocalizer : MonoBehaviour
{
    [SerializeField] string key = null;
    private TextMeshProUGUI textDisplay = null;
    private LocalizedString localizedString;

    public void setKey(string key)
    {
        this.key = key;
        Setup();
    }

    private void OnEnable()
    {
        if (!DataManager.isInitialized()) return;
        Setup();
    }

    public void Setup()
    {
        if (textDisplay == null)
        {
            textDisplay = GetComponent<TextMeshProUGUI>();
            if (textDisplay == null)
            {
                Debug.LogError($"GameObject {gameObject.name} does not have TextMeshProUGUI component");
                return;
            }
            LocalizationManager.registerStringLocalizer(this);
        }
        if (!key.IsNullOrEmpty()) UpdateKey();
    }

    private void UpdateKey()
    {
        if (!DataManager.dictLocalization.ContainsKey(key))
        {
            throw new System.ArgumentException($"{gameObject.name} is trying to call the \"{key}\" key which does not exist.");
        }
        localizedString = DataManager.dictLocalization[key];
        textDisplay.SetText(localizedString.getText());
    }
    

}
