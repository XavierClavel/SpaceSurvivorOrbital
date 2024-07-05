using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysticAmulet : Equipment
{
    private int number;
    private static bool acquireAvailable = true;

    public override void onSetup()
    {
        BonusManager.current.addBonusPowerCooldown(stats.cooldown);
        BonusManager.current.addPowerDamageMultiplier(stats.attackSpeed);

    }

    public void Awake()
    {
        if (PlayerManager.isReset)
        {
            PlayerManager.isReset = false;
            acquireAvailable = true;
        }
        
        number = Random.Range(1, 10);
        
        Acquire();

    }
    static void AcquirePower(string key)
    {
        PowerHandler powerHandler = ScriptableObjectManager.dictKeyToPowerHandler[key];
        if (PlayerManager.powers.Contains(powerHandler)) return;
        PlayerManager.AcquirePower(powerHandler);
    }

    public void Acquire()
    {
        if (acquireAvailable)
        {
            if (number == 1) AcquirePower(Vault.power.DivineLightning);
            if (number == 2) AcquirePower(Vault.power.Fairy);
            if (number == 3) AcquirePower(Vault.power.Ghost);
            if (number == 4) AcquirePower(Vault.power.BlackHole);
            if (number == 5) AcquirePower(Vault.power.SynthWave);
            if (number == 6) AcquirePower(Vault.power.ToxicZone);
            if (number == 7) AcquirePower(Vault.power.Dagger);
            if (number == 8) AcquirePower(Vault.power.FlameThrower);
            if (number == 9) AcquirePower(Vault.power.IceSpike);
            if (number == 10) AcquirePower(Vault.power.HuntersMark);

            acquireAvailable = false;
        }
    }

}
