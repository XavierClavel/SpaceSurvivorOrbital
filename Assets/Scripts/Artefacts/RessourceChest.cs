using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum resourceChest { green, yellow, blue };

public class RessourceChest : Artefact
{
    [SerializeField] private resourceChest  type;
    public override void Boost(BonusManager bonusManager)
    {
        switch (type)
        {
            case resourceChest.green:
                PlayerManager.GatherResourceGreen(stats.projectiles);
                break;
            case resourceChest.yellow:
                PlayerManager.GatherResourceOrange(stats.projectiles);
                break;
            case resourceChest.blue:
                PlayerManager.GatherResourceBlue(stats.projectiles);
                break;
        }
    }
}
