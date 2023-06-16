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
    delegate void Formatter(List<string> s);
    public static Dictionary<string, CharacterData> dictCharacters = new Dictionary<string, CharacterData>();

    private void Awake()
    {
        if (dictCharacters.Count != 0) return;
        loadText(characterData.text, x => new CharacterData(x));
        PlayerManager.setCharacter(dictCharacters["Pistolero"]);
    }

    void loadText(string text, Formatter formatter)
    {

        List<string> stringArray = text.Split('\n').ToList();
        stringArray.RemoveAt(0);

        foreach (string s in stringArray)
        {
            formatter(s.Split(';').ToList());
        }
    }

}

