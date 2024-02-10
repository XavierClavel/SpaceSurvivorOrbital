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
        UpdateKey();
    }

    private void OnEnable()
    {
        if (!DataManager.isInitialized()) return;
        Setup();
    }

    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        if (textDisplay == null)
        {
            textDisplay = GetComponent<TextMeshProUGUI>();
            LocalizationManager.registerStringLocalizer(this);
        }
        if (!key.IsNullOrEmpty()) UpdateKey();
    }

    public void UpdateText()
    {
        textDisplay.SetText(localizedString.getText());
    }

    private void UpdateKey()
    {
        if (!DataManager.dictLocalization.ContainsKey(key))
        {
            throw new System.ArgumentException($"{gameObject.name} is trying to call the \"{key}\" key which does not exist.");
        }
        localizedString = DataManager.dictLocalization[key];
        UpdateText();
    }
    

}
