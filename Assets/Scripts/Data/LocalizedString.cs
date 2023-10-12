using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum lang
{
    fr,
    en
}

public class LocalizedString
{
    public string string_FR;
    public string string_EN;
    public static lang selectedLang = lang.en;
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
}

public class LocalizedStringBuilder : DataBuilder<LocalizedString>
{

    protected override LocalizedString BuildData(List<string> s)
    {

        LocalizedString value = new LocalizedString();

        SetValue(ref value.string_EN, Vault.key.localization.EN);
        SetValue(ref value.string_FR, Vault.key.localization.FR);

        RemoveQuotationMarks(ref value.string_EN);
        RemoveQuotationMarks(ref value.string_FR);

        return value;
    }

    void RemoveQuotationMarks(ref string input)
    {
        if (input == null || input.Length < 2) return;

        if (input.First() == '\"') input = input.RemoveFirst();
        if (input.Last() == '\"') input = input.RemoveLast();
    }


}

public class DualLocalizedStringBuilder : DataBuilder<LocalizedString>
{

    private string prevKey = "A";

    protected override LocalizedString BuildData(List<string> s)
    {

        LocalizedString value = new LocalizedString();

        SetValue(ref value.string_EN, Vault.key.localization.EN);
        SetValue(ref value.string_FR, Vault.key.localization.FR);

        RemoveQuotationMarks(ref value.string_EN);
        RemoveQuotationMarks(ref value.string_FR);

        return value;
    }

    protected override string getKey(List<string> s)
    {
        if (s == null || s.Count != columnTitles.Count) return null;

        SetDictionary(columnTitles, s);

        string key = "";
        SetValue(ref key, Vault.key.Key);

        if (key == "" && prevKey == "") return null;
        string dictKey = key == "" ? prevKey + Vault.key.ButtonDescription : key + Vault.key.ButtonTitle;
        prevKey = key;

        return dictKey;
    }

    void RemoveQuotationMarks(ref string input)
    {
        if (input == null || input.Length < 2) return;

        if (input.First() == '\"') input = input.RemoveFirst();
        if (input.Last() == '\"') input = input.RemoveLast();
    }


}
