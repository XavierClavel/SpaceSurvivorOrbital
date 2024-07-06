using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SteamHelpers
{
    public static void unlockAchievement(string key)
    {
        if (!SteamManager.Initialized) return;
        Steamworks.SteamUserStats.RequestCurrentStats();
        Steamworks.SteamUserStats.SetAchievement(Vault.achievement.Test);
        Steamworks.SteamUserStats.StoreStats();
    }

    public static void clearAchievement(string key)
    {
        if (!SteamManager.Initialized) return;
        Steamworks.SteamUserStats.RequestCurrentStats();
        Steamworks.SteamUserStats.ClearAchievement(key);
        Steamworks.SteamUserStats.StoreStats();
    }
}
