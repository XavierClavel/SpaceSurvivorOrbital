using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerDataBuilder : DataBuilder<PlayerData>
{

    protected override PlayerData BuildData(List<string> s)
    {

        PlayerData stats = new PlayerData();

        SetValue(ref stats.interactor.baseDamage, Vault.key.upgrade.BaseDamage);
        SetValue(ref stats.interactor.attackSpeed, Vault.key.upgrade.AttackSpeed);
        SetValue(ref stats.interactor.range, Vault.key.upgrade.Range);
        SetValue(ref stats.interactor.cooldown, Vault.key.upgrade.Cooldown);
        SetValue(ref stats.interactor.pierce, Vault.key.upgrade.Pierce);
        SetValue(ref stats.interactor.projectiles, Vault.key.upgrade.Projectiles);
        SetValue(ref stats.interactor.spread, Vault.key.upgrade.Spread);
        SetValue(ref stats.interactor.speedWhileAiming, Vault.key.upgrade.AimingSpeed);
        SetValue(ref stats.interactor.criticalChance, Vault.key.upgrade.CriticalChance);
        SetValue(ref stats.interactor.criticalMultiplier, Vault.key.upgrade.CriticalMultiplier);
        SetValue(ref stats.interactor.magazine, Vault.key.upgrade.Magazine);
        SetValue(ref stats.interactor.magazineReloadTime, Vault.key.upgrade.MagazineCooldown);
        SetValue(ref stats.interactor.knockback, Vault.key.upgrade.Knockback);
        
        TrySetValue(ref stats.generic.boolA, "BoolA");
        TrySetValue(ref stats.generic.boolB, "BoolB");
        TrySetValue(ref stats.generic.boolC, "BoolC");
        TrySetValue(ref stats.generic.intA, "IntA");
        TrySetValue(ref stats.generic.intB, "IntB");
        TrySetValue(ref stats.generic.floatA, "FloatA");
        TrySetValue(ref stats.generic.floatB, "FloatB");
        TrySetValue(ref stats.generic.elementA, "ElementA");

        return stats;
    }

}
