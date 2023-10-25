using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AltarPanel : MonoBehaviour
{
    [SerializeField] private List<AltarItem> altarItems;
    [SerializeField] private GameObject autelPanel;

    private void Start()
    {
        List<PowerHandler> powersRemaining = ScriptableObjectManager.dictKeyToPowerHandler.Values.ToList().Difference(PlayerManager.powers);
        foreach (AltarItem altarItem in altarItems)
        {
            PowerHandler selectedPower = powersRemaining.popRandom();
            altarItem.Setup(selectedPower.getKey());
        }
    }

    public void AcquirePower()
    {
        List<PowerHandler> powersRemaining = ScriptableObjectManager.dictKeyToPowerHandler.Values.ToList().Difference(PlayerManager.powers);
        PowerHandler power = powersRemaining.getRandom();
        PlayerManager.AcquirePower(power);
        power.Activate();
        ObjectManager.HideAltarUI();
    }
}