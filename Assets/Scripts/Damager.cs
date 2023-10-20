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
    
    public HitInfo(interactorStats stats)
    {
        damage = stats.baseDamage.getRandom();
        critical = Helpers.ProbabilisticBool(stats.criticalChance);
        if (critical) damage = (int)((float)damage * stats.criticalMultiplier);
        effect = status.none;
    }
    
    
    
}

public abstract class Damager : MonoBehaviour
{
    protected SoundManager soundManager;
    protected bool reloading = false;
    WaitForSeconds waitCooldown;
    protected PlayerController player;
    protected bool autoCooldown; //whether the interactor or the inheritor should handle cooldown
    public interactorStats stats;
    [HideInInspector] public bool isUsing = false;
    protected status effect;

    protected virtual void Start()
    {
        soundManager = SoundManager.instance;
        player = PlayerController.instance;
    }

    public virtual void Setup(interactorStats stats, bool dualUse = false)
    {
        this.stats = stats;

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

    public void Hit(Collider2D[] targets, bool individualDamage = false)
    {
        Hit(targets.ToList(), individualDamage);
    }

    public void Hit(List<Collider2D> targets, bool individualDamage = false)
    {
        if (targets.Count == 0) return;
        

        foreach (Collider2D target in targets)
        {
            if (individualDamage)
            {
                Hit(target);
            }
            else
            {
                ObjectManager.dictObjectToHitable[target.gameObject].Hit(new HitInfo(stats));
            }
        }
    }

    public void Hit(Collider2D target)
    {
        Hit(target.gameObject);
    }

    public void Hit(GameObject target)
    {
        ObjectManager.dictObjectToHitable[target].Hit(new HitInfo(stats));
    }
    

    #endregion


}