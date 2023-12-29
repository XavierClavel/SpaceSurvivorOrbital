using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class PlayerManager
{

    public static bool activateRadar = false;
    public static bool activateShipArrow = false;
    public static bool activateMinerBotAttractor = false;


    //Static accessors

    public static Interactor weaponPrefab = null;


    public static PlayerData weaponData = new PlayerData();
    public static WeaponHandler weapon;
    public static CharacterHandler character;
    public static EquipmentHandler equipment;

    public static int amountGreen { get; private set; }
    public static int amountOrange { get; private set; }
    public static int amountBlue { get; private set; }

    public static bool isPlayingWithGamepad { get; private set; }
    public static int currentTimer { get; set; }
    public static int? currentHealth = null;

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

    public static void setSouls(int value)
    {
        souls = value;
        SaveManager.updateSouls(souls);
    }
    public static void spendSouls(int value)
    {
        souls -= value;
        SaveManager.updateSouls(souls);
    }

    public static void setCurrentHealth(int health)
    {
        currentHealth = health;
    }


    public static void setWeapon(PlayerData interactorData, WeaponHandler weaponHandler)
    {
        weaponData = interactorData;
        Debug.Log(weaponData.interactor.projectiles);
        Debug.Log(interactorData.interactor.projectiles);
        weaponPrefab = weaponHandler.getWeapon();
        weapon = weaponHandler;
    }

    public static void setCharacter(CharacterHandler characterHandler)
    {
        character = characterHandler;
    }

    public static void setEquipment(EquipmentHandler equipmentHandler)
    {
        equipment = equipmentHandler;
    }


    public static void Reset()
    {
        amountBlue = 0;
        amountGreen = 0;
        amountOrange = 0;
        currentHealth = null;
        currentTimer = 0;

        playerData.setBase();
        weaponData = new PlayerData();
        powers = new List<PowerHandler>();
        dictKeyToStats = new Dictionary<string, PlayerData>();
    }

    public static void GatherResourceGreen() => amountGreen++;
    public static void GatherResourceOrange() => amountOrange++;


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
                effect.Unlock();
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

    public static void ActivateRadar()
    {
        activateRadar = true;
    }
    public static void ActivateShipArrow()
    {
        activateShipArrow = true;
    }

    public static void ActivateMinerBotAttractor()
    {
        activateMinerBotAttractor = true;
    }

}
