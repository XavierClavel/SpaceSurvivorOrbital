using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum lang
{
    fr,
    en
}

public class LocalizedString : EffectData
{
    public string string_FR;
    public string string_EN;

    public static lang selectedLang = lang.en;

    static List<string> columnTitles = new List<string>();
    static string prevKey = "";

    public static void Initialize(List<string> s)
    {
        columnTitles = InitializeColumnTitles(s);
    }

    public string getText()
    {
        switch (selectedLang)
        {
            case lang.fr:
                return string_FR;

            case lang.en:
                return string_EN;

            default:
                throw new System.ArgumentException($"failed to parse {selectedLang}");
        }

    }

    public LocalizedString(List<string> s)
    {
        if (s == null || s.Count != columnTitles.Count) return;

        SetDictionary(columnTitles, s);

        SetValue(ref string_EN, Vault.key.localization.EN);
        SetValue(ref string_FR, Vault.key.localization.FR);

        DataManager.dictLocalization.Add(s[0], this);
    }

    public LocalizedString(List<string> s, bool buttonLocalization)
    {
        Debug.Log(s.Count);
        if (s == null || s.Count != columnTitles.Count) return;

        SetDictionary(columnTitles, s);

        SetValue(ref string_EN, Vault.key.localization.EN);
        SetValue(ref string_FR, Vault.key.localization.FR);

        if (string_EN.First() == '\"') string_EN = string_EN.RemoveFirst();
        if (string_EN.Last() == '\"') string_EN = string_EN.RemoveLast();

        if (string_FR.First() == '\"') string_FR = string_FR.RemoveFirst();
        if (string_FR.Last() == '\"') string_FR = string_FR.RemoveLast();

        string key = s[0].Trim();

        string dictKey = key == "" ? prevKey + Vault.key.ButtonDescription : key + Vault.key.ButtonTitle;
        Debug.Log(dictKey);
        DataManager.dictLocalization.Add(dictKey, this);

        prevKey = key;
    }


}
