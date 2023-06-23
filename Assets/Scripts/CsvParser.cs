using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class CsvParser : MonoBehaviour
{
    [SerializeField] ObjectReferencer objectReferencer;
    [SerializeField] TextAsset characterData;
    [SerializeField] TextAsset interactorData;
    [SerializeField] TextAsset localizationData;
    [SerializeField] TextAsset upgradesData;
    delegate void Formatter(List<string> s);
    public static Dictionary<string, CharacterData> dictCharacters = new Dictionary<string, CharacterData>();
    public static Dictionary<string, InteractorData> dictInteractors = new Dictionary<string, InteractorData>();
    public static Dictionary<string, LocalizedString> dictLocalization = new Dictionary<string, LocalizedString>();
    public static Dictionary<string, UpgradeData> dictUpgrades = new Dictionary<string, UpgradeData>();

    private void Awake()
    {
        if (dictCharacters.Count != 0) return;
        loadText(characterData, x => new CharacterData(x), x => CharacterData.Initialize(x));
        loadText(interactorData, x => new InteractorData(x), x => InteractorData.Initialize(x));
        loadText(localizationData, x => new LocalizedString(x), x => LocalizedString.Initialize(x));
        loadText(upgradesData, x => new UpgradeData(x), x => UpgradeData.Initialize(x));

        PlayerManager.setCharacter(dictCharacters["Pistolero"]);
        PlayerManager.setInteractor(dictInteractors["Laser"], objectReferencer.getInteractor("Laser"));
    }

    void loadText(TextAsset csv, Formatter formatter, Formatter initializer = null, int offset = 0)
    {

        List<string> stringArray = csv.text.Split('\n').ToList();
        if (initializer != null) initializer(stringArray[offset].Split(';').ToList());
        stringArray.RemoveAt(offset);

        foreach (string s in stringArray)
        {
            formatter(s.Split(';').ToList());
        }
    }

}

