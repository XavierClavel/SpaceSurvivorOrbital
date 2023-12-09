using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public resourcesStats resources = new resourcesStats();
    public characterStats character = new characterStats();
    public interactorStats interactor = new interactorStats();
    public attractorStats attractor = new attractorStats();
    public genericStats generic = new genericStats();

    public PlayerData Clone()
    {
        return new PlayerData()
        {
            resources = resources.Clone(),
            character = new characterStats().setBase(),
            interactor = interactor.Clone(),
            attractor = attractor.Clone(),
            generic = generic.Clone()

        };
    }

    public void setBase()
    {
        character.setBase();
        resources.setBase();
    }



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
            
            case effectType.knockback:
                effect.ApplyOperation(ref interactor.knockback);
                break;

            case effectType.attractorRange:
                effect.ApplyOperation(ref attractor.range);
                break;

            case effectType.attractorForce:
                effect.ApplyOperation(ref attractor.force);
                break;
            
            case effectType.effect:
                effect.ApplyOperation(ref interactor.element);
                break;
            
            case effectType.boolA:
                effect.ApplyOperation(ref generic.boolA);
                break;
                
            case effectType.boolB:
                effect.ApplyOperation(ref generic.boolB);
                break;
                
            case effectType.boolC:
                effect.ApplyOperation(ref generic.boolC);
                break;
            
            case effectType.intA:
                effect.ApplyOperation(ref generic.intA);
                break;
            
            case effectType.intB:
                effect.ApplyOperation(ref generic.intB);
                break;
            
            case effectType.floatA:
                effect.ApplyOperation(ref generic.floatA);
                break;
            
            case effectType.floatB:
                effect.ApplyOperation(ref generic.floatB);
                break;
            
            case effectType.elementA:
                effect.ApplyOperation(ref generic.elementA);
                break;
        }
    }

}

[Serializable]
public class resourcesStats
{
    public int maxPurple = 3;
    public int maxOrange = 3;
    public int maxGreen = 3;

    public resourcesStats Clone()
    {
        return new resourcesStats()
        {
            maxGreen = 3,
            maxPurple = 3,
            maxOrange = 3,
        };
    }

    public void setBase()
    {
        maxGreen = 3;
        maxPurple = 3;
        maxOrange = 3;
    }
}

[Serializable]
public class characterStats
{
    public int maxHealth;
    public float baseSpeed;
    public float damageResistanceMultiplier;

    public characterStats setBase()
    {
        maxHealth = Vault.baseStats.MaxHealth;
        baseSpeed = Vault.baseStats.Speed;
        damageResistanceMultiplier = Vault.baseStats.DamageResistance;
        return this;
    }
}

[Serializable]
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
    public int knockback;

    public status element = status.none;

    public interactorStats Clone()
    {
        return new interactorStats()
        {
            baseDamage = baseDamage,
            attackSpeed = attackSpeed,
            range = range,
            cooldown = cooldown,
            magazineReloadTime = magazineReloadTime,
            criticalChance = criticalChance,
            criticalMultiplier = criticalMultiplier,
            magazine = magazine,
            projectiles = projectiles,
            spread = spread,
            pierce = pierce,
            speedWhileAiming = speedWhileAiming,
            knockback = knockback,
            element = element,
        };
    }
}

[Serializable]
public class genericStats
{
    public bool boolA = false;
    public bool boolB = false;
    public bool boolC = false;

    public int intA = 0;
    public int intB = 0;
    
    public float floatA = 0;
    public float floatB = 0f;

    public status elementA = status.none;

    public genericStats Clone()
    {
        return new genericStats()
        {
            boolA = boolA,
            boolB = boolB,
            boolC = boolC,

            intA = intA,
            intB = intB,

            floatA = floatA,
            floatB = floatB,

            elementA = elementA,
        };
    }
}

[Serializable]
public class attractorStats
{
    public float range = 5f;
    public float force = 5f;

    public attractorStats Clone()
    {
        return new attractorStats()
        {
            range = range,
            force = force,
        };
    }
}