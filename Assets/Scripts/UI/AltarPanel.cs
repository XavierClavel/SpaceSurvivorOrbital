using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class AltarPanel : MonoBehaviour
{
    [SerializeField] private List<AltarItem> altarItems;
    [SerializeField] private GameObject autelPanel;
    [SerializeField] private TextMeshProUGUI upgradesPointsDisplay;

    private static AltarPanel instance;
    
#region MonoBehaviourEvents

    private void Start()
    {
        instance = this;
        List<PowerHandler> powersRemaining = ScriptableObjectManager.dictKeyToPowerHandler.Values.ToList().Difference(PlayerManager.powers);
        foreach (AltarItem altarItem in altarItems)
        {
            PowerHandler selectedPower = powersRemaining.popRandom();
            altarItem.Setup(selectedPower.getKey());
        }
    }
    
#endregion

    public static void UpdateAltarDisplay()
    {
        instance.upgradesPointsDisplay.SetText(PlayerManager.amountBlue.ToString());
    }
    
}