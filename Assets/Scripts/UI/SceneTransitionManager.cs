using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private GameObject overlay;

    [SerializeField] private RectTransform maskTransform;

    [SerializeField] private RectTransform imageTransform;
    [SerializeField] private bool doFadeIn;

    private static SceneTransitionManager instance;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        imageTransform.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        maskTransform.sizeDelta = 2 * Camera.main.pixelWidth * Vector2.one;
        if (!doFadeIn) return;
        overlay.SetActive(true);
        StartCoroutine(nameof(AnimationEnterScene));
    }

    public static void TransitionToScene(string newScene)
    {
        instance.overlay.SetActive(true);
        instance.imageTransform.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        instance.maskTransform.sizeDelta = Vector2.zero;
        instance.maskTransform.DOSizeDelta(2 * Camera.main.pixelWidth * Vector2.one, 1f).SetEase(Ease.InOutQuint)
            .OnComplete(delegate { SceneManager.LoadScene(newScene); });
    }

    IEnumerator AnimationEnterScene()
    {
        yield return null;
        maskTransform.DOSizeDelta(Vector2.zero, 1f).SetEase(Ease.InOutQuint);
    }

}
