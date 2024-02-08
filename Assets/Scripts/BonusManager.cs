
public class BonusManager
{
    private int bonusMaxHealth;
    private int bonusStock;
    private int bonusShield;

    public void addBonusMaxHealth(int amount) => bonusMaxHealth += amount;
    public void addBonusStock(int amount) => bonusStock += amount;
    public void addBonusShield(int amount) => bonusShield += amount;
    
    public int getBonusMaxHealth() => bonusMaxHealth;
    public int getBonusStock() => bonusStock;
    public int getBonusShield() => bonusShield;
}
