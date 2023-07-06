using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectReferencer", menuName = "Space Survivor 2D/ObjectReferencer", order = 0)]
public class ObjectReferencer : ScriptableObject
{
    [Header("Characters")]
    public Sprite pistolero;

    [Header("Weapons")]
    public Interactor gun;
    public Interactor laser;

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
}
