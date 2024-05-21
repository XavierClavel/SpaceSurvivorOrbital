using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Freezer : MonoBehaviour
{
    public Image redOverlay;
    // Appeler cette fonction pour geler l'�cran
    public void FreezeScreen(float duration)
    {
        StartCoroutine(FreezeCoroutine(duration));
    }

    private IEnumerator FreezeCoroutine(float duration)
    {
        redOverlay.gameObject.SetActive(true);
        // Arr�ter le temps
        Time.timeScale = 0f;

        // Attendre la dur�e sp�cifi�e en temps r�el
        yield return new WaitForSecondsRealtime(duration);

        // Reprendre le temps
        Time.timeScale = 1f;

        redOverlay.gameObject.SetActive(false);
    }
}
