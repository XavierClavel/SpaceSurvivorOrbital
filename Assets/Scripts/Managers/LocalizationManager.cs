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
            textDisplay.SetText(dictDisplayToLocalizedString[textDisplay].getText());
        }
    }

    private void OnDestroy()
    {
        dictDisplayToLocalizedString = new Dictionary<TextMeshProUGUI, LocalizedString>();
    }
}
