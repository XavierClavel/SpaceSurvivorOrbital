
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI soulsDisplay;

    private void Start()
    {
        updateSouls();
    }

    public void updateSouls()
    {
        soulsDisplay.SetText(PlayerManager.getSouls().ToString());
    }
}
