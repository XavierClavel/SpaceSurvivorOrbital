using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractorData : PlayerData
{
    float dps;

    public static void Initialize(List<string> s)
    {
        OverrideInitialize(s);
    }

    public InteractorData(List<string> s)
    {
        setEffects(s);

        interactor currentInteractor = (interactor)System.Enum.Parse(typeof(interactor), name);
        DataManager.dictInteractors.Add(currentInteractor, this);
    }

}
