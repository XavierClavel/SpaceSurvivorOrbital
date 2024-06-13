using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerHuntersMark : Power
{
    [SerializeField] public static List<Ennemy> markedEnemies = new List<Ennemy>();
    private int maxMarked;
    public static bool isMarked;
    [SerializeField] private CircleCollider2D markCircleCollider;

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

        if (isMarked )
        {
            StartCoroutine(nameof(Boost));
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Ennemy enemy = other.GetComponent<Ennemy>();
        if (enemy != null && !markedEnemies.Contains(enemy))
        {
            MarkEnemy(enemy);
        }
    }

    private void MarkEnemy(Ennemy enemy)
    {
        if (markedEnemies.Count < maxMarked)
        {
            markedEnemies.Add(enemy);
            enemy.ReceiveMark(stats.criticalMultiplier);
        }
    }

    IEnumerator Boost()
    {
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
            //PlayerController.ApplySpeedBoost();
        }

        yield return new WaitForSeconds(2);

        PlayerController.RemoveSpeedBoost();
        PlayerController.RemoveStrengthBoost();

        isMarked = false;
    }
}
