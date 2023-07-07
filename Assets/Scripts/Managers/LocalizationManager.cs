using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LocalizationManager : MonoBehaviour
{
    public static List<StringLocalizer> stringLocalizers = new List<StringLocalizer>();
    public static Dictionary<TextMeshProUGUI, LocalizedString> dictDisplayToLocalizedString = new Dictionary<TextMeshProUGUI, LocalizedString>();
    [SerializeField] lang selectedLang;

    private void Start()
    {
        foreach (StringLocalizer stringLocalizer in stringLocalizers) stringLocalizer.Initialize();
        stringLocalizers = new List<StringLocalizer>();
        LocalizedString.selectedLang = selectedLang;
        UpdateLocalization();
    }
    public static void UpdateLocalization()
    {
        foreach (TextMeshProUGUI textDisplay in dictDisplayToLocalizedString.Keys)
        {
            UpdateFieldDisplay(textDisplay);
        }
    }

    static void UpdateFieldDisplay(TextMeshProUGUI textDisplay)
    {
        textDisplay.SetText(dictDisplayToLocalizedString[textDisplay].getText());
    }

    public static void LocalizeTextField(string key, TextMeshProUGUI field)
    {
        if (!DataManager.dictLocalization.ContainsKey(key))
        {
            Debug.Log($"\"{key}\" is not localized yet.");
            return;
        }
        Debug.Log($"\"{key}\" is localized.");
        LocalizedString localizedString = DataManager.dictLocalization[key];
        dictDisplayToLocalizedString.Add(field, localizedString);
        UpdateFieldDisplay(field);

    }

    private void OnDestroy()
    {
        dictDisplayToLocalizedString = new Dictionary<TextMeshProUGUI, LocalizedString>();
    }
}
