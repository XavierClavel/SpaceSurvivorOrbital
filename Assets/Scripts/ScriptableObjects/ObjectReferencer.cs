using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ObjectReferencer", menuName = Vault.other.scriptableObjectMenu + "ObjectReferencer", order = 0)]
public class ObjectReferencer : ScriptableObject
{



    [Header("Interactors")]
    public Interactor gunInteractor;
    public Interactor doubleGunInteractor;
    public Interactor sniperInteractor;
    public Interactor shotgunInteractor;
    public Interactor laserInteractor;







    public Interactor getInteractor(weapon key)
    {
        switch (key)
        {
            case weapon.None:
                return null;

            case weapon.Gun:
                return gunInteractor;

            case weapon.Laser:
                return laserInteractor;

            default:
                throw new System.ArgumentException($"interactor key \"name\" not found");
        }
    }

    public Interactor getInteractor(string key)
    {
        switch (key)
        {
            case Vault.key.sprite.Sniper:
                return sniperInteractor;

            case Vault.key.sprite.Shotgun:
                return shotgunInteractor;

            case Vault.key.sprite.DoubleGun:
                return doubleGunInteractor;
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
