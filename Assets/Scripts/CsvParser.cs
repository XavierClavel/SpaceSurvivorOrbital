using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CsvParser : MonoBehaviour
{
    string pathToCsv = "";
    [SerializeField] TextAsset characterData;
    [SerializeField] TextAsset localizationData;
    delegate void Formatter(List<string> s);
    public static Dictionary<string, CharacterData> dictCharacters = new Dictionary<string, CharacterData>();
    public static Dictionary<string, LocalizedString> dictLocalization = new Dictionary<string, LocalizedString>();

    private void Awake()
    {
        if (dictCharacters.Count != 0) return;
        loadText(characterData, x => new CharacterData(x));
        loadText(localizationData, x => new LocalizedString(x), x => LocalizedString.Initializer(x));

        PlayerManager.setCharacter(dictCharacters["Pistolero"]);
    }

    void loadText(TextAsset csv, Formatter formatter, Formatter initializer = null)
    {

        List<string> stringArray = csv.text.Split('\n').ToList();
        if (initializer != null) initializer(stringArray[0].Split(';').ToList());
        stringArray.RemoveAt(0);

        foreach (string s in stringArray)
        {
            formatter(s.Split(';').ToList());
        }
    }

}

