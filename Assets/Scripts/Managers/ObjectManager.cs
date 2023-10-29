using System.Collections;
using System.Collections.Generic;
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
    public GameData gameData;
    public Slider reloadSlider;
    public Transform armTransform;

    [Header("Interactors")]
    public Interactor gun;

    public static ObjectManager instance;
    public static Altar altar;
    public static ObjectIndicator spaceshipIndicator;

    public static Dictionary<GameObject, Ennemy> dictObjectToEnnemy = new Dictionary<GameObject, Ennemy>();
    public static Dictionary<GameObject, IResource> dictObjectToResource = new Dictionary<GameObject, IResource>();
    public static Dictionary<GameObject, IInteractable> dictObjectToInteractable = new Dictionary<GameObject, IInteractable>();
    public static Dictionary<GameObject, Breakable> dictObjectToHitable = new Dictionary<GameObject, Breakable>();
    static int amountEggs = 0;
    private static int amountDens = 0;

    public static void registerEggSpawned() {
        amountEggs++;
    }

    public static void registerEggDestroyed() {
        amountEggs--;
        if (amountEggs == 0)
        {
            PlayerManager.AcquireUpgradePoint();
        }
    }
    
    public static void registerDenSpawned() {
        amountDens++;
    }

    public static void registerDenDestroyed() {
        amountDens--;
        if (amountDens == 0)
        {
            PlayerManager.AcquireUpgradePoint();
        }
    }

    private void Awake()
    {
        instance = this;
        amountEggs = 0;
        amountDens = 0;
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

    bool Transaction()
    {
        Destroy(altar.gameObject);
        return true;
    }

    public void SelectMinerBot()
    {
        if (!Transaction()) return;

        PlayerController.instance.SpawnMinerBot();
        HideAltarUI();
        //PlayerManager.AcquirePower(powerType.creusetout);
    }

    public void SelectUpgradePoint()
    {
        if (!Transaction()) return;
        PlayerManager.AcquireUpgradePoint();
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


}
