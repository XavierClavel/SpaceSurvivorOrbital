using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public abstract class Damager : MonoBehaviour
{
    protected SoundManager soundManager;
    protected bool reloading = false;
    WaitForSeconds waitCooldown;
    protected PlayerController player;
    protected bool autoCooldown; //whether the interactor or the inheritor should handle cooldown
    public interactorStats stats;
    [HideInInspector] public bool isUsing = false;

    protected virtual void Start()
    {
        soundManager = SoundManager.instance;
        player = PlayerController.instance;
    }

    public virtual void Setup(interactorStats stats, bool dualUse = false)
    {k,
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

    public void Hit(RaycastHit2D[] targets, bool individualDamage = false) {
        Hit(targets.ToList(), individualDamage);
    }

    public void Hit(List<RaycastHit2D> targets, bool individualDamage = false) {
        Hit(targets.Select(c => c.collider), individualDamage);
    }

    public void Hit(Collider2D[] targets, bool individualDamage = false) {
        Hit(targets.ToList(), individualDamage);
    }

    public void Hit(List<Collider2D> targets, bool individualDamage = false) {
        if (targets.Count == 0) return;

        int damage;
        bool critical;
        status effect = status.none;
        getDamage(out damage, out critical, out effect);

        foreach (Collider2D target in targets)
        {
            if (individualDamage) {
                Hit(target);
            }
            else {
                ObjectManager.dictObjectToHitable[target.gameObject].Hit(damage, player.effect, critical);
            }
        }
    }

    public void Hit(Collider2D target) {
        Hit(target.gameObject);
    }

    public void Hit(GameObject target) {
        int damage;
        bool critical;
        status effect = status.none;
        getDamage(out damage, out critical, out effect);

        ObjectManager.dictObjectToHitable[target].Hit(damage, player.effect, critical);
    }

    private void getDamage(out int damage, out bool critical, out status effect) {
        damage = stats.baseDamage.getRandom();
        critical = Helpers.ProbabilisticBool(stats.criticalChance);
        if (critical) damage = (int)((float)damage * stats.criticalMultiplier);
        effect = player.effect;
    }

#endregion


}