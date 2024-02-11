using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour, IMonsterStele, IResourceListener
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
    [SerializeField] private GameObject steleDisplay;
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
    private int amountDens = 0;
    private int amountDensDestroyed = 0;

    [Header("Prefabs")] 
    [SerializeField] private GameObject resourceItemOrange;
    [SerializeField] private GameObject resourceItemGreen;
    private GameObjectPool poolResourceOrange;
    private GameObjectPool poolResourceGreen;


    public static void DisplaySpaceship()
    {
        spaceship.SetActive(true);
    }

    public static void ActivateRadar()
    {
        instance.radar.SetActive(true);
    }

    public static void HideSpaceship() { spaceship.SetActive(false); }

    public void onResourceSpawned(Resource resource)
    {
        amountEggs++;
    }

    public void onResourceDestroyed(Resource resource)
    {
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
    


    private void Awake()
    {
        instance = this;
        amountEggs = 0;
        amountDens = 0;
        amountDensDestroyed = 0;
        radar.SetActive(false);
        if (Helpers.isPlatformAndroid()) pauseButton.SetActive(true);
        MonsterStele.registerListener(this);
        Resource.registerListener(this);

        poolResourceGreen = new GameObjectPool(resourceItemGreen);
        poolResourceOrange = new GameObjectPool(resourceItemOrange);
    }

    public static void recallItemOrange(GameObject go) => instance.poolResourceOrange.recall(go);
    public static void recallItemGreen(GameObject go) => instance.poolResourceGreen.recall(go);

    public static void SpawnResourcesOrange(Vector3 position, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            instance.poolResourceOrange.get(position + instance.randomPos());
        }
    }
    
    
    public static void SpawnResourcesGreen(Vector3 position, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            instance.poolResourceGreen.get(position + instance.randomPos());
        }
    }

    public static void SpawnResources(type resourceType, Vector3 position, int amount)
    {
        if (resourceType == type.green) SpawnResourcesGreen(position, amount);
        else if (resourceType == type.orange) SpawnResourcesOrange(position, amount);
        ResourcesAttractor.ForceUpdate();
    }
    
    public static void SpawnResources(Vector3 position, int amount)
    {
        SpawnResources(Helpers.getRandomEnum<type>(), position, amount);
    }
    
    Vector3 randomPos()
    {
        float signA = Helpers.getRandomSign();
        float signB = Helpers.getRandomSign();
        return signA * Helpers.getRandomFloat(1.5f) * Vector2.up + signB * Helpers.getRandomFloat(1.5f) * Vector2.right;
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
        
        Resource.unregisterListener(this);
        MonsterStele.unregisterListener(this);
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

    public void onSteleDestroyed(MonsterStele stele)
    {
        amountDensDestroyed++;
        Debug.Log($"Registered stele destroyed, current amount destroyed is now {amountDensDestroyed}");
        amountDens--;
        instance.altarMonsterCurrent.SetText(amountDensDestroyed.ToString());
        Debug.Log($"Text value is {instance.altarMonsterCurrent.text}");

        if (amountDens > 0) return;
        Instantiate(instance.shipAppearPS, PlayerController.instance.transform);
        SoundManager.PlaySfx(PlayerController.instance.transform, key: "Monster_Altar_Destroy");
        DisplaySpaceship();
    }

    public void onSteleSpawned(MonsterStele stele)
    {
        amountDens++;
        altarMonsterTotal.text = amountDens.ToString();
    }

    public static void DisableSteleDisplay()
    {
        instance.steleDisplay.SetActive(false);
    }
}
