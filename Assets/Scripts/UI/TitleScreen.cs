using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour, UIPanel
{
    private static TitleScreen instance;
    public static bool isSelectionFree { get; private set; }
    [SerializeField] private TextMeshProUGUI soulsDisplay;
    [SerializeField] private TextMeshProUGUI soulsDisplay2;

    [Header("Debug")] 
    [SerializeField] private bool freeSelection;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    public void Setup()
    {
        ResetManager.Reset();
        isSelectionFree = freeSelection;
        UpdateSoulsDisplay();
    }

    public RectTransform getUITransform()
    {
        return GetComponent<RectTransform>();
    }

    public void ResetSave()
    {
        SaveManager.Reset();
        UpdateSoulsDisplay();
        SceneTransitionManager.TransitionToScene(gameScene.titleScreen);
    }

    public void AddSouls()
    {
        SaveManager.setSouls(SaveManager.getSouls() + 100);
        UpdateSoulsDisplay();
    }

    public void LoadTuto()
    {
        ResetManager.Reset();
        DataSelector.selectedCharacter = Vault.character.Knil;
        DataSelector.selectedWeapon = Vault.weapon.Gun;
        PlayerManager.isTuto = true;
        DataSelector.instance.ValidateSelection();
        SoundManager.PlaySfx(transform, key: "Ship_TakeOff");
        SceneTransitionManager.TransitionToScene(gameScene.planetJungle);
    }

    public static void UpdateSoulsDisplay()
    {
        instance.soulsDisplay.SetText(SaveManager.getSouls().ToString());
        instance.soulsDisplay2.SetText(SaveManager.getSouls().ToString());
    }

}

public static class ResetManager
{
    public static void Reset()
    {
        Debug.Log("game state reset");
        NodeManager.Reset();
        DataSelector.Reset();
        PlayerManager.Reset();
        PlanetSelectionManager.GenerateData();
        PlanetSelector.ResetDifficulty();
        Planet.Reset();
        UpgradesDisplayManager.Reset();
    }

}