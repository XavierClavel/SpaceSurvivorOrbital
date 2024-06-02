using System;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveManager : MonoBehaviour
{
    [SerializeField] private List<ShockwaveInfo> shockwavesInfo;
    private static Dictionary<string, ComponentPool<Shockwave>> shockwaves;

    private void Awake()
    {
        shockwaves = new Dictionary<string, ComponentPool<Shockwave>>();
        shockwavesInfo.ForEach(it => shockwaves[it.key] = new ComponentPool<Shockwave>(it.shockwave).setTimer(5f));
    }

    public static void SpawnShockwave(string key, Vector2 position, float size, int damage, int knockback, status effect)
    {
        Debug.Log("exploding");
        shockwaves[key]
            .get(position)
            .setup(size, damage, effect, knockback)
            .doShockwave();
    }
    
    public static void SpawnShockwave(string key, Vector2 position, float size)
    {
        SpawnShockwave(key, position, size, 1, 0, status.none);
    }
    
}

[Serializable]
public class ShockwaveInfo
{
    public string key;
    public Shockwave shockwave;
}
