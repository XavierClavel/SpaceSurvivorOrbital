using System.Collections.Generic;
using UnityEngine;

public interface IPlayerEvents
{
    bool onPlayerDeath();
    bool onPlayerHit(bool shieldHit);
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
    
    /**
     * Adds a listener for player events. The listener will be notified of every player event.
     */
    public static void registerListener(IPlayerEvents listener)
    {
        eventListeners.Add(listener);
    }
    
    /**
     * Removes a listener from player events.
     */
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
    
    /**
     * Dispatches event "onPlayerHit". Returns true if one listener cancels player hit.
     * <param name="shieldHit">Whether a shield will be removed instead of a heart.</param>
     */
    public static bool onPlayerHit(bool shieldHit)
    {
        bool value = false;
        foreach (var eventListener in eventListeners)
        {
            value = eventListener.onPlayerHit(shieldHit) || value;
        }
        return value;
    }


}
