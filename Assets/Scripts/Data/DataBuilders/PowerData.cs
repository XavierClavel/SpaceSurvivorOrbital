using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerDataBuilder : DataBuilder<interactorStats>
{

    protected override interactorStats BuildData(List<string> s)
    {

        interactorStats stats = new interactorStats();

        SetValue(ref stats.baseDamage, Vault.key.upgrade.BaseDamage);
        SetValue(ref stats.attackSpeed, Vault.key.upgrade.AttackSpeed);
        SetValue(ref stats.range, Vault.key.upgrade.Range);
        SetValue(ref stats.cooldown, Vault.key.upgrade.Cooldown);
        SetValue(ref stats.pierce, Vault.key.upgrade.Pierce);
        SetValue(ref stats.projectiles, Vault.key.upgrade.Projectiles);
        SetValue(ref stats.spread, Vault.key.upgrade.Spread);
        SetValue(ref stats.speedWhileAiming, Vault.key.upgrade.AimingSpeed);
        SetValue(ref stats.criticalChance, Vault.key.upgrade.CriticalChance);
        SetValue(ref stats.criticalMultiplier, Vault.key.upgrade.CriticalChance);
        SetValue(ref stats.magazine, Vault.key.upgrade.Magazine);
        SetValue(ref stats.magazineReloadTime, Vault.key.upgrade.MagazineCooldown);
        stats.CalculateDPS();

        return stats;
    }

}
