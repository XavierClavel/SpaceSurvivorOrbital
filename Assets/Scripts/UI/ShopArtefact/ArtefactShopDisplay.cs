using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ArtefactShopDisplay : MonoBehaviour, ISoulsListener
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI costDisplay;
    [SerializeField] private Button buyButton;
    [SerializeField] private GameObject hideOnBuy;
    [SerializeField] private StringLocalizer descriptionDisplay;
    private string key;
    private int cost;

    public void Setup(ArtefactHandler artefact)
    {
        EventManagers.souls.registerListener(this);
        
        key = artefact.getKey();
        gameObject.name = key;
        cost = (int)(DataManager.dictCost[artefact.getKey()] * BonusManager.current.getMerchantPricesMultiplier());
        icon.sprite = artefact.getIcon();
        costDisplay.SetText(cost.ToString());
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(onBuy);
        descriptionDisplay.setKey(key + Vault.key.ButtonDescription);

        updateInteractibility(PlayerManager.getSouls());
    }

    private void OnDestroy()
    {
        EventManagers.souls.unregisterListener(this);
    }

    private void onBuy()
    {
        if (!ShopArtefact.Transaction(cost)) return;
        PlayerManager.AcquireArtefact(key);
        gameObject.SetActive(false);
    }

    public void updateInteractibility(int value)
    {
        buyButton.interactable = cost <= value;
    }

    public void onSoulsAmountChange(int value)
    {
       updateInteractibility(value);
    }
}
