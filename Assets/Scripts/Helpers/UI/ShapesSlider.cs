using System.Collections;
using System.Collections.Generic;
using Shapes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class ShapesSlider : MonoBehaviour
{
    
    [SerializeField] private Rectangle fill;
    [SerializeField] private int fillMaxValue;
    private int sliderMaxValue;
    private UnityAction onComplete = null;

    [Header("Optional")] 
    [SerializeField] private TextMeshProUGUI valueDisplay = null;

    private float increment;
    private float ratio;
    private int currentValue;
    private bool locked = false;

    public ShapesSlider setMaxSliderValue(int value)
    {
        sliderMaxValue = value;
        increment = 1f / sliderMaxValue;
        ratio = fillMaxValue * increment;
        return this;
    }

    public ShapesSlider addOnCompleteAction(UnityAction action)
    {
        onComplete = action;
        return this;
    } 

    public void increase()
    {
        setValue(currentValue + 1);
    }

    public ShapesSlider setValue(int value)
    {
        if (locked) return this;
        currentValue = value;
        UpdateSlider();
        if (value != sliderMaxValue) return this;
        onComplete?.Invoke();
        return this;
    }

    public int getValue() => currentValue;

    public void resetValue()
    {
        currentValue = 0;
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        fill.Height = currentValue * ratio;
        if (valueDisplay == null) return;
        valueDisplay.SetText($"{currentValue * 100 * increment}%");
    }

    public void Lock()
    {
        locked = false;
    }

}
