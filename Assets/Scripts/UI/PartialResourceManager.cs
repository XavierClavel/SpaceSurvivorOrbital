using System;
using UnityEngine;

public class PartialResourceManager : ShapesSlider, IResourcesListener
{
    [SerializeField] private resourceType type;

    private void Start()
    {
        switch (type)
        {
            case resourceType.green:
                setMaxSliderValue(ConstantsData.resourcesFillAmount);
                setValue(PlayerManager.getPartialResourceGreen());
                break;
            
            case resourceType.orange:
                setMaxSliderValue(ConstantsData.resourcesFillAmount);
                setValue(PlayerManager.getPartialResourceOrange());
                break;
        }
        EventManagers.resources.registerListener(this);
    }

    protected override void onComplete()
    {
        resetValue();
        switch (type)
        {
            case resourceType.green:
                PlayerManager.GatherResourceGreen();
                break;
            
            case resourceType.orange:
                PlayerManager.GatherResourceOrange();
                break;
        }
    }

    public void onResourcePickup(resourceType type)
    {
        if (type != this.type) return;
        increase();
    }
}
