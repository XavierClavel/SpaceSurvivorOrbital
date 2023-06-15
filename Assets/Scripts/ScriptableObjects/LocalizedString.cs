using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum lang
{
    fr,
    en
}

[CreateAssetMenu(fileName = "LocalizedString", menuName = "Space Survivor 2D/LocalizedString", order = 0)]
public class LocalizedString : ScriptableObject
{
    [SerializeField] string string_FR;
    [SerializeField] string string_EN;

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

}
