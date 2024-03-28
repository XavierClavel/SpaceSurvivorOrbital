using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerIceSpikes : Power
{
    ComponentPool<IceSpike> pool;
    [SerializeField] private IceSpike iceSpikePrefab;
    private static PowerIceSpike instance;
    
    
}
