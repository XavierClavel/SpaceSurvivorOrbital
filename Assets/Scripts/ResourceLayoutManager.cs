using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceLayoutManager : MonoBehaviour
{
    [SerializeField] List<Slider> sliders;
    int amountToFill = 30;
    int sliderIndex = 0;

    private void Start()
    {
        foreach (Slider slider in sliders)
        {
            slider.maxValue = amountToFill;
        }
    }

    public void AddResource()
    {
        sliders[sliderIndex].value++;
        if (sliders[sliderIndex].value == amountToFill && sliderIndex + 1 < sliders.Count) sliderIndex++;
    }

}
