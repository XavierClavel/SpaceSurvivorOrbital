using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class AltarPanel : MonoBehaviour
{
    private List<AltarItem> altarItems;
    [SerializeField] private AltarItem itemPrefab;
    [SerializeField] private GameObject blueResourcePrefab;
    [SerializeField] private GameObject autelPanel;
    [SerializeField] private TextMeshProUGUI upgradesPointsDisplay;
    [SerializeField] private RectTransform layout;
    private int displayAmount = 3;

    private static AltarPanel instance;
    
#region MonoBehaviourEvents

    private void Awake()
    {
        instance = this;
    }
    
#endregion

    public static void UpdateAltarDisplay()
    {
        instance.layout.KillAllChildren();
        //instance.upgradesPointsDisplay.SetText(PlayerManager.amountBlue.ToString());
        List<PowerHandler> powersRemaining = ScriptableObjectManager.dictKeyToPowerHandler.Values.ToList().Difference(PlayerManager.powers);
        for (int i = 0; i < instance.displayAmount; i++)
        {
            if (PlayerManager.powers.Count >= 6 || powersRemaining.Count == 0)
            {
                continue;
            }
            PowerHandler selectedPower = powersRemaining.popRandom();


            AltarItem altarItem = GameObject.Instantiate(instance.itemPrefab, instance.layout, true);
            altarItem.transform.localScale = Vector3.one;
            altarItem.Setup(selectedPower.getKey());
            
            
            
            
            
        }

        if (!PlayerManager.isTuto)
        {
            Instantiate(instance.blueResourcePrefab, instance.layout).transform.localScale = Vector3.one;
        }
    }
    
}