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
    public static Dictionary<int, int> mapper;
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
        if (s.Count != 3) throw new System.ArgumentOutOfRangeException();

        string_EN = s[mapper[1]];
        string_FR = s[mapper[2]];

        Debug.Log(s[0]);

        CsvParser.dictLocalization.Add(s[0], this);
    }

    public static void Initializer(List<string> values)
    {
        mapper = new Dictionary<int, int>();
        for (int i = 1; i < values.Count; i++)
        {
            mapper[i] = columnToKey(values[i]);
        }
    }

    public static int columnToKey(string columnName)
    {
        columnName = columnName.Trim();
        switch (columnName)
        {
            case "EN":
                return 1;

            case "FR":
                return 2;

            default:
                throw new System.ArgumentException($"\"{columnName}\" column name is not recognized");
        }
    }

}
