using System.Collections;
using System.Collections.Generic;
using Shapes;
using TMPro;
using UnityEngine;

public class ChargedSelectButton : SelectButton
{
    private const int maxCharge = 4;
    [SerializeField] private TextMeshProUGUI costEquipment;


    public void setCharge(int charge)
    {
        costEquipment.text = charge.ToString();
    }
}
