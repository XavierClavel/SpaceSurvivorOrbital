using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UpgradeButton : TreeButton
{
    [SerializeField] int upgradePointsCost;
    [SerializeField] TextMeshProUGUI upgradePointsCostText;

    protected override void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();

        upgradePointsCostText.text = upgradePointsCost.ToString();
    }

    protected override bool SpendResources()
    {
        if (PlayerManager.upgradePointsAmount < upgradePointsCost) return false;

        PlayerManager.SpendUpgradePoints(upgradePointsCost);
        return true;
    }
}
