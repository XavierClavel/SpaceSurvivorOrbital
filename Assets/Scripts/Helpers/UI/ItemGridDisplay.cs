using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public abstract class ItemGridDisplay<T>: MonoBehaviour
{
    private List<T> items = new List<T>();

    public void addItem(T item)
    {
        items.TryAdd(item);
        updateDisplay();
    }

    public void addItem(List<T> items)
    {
        if (items.isEmpty()) updateDisplay();
        else items.ForEach(addItem);
    }

    public void removeItem(T item)
    {
        items.TryRemove(item);
        updateDisplay();
    }

    private void updateDisplay()
    {
        var currentState = transform.getChildren().map(it => it.name);
        var newState = items.map(getKey);
        var diff = currentState.compareTo(newState);

        var toAdd = diff.Item1;
        var toRemove = diff.Item2;
        
        toAdd.ForEach(AddItem);
        toRemove.ForEach(RemoveItem);
    }

    private void RemoveItem(string key)
    {
        var children = transform.getChildren();
        var child = children.Find(it => it.name == key);
        Object.Destroy(child);
    }

    private void RemoveItem(List<string> keys)
    {
        keys.ForEach(RemoveItem);
    }

    protected virtual void AddItem(string key)
    {
        GameObject go = new GameObject
        {
            transform =
            {
                parent = transform
            },
            name = key
        };
        Image image = go.AddComponent(typeof(Image)) as Image;
        image.sprite = getSprite(items.Find(it => getKey(it) == key));
        go.transform.setParent(transform);

    }

    private void AddItem(List<string> keys)
    {
        keys.ForEach(AddItem);
    }
    
    /**
     * Must provide a key unique to the given instance of item.
     */
    protected abstract string getKey(T item);

    protected abstract Sprite getSprite(T item);

    protected T getItem(string key)
    {
        return items.Find(it => getKey(it) == key);
    }
    
    protected Sprite getSprite(string key)
    {
        return getSprite(getItem(key));
    }

}
