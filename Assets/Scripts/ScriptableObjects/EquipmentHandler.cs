using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentHandler", menuName = Vault.other.scriptableObjectMenu + "EquipmentHandler", order = 0)]
public class EquipmentHandler : HidableObjectHandler
{   
    [SerializeField] private Equipment equipment;
    
    public void Activate()
    {
        Equipment instance = GameObject.Instantiate(equipment);
        instance.Setup(PlayerManager.dictKeyToStats[key]);
    }
    
}