using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class PlayerManager
{

    public static bool activateShipArrow = false;
    public static bool activateMinerBotAttractor = false;


    //Static accessors

    public static Interactor weaponPrefab = null;


    public static PlayerData weaponData = new PlayerData();
    public static WeaponHandler weapon;
    public static CharacterHandler character;

    public static int amountGreen { get; private set; }
    public static int amountOrange { get; private set; }
    public static int filledGreen { get; private set; }
    public static int filledOrange { get; private set; }
    public static int amountBlue { get; private set; }

    public static bool isPlayingWithGamepad { get; private set; }
    public static int currentTimer { get; set; }
    public static int damageTaken = 0;

    public static List<PowerHandler> powers = new List<PowerHandler>();
    public static List<EquipmentHandler> equipments = new List<EquipmentHandler>();

    public static PlayerData playerData = new PlayerData();
    public static Dictionary<string, PlayerData> dictKeyToStats = new Dictionary<string, PlayerData>();
    private static int souls = 0;

    public static int getSouls()
    {
        return souls;
    }

    public static void setSouls()
    {
        setSouls((SaveManager.retrieveSouls()));
    }

    public static void gainSouls(int value)
    {
        setSouls(getSouls() + value);
    }

    public static void setSouls(int value)
    {
        souls = value;
        SaveManager.updateSouls(souls);
    }
    public static int spendSouls(int value)
    {
        souls -= value;
        SaveManager.updateSouls(souls);
        return souls;
    }

    public static void setCurrentDamage(int health, int maxHealth)
    {
        damageTaken = maxHealth - health;
    }


    public static void setWeapon(PlayerData interactorData, WeaponHandler weaponHandler)
    {
        weaponData = interactorData;
        weaponPrefab = weaponHandler.getWeapon();
        weapon = weaponHandler;
    }

    public static void setCharacter(CharacterHandler characterHandler)
    {
        character = characterHandler;
        CharacterManager.applyCharacterEffect(playerData.character, characterHandler.getKey());
        //characterPrefab = characterHandler.getCharacter();
        //character = characterHandler;
    }


    public static void Reset()
    {
        amountBlue = 0;
        amountGreen = 0;
        amountOrange = 0;
        filledGreen = 0;
        filledOrange = 0;
        damageTaken = 0;
        currentTimer = 0;

        playerData.setBase();
        weaponData = new PlayerData();
        powers = new List<PowerHandler>();
        equipments = new List<EquipmentHandler>();
        dictKeyToStats = new Dictionary<string, PlayerData>();
    }

    public static void GatherResourceGreen() => amountGreen++;
    public static void GatherResourceOrange() => amountOrange++;
    public static void setPartialResourceGreen(int value) => filledGreen = value;
    public static int getPartialResourceGreen() =>  filledGreen;
    public static void setPartialResourceOrange(int value) => filledOrange = value;
    public static int getPartialResourceOrange() => filledOrange;


    public static void SetControlMode(bool boolean) => isPlayingWithGamepad = boolean;

    public static void AcquirePower(PowerHandler powerHandler)
    {
        powers.Add(powerHandler);
        dictKeyToStats[powerHandler.getKey()] = DataManager.dictPowers[powerHandler.getKey()].Clone();
    }

    public static void AcquirePower(string key)
    {
        AcquirePower(ScriptableObjectManager.dictKeyToPowerHandler[key]);
    }

    public static void AcquireEquipment(EquipmentHandler equipmentHandler)
    {
        equipments.Add(equipmentHandler);
        dictKeyToStats[equipmentHandler.getKey()] = DataManager.dictEquipments[equipmentHandler.getKey()].Clone();
    }

    public static void AcquireEquipment(string key)
    {
        AcquireEquipment(ScriptableObjectManager.dictKeyToEquipmentHandler[key]);
    }

    public static void AcquireUpgradePoint()
    {
        amountBlue++;
    }

    public static void SpendUpgradePoints(int amount)
    {
        amountBlue -= amount;
    }

    public static PlayerData getPlayerData(string key)
    {
        if (key == weapon.getKey()) return weaponData;
        if (key == character.getKey()) return playerData;
        return dictKeyToStats[key];
    }

    public static void ApplyModification(Effect effect)
    {
        Debug.Log(effect.target);
        switch (effect.effect)
        {
            case effectType.unlocks:
                break;

            case effectType.weapon:
                effect.ApplyOperation(ref weaponPrefab);
                break;

            default:
                getPlayerData(effect.target).ApplyEffect(effect);
                break;
        }
    }

    public static void SpendResources(int costGreen, int costOrange)
    {
        amountGreen -= costGreen;
        amountOrange -= costOrange;
    }
    

}
