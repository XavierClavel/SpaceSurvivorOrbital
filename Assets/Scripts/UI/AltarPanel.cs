using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class AltarPanel : MonoBehaviour
{
    private List<AltarItem> altarItems;
    [SerializeField] private AltarItem itemPrefab;
    [SerializeField] private AltarItem blueResourcePrefab;
    [SerializeField] private GameObject autelPanel;
    [SerializeField] private TextMeshProUGUI upgradesPointsDisplay;
    [SerializeField] private RectTransform layout;
    private int displayAmount = 2;

    private static AltarPanel instance;
    
#region MonoBehaviourEvents

    private void Awake()
    {
        instance = this;
    }
    
#endregion

    public static void UpdateAltarDisplay()
    {
        AltarItem selectedItem = null;
        instance.layout.KillAllChildren();
        //instance.upgradesPointsDisplay.SetText(PlayerManager.amountBlue.ToString());
        List<PowerHandler> powersRemaining = ScriptableObjectManager.getDiscoveredPowers().Difference(PlayerManager.powers);
        for (int i = 0; i < instance.displayAmount; i++)
        {
            if (PlayerManager.powers.Count >= 6 || powersRemaining.Count == 0)
            {
                continue;
            }
            PowerHandler selectedPower = powersRemaining.popRandom();


            AltarItem altarItem = GameObject.Instantiate(instance.itemPrefab, instance.layout);
            altarItem.transform.localScale = Vector3.one;
            altarItem.Setup(selectedPower.getKey());

            if (selectedItem == null) selectedItem = altarItem;
        }

        if (!PlayerManager.isTuto && !PlayerManager.isFullBlue())
        {
            AltarItem blueResourceItem = Instantiate(instance.blueResourcePrefab, instance.layout);
            blueResourceItem.setAction(ObjectManager.instance.SelectUpgradePoint);
            blueResourceItem.transform.localScale = Vector3.one;
            if (selectedItem == null) selectedItem = blueResourceItem;
        }
        selectedItem?.setSelected();
    }
    
}