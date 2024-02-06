using System.Collections;
using System.Collections.Generic;
using Shapes;
using UnityEngine;

public class ChargedSelectButton : SelectButton
{
    private const int maxCharge = 4;
    [SerializeField] private Disc disc;

    public void setCharge(int charge)
    {
        disc.AngRadiansStart = 0f;
        disc.AngRadiansEnd = 2f * Mathf.PI * charge / maxCharge;
    }
}
