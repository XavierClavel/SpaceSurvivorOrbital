using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ObjectReferencer", menuName = Vault.other.scriptableObjectMenu + "ObjectReferencer", order = 0)]
public class ObjectReferencer : ScriptableObject
{
    [Header("Characters")]
    public Sprite pistolero;

    [Header("Weapons")]
    public Interactor gun;
    public Interactor laser;

    public Sprite getSpriteGeneric<TEnum>(int value) where TEnum : struct, IConvertible, IComparable, IFormattable
    {
        if (typeof(TEnum) == typeof(character)) return getCharacterSprite((character)value);
        else if (typeof(TEnum) == typeof(weapon)) return getWeaponSprite((weapon)value);
        else throw new System.ArgumentException($"{value} key not found");
    }

    public Sprite getCharacterSprite(character key)
    {
        switch (key)
        {
            case character.Pistolero:
                return pistolero;

            default:
                throw new System.ArgumentException($"interactor key \"name\" not found");
        }
    }

    public Sprite getWeaponSprite(weapon key)
    {
        return getInteractor(key).GetComponentInChildren<SpriteRenderer>().sprite;
    }

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

            default:
                throw new System.ArgumentException($"interactor key \"name\" not found");
        }
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
