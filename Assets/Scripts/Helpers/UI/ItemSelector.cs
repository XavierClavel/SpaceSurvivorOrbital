using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[Serializable]
public class SelectableItem
{
    public string name;
    public string key;
}

public class ItemSelector : MonoBehaviour
{
    [SerializeField] protected List<SelectableItem> selectableItems;
    [SerializeField] private TextMeshProUGUI textDisplay;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    private int currentIndex = 0;
    
    void Start()
    {
        nextButton.onClick.AddListener(Next);
        previousButton.onClick.AddListener(Previous);

        currentIndex = getStartSelectedItem();
        
        DisplayItem();
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

    private void DisplayItem(SelectableItem selectableItem)
    {
        textDisplay.SetText(selectableItem.name);
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
