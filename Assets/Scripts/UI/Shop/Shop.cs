using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private SoulsDisplay soulsDisplay;
    [SerializeField] private LayoutManager healthBar;
    
    [SerializeField] private Button blueResourceButton;
    [SerializeField] private Button greenResourceButton;
    [SerializeField] private Button orangeResourceButton;
    [SerializeField] private Button healButton;

    [SerializeField] private TextMeshProUGUI textBlueResource;
    [SerializeField] private TextMeshProUGUI textGreenResource;
    [SerializeField] private TextMeshProUGUI textOrangeResource;
    [SerializeField] private TextMeshProUGUI textHealButton;
    
    private const int costHealth = 20;
    private const int costResourceBlue = 50;
    private const int costResourceOrange = 30;
    private const int costResourceGreen = 30;

    public static int maxHealth;
    public static int maxStock;
    
    private static Shop instance;
    private BonusManager bonusManager = new BonusManager();

    public static void Reset()
    {
        maxHealth = 0;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        bonusManager.applyCharacterEffect();
        bonusManager.applyEquipmentsEffects();
        maxHealth = PlayerManager.playerData.character.maxHealth + bonusManager.getBonusMaxHealth();
        maxStock = PlayerManager.playerData.resources.maxOrange + bonusManager.getBonusStock();
        
        healthBar.Setup(maxHealth,  maxHealth - PlayerManager.damageTaken);
        healthBar.SetupShields(bonusManager.getBonusShield());
        
        UpdateHealButton();
        UpdateGreenResourceButton();
        UpdateOrangeResourceButton();
    }

    private void UpdateHealButton()
    {
        if (PlayerManager.damageTaken != 0) return;
        healButton.interactable = false;
        textHealButton.SetText("Full");
    }

    private void UpdateGreenResourceButton()
    {
        if (PlayerManager.amountGreen != maxStock) return;
        greenResourceButton.interactable = false;
        textGreenResource.SetText("Full");
        PlayerManager.setPartialResourceGreen(0);
    }
    
    private void UpdateOrangeResourceButton()
    {
        if (PlayerManager.amountOrange != maxStock) return;
        greenResourceButton.interactable = false;
        textOrangeResource.SetText("Full");
        PlayerManager.setPartialResourceOrange(0);
    }


    private static bool Transaction(int cost)
    {
        if (PlayerManager.getSouls() < cost) return false;
        PlayerManager.spendSouls(cost);
        SoundManager.PlaySfx(instance.transform, key: "Button_Buy");
        instance.soulsDisplay.updateSouls();
        return true;
    }
    
    public void BuyHealth()
    {
        if (!Transaction(costHealth)) return;
        PlayerManager.damageTaken = Mathf.Clamp(PlayerManager.damageTaken - 3, 0, 1000);
        healthBar.SetAmount(maxHealth - PlayerManager.damageTaken);
    }

    public void BuyResourceBlue()
    {
        if (!Transaction(costResourceBlue)) return;
        PlayerManager.AcquireUpgradePoint();
        ResourcesDisplay.UpdateResourcesDisplay();
    }

    public void BuyResourceOrange()
    {
        if (!Transaction(costResourceOrange)) return;
        PlayerManager.GatherResourceOrange();
        ResourcesDisplay.UpdateResourcesDisplay();
        UpdateOrangeResourceButton();
    }

    public void BuyResourceGreen()
    {
        if (!Transaction(costResourceGreen)) return;
        PlayerManager.GatherResourceGreen();
        ResourcesDisplay.UpdateResourcesDisplay();
        UpdateGreenResourceButton();
    }
    
    
}
