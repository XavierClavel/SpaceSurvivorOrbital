using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizedString
{
    public Dictionary<string, string> dictString = new Dictionary<string, string>();
    public string getText()
    {
        return dictString[LocalizationManager.getLanguage()];
    }
}

public class LocalizedStringBuilder : DataBuilder<LocalizedString>
{

    protected override LocalizedString BuildData(List<string> s)
    {

        LocalizedString value = new LocalizedString();
        string string_EN = getValue(Vault.key.localization.EN, string.Empty);
        string string_FR = getValue(Vault.key.localization.FR, string.Empty);

        RemoveQuotationMarks(ref string_EN);
        RemoveQuotationMarks(ref string_FR);

        value.dictString[Vault.key.localization.EN] = string_EN;
        value.dictString[Vault.key.localization.FR] = string_FR;

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

        string string_EN = "";
        string string_FR = "";
        SetValue(ref string_EN, Vault.key.localization.EN);
        SetValue(ref string_FR, Vault.key.localization.FR);

        RemoveQuotationMarks(ref string_EN);
        RemoveQuotationMarks(ref string_FR);

        value.dictString[Vault.key.localization.EN] = string_EN;
        value.dictString[Vault.key.localization.FR] = string_FR;

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
