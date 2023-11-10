using UnityEngine;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DataManager", menuName = Vault.other.scriptableObjectMenu + "DataManager", order = 0)]
public class DataManager : ScriptableObject
{
    [SerializeField] TextAsset weaponData;
    [SerializeField] TextAsset powerData;
    [SerializeField] TextAsset breakableData;
    [SerializeField] List<TextAsset> localizationData;
    [SerializeField] TextAsset buttonLocalization;
    [SerializeField] List<TextAsset> upgradesData;
    [SerializeField] private TextAsset spawnData;

    public static Dictionary<string, interactorStats> dictWeapons = new Dictionary<string, interactorStats>();
    public static Dictionary<string, interactorStats> dictPowers = new Dictionary<string, interactorStats>();
    public static Dictionary<string, ObjectData> dictObjects = new Dictionary<string, ObjectData>();
    public static Dictionary<string, LocalizedString> dictLocalization = new Dictionary<string, LocalizedString>();
    public static Dictionary<string, UpgradeData> dictUpgrades = new Dictionary<string, UpgradeData>();
    public static Dictionary<string, Dictionary<string, UpgradeData>> dictKeyToDictUpgrades = new Dictionary<string, Dictionary<string, UpgradeData>>();
    public static Dictionary<string, SpawnData> dictDifficulty = new Dictionary<string, SpawnData>();
    [SerializeField] string selectedCharacter = "Knil";
    [SerializeField] string selectedWeapon = "Laser";
    private static DataManager instance;

    
    public void LoadData()
    {
        if (instance != null) return;
        instance = this;
        
        ScriptableObjectManager.LoadScriptableObjects();
        
        DamagerDataBuilder damagerDataBuilder = new DamagerDataBuilder();
        ObjectDataBuilder objectDataBuilder = new ObjectDataBuilder();
        UpgradeDataBuilder upgradeDataBuilder = new UpgradeDataBuilder();
        LocalizedStringBuilder localizedStringBuilder = new LocalizedStringBuilder();
        DualLocalizedStringBuilder dualLocalizedStringBuilder = new DualLocalizedStringBuilder();
        SpawnDataBuilder spawnDataBuilder = new SpawnDataBuilder();

        damagerDataBuilder.loadText(weaponData, ref dictWeapons, "Weapons");
        damagerDataBuilder.loadText(powerData, ref dictPowers, "Powers");

        objectDataBuilder.loadText(breakableData, ref dictObjects, "Entities");
        
        foreach (TextAsset data in upgradesData)
        {
            upgradeDataBuilder.loadText(data, ref dictUpgrades, $"Upgrades/{data.name}");
        }

        foreach (TextAsset data in localizationData)
        {
            localizedStringBuilder.loadText(data, ref dictLocalization, $"Localization/{data.name}");
        }
        dualLocalizedStringBuilder.loadText(buttonLocalization, ref dictLocalization, "Button Localization");
        
        spawnDataBuilder.loadText(spawnData, ref dictDifficulty, "Spawn");

        PlayerManager.playerData.character.setBase();

        PlayerData weaponPlayerData = new PlayerData();
        weaponPlayerData.interactor = dictWeapons[selectedWeapon];
        PlayerManager.setWeapon(weaponPlayerData, ScriptableObjectManager.dictKeyToWeaponHandler[selectedWeapon]);
        PlayerManager.setCharacter(ScriptableObjectManager.dictKeyToCharacterHandler[selectedCharacter]);
    }

}

