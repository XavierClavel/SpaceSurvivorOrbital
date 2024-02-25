using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtefactShopDisplay : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI costDisplay;
    [SerializeField] private Button buyButton;
    private string key;
    private int cost;

    public void Setup(ArtefactHandler artefact)
    {
        key = artefact.getKey();
        gameObject.name = key;
        cost = (int)(DataManager.dictCost[artefact.getKey()] * BonusManager.current.getMerchantPricesMultiplier());
        icon.sprite = artefact.getIcon();
        costDisplay.SetText(cost.ToString());

        UpdateInteractibility();
    }

    private void onBuy()
    {
        if (!ShopArtefact.Transaction(cost)) return;
        PlayerManager.AcquireArtefact(key);
        gameObject.SetActive(false);
    }

    public void UpdateInteractibility()
    {
        buyButton.interactable = cost <= PlayerManager.getSouls();
    }
}
