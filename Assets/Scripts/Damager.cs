using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public struct HitInfo
{
    private int damage;
    public readonly bool critical;
    public readonly HashSet<status> effect;
    public readonly int knockback;
    private float multiplier;
    
    public int getDamage() => (int)(damage * multiplier);

    public HitInfo ApplyBonus()
    {
        damage = (int) (damage * BonusManager.current.getBonusStrength());
        return this;
    }
    
    public HitInfo(int damage, bool critical, HashSet<status> effect)
    {
        this.damage = damage;
        this.critical = critical;
        this.effect = effect;
        this.knockback = 0;
        this.multiplier = 1;
    }

    public void addDamageMultiplier()
    {
        this.damage = (int) (damage * PlayerController.getDamageMultiplier());
    }

    public HitInfo(int damage, bool critical, status effect, int knockback)
    {
        this.damage = damage;
        this.critical = critical;
        this.effect = new HashSet<status>();
        this.effect.Add(effect);
        this.knockback = knockback;
        this.multiplier = 1;
    }

    public HitInfo(int damage, bool critical, status effect)
    {
        this.damage = damage;
        this.critical = critical;
        this.effect = new HashSet<status>();
        this.effect.Add(effect);
        this.knockback = 0;
        this.multiplier = 1;
    }

    public HitInfo(int damage)
    {
        this.damage = damage;
        this.critical = false;
        this.effect = new HashSet<status>();
        this.knockback = 0;
        this.multiplier = 1;
    }
    
    public HitInfo(interactorStats stats, status effect = status.none)
    {
        damage = stats.baseDamage.getRandom();
        critical = Helpers.ProbabilisticBool(stats.criticalChance * BonusManager.current.getWeaponCriticalChance());
        if (critical) damage = (int)((float)damage * stats.criticalMultiplier * BonusManager.current.getWeaponCriticalMultiplier());
        this.effect = new HashSet<status>();
        this.effect.Add(effect);
        this.knockback = stats.knockback;
        this.multiplier = 1;
    }
    
    public HitInfo(interactorStats stats)
    {
        damage = stats.baseDamage.getRandom();
        critical = Helpers.ProbabilisticBool(stats.criticalChance * BonusManager.current.getWeaponCriticalChance());
        if (critical) damage = (int)((float)damage * stats.criticalMultiplier * BonusManager.current.getWeaponCriticalMultiplier());
        this.effect = new HashSet<status>();
        this.effect.Add(stats.element);
        this.knockback = stats.knockback;
        this.multiplier = 1;
    }
    
    public HitInfo(interactorStats stats, HashSet<status> bonusStatuses)
    {
        damage = stats.baseDamage.getRandom();
        critical = Helpers.ProbabilisticBool(stats.criticalChance * BonusManager.current.getWeaponCriticalChance());
        if (critical) damage = (int)(damage * stats.criticalMultiplier * BonusManager.current.getWeaponCriticalMultiplier());
        this.effect = new HashSet<status>();
        this.effect.Add(stats.element);
        this.knockback = stats.knockback;
        
        foreach (var bonusStatus in bonusStatuses)
        {
            this.effect.Add(bonusStatus);
        }
        this.multiplier = 1;
    }

    public HitInfo applyDamageMultiplier(float multiplier)
    {
        this.multiplier *= multiplier;
        return this;
    }
    
    
    
}

public abstract class Damager : MonoBehaviour
{
    protected bool reloading = false;
    protected PlayerController player;
    protected bool autoCooldown; //whether the interactor or the inheritor should handle cooldown
    [HideInInspector] public interactorStats stats;
    [HideInInspector] public PlayerData fullStats;
    [HideInInspector] public bool isUsing = false;

    protected virtual void Start()
    {
        player = PlayerController.instance;
    }

    public virtual void Setup(PlayerData fullStats)
    {
        player = PlayerController.instance;
        this.stats = fullStats.interactor;
        this.fullStats = fullStats;
    }

    public void Use()
    {
        onUse();
        if (stats.cooldown == 0f) return;
        if (autoCooldown) StartCoroutine(nameof(Cooldown));
    }

    protected abstract void onUse();

    protected IEnumerator Cooldown()
    {
        reloading = true;
        yield return Helpers.getWait(stats.cooldown * BonusManager.current.getWeaponCooldownMultiplier());
        reloading = false;
        if (isUsing) Use();
    }

    #region DealDamage

    public void Hit(RaycastHit2D[] targets, bool individualDamage = false)
    {
        Hit(targets.ToList(), individualDamage);
    }

    public void Hit(List<RaycastHit2D> targets, bool individualDamage = false)
    {
        Hit(targets.Select(c => c.collider).ToList(), individualDamage);
    }

    public void Hit(Collider2D[] targets, bool individualDamage = false, status effect = status.none)
    {
        Hit(targets.ToList(), individualDamage, effect);
    }

    public void Hit(List<Collider2D> targets, bool individualDamage = false, status effect = status.none)
    {
        if (targets.Count == 0) return;
        

        foreach (Collider2D target in targets)
        {
            if (individualDamage)
            {
                Hit(target, effect);
            }
            else
            {
                ObjectManager.retrieveHitable(target.gameObject)?.Hit(new HitInfo(stats));
            }
        }
    }

    public void Hit(Collider2D target, status effect = status.none)
    {
        Hit(target.gameObject, effect);
    }

    public void Hit(GameObject target, status effect = status.none)
    {
        ObjectManager.retrieveHitable(target)?.Hit(new HitInfo(stats, effect));
    }
    

    #endregion


}