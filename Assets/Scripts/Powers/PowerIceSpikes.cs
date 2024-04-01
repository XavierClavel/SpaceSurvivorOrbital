using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerIceSpikes : Power
{
    public static ComponentPool<IceSpike> pool;
    [SerializeField] private IceSpike iceSpikePrefab;
    private static PowerIceSpike instance;

    public override void onSetup()
    {
        pool = new ComponentPool<IceSpike>(iceSpikePrefab);
        
    }
}
