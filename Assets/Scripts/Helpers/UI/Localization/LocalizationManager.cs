using System.Collections;
using System.Collections.Generic;
using MyBox;
using UnityEngine;
using TMPro;

public static class LocalizationManager
{
    public static HashSet<StringLocalizer> stringLocalizers = new HashSet<StringLocalizer>();
    private static string selectedLanguage = "FR";

    public static string getLanguage() => selectedLanguage;

    public static void registerStringLocalizer(StringLocalizer s) => stringLocalizers.Add(s);
    public static void unregisterStringLocalizer(StringLocalizer s) => stringLocalizers.Remove(s);

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

}
