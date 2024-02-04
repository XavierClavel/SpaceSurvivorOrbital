using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour
{
    [Header("UI")]
    public LayoutManager healthBar;
    [SerializeField] GameObject pauseButton;
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] GameObject altarUI;
    [SerializeField] GameObject altarFirstSelected;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject loseScreenFirstSelected;
    [SerializeField] private GameObject radar;
    [SerializeField] private Slider bossHealthbar;
    public Transform powersDisplayLayout;
    public PowerDisplay powerDisplayPrefab;
    public Slider reloadSlider;
    
    [SerializeField] public TextMeshProUGUI altarMonsterTotal;
    [SerializeField] public TextMeshProUGUI altarMonsterCurrent;

    public ParticleSystem firePS;
    public ParticleSystem lightningPS;
    public ParticleSystem icePS;
    public GameObject upgradePS;
    public ParticleSystem shipAppearPS;
    public ParticleSystem difficultyPS;

    private bool upgradeGain = false;

    [Header("Interactors")]
    public Interactor gun;

    public static ObjectManager instance;
    public static Altar altar;
    public static ObjectIndicator spaceshipIndicator;
    public static GameObject spaceship;

    public static Dictionary<GameObject, Ennemy> dictObjectToEnnemy = new Dictionary<GameObject, Ennemy>();
    public static Dictionary<GameObject, IResource> dictObjectToResource = new Dictionary<GameObject, IResource>();
    public static Dictionary<GameObject, IInteractable> dictObjectToInteractable = new Dictionary<GameObject, IInteractable>();
    public static Dictionary<GameObject, Breakable> dictObjectToHitable = new Dictionary<GameObject, Breakable>();
    static int amountEggs = 0;
    private static int amountDens = 0;
    private static int amountDensDestroyed = 0;

    public void Start()
    {
        altarMonsterTotal.text = PlanetManager.getDensAmount().ToString();
    }


    public static void DisplaySpaceship()
    {
        spaceship.SetActive(true);
    }

    public static void ActivateRadar()
    {
        instance.radar.SetActive(true);
    }

    public static void HideSpaceship() { spaceship.SetActive(false); }

    public static void registerEggSpawned() {
        amountEggs++;
    }

    public static void registerEggDestroyed() {
        amountEggs--;
        if (amountEggs > 0) return;
        
        PlayerManager.AcquireUpgradePoint();
        SoundManager.PlaySfx(PlayerController.instance.transform, key: "Collectible_Blue");
        UpgradeUpDisplay();
    }

    private static void UpgradeUpDisplay()
    {
        GameObject newInstanceUpgrade = Instantiate(instance.upgradePS, PlayerController.instance.transform);
        Destroy(newInstanceUpgrade,2f);
    }

    public static void registerHitable(Breakable hitable)
    {
        dictObjectToHitable[hitable.gameObject] = hitable;
    }

    public static void unregisterHitable(GameObject gameObject)
    {
        dictObjectToHitable.TryRemove(gameObject);
    }

    public static Breakable retrieveHitable(GameObject gameObject)
    {
        return !dictObjectToHitable.ContainsKey(gameObject) ? null : dictObjectToHitable[gameObject];
    }
    
    public static void registerDenSpawned() {
        amountDens++;
    }

    public static void registerDenDestroyed() {
        amountDensDestroyed++;
        amountDens--;
        instance.altarMonsterCurrent.SetText(amountDensDestroyed.ToString());

        if (amountDens > 0) return;
        Instantiate(instance.shipAppearPS, PlayerController.instance.transform);
        SoundManager.PlaySfx(PlayerController.instance.transform, key: "Monster_Altar_Destroy");
        DisplaySpaceship();
    }

    private void Awake()
    {
        instance = this;
        amountEggs = 0;
        amountDens = 0;
        amountDensDestroyed = 0;
        radar.SetActive(false);
        if (Helpers.isPlatformAndroid()) pauseButton.SetActive(true);

    }

    public static void DisplayAltarUI()
    {
        instance.altarUI.SetActive(true);
        instance.pauseMenu.PauseGame(false);
        InputManager.setSelectedObject(instance.altarFirstSelected);
    }

    public static void HideAltarUI()
    {
        instance.altarUI.SetActive(false);
        instance.pauseMenu.ResumeGame();
    }

    public static void HitEnnemy(GameObject target, HitInfo hitInfo)
    {
        if (!dictObjectToEnnemy.ContainsKey(target)) return;
        dictObjectToEnnemy[target].Hit(hitInfo);
    }

    public static void HitObject(GameObject target, HitInfo hitInfo)
    {
        if (!dictObjectToHitable.ContainsKey(target)) return;
        dictObjectToHitable[target].Hit(hitInfo);
    }
    

    public void SelectUpgradePoint()
    {
        PlayerManager.AcquireUpgradePoint();
        altar.DepleteAltar();
        HideAltarUI();
    }

    public void Close()
    {
        HideAltarUI();
    }

    public static void DisplayLoseScreen()
    {
        instance.pauseMenu.PauseGame(false);
        instance.loseScreen.SetActive(true);
        InputManager.setSelectedObject(instance.loseScreenFirstSelected);
    }


    private void OnDestroy()
    {
        dictObjectToEnnemy = new Dictionary<GameObject, Ennemy>();
        dictObjectToInteractable = new Dictionary<GameObject, IInteractable>();
        dictObjectToResource = new Dictionary<GameObject, IResource>();
    }

    public static PowerDisplay AddPowerDisplay()
    {
        PowerDisplay powerDisplay = Instantiate(instance.powerDisplayPrefab, instance.powersDisplayLayout);
        return powerDisplay;
    }

    public static Slider getBossHealthbar()
    {
        return instance.bossHealthbar;
    }


}
