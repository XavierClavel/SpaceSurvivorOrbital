using System;
using System.Linq;
using UnityEngine;
    
public class ArtefactsDisplay : ItemGridDisplay<ArtefactHandler>, IArtefactListener
{
    protected override string getKey(ArtefactHandler item)
    {
        return item.getKey();
    }

    protected override Sprite getSprite(ArtefactHandler item)
    {
        return item.getIcon();
    }

    private void Start()
    {
        Debug.Log("doing something");
        Debug.Log(PlayerManager.artefacts.Count);
        addItem(PlayerManager.artefacts.ToList());
        EventManagers.artefacts.registerListener(this);
    }

    private void OnDestroy()
    {
        EventManagers.artefacts.unregisterListener(this);
    }

    public void onArtefactPickup(ArtefactHandler artefact)
    {
        addItem(artefact);
    }
}
