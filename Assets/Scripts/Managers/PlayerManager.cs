using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Shapes;

public static class PlayerManager
{
    public static bool isTuto = false;
    public static bool isDemo = false;
    public static string currentBoss = "ShipShop";
    public static bool isReset = false;

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
    public static int filledBlue { get; private set; }

    public static bool isPlayingWithGamepad { get; private set; }
    public static int currentTimer { get; set; }
    public static int damageTaken = 0;
    private static int resurrection = 0;
    private static int globalDifficulty = 0;

    public static HashSet<PowerHandler> powers = new HashSet<PowerHandler>();
    public static HashSet<EquipmentHandler> equipments = new HashSet<EquipmentHandler>();
    public static HashSet<ArtefactHandler> artefacts = new HashSet<ArtefactHandler>();

    public static PlayerData playerData = new PlayerData();
    public static Dictionary<string, PlayerData> dictKeyToStats = new Dictionary<string, PlayerData>();
    private static int souls = 0;

    public static int getSouls() => souls;

    public static void resetSouls() => setSouls(0);

    public static void gainSouls(int value) => setSouls(souls + value);

    public static void setSouls(int value) => souls = value;
    public static void spendSouls(int value) => setSouls(souls - value);

    public static void saveSouls()
    {
        int currentSavedSouls = SaveManager.getSouls();
        SaveManager.setSouls(currentSavedSouls + souls);
        resetSouls();
    }

    public static void setCurrentDamage(int health, int maxHealth)
    {
        damageTaken = maxHealth - health;
    }

    public static void setResurrection(int value) => resurrection = value;
    public static int getResurrection() => resurrection;

    /**
     * Returns false if no resurrections left.
     * Else, spends 1 resurrection and returns true.
     */
    public static bool consumeResurrection()
    {
        if (getResurrection() <= 0) return false;
        resurrection--;
        return true;
    }

    public static void increaseDifficulty(bool onPlanet = false)
    {
        if (globalDifficulty >= DataManager.dictDifficulty.Keys.Count) return;
        globalDifficulty++;
        EventManagers.difficulty.getListeners().ForEach(it => it.onDifficultyChange(globalDifficulty));
        if (onPlanet)
        {
            //Instantiate(ObjectManager.instance.difficultyPS, PlayerController.instance.transform);
            //SoundManager.PlaySfx(PlayerController.instance.transform, key: "Difficulty_Up");
        }
        Debug.Log($"Difficulty increased, now {globalDifficulty}");
    }

    public static void resetDifficulty()
    {
        globalDifficulty = 0;
    }

    public static int getDifficulty() => globalDifficulty;


    public static void setWeapon(PlayerData interactorData, WeaponHandler weaponHandler)
    {
        weaponData = interactorData;
        weaponPrefab = weaponHandler.getWeapon();
        weapon = weaponHandler;
    }

    public static void setCharacter(CharacterHandler characterHandler)
    {
        character = characterHandler;
        //characterPrefab = characterHandler.getCharacter();
        //character = characterHandler;
    }


    public static void Reset()
    {
        isTuto = false;
        amountBlue = 0;
        amountGreen = 0;
        amountOrange = 0;
        filledGreen = 0;
        filledOrange = 0;
        filledBlue = 0;
        damageTaken = 0;
        resurrection = 0;
        currentTimer = 0;
        globalDifficulty = 0;
        isReset = true;
        saveSouls();

        playerData.setBase();
        weaponData = new PlayerData();
        powers = new HashSet<PowerHandler>();
        equipments = new HashSet<EquipmentHandler>();
        artefacts = new HashSet<ArtefactHandler>();
        dictKeyToStats = new Dictionary<string, PlayerData>();
    }

    public static void GatherResourceGreen(int resource)
    {
        amountGreen += resource;
        Debug.Log(amountGreen);
        EventManagers.fullResources.dispatchEvent(it => it.onFullResourceAmountChange(resourceType.green, amountGreen));
    } 
    public static void GatherResourceOrange(int resource)
    {
        amountOrange += resource;
        EventManagers.fullResources.dispatchEvent(it => it.onFullResourceAmountChange(resourceType.orange, amountOrange));
    }

    public static void GatherResourceBlue(int resource)
    {
        amountBlue += resource;
        EventManagers.fullResources.dispatchEvent(it => it.onFullResourceAmountChange(resourceType.blue, amountBlue));
    }


    public static void setPartialResourceGreen(int value) => filledGreen = value;
    public static int getPartialResourceGreen() =>  filledGreen;
    public static void setPartialResourceOrange(int value) => filledOrange = value;
    public static int getPartialResourceOrange() => filledOrange;
    public static void setPartialResourceBlue(int value) => filledBlue = value;
    public static int getPartialResourceBlue() => filledBlue;


    public static void SetControlMode(bool boolean) => isPlayingWithGamepad = boolean;

    public static void AcquirePower(PowerHandler powerHandler)
    {
        powers.Add(powerHandler);
        dictKeyToStats[powerHandler.getKey()] = DataManager.dictPowers[powerHandler.getKey()].Clone();
        EventManagers.powers.getListeners().ForEach(it => it.onPowerPickup(powerHandler));
    }

    public static void AcquirePower(string key)
    {
        AcquirePower(ScriptableObjectManager.dictKeyToPowerHandler[key]);
    }

    public static void AcquireEquipment(EquipmentHandler equipmentHandler)
    {
        Debug.Log($"Acquiring equipment {equipmentHandler.getKey()}");
        equipments.Add(equipmentHandler);
        dictKeyToStats[equipmentHandler.getKey()] = DataManager.dictEquipments[equipmentHandler.getKey()].Clone();
    }

    public static void AcquireEquipment(string key)
    {
        AcquireEquipment(ScriptableObjectManager.dictKeyToEquipmentHandler[key]);
    }

    public static void AcquireArtefact(ArtefactHandler artefactHandler)
    {
        if (!artefactHandler.isSingleUse())
        {
            artefacts.Add(artefactHandler);
            EventManagers.artefacts.getListeners().ForEach(it => it.onArtefactPickup(artefactHandler));
        }
        dictKeyToStats[artefactHandler.getKey()] = DataManager.dictArtefacts[artefactHandler.getKey()].Clone();
        artefactHandler.Activate();
    }
    
    public static void AcquireArtefact(string key)
    {
        AcquireArtefact(ScriptableObjectManager.dictKeyToArtefactHandler[key]);
    }
    
    public static void AcquireUpgradePoint()
    {
        amountBlue++;
        EventManagers.fullResources.dispatchEvent(it => it.onFullResourceAmountChange(resourceType.blue, amountBlue));
    }

    public static void SpendUpgradePoints(int amount)
    {
        amountBlue -= amount;
        EventManagers.fullResources.dispatchEvent(it => it.onFullResourceAmountChange(resourceType.blue, amountBlue));
    }

    public static bool isFullBlue() =>
        amountBlue >= PlayerManager.playerData.resources.maxBlue + BonusManager.current.getBonusStock();
        
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
        EventManagers.fullResources.dispatchEvent(it => it.onFullResourceAmountChange(resourceType.green, amountGreen));
        EventManagers.fullResources.dispatchEvent(it => it.onFullResourceAmountChange(resourceType.orange, amountOrange));
    }
    

}
