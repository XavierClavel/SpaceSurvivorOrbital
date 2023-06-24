using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectReferencer", menuName = "Space Survivor 2D/ObjectReferencer", order = 0)]
public class ObjectReferencer : ScriptableObject
{
    public Interactor gun;
    public Interactor laser;

    public Interactor getInteractor(interactor key)
    {
        switch (key)
        {
            case interactor.Gun:
                return gun;

            case interactor.Laser:
                return laser;

            default:
                throw new System.ArgumentException($"interactor key \"name\" not found");
                break;
        }
        return null;
    }
}
