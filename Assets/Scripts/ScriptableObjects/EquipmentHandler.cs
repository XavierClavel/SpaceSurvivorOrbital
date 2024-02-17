using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentHandler", menuName = Vault.other.scriptableObjectMenu + "EquipmentHandler", order = 0)]
public class EquipmentHandler : HidableObjectHandler
{   
    [SerializeField] private Equipment equipment;
    [SerializeField] private int charge;
    [SerializeField] private bool booster;
    
    public void Activate(BonusManager bonusManager)
    {
        Equipment instance = GameObject.Instantiate(equipment);
        instance.Setup(PlayerManager.dictKeyToStats[key].Clone());
        if (booster)
        {
            instance.Boost(bonusManager);
        }
    }

    public int getCharge() => charge;
    public bool isBooster() => booster;

}