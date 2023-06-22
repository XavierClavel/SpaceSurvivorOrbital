using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactor : MonoBehaviour
{
    public bool isUsing = false;
    protected bool reloading = false;
    float cooldown = 1f;
    WaitForSeconds waitCooldown;

    static LayerMask weaponLayerMask;
    static LayerMask toolLayerMask;
    LayerMask currentLayerMask;

    public void SwitchMode()
    {
        currentLayerMask = currentLayerMask == weaponLayerMask ? toolLayerMask : weaponLayerMask;
    }

    public virtual void StartUsing()
    {
        isUsing = true;
        if (reloading) return;
        onStartUsing();
        if (cooldown == 0f) return;
        Use();
    }

    public void Use()
    {
        onUse();
        StartCoroutine(nameof(Reload));
    }

    public void StopUsing()
    {
        isUsing = false;
        onStopUsing();
    }

    protected IEnumerator Reload()
    {
        reloading = true;
        yield return waitCooldown;
        reloading = false;
        if (isUsing) Use();
    }

    public abstract void onStartUsing();
    public abstract void onStopUsing();
    public abstract void onUse();
}
