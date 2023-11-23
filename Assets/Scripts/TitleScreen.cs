using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour, UIPanel
{
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
    }


}

public static class ResetManager
{
    public static void Reset()
    {
        NodeManager.Reset();
        DataSelector.Reset();
        PlayerManager.ResetTimer();
        PlayerManager.Reset();
        PlanetSelectionManager.Reset();
        Planet.Reset();
    }
}