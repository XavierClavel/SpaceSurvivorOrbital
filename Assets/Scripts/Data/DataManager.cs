using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public enum character
{
    None,
    Pistolero
}

public enum weapon
{
    None,
    Gun,
    Laser,
    Fist,
}

public enum tool
{
    None,
    Pickaxe
}


public class DataManager : MonoBehaviour
{
    public ObjectReferencer objectReferencer;
    [SerializeField] TextAsset characterData;
    [SerializeField] TextAsset weaponData;
    [SerializeField] TextAsset toolData;
    [SerializeField] TextAsset powerData;
    [SerializeField] TextAsset breakableData;
    [SerializeField] List<TextAsset> localizationData;
    [SerializeField] TextAsset buttonLocalization;
    [SerializeField] TextAsset upgradesData;
    delegate void Formatter(List<string> s);
    public static Dictionary<weapon, InteractorData> dictWeapons = new Dictionary<weapon, InteractorData>();
    public static Dictionary<tool, InteractorData> dictTools = new Dictionary<tool, InteractorData>();
    public static Dictionary<string, interactorStats> dictPowers = new Dictionary<string, interactorStats>();
    public static Dictionary<string, ObjectData> dictObjects = new Dictionary<string, ObjectData>();
    public static Dictionary<string, LocalizedString> dictLocalization = new Dictionary<string, LocalizedString>();
    public static Dictionary<string, UpgradeData> dictUpgrades = new Dictionary<string, UpgradeData>();
    public static Dictionary<string, Dictionary<string, UpgradeData>> dictKeyToDictUpgrades = new Dictionary<string, Dictionary<string, UpgradeData>>();
    [SerializeField] character selectedCharacter = character.Pistolero;
    [SerializeField] weapon selectedWeapon = weapon.Laser;
    [SerializeField] tool selectedTool = tool.None;
    public static DataManager instance;


    //TODO: use generics instead of delegates
    private void Awake()
    {
        if (!SingletonManager.OnInstanciation(this)) return;
        instance = SingletonManager.get<DataManager>();

        loadText("Weapons", weaponData, x => new InteractorData(x), x => InteractorData.Initialize(x));
        loadText("Powers", powerData, x => new PowerData(x), x => PowerData.Initialize(x));

        loadText("Upgrades", upgradesData, x => new UpgradeData(x), x => UpgradeData.Initialize(x));

        loadText("Breakables", breakableData, x => new ObjectData(x), x => ObjectData.Initialize(x));

        foreach (TextAsset data in localizationData)
        {
            loadText("Localization", data, x => new LocalizedString(x), x => LocalizedString.Initialize(x));
        }
        loadText("Button Localization", buttonLocalization, x => new LocalizedString(x, true), x => LocalizedString.Initialize(x));

        PlayerManager.playerData.character.setBase();

        PlayerManager.setWeapon(dictWeapons[selectedWeapon].interactorData, objectReferencer.getInteractor(selectedWeapon));
        if (selectedTool != tool.None) PlayerManager.setTool(dictTools[selectedTool].interactorData, objectReferencer.getInteractor(selectedTool));
    }

    void loadText(string tableName, TextAsset csv, Formatter formatter, Formatter initializer = null)
    {
        try {
            loadText(csv, formatter, initializer);
        } catch (Exception e) {
            Debug.LogError($"Failed to read value in table \"{tableName}\"");
        }
    }

    void loadText(TextAsset csv, Formatter formatter, Formatter initializer = null, int offset = 0)
    {

        List<string> stringArray = csv.text.Split('\n').ToList();
        if (initializer != null) initializer(stringArray[offset].Split(';').ToList());
        stringArray.RemoveAt(offset);

        List<string> correctedArray = new List<string>();
        string currentString = "";
        bool insideComma = false;

        foreach (string s in stringArray)
        {
            currentString += s;
            if (insideComma) insideComma = s.Count(f => f == '"') % 2 == 0;
            else insideComma = s.Count(f => f == '"') % 2 == 1;

            if (!insideComma)
            {
                currentString = currentString.Trim();
                correctedArray.Add(currentString);
                currentString = "";
            }
            else currentString += "\n";
        }

        foreach (string s in correctedArray)
        {
            List<string> list = s.Split(';').ToList();
            //if (s.Trim()[0] == ';') list.Insert(0, "");
            formatter(list);
        }
    }

}

