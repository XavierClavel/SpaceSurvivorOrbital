using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectReferncer", menuName = "Space Survivor 2D/ObjectReferencer", order = 0)]
public class ObjectReferencer : ScriptableObject
{
    public Interactor gun;

    public Interactor getInteractor(string key)
    {
        switch (key)
        {
            case "Gun":
                return gun;

            default:
                Debug.Log(name);
                break;
        }
        return null;
    }
}
