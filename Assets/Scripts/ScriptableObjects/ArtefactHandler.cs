using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArtefactHandler", menuName = Vault.other.scriptableObjectMenu + "ArtefactHandler", order = 0)]
public class ArtefactHandler : HidableObjectHandler
{
    [SerializeField] private Artefact artefact;
    [SerializeField] private bool booster;
    [SerializeField] private bool singleUse = false;
    public void Activate()
    {
        Artefact instance = GameObject.Instantiate(artefact);
        Debug.Log(instance.name);
        instance.Setup(PlayerManager.dictKeyToStats[key]);
        if (booster)
        {
            instance.Boost(BonusManager.current);
        }
    }

    public bool isSingleUse() => singleUse;
}
