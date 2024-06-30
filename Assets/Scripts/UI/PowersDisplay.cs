using System;
using System.Linq;
using UnityEngine;
    
public class PowersDisplay : ItemGridDisplay<PowerHandler>, IPowerListener
{
    protected override string getKey(PowerHandler item)
    {
        return item.getKey();
    }

    protected override Sprite getSprite(PowerHandler item)
    {
        return item.getIcon();
    }

    private void Start()
    {
        addItem(PlayerManager.powers.ToList());
        EventManagers.powers.registerListener(this);
    }

    private void OnDestroy()
    {
        EventManagers.powers.unregisterListener(this);
    }

    public void onPowerPickup(PowerHandler power)
    {
        addItem(power);
    }
}
