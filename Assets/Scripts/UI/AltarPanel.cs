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

    private void Awake()
    {
        instance = this;
        //instance.autelPanel.SetActive(true);
        //instance.autelPanel.SetActive(false);
    }
    
#endregion

    public static void UpdateAltarDisplay()
    {
        Debug.Log(instance.gameObject.name);
        Debug.Log(instance.upgradesPointsDisplay == null);
        instance.upgradesPointsDisplay.SetText(PlayerManager.amountBlue.ToString());
        List<PowerHandler> powersRemaining = ScriptableObjectManager.dictKeyToPowerHandler.Values.ToList().Difference(PlayerManager.powers);
        foreach (AltarItem altarItem in instance.altarItems)
        {
            if (powersRemaining.Count == 0) continue;
            PowerHandler selectedPower = powersRemaining.popRandom();
            altarItem.Setup(selectedPower.getKey());
            Debug.Log(selectedPower.getKey());
        }
    }
    
}