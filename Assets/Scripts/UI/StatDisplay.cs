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
        if (effect == effectShow.maxHealth) { showText.text = PlayerManager.playerData.character.maxHealth.ToString(); }

        if (effect == effectShow.baseSpeed) { showText.text = PlayerManager.playerData.character.baseSpeed.ToString(); }

        if (effect == effectShow.damageResistanceMultiplier) { showText.text = PlayerManager.playerData.character.damageResistanceMultiplier.ToString(); }

        if (effect == effectShow.baseDamage) { showText.text = PlayerManager.weaponData.interactor.baseDamage.ToString(); }

        if (effect == effectShow.attackSpeed) { showText.text = PlayerManager.weaponData.interactor.attackSpeed.ToString(); }

        if (effect == effectShow.range) { showText.text = PlayerManager.weaponData.interactor.range.ToString(); }

        if (effect == effectShow.bulletReloadTime) { showText.text = PlayerManager.weaponData.interactor.cooldown.ToString(); }

        if (effect == effectShow.magazineReloadTime) { showText.text = PlayerManager.weaponData.interactor.magazineReloadTime.ToString(); }

        if (effect == effectShow.criticalChance) { showText.text = PlayerManager.weaponData.interactor.criticalChance.ToString(); }

        if (effect == effectShow.criticalMultiplier) { showText.text = PlayerManager.weaponData.interactor.criticalMultiplier.ToString(); }

        if (effect == effectShow.pierce) { showText.text = PlayerManager.weaponData.interactor.pierce.ToString(); }

        if (effect == effectShow.speed_aimingDemultiplier) { showText.text = PlayerManager.weaponData.interactor.speedWhileAiming.ToString(); }

        if (effect == effectShow.attractorRange) { showText.text = PlayerManager.playerData.attractor.range.ToString(); }

        if (effect == effectShow.attractorForce) { showText.text = PlayerManager.playerData.attractor.force.ToString(); }
    }
}
