using System;
using System.Collections.Generic;

public static class EventManagers
{
    public static readonly EventManager<IResourcesListener> resources = new EventManager<IResourcesListener>();

    public static readonly EventManager<IFullResourcesListener> fullResources =
        new EventManager<IFullResourcesListener>();
    public static readonly EventManager<IEggListener> eggs = new EventManager<IEggListener>();
    public static readonly EventManager<IEnnemyListener> ennemies = new EventManager<IEnnemyListener>();
    public static readonly EventManager<IAltarListener> altar = new EventManager<IAltarListener>();
    public static readonly EventManager<IMonsterStele> monsterSteles = new EventManager<IMonsterStele>();
    public static readonly EventManager<IPlayerEvents> player = new EventManager<IPlayerEvents>();
    
    
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