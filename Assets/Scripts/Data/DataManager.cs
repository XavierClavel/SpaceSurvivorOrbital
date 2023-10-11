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
        UpgradeDataBuilder upgradeDataBuilder = new UpgradeDataBuilder();
        LocalizedStringBuilder localizedStringBuilder = new LocalizedStringBuilder();
        DualLocalizedStringBuilder dualLocalizedStringBuilder = new DualLocalizedStringBuilder();

        damagerDataBuilder.loadText(weaponData, ref dictWeapons, "Weapons");
        damagerDataBuilder.loadText(powerData, ref dictPowers, "Powers");

        objectDataBuilder.loadText(breakableData, ref dictObjects, "Entities");

        upgradeDataBuilder.loadText(upgradesData, ref dictUpgrades, "Upgrades");

        foreach (TextAsset data in localizationData)
        {
            localizedStringBuilder.loadText(data, ref dictLocalization, "Localization");
        }
        dualLocalizedStringBuilder.loadText(buttonLocalization, ref dictLocalization, "Button Localization");

        PlayerManager.playerData.character.setBase();

        PlayerData weaponPlayerData = new PlayerData();
        weaponPlayerData.interactor = dictWeapons[selectedWeapon];
        PlayerManager.setWeapon(weaponPlayerData, ScriptableObjectManager.dictKeyToWeaponHandler[selectedWeapon].getWeapon());
    }

}

