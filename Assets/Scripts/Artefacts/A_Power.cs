
public class A_Power : Artefact
{
    
    public override void Boost(BonusManager bonusManager)
    {
        PlayerController.bonusManager.addBonusStrength(stats.baseDamage.x);
    }

}
