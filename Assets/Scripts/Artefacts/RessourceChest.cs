using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum resourceChest { green, yellow, blue };

public class RessourceChest : Artefact
{
    [SerializeField] private resourceChest  type;
    public override void Boost(BonusManager bonusManager)
    {
        if (type == resourceChest.green) {PlayerManager.GatherResourceGreen(stats.projectiles);} 
        else if (type == resourceChest.yellow) { PlayerManager.GatherResourceOrange(stats.projectiles);}
        else if (type == resourceChest.blue) { PlayerManager.GatherResourceBlue(stats.projectiles); }
        
    }
}
