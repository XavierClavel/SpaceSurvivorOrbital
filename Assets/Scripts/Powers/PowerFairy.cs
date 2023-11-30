using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerFairy : Power
{
    public Fairy fairy;
    [SerializeField] private Vector2 fairyOffset1;
    [SerializeField] private Vector2 fairyOffset2;
    
    public override void onSetup()
    {
        Fairy newFairy = Instantiate(fairy);
        newFairy.Setup(stats, Vector2.zero,stats.pierce == 4);
        
        if (stats.magazine >= 1) Instantiate(fairy, transform.position, Quaternion.identity).Setup(stats, fairyOffset1);
        if (stats.magazine == 2) Instantiate(fairy, transform.position, Quaternion.identity).Setup(stats, fairyOffset2);
    }
}
