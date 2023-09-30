using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq

public class AltarPanel : MonoBehaviour {
    public void AcquirePower() {
        List<PowerHandler> powersRemaining = PanelSelector.dictKeyToPowerHandler.Values.ToList().Difference(PlayerManager.powers);
        PowerHandler power = powersRemaining.getRandom();
        PlayerManager.AcquirePower(power);
        power.Activate();
    }
}