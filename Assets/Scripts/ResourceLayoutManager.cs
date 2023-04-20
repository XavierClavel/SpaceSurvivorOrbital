using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceLayoutManager : MonoBehaviour
{
    List<Slider> sliders = new List<Slider>();
    [SerializeField] Slider sliderObject;
    int amountToFill = 30;
    int sliderIndex = 0;

    public void Setup(int nbSliders, int amountToFill)
    {
        this.amountToFill = amountToFill;
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
        sliders[sliderIndex].value++;
        if (sliders[sliderIndex].value == amountToFill && sliderIndex + 1 < sliders.Count) sliderIndex++;
    }

}
