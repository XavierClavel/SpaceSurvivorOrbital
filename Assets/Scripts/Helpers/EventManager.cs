using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager<T>
{
    private UnityEvent e = new UnityEvent();
    private List<T> listeners = new List<T>();

    public void resetListeners()
    {
        listeners = new List<T>();
    }
    
    /**
     * Adds a listener for events. The listener will be notified of every event.
     */
    public void registerListener(T listener)
    {
        listeners.Add(listener);
    }
    
    /**
     * Removes a listener from events manager.
     */
    public void unregisterListener(T listener)
    {
        listeners.TryRemove(listener);
    }

    public List<T> getListeners() => listeners;

    public void dispatchEvent(UnityAction<T> action)
    {
        listeners.ForEach(it => action(it));
    }


}
