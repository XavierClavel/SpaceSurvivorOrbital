using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopArtefact : MonoBehaviour
{
    [SerializeField] private SoulsDisplay soulsDisplay;
    [SerializeField] private LayoutManager healthBar;

    [SerializeField] private List<ArtefactShopDisplay> artefactDisplays;

    public static int maxHealth;
    public static int maxStock;
    
    private static ShopArtefact instance;
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
        bonusManager.applyArtefactsEffects();
        maxHealth = PlayerManager.playerData.character.maxHealth + bonusManager.getBonusMaxHealth();
        maxStock = PlayerManager.playerData.resources.maxOrange + bonusManager.getBonusStock();
        
        healthBar.Setup(maxHealth,  maxHealth - PlayerManager.damageTaken);
        healthBar.SetupShields(bonusManager.getBonusShield());
        
        List<ArtefactHandler> artefactsRemaining = ScriptableObjectManager.dictKeyToArtefactHandler.Values.Difference(PlayerManager.artefacts);
        artefactDisplays.ForEach(it =>
            {
                if (artefactsRemaining.isEmpty()) return;
                it.Setup(artefactsRemaining.popRandom());
            }
        );
    }


    public static bool Transaction(int cost)
    {
        if (PlayerManager.getSouls() < cost) return false;
        PlayerManager.spendSouls(cost);
        SoundManager.PlaySfx(instance.transform, key: "Button_Buy");
        return true;
    }

}
