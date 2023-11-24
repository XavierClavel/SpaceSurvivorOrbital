using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleScreen : MonoBehaviour, UIPanel
{
    private static TitleScreen instance;
    public static bool isSelectionFree { get; private set; }
    [SerializeField] private TextMeshProUGUI soulsDisplay;
    
    [Header("Debug")] 
    [SerializeField] private bool freeSelection;

    private void Awake()
    {
        isSelectionFree = freeSelection;
        instance = this;
        UpdateSoulsDisplay();
    }

    // Start is called before the first frame update
    public void Setup()
    {
        ResetManager.Reset();
    }

    public RectTransform getUITransform()
    {
        return GetComponent<RectTransform>();
    }

    public void ResetSave()
    {
        SaveManager.Reset();
        UpdateSoulsDisplay();
    }

    public void AddSouls()
    {
        PlayerManager.setSouls(PlayerManager.getSouls() + 100);
        UpdateSoulsDisplay();
        
    }

    public static void UpdateSoulsDisplay()
    {
        instance.soulsDisplay.SetText(SaveManager.retrieveSouls().ToString());
    }


}

public static class ResetManager
{
    public static void Reset()
    {
        Debug.Log("game state reset");
        NodeManager.Reset();
        DataSelector.Reset();
        PlayerManager.ResetTimer();
        PlayerManager.Reset();
        PlanetSelectionManager.Reset();
        Planet.Reset();
    }
}