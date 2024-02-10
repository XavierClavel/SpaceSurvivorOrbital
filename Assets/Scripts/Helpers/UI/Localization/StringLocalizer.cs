using System.Collections;
using System.Collections.Generic;
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
        OnEnable();
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
        if (key == null) return;
        if (textDisplay == null)
        {
            textDisplay = GetComponent<TextMeshProUGUI>();
            Initialize();
        }
        UpdateText();
    }

    public void UpdateText()
    {
        textDisplay.SetText(localizedString.getText());
    }


    public void Initialize()
    {
        if (!DataManager.dictLocalization.ContainsKey(key))
        {
            throw new System.ArgumentException($"{gameObject.name} is trying to call the \"{key}\" key which does not exist.");
        }
        localizedString = DataManager.dictLocalization[key];
        LocalizationManager.registerStringLocalizer(this);
    }






}
