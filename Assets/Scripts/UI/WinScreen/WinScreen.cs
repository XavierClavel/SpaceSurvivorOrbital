using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    [SerializeField] private GameObject unlockedPanel;
    [SerializeField] private RectTransform unlockedLayout;
    [SerializeField] private Image unlockedPrefab;
    
    //The current progress step that was just achieved, Start if nothing new
    private static availability progressMade = availability.Boss1;

    public static void setProgress(availability progress = availability.Start)
    {
        progressMade = progress;
    }
    // Start is called before the first frame update
    void Start()
    {
        if (progressMade == availability.Start || !SaveManager.addProgression(progressMade))
        {
            unlockedPanel.SetActive(false);
            return;
        }
        
        unlockedLayout.KillAllChildren();

        var newPowers = ScriptableObjectManager.getPowers().Where(it => it.isDiscoveredAt(progressMade));
        foreach (var newPower in newPowers)
        {
            Image powerImage = Instantiate(unlockedPrefab, unlockedLayout, true);
            powerImage.transform.localScale = Vector3.one;
            powerImage.transform.position = Vector3.zero;
            powerImage.sprite = newPower.getIcon();
            powerImage.name = $"Unlocked {newPower.getKey()}";
        }
        
        var newEquipments = ScriptableObjectManager.getEquipments().Where(it => it.isDiscoveredAt(progressMade));
        foreach (var newEquipment in newEquipments)
        {
            Image equipmentImage = Instantiate(unlockedPrefab, unlockedLayout, true);
            equipmentImage.transform.localScale = Vector3.one;
            equipmentImage.transform.position = Vector3.zero;
            equipmentImage.sprite = newEquipment.getIcon();
            equipmentImage.name = $"Unlocked {newEquipment.getKey()}";
        }
    }

}
