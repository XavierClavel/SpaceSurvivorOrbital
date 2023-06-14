using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeat : MonoBehaviour
{
    public GameObject winText;
    
    private void OnDestroy()
    {
        winText.SetActive(true);
    }
}
