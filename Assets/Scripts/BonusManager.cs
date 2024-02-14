
public class BonusManager
{
    private int bonusMaxHealth;
    private int bonusStock;
    private int bonusShield;
    private int bonusAltarItems;

    private float bonusSpeed = 1f;
    private float bonusStrength = 1f;
    private float bonusResources = 1f;

    public void addBonusMaxHealth(int amount) => bonusMaxHealth += amount;
    public void addBonusStock(int amount) => bonusStock += amount;
    public void addBonusShield(int amount) => bonusShield += amount;
    public void addBonusAltarItem(int amount) => bonusAltarItems += amount;
    public void addBonusSpeed(float amount) => bonusSpeed *= amount;
    public void addBonusStrength(float amount) => bonusStrength *= amount;
    public void addBonusResources(float amount) => bonusResources *= amount;
    
    
    public int getBonusMaxHealth() => bonusMaxHealth;
    public int getBonusStock() => bonusStock;
    public int getBonusShield() => bonusShield;
    public int getBonusAltarItem() => bonusAltarItems;
    public float getBonusSpeed() => bonusSpeed;
    public float getBonusStrength() => bonusStrength;
    public float getBonusResources() => bonusResources;

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
}
