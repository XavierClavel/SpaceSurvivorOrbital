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
    [SerializeField] private GameObject thanksPanel;
    private static bool killedBossForFirstTime = false;
    
    public static void firstBoss(bool isKilled)
    {
        killedBossForFirstTime = isKilled;
    }

    private void Awake()
    {
        thanksPanel.SetActive(killedBossForFirstTime);
        killedBossForFirstTime = false;
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
        SceneTransitionManager.TransitionToScene(gameScene.titleScreen);
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

    public void CloseThanks()
    {
        thanksPanel.SetActive(false);
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
        Planet.Reset();
        UpgradesDisplayManager.Reset();
        ResurrectionManager.reset();
    }

}