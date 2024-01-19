using UnityEngine;

[CreateAssetMenu(fileName = "WeaponHandler", menuName = Vault.other.scriptableObjectMenu + "WeaponHandler", order = 0)]
public class WeaponHandler : ObjectHandler
{
    [SerializeField] private Interactor weapon;
    [SerializeField] private bool hide;

    public Interactor getWeapon() { return weapon; }

    public bool canBeSelected()
    {
        return !hide;}

    public void Activate()
    {
        Interactor instance = GameObject.Instantiate(weapon);
        instance.Setup(PlayerManager.dictKeyToStats[key]);
    }
}