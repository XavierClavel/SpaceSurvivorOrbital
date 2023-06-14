using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDefeat : MonoBehaviour
{
    public GameObject winText;
    
    private void OnDestroy()
    {
        Instantiate(winText);
        Time.timeScale = 0f;
        Time.fixedDeltaTime = 0f;
        SoundManager.instance.StopTime();

        PlayerController.instance.controls.Disable();
        if (!PlayerController.isPlayingWithGamepad) Cursor.visible = true;
    }
}
