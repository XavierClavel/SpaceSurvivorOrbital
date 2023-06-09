using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum resourceType { violet, orange, green }
public class ResourceLayoutManager : MonoBehaviour
{
    List<Slider> sliders = new List<Slider>();
    resourceType resource;
    [SerializeField] Slider sliderObject;
    int amountToFill = 30;
    int sliderIndex = 0;
    bool full = false;

    public void Setup(int nbSliders, int amountToFill, resourceType resource)
    {
        this.amountToFill = amountToFill;
        this.resource = resource;
        for (int i = 0; i < nbSliders; i++)
        {
            Slider slider = Instantiate(sliderObject, Vector3.zero, Quaternion.identity);
            slider.transform.SetParent(transform, false);
            slider.maxValue = amountToFill;
            sliders.Insert(0, slider);
        }
    }

    public void AddResource()
    {
        if (full) return;
        sliders[sliderIndex].value++;
        if (sliders[sliderIndex].value < amountToFill) return;
        sliderIndex++;
        OnSliderComplete();
        full = sliderIndex >= sliders.Count;
    }

    void OnSliderComplete()
    {
        switch (resource)
        {
            case resourceType.orange:
                PlayerManager.GatherResourceOrange();
                break;

            case resourceType.green:
                PlayerManager.GatherResourceGreen();
                break;

            case resourceType.violet:
                PlayerManager.GatherResourceViolet();
                //PlayerController.ActivateSpaceship();
                break;
        }
    }

}
