using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class SelectorSelectableItem
{
    public string name;
    public string key;
}

public class ItemSelector : MonoBehaviour
{
    [SerializeField] protected List<SelectorSelectableItem> selectableItems;
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    private int currentIndex = 0;

    public string getValue() => selectableItems[currentIndex].key;
    
    void Start()
    {
        nextButton.onClick.AddListener(Next);
        previousButton.onClick.AddListener(Previous);
        
        DisplayItem();
        UpdateButtons();
    }

    public void setSelected(string key)
    {
        currentIndex = selectableItems.FindIndex(it => it.key == key);
        if (currentIndex == -1)
        {
            Debug.LogError($"Item {key} is not defined for item selector {gameObject.name}");
            currentIndex = 0;
        }
        SelectItem();
        UpdateButtons();
    }

    protected virtual int getStartSelectedItem()
    {
        return 0;
    }

    private void UpdateButtons()
    {
        if (currentIndex == selectableItems.maxIndex()) nextButton.gameObject.SetActive(false);
        if (currentIndex == 0) previousButton.gameObject.SetActive(false);
    }

    private void DisplayItem(SelectorSelectableItem selectorSelectableItem)
    {
        textDisplay.SetText(selectorSelectableItem.name);
    }

    private void DisplayItem(int index) => DisplayItem(selectableItems[index]);
    private void DisplayItem() => DisplayItem(currentIndex);

    private void SelectItem()
    {
        DisplayItem();
        onSelected(selectableItems[currentIndex].key);
    }

    public virtual void onSelected(string key)
    {
        
    }

    public void Next()
    {
        if (currentIndex >= selectableItems.maxIndex()) return;
        if (currentIndex == 0) previousButton.gameObject.SetActive(true);
        currentIndex++;
        SelectItem();
        if (currentIndex == selectableItems.maxIndex()) nextButton.gameObject.SetActive(false);
    }

    public void Previous()
    { 
        if (currentIndex == 0) return;
        if (currentIndex == selectableItems.maxIndex()) nextButton.gameObject.SetActive(true);
        currentIndex--;
        SelectItem();
        if (currentIndex == 0) previousButton.gameObject.SetActive(false);
    }
    
}
