using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using TMPro;

public class LocalizationManager : MonoBehaviour
{
    public static HashSet<StringLocalizer> stringLocalizers = new HashSet<StringLocalizer>();
    private static string selectedLanguage = "FR";

    public static string getLanguage() => selectedLanguage;

    public static void registerStringLocalizer(StringLocalizer s) => stringLocalizers.Add(s);

    public void getLocalizedString(string key)
    {
        DataManager.dictLocalization[key].getText();
    }

    public static void setLanguage(string value)
    {
        selectedLanguage = value;
        UpdateLocalization();
    }
    
    public static void UpdateLocalization()
    {
        stringLocalizers.ForEach(it => it.Setup());
    }
    
    /**
     * Adds localized text to a text field using the given key
     */
    public static void LocalizeTextField(string key, TextMeshProUGUI field)
    {
        Debug.Log(key);
        if (key.IsNullOrEmpty()) return;
        field.gameObject.AddComponent<StringLocalizer>().setKey(key);
    }

    private void OnDestroy()
    {
        stringLocalizers = new HashSet<StringLocalizer>();
    }
}
