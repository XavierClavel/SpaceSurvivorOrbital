
public class BonusManager
{
    public static BonusManager current = null;
    
    private int bonusMaxHealth;
    private int bonusStock;
    private int bonusShield;
    private int bonusAltarItems;

    private float bonusSpeed = 1f;
    private float bonusStrength = 1f;
    private float bonusResources = 1f;

    private float weaponCooldown = 1f;

    private float powerCooldown = 1f;

    private float powerStrength = 1f;
    private float equipmentStrength = 1f;
    private float merchantPrices = 1f;
    private float fireDamageMultiplier = 1f;

    public void addBonusMaxHealth(int amount) => bonusMaxHealth += amount;
    public void addBonusStock(int amount) => bonusStock += amount;
    public void addBonusShield(int amount) => bonusShield += amount;
    public void addBonusAltarItem(int amount) => bonusAltarItems += amount;
    public void addBonusSpeed(float amount) => bonusSpeed *= amount;
    public void addBonusStrength(float amount) => bonusStrength *= amount;
    public void addBonusResources(float amount) => bonusResources *= amount;
    public void addBonusPowerCooldown(float amount) => powerCooldown *= amount;
    public void addPowerDamageMultiplier(float amount) => powerStrength *= amount;


    public int getBonusMaxHealth() => bonusMaxHealth;
    public int getBonusStock() => bonusStock;
    public int getBonusShield() => bonusShield;
    public int getBonusAltarItem() => bonusAltarItems;
    public float getBonusSpeed() => bonusSpeed;
    public float getBonusStrength() => bonusStrength;
    public float getBonusResources() => bonusResources;

    public float getPowerCooldownMultiplier() => powerCooldown;
    public float getPowerDamageMultiplier() => powerStrength;

    public float getFireDamageMultiplier() => fireDamageMultiplier;
    public float getMerchantPricesMultiplier() => merchantPrices;

    public void applyEquipmentsEffects()
    {
        foreach (EquipmentHandler equipmentHandler in PlayerManager.equipments)
        {
            if (!equipmentHandler.isBooster())
            {
                continue;
            }
            equipmentHandler.Activate(this);
        }
    }
    
    public void applyArtefactsEffects()
    {
        foreach (ArtefactHandler artefactHandler in PlayerManager.artefacts) {
            artefactHandler.Activate(this);
        }
    }

    public BonusManager()
    {
        current = this;
    }
}
