using System;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectManager : MonoBehaviour, IMonsterStele, IPlayerEvents, IEggListener, IEnemyListener
{
    [Header("UI")]
    public LayoutManager healthBar;
    [SerializeField] GameObject pauseButton;
    [SerializeField] GameObject altarUI;
    [SerializeField] GameObject fountainUI;
    [SerializeField] GameObject altarFirstSelected;
    [SerializeField] GameObject loseScreen;
    [SerializeField] GameObject loseScreenFirstSelected;
    [SerializeField] private GameObject radar;
    [SerializeField] private Slider bossHealthbar;
    [SerializeField] private GameObject steleDisplay;
    public Transform powersDisplayLayout;
    public PowerDisplay powerDisplayPrefab;
    public Slider reloadSlider;
    public Slider dashSlider;
    public PartialResourceManager sliderOrange;
    public PartialResourceManager sliderGreen;
    public PartialResourceManager sliderBlue;

    [SerializeField] public TextMeshProUGUI altarMonsterTotal;
    [SerializeField] public TextMeshProUGUI altarMonsterCurrent;
    [SerializeField] private Image characterDisplay;
    [SerializeField] private TextMeshProUGUI bulletsDisplay;
    [SerializeField] public GameObject objectiveDestroy;
    [SerializeField] public GameObject objectiveBoss;
    [SerializeField] public GameObject shipReady;
    [SerializeField] private GameObject chest;

    public ParticleSystem firePS;
    public ParticleSystem lightningPS;
    public ParticleSystem icePS;
    public GameObject upgradePS;
    public ParticleSystem shipAppearPS;
    public ParticleSystem difficultyPS;
    public ParticleSystem monsterKillPS;

    [Header("Interactors")]
    public Interactor gun;

    public static ObjectManager instance;
    public static Altar altar;
    public static Fountain fountain;
    public static ObjectIndicator spaceshipIndicator;
    public static GameObject spaceship;

    public static Dictionary<GameObject, Ennemy> dictObjectToEnnemy = new Dictionary<GameObject, Ennemy>();
    public static Dictionary<GameObject, IResource> dictObjectToResource = new Dictionary<GameObject, IResource>();
    public static Dictionary<GameObject, IInteractable> dictObjectToInteractable = new Dictionary<GameObject, IInteractable>();
    public static Dictionary<GameObject, Breakable> dictObjectToHitable = new Dictionary<GameObject, Breakable>();
    static int amountEggs = 0;
    private int amountDens = 0;
    private int amountDensDestroyed = 0;
    private Camera cam;
    private Vector3 camHalfSize;

    [Header("Prefabs")] 
    [SerializeField] private GameObject resourceItemOrange;
    [SerializeField] private GameObject resourceItemGreen;
    [SerializeField] private GameObject resourceItemBlue;
    private GameObjectPool poolResourceOrange;
    private GameObjectPool poolResourceGreen;
    private GameObjectPool poolResourceBlue;

    private ComponentPool<ParticleSystem> enemiesExplosionPsPool;


    public static void DisplaySpaceship()
    {
        spaceship.SetActive(true);
    }

    public static void ActivateRadar()
    {
        instance.radar.SetActive(true);
    }

    public static void HideSpaceship() { spaceship.SetActive(false); }

    public void onEggSpawned(Resource resource)
    {
        amountEggs++;
    }

    public void onEggDestroyed(Resource resource)
    {
        ObjectManager.SpawnResources(
            resource.resourceType,
            resource.transform.position,
            (int)(resource.dropInterval.getRandom() * BonusManager.current.getBonusResources()));
        
        amountEggs--;
        if (amountEggs > 0) return;
        
        //PlayerManager.AcquireUpgradePoint();
        //SoundManager.PlaySfx(PlayerController.instance.transform, key: "Collectible_Blue");
        //UpgradeUpDisplay();
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
        EventManagers.monsterSteles.registerListener(this);
        EventManagers.eggs.registerListener(this);
        EventManagers.enemies.registerListener(this);

        poolResourceGreen = new GameObjectPool(resourceItemGreen);
        poolResourceOrange = new GameObjectPool(resourceItemOrange);
        poolResourceBlue = new GameObjectPool(resourceItemBlue);
    }

    private void Start()
    {
        characterDisplay.sprite = DataSelector.getSelectedCharacter().getIcon();
        cam = Camera.main;
        camHalfSize = new Vector3(cam.pixelWidth * 0.5f, cam.pixelHeight * 0.5f, 0);
        enemiesExplosionPsPool = new ComponentPool<ParticleSystem>(monsterKillPS).setTimer(1f);
    }

    public static void recallItemOrange(GameObject go) => instance.poolResourceOrange.recall(go);
    public static void recallItemGreen(GameObject go) => instance.poolResourceGreen.recall(go);
    public static void recallItemBlue(GameObject go) => instance.poolResourceBlue.recall(go);

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

    public static void SpawnResourcesBlue(Vector3 position, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            instance.poolResourceBlue.get(position + instance.randomPos());
        }
    }

    public static void SpawnResources(type resourceType, Vector3 position, int amount)
    {
        if (resourceType == type.green) SpawnResourcesGreen(position, amount);
        else if (resourceType == type.orange) SpawnResourcesOrange(position, amount);
        else if (resourceType == type.blue) SpawnResourcesBlue(position, amount);
        ResourcesAttractor.ForceUpdate();
    }
    
    public static void SpawnResources(Vector3 position, int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            SpawnResources(Helpers.getRandomEnum<type>(), position, 1);
        }
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
        PauseMenu.instance.PauseGame(false);
        InputManager.setSelectedObject(instance.altarFirstSelected);
    }

    public static void DisplayFountainUI()
    {
        instance.fountainUI.SetActive(true);
        PauseMenu.instance.PauseGame(false);
    }
    public static void FountainHeal()
    {
        PlayerController.Heal(PlayerController.instance.maxHealth);
        fountain.DepleteFountain();
        instance.fountainUI.SetActive(false);
        PauseMenu.instance.ResumeGame();
    }

    public static void HideAltarUI()
    {
        instance.altarUI.SetActive(false);
        PauseMenu.instance.ResumeGame();
    }

    public static void HideFountainUI()
    {
        instance.fountainUI.SetActive(false);
        PauseMenu.instance.ResumeGame();
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
        PauseMenu.instance.PauseGame(false);
        instance.loseScreen.SetActive(true);
        InputManager.setSelectedObject(instance.loseScreenFirstSelected);
    }


    private void OnDestroy()
    {
        dictObjectToEnnemy = new Dictionary<GameObject, Ennemy>();
        dictObjectToInteractable = new Dictionary<GameObject, IInteractable>();
        dictObjectToResource = new Dictionary<GameObject, IResource>();
        
        EventManagers.eggs.unregisterListener(this);
        EventManagers.monsterSteles.unregisterListener(this);
        EventManagers.enemies.unregisterListener(this);
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
        enemiesExplosionPsPool.get(stele.transform.position);
        amountDensDestroyed++;
        amountDens--;
        instance.altarMonsterCurrent.SetText(amountDensDestroyed.ToString());

        if (amountDens > 0) return;
        Instantiate(instance.shipAppearPS, PlayerController.instance.transform);
        instance.objectiveDestroy.SetActive(false);
        instance.shipReady.SetActive(true);
        SoundManager.PlaySfx(PlayerController.instance.transform, key: "Monster_Altar_Destroy");
        DisplaySpaceship();
    }

    public static void onBossDestroyed()
    {
        instance.objectiveBoss.SetActive(false);
        instance.shipReady.SetActive(true);
    }

    public void onSteleSpawned(MonsterStele stele)
    {
        amountDens++;
        altarMonsterTotal.text = amountDens.ToString();
    }

    public static void DisableSteleDisplay()
    {
        instance.steleDisplay.SetActive(false);
        instance.objectiveDestroy.SetActive(false);
        instance.objectiveBoss.SetActive(true);
    }

    public bool onPlayerDeath()
    {
        return false;
    }

    public bool onPlayerHit(bool shieldHit)
    {
        return false;
    }

    public static void UpdateBulletsDisplay(int amount)
    {
        instance.bulletsDisplay.SetText($"x{amount}");
    }

    public static void SpawnChest(Vector3 position)
    {
        Instantiate(instance.chest, position, Quaternion.identity);
    }
    
    public void onEnnemyDeath(Ennemy enemy)
    {
        if (enemy.doUseKillPs) enemiesExplosionPsPool.get(enemy.transform.position);
    }

    Tween sliderTween;
    public void onDash()
    {
            StartCoroutine(nameof(DashUi));
    }

    public IEnumerator DashUi()
    {
        dashSlider.gameObject.SetActive(true);
        dashSlider.value = 0f;
        sliderTween = dashSlider.DOValue(1f, PlayerController.instance.dashCooldown).SetEase(Ease.Linear);
        yield return new WaitForSeconds(PlayerController.instance.dashCooldown);
        dashSlider.gameObject.SetActive(false);
    }
}
