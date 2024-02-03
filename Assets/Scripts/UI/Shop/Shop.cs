using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    private const int costHealth = 50;
    private const int costResourceBlue = 100;
    private const int costResourceOrange = 50;
    private const int costResourceGreen = 50;
    
    private static bool Transaction(int cost)
    {
        if (PlayerManager.getSouls() < cost) return false;
        PlayerManager.spendSouls(cost);
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
    }

    public void BuyResourceOrange()
    {
        if (!Transaction(costResourceOrange)) return;
        PlayerManager.GatherResourceOrange();
    }

    public void BuyResourceGreen()
    {
        if (!Transaction(costResourceGreen)) return;
        PlayerManager.GatherResourceGreen();
    }
    
    
}
