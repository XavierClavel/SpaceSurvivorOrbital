using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum lang
{
    fr,
    en
}

public class LocalizedString : TemplateData
{
    public string string_FR;
    public string string_EN;

    public static lang selectedLang = lang.en;

    static List<string> firstLineValue = new List<string> {
        "Key",
        "EN",
        "FR"
    };

    public string getText()
    {
        switch (selectedLang)
        {
            case lang.fr:
                return string_FR;

            case lang.en:
                return string_EN;
        }

        return "error";
    }

    public LocalizedString(List<string> s)
    {
        if (s == null || s.Count != firstLineValue.Count) return;

        Helpers.printList(s);
        Helpers.SetMappedValue(s, mapper, 1, out string_EN);
        Helpers.SetMappedValue(s, mapper, 2, out string_FR);

        DataManager.dictLocalization.Add(s[0], this);
    }

    public static void Initialize(List<string> s)
    {
        Helpers.printList(s);
        InitializeMapping(s, firstLineValue);
    }

}
