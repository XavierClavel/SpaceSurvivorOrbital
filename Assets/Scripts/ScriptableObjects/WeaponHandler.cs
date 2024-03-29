using UnityEngine;

[CreateAssetMenu(fileName = "WeaponHandler", menuName = Vault.other.scriptableObjectMenu + "WeaponHandler", order = 0)]
public class WeaponHandler : HidableObjectHandler
{
    [SerializeField] private Interactor weapon;

    public Interactor getWeapon() { return weapon; }
    
    public void Activate()
    {
        Interactor instance = GameObject.Instantiate(weapon);
        instance.Setup(PlayerManager.dictKeyToStats[key]);
    }
}