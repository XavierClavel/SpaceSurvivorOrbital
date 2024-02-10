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
    [SerializeField] TextAsset equipmentsData;
    [SerializeField] private TextAsset artefactsData;
    [SerializeField] TextAsset breakableData;
    [SerializeField] List<TextAsset> localizationData;
    [SerializeField] TextAsset buttonLocalization;
    [SerializeField] List<TextAsset> upgradesData;
    [SerializeField] private TextAsset spawnData;
    [SerializeField] private TextAsset constantsData;
    [SerializeField] private TextAsset costData;

    public static Dictionary<string, PlayerData> dictWeapons = new Dictionary<string, PlayerData>();
    public static Dictionary<string, PlayerData> dictPowers = new Dictionary<string, PlayerData>();
    public static Dictionary<string, PlayerData> dictArtefacts = new Dictionary<string, PlayerData>();
    public static Dictionary<string, PlayerData> dictEquipments = new Dictionary<string, PlayerData>();
    public static Dictionary<string, ObjectData> dictObjects = new Dictionary<string, ObjectData>();
    public static Dictionary<string, LocalizedString> dictLocalization = new Dictionary<string, LocalizedString>();
    public static Dictionary<string, UpgradeData> dictUpgrades = new Dictionary<string, UpgradeData>();
    public static Dictionary<string, Dictionary<string, UpgradeData>> dictKeyToDictUpgrades = new Dictionary<string, Dictionary<string, UpgradeData>>();
    public static Dictionary<string, SpawnData> dictDifficulty = new Dictionary<string, SpawnData>();
    public static Dictionary<string, int> dictCost = new Dictionary<string, int>();
    [SerializeField] string selectedCharacter = "Knil";
    [SerializeField] string selectedWeapon = "Laser";
    private static DataManager instance;

    
    public void LoadData()
    {
        if (instance != null) return;
        instance = this;
        
        getSaveData();
        
        ScriptableObjectManager.LoadScriptableObjects();
        
        DamagerDataBuilder damagerDataBuilder = new DamagerDataBuilder();
        ObjectDataBuilder objectDataBuilder = new ObjectDataBuilder();
        UpgradeDataBuilder upgradeDataBuilder = new UpgradeDataBuilder();
        LocalizedStringBuilder localizedStringBuilder = new LocalizedStringBuilder();
        DualLocalizedStringBuilder dualLocalizedStringBuilder = new DualLocalizedStringBuilder();
        SpawnDataBuilder spawnDataBuilder = new SpawnDataBuilder();
        ConstantsDataBuilder constantsDataBuilder = new ConstantsDataBuilder();
        CostDataBuilder costDataBuilder = new CostDataBuilder();

        damagerDataBuilder.loadText(weaponData, ref dictWeapons, "Weapons");
        damagerDataBuilder.loadText(powerData, ref dictPowers, "Powers");
        damagerDataBuilder.loadText(equipmentsData, ref dictEquipments, "Equipments");
        damagerDataBuilder.loadText(artefactsData, ref dictArtefacts, "Artefacts");

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
        
        constantsDataBuilder.loadText(constantsData);
        costDataBuilder.loadText(costData, ref dictCost, "Cost");

        PlayerManager.playerData.character.setBase();

        DataSelector.selectedCharacter = selectedCharacter;
        DataSelector.selectedWeapon = selectedWeapon;
        
        PlayerManager.setWeapon(dictWeapons[selectedWeapon].Clone(), ScriptableObjectManager.dictKeyToWeaponHandler[selectedWeapon]);
        PlayerManager.setCharacter(ScriptableObjectManager.dictKeyToCharacterHandler[selectedCharacter]);

        PlanetSelectionManager.GenerateData();
    }

    private void getSaveData()
    {
        SaveManager.Load();
        int souls = SaveManager.retrieveSouls();
        PlayerManager.setSouls(souls);
    }

}

