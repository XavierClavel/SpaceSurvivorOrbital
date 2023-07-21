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
        if (s == null || s.Count != columnTitles.Count) return;

        SetDictionary(columnTitles, s);

        SetValue(ref string_EN, Vault.key.localization.EN);
        SetValue(ref string_FR, Vault.key.localization.FR);

        RemoveQuotationMarks(ref string_EN);
        RemoveQuotationMarks(ref string_FR);

        string key = s[0].Trim();

        string dictKey = key == "" ? prevKey + Vault.key.ButtonDescription : key + Vault.key.ButtonTitle;
        DataManager.dictLocalization.Add(dictKey, this);

        prevKey = key;
    }

    void RemoveQuotationMarks(ref string input)
    {
        if (input == null || input.Length < 2) return;

        if (input.First() == '\"') input = input.RemoveFirst();
        if (input.Last() == '\"') input = input.RemoveLast();
    }


}
