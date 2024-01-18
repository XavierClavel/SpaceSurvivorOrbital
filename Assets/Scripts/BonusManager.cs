
public class BonusManager
{
    private int bonusMaxHealth;
    private int bonusStock;

    public void addBonusMaxHealth(int amount)
    {
        bonusMaxHealth += amount;
    }

    public void addBonusStock(int amount)
    {
        bonusStock += amount;
    }

    public int getBonusMaxHealth()
    {
        return bonusMaxHealth;
    }

    public int getBonusStock()
    {
        return bonusStock;
    }
}
