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
    public Sprite gun;
    public Sprite sniper;
    public Sprite shotgun;
    public Sprite laser;

    [Header("Tools")]
    public Sprite pickaxe;

    [Header("Ships")]
    public Sprite ship;

    [Header("Interactors")]
    public Interactor gunInteractor;
    public Interactor sniperInteractor;
    public Interactor shotgunInteractor;
    public Interactor laserInteractor;


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
        Debug.Log(DataSelector.selectedCharacter);
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

    public Sprite getShipSprite()
    {
        return ship;
    }

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
        }
        throw new System.ArgumentException();
    }

    public Sprite getSprite(string key)
    {
        switch (key)
        {
            case Vault.key.sprite.Sniper:
                return sniper;

            case Vault.key.sprite.Shotgun:
                return shotgun;
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
