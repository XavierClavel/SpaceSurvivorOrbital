using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.UIElements;

public class PowerGhost : Power, IEnnemyListener
{
    [SerializeField] private GameObject ghost;
    [SerializeField] private Animation anim;

    [SerializeField] private Shockwave shockwave;

    private bool isShockwaveEnabled;
    private int shockwaveDamage;
    private float shockwaveMaxRange;
    private status shockwaveElement;

    [SerializeField] private int spawnsEvery = 1;
    private int spawnCounter = 0;

    
    public override void Setup(PlayerData _)
    {
        base.Setup(_);
        Ennemy.registerListener(this);
        
        isShockwaveEnabled = fullStats.generic.boolA;
        shockwaveMaxRange = fullStats.generic.floatA;
        shockwaveDamage = fullStats.generic.intA;
        shockwaveElement = fullStats.generic.elementA;
        
        Debug.Log($"shockwave range : {shockwaveMaxRange}");
    }

    private void OnDestroy()
    {
        Ennemy.unregisterListener(this);
    }

    public void onEnnemyDeath(Vector2 position)
    {
        Debug.Log("on ennemy death");
        spawnCounter++;
        if (spawnCounter < spawnsEvery) return;
        spawnCounter = 0;
        SpawnGhost(position);
    }

    public void SpawnGhost(Vector2 position)
    {
        Debug.Log("spawn ghost");
        Instantiate(ghost);
        ghost.transform.position = position;

        StartCoroutine(WaitBeforeShock(2));

        Shockwave shockwaveGhost = Instantiate(shockwave);
        shockwaveGhost.transform.position = position;
        shockwaveGhost.transform.localScale = Vector3.zero;
        shockwaveGhost.Setup(shockwaveMaxRange, shockwaveDamage, shockwaveElement);
        shockwaveGhost.doShockwave();

    }
    private IEnumerator WaitBeforeShock(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }
}
