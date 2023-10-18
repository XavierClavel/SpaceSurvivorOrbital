using UnityEngine;

[CreateAssetMenu(fileName = "WeaponHandler", menuName = Vault.other.scriptableObjectMenu + "WeaponHandler", order = 0)]
public class WeaponHandler : ObjectHandler
{
    [SerializeField] private Interactor weapon;

    public Interactor getWeapon() { return weapon; }

    public void Activate()
    {
        interactorStats stats = PlayerManager.dictKeyToStats[key].interactor;
        Interactor instance = GameObject.Instantiate(weapon);
        instance.Setup(stats);
    }
}