using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private SoulsDisplay soulsDisplay;
    
    private const int costHealth = 20;
    private const int costResourceBlue = 30;
    private const int costResourceOrange = 10;
    private const int costResourceGreen = 10;

    public static int maxHealth;
    public static int healthLost;
    public static int shieldsAmount;
    private static Shop instance;
    private BonusManager bonusManager = new BonusManager();

    public static void Reset()
    {
        maxHealth = 0;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        foreach (EquipmentHandler equipmentHandler in PlayerManager.equipments)
        {
            if (!equipmentHandler.isBooster())
            {
                continue;
            }
            equipmentHandler.Activate(bonusManager);
        }
        maxHealth = PlayerManager.playerData.character.maxHealth + bonusManager.getBonusMaxHealth();
    }


    private static bool Transaction(int cost)
    {
        if (PlayerManager.getSouls() < cost) return false;
        PlayerManager.spendSouls(cost);
        instance.soulsDisplay.updateSouls();
        return true;
    }
    
    public void BuyHealth()
    {
        if (!Transaction(costHealth)) return;
        PlayerManager.damageTaken = 0;
    }

    public void BuyResourceBlue()
    {
        if (!Transaction(costResourceBlue)) return;
        PlayerManager.AcquireUpgradePoint();
        ResourcesDisplay.UpdateResourcesDisplay();
    }

    public void BuyResourceOrange()
    {
        if (!Transaction(costResourceOrange)) return;
        PlayerManager.GatherResourceOrange();
        ResourcesDisplay.UpdateResourcesDisplay();
    }

    public void BuyResourceGreen()
    {
        if (!Transaction(costResourceGreen)) return;
        PlayerManager.GatherResourceGreen();
        ResourcesDisplay.UpdateResourcesDisplay();
    }
    
    
}
