using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum resourceType { orange, green }
public class ResourceLayoutManager : MonoBehaviour
{
    List<Slider> sliders = new List<Slider>();
    resourceType resource;
    [SerializeField] Slider sliderObject;
    int amountToFill;
    int sliderIndex = 0;
    bool full = false;

    public ParticleSystem greenFull;
    public ParticleSystem yellowFull;

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
        OnSliderComplete(true);
    }

    void FillSlider(bool newResources = false)
    {
        if (full) return;
        sliders[sliderIndex].value = amountToFill;
        OnSliderComplete(newResources);
    }

    public void setPartialAmount(int amount)
    {
        if (full) return;
        sliders[sliderIndex].value = amount;
    }

    public void EmptySlider()
    {
        if (sliderIndex == 0) return;
        if (full)
        {
            sliderIndex--;
            full = false;
        }
        sliders[sliderIndex - 1].value = sliders[sliderIndex].value;
        sliders[sliderIndex].value = 0f;
        sliderIndex--;
    }

    public void FillNSliders(int N, bool newResources = false)
    {
        for (int i = 0; i < N; i++)
        {
            FillSlider(newResources);
        }
    }


    void OnSliderComplete(bool newResources)
    {
        sliderIndex++;
        full = sliderIndex >= sliders.Count;
        

        if (!newResources) return;

        switch (resource)
        {
            case resourceType.orange:
                PlayerManager.GatherResourceOrange();
                SoundManager.PlaySfx(transform, key: "Collectible_Full");
                Instantiate(yellowFull, PlayerController.instance.transform);
                break;

            case resourceType.green:
                PlayerManager.GatherResourceGreen();
                SoundManager.PlaySfx(transform, key: "Collectible_Full");
                Instantiate(greenFull, PlayerController.instance.transform);
                break;

        }
    }

    public int getCurrentAmount()
    {
        if (full) return 0;
        return (int)sliders[sliderIndex].value;
    }

}
