
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoulsDisplay : MonoBehaviour, ISoulsListener
{
    [SerializeField] private TextMeshProUGUI soulsDisplay;

    private void Start()
    {
        EventManagers.souls.registerListener(this);
        int souls = SceneManager.GetActiveScene().name == Vault.scene.TitleScreen
            ? SaveManager.getSouls()
            : PlayerManager.getSouls();
        updateSouls(souls);
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
