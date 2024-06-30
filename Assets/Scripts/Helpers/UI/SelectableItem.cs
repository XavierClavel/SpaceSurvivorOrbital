using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectableItem: MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    private string key;
    [SerializeField] private Image image;
    private List<ISelectableItemListener> listeners = new List<ISelectableItemListener>();

    public string getKey() => key;

    public void setup(string key, Sprite sprite)
    {
        this.key = key;
        image.sprite = sprite;
    }

    public void registerListener(ISelectableItemListener listener)
    {
        listeners.Add(listener);
    }
    
    public void unregisterListener(ISelectableItemListener listener)
    {
        listeners.TryRemove(listener);
    }
    
    public void OnSelect(BaseEventData eventData)
    {
        listeners.ForEach(it => it.onSelect(this));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        listeners.ForEach(it => it.onStartHighlight(this));
    }
}
