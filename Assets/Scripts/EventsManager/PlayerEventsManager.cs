using System.Collections.Generic;
using UnityEngine;

public interface IPlayerEvents
{
    bool onPlayerDeath();
}

/**
 * Dispatches player events
 */
public class PlayerEventsManager
{
    private static List<IPlayerEvents> eventListeners = new List<IPlayerEvents>();

    public static void resetListeners()
    {
        eventListeners = new List<IPlayerEvents>();
    }

    public static void registerListener(IPlayerEvents listener)
    {
        eventListeners.Add(listener);
    }

    public static void unregisterListener(IPlayerEvents listener)
    {
        eventListeners.TryRemove(listener);
    }
    
    /**
     * Dispatch event "onPlayerDeath". Returns true if one listener cancels player death.
     */
    public static bool onPlayerDeath()
    {
        bool value = false;
        foreach (var eventListener in eventListeners)
        {
            value = eventListener.onPlayerDeath() || value;
        }
        return value;
    }

}
