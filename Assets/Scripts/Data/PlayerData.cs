using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    public resourcesStats resources = new resourcesStats();
    public characterStats character = new characterStats();
    public interactorStats interactor = new interactorStats();
    public attractorStats attractor = new attractorStats();



    public void ApplyEffect(Effect effect)
    {
        Debug.Log(effect.effect);
        switch (effect.effect)
        {
            case effectType.maxPurple:
                effect.ApplyOperation(ref resources.maxPurple);
                Debug.Log(resources.maxPurple);
                break;

            case effectType.maxOrange:
                effect.ApplyOperation(ref resources.maxOrange);
                break;

            case effectType.maxGreen:
                effect.ApplyOperation(ref resources.maxGreen);
                break;

            case effectType.maxHealth:
                effect.ApplyOperation(ref character.maxHealth);
                break;

            case effectType.baseSpeed:
                effect.ApplyOperation(ref character.baseSpeed);
                break;

            case effectType.damageResistanceMultiplier:
                effect.ApplyOperation(ref character.damageResistanceMultiplier);
                break;

            case effectType.baseDamage:
                effect.ApplyOperation(ref interactor.baseDamage);
                break;

            case effectType.attackSpeed:
                effect.ApplyOperation(ref interactor.attackSpeed);
                break;

            case effectType.range:
                effect.ApplyOperation(ref interactor.range);
                break;

            case effectType.bulletReloadTime:
                effect.ApplyOperation(ref interactor.cooldown);
                break;

            case effectType.magazineReloadTime:
                effect.ApplyOperation(ref interactor.magazineReloadTime);
                break;

            case effectType.criticalChance:
                effect.ApplyOperation(ref interactor.criticalChance);
                break;

            case effectType.criticalMultiplier:
                effect.ApplyOperation(ref interactor.criticalMultiplier);
                break;

            case effectType.projectiles:
                effect.ApplyOperation(ref interactor.projectiles);
                break;

            case effectType.spread:
                effect.ApplyOperation(ref interactor.spread);
                break;

            case effectType.pierce:
                effect.ApplyOperation(ref interactor.pierce);
                break;

            case effectType.aimingSpeed:
                effect.ApplyOperation(ref interactor.speedWhileAiming);
                break;

            case effectType.magazine:
                effect.ApplyOperation(ref interactor.magazine);
                break;

            case effectType.attractorRange:
                effect.ApplyOperation(ref attractor.range);
                break;

            case effectType.attractorForce:
                effect.ApplyOperation(ref attractor.force);
                break;
        }
    }

}


public class resourcesStats
{
    public int maxPurple = 3;
    public int maxOrange = 3;
    public int maxGreen = 3;
}

public class characterStats
{
    public int maxHealth;
    public float baseSpeed;
    public float damageResistanceMultiplier;

    public void setBase()
    {
        maxHealth = Vault.baseStats.MaxHealth;
        baseSpeed = Vault.baseStats.Speed;
        damageResistanceMultiplier = Vault.baseStats.DamageResistance;
    }
}

public class interactorStats
{
    public Vector2Int baseDamage;
    public int attackSpeed;
    public float range;

    public float cooldown;
    public float magazineReloadTime;

    public float criticalChance;
    public float criticalMultiplier;

    public int magazine;
    public int projectiles;
    public float spread;

    public int pierce;
    public float speedWhileAiming;

    public int dps;

    public void CalculateDPS()
    {
        if (cooldown == 0f) dps = baseDamage.Mean();
        else dps = baseDamage.Mean(); //(int)((float)baseDamage.Mean() / cooldown);
    }
}

public class attractorStats
{
    public float range;
    public float force;
}