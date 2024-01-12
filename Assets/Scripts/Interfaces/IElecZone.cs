

using System.Collections.Generic;

public interface IElecZone
{
    public void OnElecStart();
    public void OnElecStop();
}

public static class ElecEventManager
{
    public static List<IElecZone> eventListeners;

    public static void registerListener(IElecZone listener)
    {
        eventListeners.Add(listener);
    }

    public static void unregisterListener(IElecZone listener)
    {
        eventListeners.TryRemove(listener);
    }

    public static void ElecStart()
    {
        foreach (var eventListener in eventListeners)
        {
            eventListener.OnElecStart();
        }
    }

    public static void ElecStop()
    {
        foreach (var eventListener in eventListeners)
        {
            eventListener.OnElecStop();
        }
    }
}
