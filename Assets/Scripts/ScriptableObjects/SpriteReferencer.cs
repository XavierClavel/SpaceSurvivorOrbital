using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SpriteReferencer", menuName = Vault.other.scriptableObjectMenu + "SpriteReferencer", order = 0)]
public class SpriteReferencer : ScriptableObject
{
    [Header("Characters")]
    public Sprite pistolero;

    [Header("Weapons")]
    public Sprite gun;
    public Sprite doubleGun;
    public Sprite sniper;
    public Sprite shotgun;
    public Sprite laser;
    public Sprite fist;

    [Header("Tools")]
    public Sprite pickaxe;

    [Header("Ships")]
    public Sprite ship;

    [Header("Powers")]
    public Sprite creusetoutSprite;
    public Sprite divineLightningSprite;
    public Sprite toxicCloudSprite;

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
                throw new System.ArgumentException($"interactor key \"{key}\" not found");
        }
    }

    public Sprite getCharacterSprite()
    {
        return getCharacterSprite(DataSelector.selectedCharacter);
    }

    public Sprite getWeaponSprite(weapon key)
    {
        switch (key)
        {
            case weapon.Gun:
                return gun;

            case weapon.Laser:
                return laser;

            case weapon.Fist:
                return fist;

            default:
                throw new System.ArgumentException($"interactor key \"{key}\" not found");
        }
    }

    public Sprite getWeaponSprite()
    {
        return getWeaponSprite(DataSelector.selectedWeapon);
    }

    public Sprite getToolSprite(tool key)
    {
        switch (key)
        {
            case tool.Pickaxe:
                return pickaxe;

            default:
                throw new System.ArgumentException($"key \"{key}\" not found");
        }
    }

    public Sprite getToolSprite()
    {
        return getToolSprite(DataSelector.selectedTool);
    }

    public Sprite getSprite(string key)
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

    public Sprite getShipSprite()
    {
        return ship;
    }
}
