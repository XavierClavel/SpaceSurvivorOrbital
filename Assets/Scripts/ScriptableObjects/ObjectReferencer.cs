using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ObjectReferencer", menuName = Vault.other.scriptableObjectMenu + "ObjectReferencer", order = 0)]
public class ObjectReferencer : ScriptableObject
{



    [Header("Interactors")]
    public Interactor gun;
    public Interactor doubleGun;
    public Interactor sniper;
    public Interactor shotgun;
    public Interactor laser;
    public Interactor fist;






    public Interactor getInteractor(weapon key)
    {
        switch (key)
        {
            case weapon.None:
                return null;

            case weapon.Gun:
                return gun;

            case weapon.Laser:
                return laser;

            case weapon.Fist:
                return fist;

            default:
                throw new System.ArgumentException($"interactor key \"name\" not found");
        }
    }

    public Interactor getInteractor(string key)
    {
        switch (key)
        {
            case Vault.key.sprite.Sniper:
                return sniper;

            case Vault.key.sprite.Shotgun:
                return shotgun;

            case Vault.key.sprite.DoubleGun:
                return doubleGun;
        }
        throw new System.ArgumentException();
    }



    public Interactor getInteractor(tool key)
    {
        switch (key)
        {
            case tool.None:
                return null;

            default:
                throw new System.ArgumentException($"interactor key \"name\" not found");
        }
    }
}
