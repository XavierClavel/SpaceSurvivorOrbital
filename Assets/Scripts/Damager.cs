using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public struct HitInfo
{
    public readonly int damage;
    public readonly bool critical;
    public readonly status effect;

    public HitInfo(int damage, bool critical, status effect)
    {
        this.damage = damage;
        this.critical = critical;
        this.effect = effect;
    }

    public HitInfo(int damage)
    {
        this.damage = damage;
        this.critical = false;
        this.effect = status.none;
    }
    
    public HitInfo(interactorStats stats, status effect = status.none)
    {
        damage = stats.baseDamage.getRandom();
        critical = Helpers.ProbabilisticBool(stats.criticalChance);
        if (critical) damage = (int)((float)damage * stats.criticalMultiplier);
        this.effect = effect;
    }
    
    public HitInfo(interactorStats stats)
    {
        damage = stats.baseDamage.getRandom();
        critical = Helpers.ProbabilisticBool(stats.criticalChance);
        if (critical) damage = (int)((float)damage * stats.criticalMultiplier);
        this.effect = stats.element;
    }
    
    
    
}

public abstract class Damager : MonoBehaviour
{
    protected bool reloading = false;
    WaitForSeconds waitCooldown;
    protected PlayerController player;
    protected bool autoCooldown; //whether the interactor or the inheritor should handle cooldown
    public interactorStats stats;
    public PlayerData fullStats;
    [HideInInspector] public bool isUsing = false;

    protected virtual void Start()
    {
        player = PlayerController.instance;
    }

    public virtual void Setup(PlayerData fullStats)
    {
        this.stats = fullStats.interactor;
        this.fullStats = fullStats;
        waitCooldown = Helpers.GetWait(stats.cooldown);
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
        yield return waitCooldown;
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
                ObjectManager.dictObjectToHitable[target.gameObject].Hit(new HitInfo(stats));
            }
        }
    }

    public void Hit(Collider2D target, status effect = status.none)
    {
        Hit(target.gameObject, effect);
    }

    public void Hit(GameObject target, status effect = status.none)
    {
        ObjectManager.dictObjectToHitable[target].Hit(new HitInfo(stats, effect));
    }
    

    #endregion


}