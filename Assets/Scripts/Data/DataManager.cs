using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DataManager", menuName = Vault.other.scriptableObjectMenu + "DataManager", order = 0)]
public class DataManager : ScriptableObject
{
    [SerializeField] TextAsset characterData;
    [SerializeField] TextAsset weaponData;
    [SerializeField] TextAsset toolData;
    [SerializeField] TextAsset powerData;
    [SerializeField] TextAsset breakableData;
    [SerializeField] List<TextAsset> localizationData;
    [SerializeField] TextAsset buttonLocalization;
    [SerializeField] TextAsset upgradesData;
    delegate void Formatter(List<string> s);

    public static Dictionary<string, interactorStats> dictWeapons = new Dictionary<string, interactorStats>();
    public static Dictionary<string, interactorStats> dictPowers = new Dictionary<string, interactorStats>();
    public static Dictionary<string, ObjectData> dictObjects = new Dictionary<string, ObjectData>();
    public static Dictionary<string, LocalizedString> dictLocalization = new Dictionary<string, LocalizedString>();
    public static Dictionary<string, UpgradeData> dictUpgrades = new Dictionary<string, UpgradeData>();
    public static Dictionary<string, Dictionary<string, UpgradeData>> dictKeyToDictUpgrades = new Dictionary<string, Dictionary<string, UpgradeData>>();
    [SerializeField] string selectedCharacter = "Knil";
    [SerializeField] string selectedWeapon = "Laser";
    private static DataManager instance;


    //TODO: use generics instead of delegates
    public void LoadData()
    {
        if (instance != null) return;
        instance = this;

        ScriptableObjectManager.LoadScriptableObjects();

        DamagerDataBuilder damagerDataBuilder = new DamagerDataBuilder();
        ObjectDataBuilder objectDataBuilder = new ObjectDataBuilder();

        damagerDataBuilder.loadText(weaponData, ref dictWeapons, "Weapons");
        damagerDataBuilder.loadText(powerData, ref dictPowers, "Powers");

        objectDataBuilder.loadText(breakableData, ref dictObjects, "Entities");

        loadText("Upgrades", upgradesData, x => new UpgradeData(x), x => UpgradeData.Initialize(x));

        foreach (TextAsset data in localizationData)
        {
            loadText("Localization", data, x => new LocalizedString(x), x => LocalizedString.Initialize(x));
        }
        loadText("Button Localization", buttonLocalization, x => new LocalizedString(x, true), x => LocalizedString.Initialize(x));

        PlayerManager.playerData.character.setBase();

        PlayerData weaponPlayerData = new PlayerData();
        weaponPlayerData.interactor = dictWeapons[selectedWeapon];
        PlayerManager.setWeapon(weaponPlayerData, ScriptableObjectManager.dictKeyToWeaponHandler[selectedWeapon].getWeapon());
    }

    void loadText(string tableName, TextAsset csv, Formatter formatter, Formatter initializer = null)
    {
        try
        {
            loadText(csv, formatter, initializer);
        }
        catch (System.Exception e)
        {
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

