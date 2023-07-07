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
    Laser
}

public enum tool
{
    None
}

public enum objects
{
    Purple1,
    Purple2,
    Purple3,
    Green1,
    Green2,
    Green3,
    Orange1,
    Orange2,
    Orange3,
    Blob
}

public class DataManager : MonoBehaviour
{
    public ObjectReferencer objectReferencer;
    [SerializeField] TextAsset characterData;
    [SerializeField] TextAsset weaponData;
    [SerializeField] TextAsset toolData;
    [SerializeField] TextAsset breakableData;
    [SerializeField] List<TextAsset> localizationData;
    [SerializeField] TextAsset upgradesData;
    delegate void Formatter(List<string> s);
    public static Dictionary<weapon, InteractorData> dictWeapons = new Dictionary<weapon, InteractorData>();
    public static Dictionary<tool, InteractorData> dictTools = new Dictionary<tool, InteractorData>();
    public static Dictionary<objects, ObjectData> dictObjects = new Dictionary<objects, ObjectData>();
    public static Dictionary<string, LocalizedString> dictLocalization = new Dictionary<string, LocalizedString>();
    public static Dictionary<string, UpgradeData> dictUpgrades = new Dictionary<string, UpgradeData>();
    [SerializeField] character selectedCharacter = character.Pistolero;
    [SerializeField] weapon selectedWeapon = weapon.Laser;
    [SerializeField] tool selectedTool = tool.None;
    public static DataManager instance;
    public static CharacterData baseStats;
    static bool initialized = false;

    private void Awake()
    {
        if (initialized)
        {
            Destroy(gameObject);
            return;
        }

        initialized = true;
        instance = this;

        loadText(weaponData, x => new InteractorData(x, selectorType.weapon), x => InteractorData.Initialize(x));
        loadText(toolData, x => new InteractorData(x, selectorType.tool), x => InteractorData.Initialize(x));

        loadText(upgradesData, x => new UpgradeData(x), x => UpgradeData.Initialize(x));

        loadText(breakableData, x => new ObjectData(x), x => ObjectData.Initialize(x));

        foreach (TextAsset data in localizationData)
        {
            loadText(data, x => new LocalizedString(x), x => LocalizedString.Initialize(x));
        }

        PlayerManager.setBase();

        Debug.Log(selectedWeapon);

        PlayerManager.setWeapon(dictWeapons[selectedWeapon].interactorStats, objectReferencer.getInteractor(selectedWeapon));
        if (selectedTool != tool.None) PlayerManager.setTool(dictTools[selectedTool].interactorStats, objectReferencer.getInteractor(selectedTool));
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

