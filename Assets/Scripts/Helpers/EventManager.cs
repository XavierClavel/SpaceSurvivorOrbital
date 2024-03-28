using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManagers
{
    public static readonly EventManager<IResourcesListener> resources = new EventManager<IResourcesListener>();
    public static readonly EventManager<IEggListener> eggs = new EventManager<IEggListener>();
    
    
    
    
    private static Dictionary<Type, object> dict;

    public static EventManager<T> getEventManager<T>()
    {
        return dict.getorPut(typeof(T), new EventManager<T>()) as EventManager<T>;
    }

    public static void registerListener<T>(T value)
    {
        getEventManager<T>().registerListener(value);
    }
    
    public static void unregisterListener<T>(T value)
    {
        getEventManager<T>().unregisterListener(value);
    }
    
    
}

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
