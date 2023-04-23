using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShipManager : MonoBehaviour
{

    public List<Upgrade> buyableUpgrades;
    public List<Button> buttons;
    public List<Image> image;

    private void Start()
    {
        Debug.Log(PlayerManager.amountGreen);
        Debug.Log(PlayerManager.amountOrange);
    }

    public void Next()
    {
        SceneManager.LoadScene("Level 2");
    }

    public void Buy(int id)//int costOrange, int costGreen)//, change type, int increase)
    {
        Upgrade upgrade = buyableUpgrades[id];
        if (PlayerManager.HasResources(upgrade.costGreen, upgrade.costOrange))
        {
            PlayerManager.Upgrade(upgrade.type, upgrade.increase);
            //buttons[id].interactable = false;
            image[id].color = Color.green;
        }
        else
        {
            image[id].color = Color.red;
        }
    }

}
