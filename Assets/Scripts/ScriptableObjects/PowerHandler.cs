using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : ScriptableObject
{
    [SerializeField] protected string key;
    [SerializeField] protected Sprite icon;

    public string getKey() {return key;}
    public Sprite getIcon() {return icon;}
}


[CreateAssetMenu(fileName = "PowerHandler", menuName = Vault.other.scriptableObjectMenu + "PowerHandler", order = 0)]
public class PowerHandler : ObjectHandler
{
    [SerializeField] private Power power;

    public Power getPower() {return power;}

    public void Activate() {
        interactorStats stats = PlayerManager.dictKeyToStats[key];
        Power instance = GameObject.Instantiate(power);
        instance.Setup(stats);
    }
}

[CreateAssetMenu(fileName = "WeaponHandler", menuName = Vault.other.scriptableObjectMenu + "WeaponHandler", order = 0)]
public class WeaponHandler : ObjectHandler
{
    [SerializeField] private Interactor weapon;

    public Interactor getWeapon() {return weapon;}

    public void Activate() {
        interactorStats stats = PlayerManager.dictKeyToStats[key];
        Interactor instance = GameObject.Instantiate(weapon);
        instance.Setup(stats);
    }
}

[CreateAssetMenu(fileName = "CharacterHandler", menuName = Vault.other.scriptableObjectMenu + "CharacterHandler", order = 0)]
public class CharacterHandler : ObjectHandler
{
    [SerializeField] private string power1;
    [SerializeField] private string power2;

    //public List<string> getPowers() {return ;}

}