using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ResetManager.Reset();
    }


}

public static class ResetManager
{
    public static void Reset()
    {
        NodeManager.Reset();
        DataSelector.Reset();
        PlayerManager.ResetTimer();
        PlanetSelectionManager.Reset();
        Planet.Reset();
    }
}