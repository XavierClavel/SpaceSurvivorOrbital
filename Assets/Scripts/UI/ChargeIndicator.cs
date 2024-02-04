using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class ChargeIndicator : MonoBehaviour
{
    [SerializeField] Rectangle fill;
    [SerializeField] Rectangle border;


    public void Fill()
    {
        fill.Color = DataSelector.instance.colorFilled;
    }

    public void Lock()
    {
        fill.Color = DataSelector.instance.colorLocked;
    }

    public void Unlock()
    {
        fill.Color = DataSelector.instance.colorUnfilled;
    }

    public void Unfill()
    {
        fill.Color = DataSelector.instance.colorUnfilled;
    }
    
}
