using System;
using UnityEngine;

public class FullResourceDisplay : DiscreteBarHandler, IFullResourcesListener
{
    [SerializeField] private resourceType type;

    private void Start()
    {
        switch (type)
        {
            case resourceType.blue:
                Initialize(PlayerManager.playerData.resources.maxBlue + BonusManager.current.getBonusStock(), PlayerManager.amountBlue);
                break;
            
            case resourceType.green:
                Initialize(PlayerManager.playerData.resources.maxGreen + BonusManager.current.getBonusStock(), PlayerManager.amountGreen);
                break;
            
            case resourceType.orange:
                Initialize(PlayerManager.playerData.resources.maxOrange + BonusManager.current.getBonusStock(), PlayerManager.amountOrange);
                break;
        }
        EventManagers.fullResources.registerListener(this);
    }

    public void onFullResourceAmountChange(resourceType type, int amount)
    {
        if (this.type != type) return;
        setAmount(amount);
    }

    private void OnDestroy()
    {
        EventManagers.fullResources.unregisterListener(this);
    }
}
