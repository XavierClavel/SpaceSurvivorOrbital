
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoulsDisplay : MonoBehaviour, ISoulsListener
{
    [SerializeField] private TextMeshProUGUI soulsDisplay;

    private void Awake()
    {
        EventManagers.souls.registerListener(this);
    }

    private void Start()
    {
        updateSouls(getSouls());
    }

    private int getSouls()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case Vault.scene.TitleScreen:
                return SaveManager.getSouls();
            case Vault.scene.Planet:
                return PlayerController.getSouls();
            default:
                return PlayerManager.getSouls();
            
        }
    }

    private void OnDestroy()
    {
        EventManagers.souls.unregisterListener(this);
    }

    private void updateSouls(int value)
    {
        soulsDisplay.SetText(value.ToString());
    }

    public void onSoulsAmountChange(int value)
    {
        updateSouls(value);
    }
}
