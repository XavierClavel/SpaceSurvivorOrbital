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

    public static float invulnerabilityFrameDuration { get; private set; }

    public static status statusEffect { get; private set; }

    public static int poisonDamage { get; private set; }
    public static float poisonDuration { get; private set; }
    public static float poisonPeriod { get; private set; }

    public static int fireDamage { get; private set; }
    public static float fireDuration { get; private set; }
    public static float firePeriod { get; private set; }

    public static float iceSpeedMultiplier { get; private set; }
    public static float iceDuration { get; private set; }

    public static Interactor weaponPrefab = null;
    public static Interactor toolPrefab = null;


    public static PlayerData weaponData;
    public static WeaponHandler weapon;
    public static CharacterHandler character;

    public static int amountGreen { get; private set; }
    public static int amountOrange { get; private set; }

    public static bool isPlayingWithGamepad { get; private set; }
    public static int currentTimer { get; set; }
    public static int? currentHealth = null;

    public static List<PowerHandler> powers = new List<PowerHandler>();
    public static int amountBlue { get; private set; }

    public static PlayerData playerData = new PlayerData();
    public static Dictionary<string, PlayerData> dictKeyToStats = new Dictionary<string, PlayerData>();
    static int ennemyScoring = 0;

    public static void AddEnnemyScore(int value)
    {
        ennemyScoring += value;
    }

    public static void setCurrentHealth(int health)
    {
        currentHealth = health;
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
    }
    

    public static void Reset()
    {
        amountBlue = 0;
        amountGreen = 0;
        amountOrange = 0;
        ennemyScoring = 0;
        currentHealth = null;

        playerData = new PlayerData();
        weaponData = new PlayerData();
        powers = new List<PowerHandler>();
    }

    public static void GatherResourceGreen() => amountGreen++;
    public static void GatherResourceOrange() => amountOrange++;


    public static void SetControlMode(bool boolean) => isPlayingWithGamepad = boolean;

    public static void AcquirePower(PowerHandler powerHandler)
    {
        powers.Add(powerHandler);
        PlayerData playerData = new PlayerData();
        playerData.interactor = DataManager.dictPowers[powerHandler.getKey()]; //TODO copy
        dictKeyToStats[powerHandler.getKey()] = playerData;
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

            case effectType.tool:
                effect.ApplyOperation(ref toolPrefab);
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

    public static void ResetTimer()
    {
        currentTimer = 0;
    }
}
