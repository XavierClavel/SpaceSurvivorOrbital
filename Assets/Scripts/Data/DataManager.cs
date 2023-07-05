using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public enum character
{
    Pistolero
}

public enum interactor
{
    None,
    Gun,
    Laser
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
    [SerializeField] TextAsset interactorData;
    [SerializeField] TextAsset breakableData;
    [SerializeField] TextAsset localizationData;
    [SerializeField] TextAsset upgradesData;
    delegate void Formatter(List<string> s);
    public static Dictionary<character, CharacterData> dictCharacters = new Dictionary<character, CharacterData>();
    public static Dictionary<interactor, InteractorData> dictInteractors = new Dictionary<interactor, InteractorData>();
    public static Dictionary<objects, ObjectData> dictObjects = new Dictionary<objects, ObjectData>();
    public static Dictionary<string, LocalizedString> dictLocalization = new Dictionary<string, LocalizedString>();
    public static Dictionary<string, UpgradeData> dictUpgrades = new Dictionary<string, UpgradeData>();
    [SerializeField] character selectedCharacter = character.Pistolero;
    [SerializeField] interactor selectedWeapon = interactor.Laser;
    [SerializeField] interactor selectedTool = interactor.None;
    public static DataManager instance;
    public static CharacterData baseStats;

    private void Awake()
    {
        instance = this;
        if (dictCharacters.Count != 0) return;
        loadText(characterData, x => new CharacterData(x), x => CharacterData.Initialize(x));
        loadText(interactorData, x => new InteractorData(x), x => InteractorData.Initialize(x));
        loadText(upgradesData, x => new UpgradeData(x), x => UpgradeData.Initialize(x));

        loadText(breakableData, x => new ObjectData(x), x => ObjectData.Initialize(x));
        loadText(localizationData, x => new LocalizedString(x), x => LocalizedString.Initialize(x));

        PlayerManager.setBase();

        if (!DebugManager.testVersion) return;
        PlayerManager.setWeapon(dictInteractors[selectedWeapon].interactorStats, objectReferencer.getInteractor(selectedWeapon));
        if (selectedTool != interactor.None) PlayerManager.setTool(dictInteractors[selectedTool].interactorStats, objectReferencer.getInteractor(selectedWeapon));
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

