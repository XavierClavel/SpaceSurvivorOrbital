using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHuntersMark : Power, IEnemyListener
{
    [SerializeField] private CircleCollider2D markCircleCollider;
    
    private int maxMarked;
    private bool boostSpeed;
    private bool boostStrenght;
    private bool boostInvicible;
    private readonly List<Ennemy> availableEnemies = new List<Ennemy>();
    private readonly List<Ennemy> markedEnemies = new List<Ennemy>();

    public override void onSetup()
    {
        maxMarked = stats.projectiles;
        boostSpeed = fullStats.generic.boolA;
        boostStrenght = fullStats.generic.boolB;
        boostInvicible = fullStats.generic.boolC;
        
        EventManagers.enemies.registerListener(this);
    }

    private void OnDestroy()
    {
        EventManagers.enemies.unregisterListener(this);
    }

    private void Update()
    {
        markCircleCollider.transform.position = player.transform.position;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!ObjectManager.dictObjectToEnnemy.TryGetValue(other.gameObject, out var enemy)) return;
        if (markedEnemies.Count < maxMarked)
        {
            MarkEnemy(enemy);
        }
        else
        {
            availableEnemies.Add(enemy);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!ObjectManager.dictObjectToEnnemy.TryGetValue(other.gameObject, out var enemy)) return;
        availableEnemies.TryRemove(enemy);
    }

    private void MarkEnemy(Ennemy enemy)
    {
        if (enemy.isMarked) return;
        markedEnemies.Add(enemy);
        enemy.ReceiveMark(stats.criticalMultiplier);
    }

    IEnumerator KillBoost()
    {
        // TODO: rework boost system to prevent stacking / unstacking issues
        if (boostSpeed) PlayerController.ApplySpeedBoost();
        if (boostStrenght) PlayerController.ApplyStrengthBoost();
        if (boostInvicible) PlayerController.instance.StartCoroutine(nameof(PlayerController.InvulnerabilityFrame));

        yield return Helpers.getWait(2f);

        if (boostSpeed) PlayerController.RemoveSpeedBoost();
        if (boostStrenght) PlayerController.RemoveStrengthBoost();
    }

    public void onEnnemyDeath(Ennemy enemy)
    {
        availableEnemies.TryRemove(enemy);
        if (!markedEnemies.Contains(enemy)) return;
        markedEnemies.Remove(enemy);
        StopCoroutine(nameof(KillBoost));
        StartCoroutine(nameof(KillBoost));

        if (availableEnemies.isEmpty()) return;
        MarkEnemy(availableEnemies.getRandom());
    }
}
