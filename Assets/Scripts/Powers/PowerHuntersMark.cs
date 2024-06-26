using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHuntersMark : Power
{
    [SerializeField] public static List<Ennemy> markedEnemies = new List<Ennemy>();
    private int maxMarked;
    public static bool isMarked;
    [SerializeField] private CircleCollider2D markCircleCollider;

    public bool isBoost = false;
    public bool boostSpeed;
    public bool boostStrenght;
    public bool boostInvicible;

    public override void onSetup()
    {
        maxMarked = stats.projectiles;
        boostSpeed = fullStats.generic.boolA;
        boostStrenght = fullStats.generic.boolB;
        boostInvicible = fullStats.generic.boolC;
    }
    private void Update()
    {
        markCircleCollider.transform.position = player.transform.position;

        if (isMarked && !isBoost)
        {
            StartCoroutine(nameof(Boost));
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Ennemy enemy = other.GetComponent<Ennemy>();
        if (enemy != null && markedEnemies.Count < maxMarked)
        {
            MarkEnemy(enemy);
        }
    }

    private void MarkEnemy(Ennemy enemy)
    {
            markedEnemies.Add(enemy);
            enemy.ReceiveMark(stats.criticalMultiplier);
    }

    IEnumerator Boost()
    {
        isBoost = true;

        if (boostSpeed)
        {
            PlayerController.ApplySpeedBoost();
        }

        if (boostStrenght)
        {
            PlayerController.ApplyStrengthBoost();
        }

        if (boostInvicible)
        {
            PlayerController.instance.StartCoroutine("InvulnerabilityFrame");
        }

        yield return new WaitForSeconds(2);

        PlayerController.RemoveSpeedBoost();
        PlayerController.RemoveStrengthBoost();

        isMarked = false;
        isBoost = false;
    }
}
