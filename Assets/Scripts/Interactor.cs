using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactor : MonoBehaviour
{
    public bool isUsing = false;
    protected bool reloading = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    public virtual void StartUsing()
    {
        isUsing = true;
        if (!reloading) Use();
    }

    public abstract void Use();

    public virtual void StopUsing()
    {
        isUsing = false;
    }
}
