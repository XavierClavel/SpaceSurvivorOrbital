using System;
using UnityEngine;

public class PartialResourceManager : ShapesSlider, IResourcesListener
{
    [SerializeField] private resourceType type;

    private void Start()
    {
        setMaxSliderValue(ConstantsData.resourcesFillAmount);
        int currentValue = 0;
        switch (type)
        {
            case resourceType.green:
                currentValue = PlayerManager.getPartialResourceGreen();
                break;
            
            case resourceType.orange:
                currentValue = PlayerManager.getPartialResourceOrange();
                break;
        }

        setValue(currentValue);
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
