using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum effectShow
{
    maxViolet,
    maxOrange,
    maxGreen,

    fillAmountViolet,
    fillAmountOrange,
    fillAmountGreen,

    maxHealth,
    baseSpeed,
    damageResistanceMultiplier,

    baseDamage,
    attackSpeed,
    range,

    bulletReloadTime,
    magazineReloadTime,

    criticalChance,
    criticalMultiplier,

    pierce,
    speed_aimingDemultiplier,
    magazine,

    effect,

    poisonDamage,
    poisonDuration,
    poisonPeriod,

    fireDamage,
    fireDuration,
    firePeriod,

    iceSpeedMultiplier,
    iceDuration,

    toolPower,
    toolReloadTime,
    toolRange,

    attractorRange,
    attractorForce,

    weapon,
    tool

}
public class StatDisplay : MonoBehaviour
{
    [SerializeField] public effectShow effect;
    public TextMeshProUGUI showText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (effect == effectShow.maxHealth) { showText.text = PlayerManager.maxHealth.ToString(); }

        if (effect == effectShow.baseSpeed) { showText.text = PlayerManager.baseSpeed.ToString(); }

        if (effect == effectShow.damageResistanceMultiplier) { showText.text = PlayerManager.damageResistanceMultiplier.ToString(); }

        if (effect == effectShow.baseDamage) { showText.text = PlayerManager.weaponStats.baseDamage.ToString(); }

        if (effect == effectShow.attackSpeed) { showText.text = PlayerManager.weaponStats.attackSpeed.ToString(); }

        if (effect == effectShow.range) { showText.text = PlayerManager.weaponStats.range.ToString(); }

        if (effect == effectShow.bulletReloadTime) { showText.text = PlayerManager.weaponStats.cooldown.ToString(); }

        if (effect == effectShow.magazineReloadTime) { showText.text = PlayerManager.weaponStats.magazineReloadTime.ToString(); }

        if (effect == effectShow.criticalChance) { showText.text = PlayerManager.weaponStats.criticalChance.ToString(); }

        if (effect == effectShow.criticalMultiplier) { showText.text = PlayerManager.weaponStats.criticalMultiplier.ToString(); }

        if (effect == effectShow.pierce) { showText.text = PlayerManager.weaponStats.pierce.ToString(); }

        if (effect == effectShow.speed_aimingDemultiplier) { showText.text = PlayerManager.weaponStats.speedWhileAiming.ToString(); }

        if (effect == effectShow.toolPower) { showText.text = PlayerManager.toolPower.ToString(); }

        if (effect == effectShow.toolReloadTime) { showText.text = PlayerManager.toolReloadTime.ToString(); }

        if (effect == effectShow.toolRange) { showText.text = PlayerManager.toolRange.ToString(); }

        if (effect == effectShow.attractorRange) { showText.text = PlayerManager.attractorRange.ToString(); }

        if (effect == effectShow.attractorForce) { showText.text = PlayerManager.attractorForce.ToString(); }
    }
}
