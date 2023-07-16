using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NodeManager.Reset();
        DataSelector.Reset();
        PlayerManager.ResetTimer();
    }


}
