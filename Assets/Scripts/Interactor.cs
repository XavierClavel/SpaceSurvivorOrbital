using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactor : MonoBehaviour
{
    public bool isUsing = false;
    protected bool reloading = false;
    float cooldown = 1f;
    WaitForSeconds waitCoolDown;

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
        yield return new WaitForSeconds(cooldown);
        reloading = false;
        if (isUsing) Use();
    }

    public abstract void onStartUsing();
    public abstract void onStopUsing();
    public abstract void onUse();
}
