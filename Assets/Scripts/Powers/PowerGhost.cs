using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

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

    protected override void Start()
    {
        isShockwaveEnabled = fullStats.generic.boolA;
        shockwaveMaxRange = fullStats.generic.floatA;
        shockwaveDamage = fullStats.generic.intA;

        shockwave = Instantiate(shockwave, player.transform, true);
        shockwave.transform.localScale = Vector3.zero;
        shockwave.transform.localPosition = Vector3.zero;
        shockwave.Setup(shockwaveMaxRange, shockwaveDamage, shockwaveElement);
    }
    
    public override void Setup(PlayerData stats)
    {
        base.Setup(stats);
        Ennemy.registerListener(this);
    }

    private void OnDestroy()
    {
        Ennemy.unregisterListener(this);
    }

    public void onEnnemyDeath(Vector2 position)
    {
        spawnCounter++;
        if (spawnCounter >= spawnsEvery)
        {
            spawnCounter = 0;
            SpawnGhost(position);
        }
    }

    public void SpawnGhost(Vector2 position)
    {
        Instantiate(this.ghost);
        ghost.transform.position = position;
        StartCoroutine(Explosion());
    }

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(1f);

        shockwave.doShockwave();

        Destroy(gameObject);
    }
}
