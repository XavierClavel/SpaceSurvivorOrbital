using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Counter
{
    private bool value = false;
    private float duration;
    private float remainingTime = -1f;
    private MonoBehaviour context;
    private UnityAction onStart = null;
    private UnityAction onComplete = null;
    private IEnumerator counter()
    {
        onStart?.Invoke();
        value = true;
        remainingTime = duration;
        while (remainingTime > 0f)
        {
            yield return Helpers.getWaitFixed();
            remainingTime -= Time.fixedDeltaTime;
        }

        value = false;
        onComplete?.Invoke();
    }

    public void ResetCounter()
    {
        if (remainingTime <= 0f) context.StartCoroutine(counter());
        else remainingTime = duration;
    }

    public bool getValue()
    {
        return value;
    }

    public Counter(MonoBehaviour context, float duration)
    {
        this.context = context;
        this.duration = duration;
    }

    public Counter addOnStartEvent(UnityAction action)
    {
        onStart = action;
        return this;
    }

    public Counter addOnCompleteEvent(UnityAction action)
    {
        onComplete = action;
        return this;
    }
    
}