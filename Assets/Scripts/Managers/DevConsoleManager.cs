using System;
using System.Collections;
using System.Collections.Generic;
using DavidFDev.DevConsole;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public static class DevConsoleManager
{
    private static bool initiliazed = false;
    // Start is called before the first frame update
    public static void setup()
    {
        if (initiliazed) return;
        initiliazed = true;
        
        DevConsole.EnableConsole();
        DevConsole.SetToggleKey(Key.Digit1);
        DevConsole.Log("Hello world!");

        setCommands();
        
        Debug.developerConsoleEnabled = true;
    }

    private static void setCommands()
    {
        DevConsole.AddCommand(Command.Create<string>(
            name: "achievement_add",
            aliases: "achievement_set",
            helpText: "Adds an achievement",
            p1: Parameter.Create(
                name: "key",
                helpText: "Achievement key"
            ),
            callback: SteamHelpers.unlockAchievement
        ));
        
        DevConsole.AddCommand(Command.Create<string>(
            name: "achievement_remove",
            aliases: "achievement_clear",
            helpText: "Adds an achievement",
            p1: Parameter.Create(
                name: "key",
                helpText: "Achievement key"
            ),
            callback: SteamHelpers.clearAchievement
        ));

        DevConsole.AddCommand(Command.Create<string>(
            name: "power_add",
            aliases: "power_append",
            helpText: "Adds a power",
            p1: Parameter.Create(
                name: "key",
                helpText: "Achievement key"
            ),
            callback: (string key) =>
            {
                PowerHandler powerHandler = ScriptableObjectManager.dictKeyToPowerHandler[key];
                PlayerManager.AcquirePower(powerHandler);
                if (SceneManager.GetActiveScene().name == Vault.scene.Planet) powerHandler.Activate();
            }
        ));
        
        DevConsole.AddCommand(Command.Create<int>(
            name: "souls_add",
            aliases: "souls_increase",
            helpText: "Adds souls",
            p1: Parameter.Create(
                name: "key",
                helpText: "Achievement key"
            ),
            callback: (int value) =>
            {
                if (SceneManager.GetActiveScene().name == Vault.scene.TitleScreen)
                {
                    SaveManager.addSouls(value);
                }
                else
                {
                    PlayerManager.gainSouls(value);
                }
            }
        ));
        
        DevConsole.AddCommand(Command.Create<Boolean?>(
            name: "free_upgrades",
            aliases: "upgrades_free",
            helpText: "Free upgrades !",
            p1: Parameter.Create(
                name: "value",
                helpText: "param value"
            ),
            callback: (value) =>
            {
                DebugManager._freeUpgrades = value ?? true;
            }
        ));
    }
}
