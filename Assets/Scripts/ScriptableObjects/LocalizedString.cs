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
        }

        return "error";
    }

    public LocalizedString(List<string> s)
    {
        Debug.Log(s.Count);
        if (s.Count != 3) throw new System.ArgumentOutOfRangeException();

        string_EN = s[1];
        string_FR = s[2];

        CsvParser.dictLocalization.Add(s[0], this);
    }

}
