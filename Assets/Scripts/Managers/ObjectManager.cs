using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] PauseMenu pauseMenu;
    [SerializeField] GameObject altarUI;
    static ObjectManager instance;
    public static Altar altar;

    public static Dictionary<GameObject, Ennemy> dictObjectToEnnemy = new Dictionary<GameObject, Ennemy>();
    public static Dictionary<GameObject, Resource> dictObjectToResource = new Dictionary<GameObject, Resource>();
    public static Dictionary<GameObject, IInteractable> dictObjectToInteractable = new Dictionary<GameObject, IInteractable>();

    private void Awake()
    {
        instance = this;
    }

    public static void DisplayAltarUI()
    {
        instance.altarUI.SetActive(true);
        instance.pauseMenu.PauseGame(false);
    }

    public static void HideAltarUI()
    {
        instance.altarUI.SetActive(false);
        instance.pauseMenu.ResumeGame();
    }

    bool Transaction()
    {
        if (PlayerManager.amountViolet == 0) return false;
        PlayerManager.SpendPurple(1);
        PlayerController.instance.SpendVioletCapsule();
        Destroy(altar.gameObject);

        return true;
    }

    public void SelectMinerBot()
    {
        if (!Transaction()) return;

        PlayerController.instance.SpawnMinerBot();
        HideAltarUI();
        PlayerManager.AcquirePower(power.minerBot);
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


    private void OnDestroy()
    {
        dictObjectToEnnemy = new Dictionary<GameObject, Ennemy>();
        dictObjectToInteractable = new Dictionary<GameObject, IInteractable>();
        dictObjectToResource = new Dictionary<GameObject, Resource>();
    }


}
