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
    private float canvasX;
    private float canvasY;

    private static SceneTransitionManager instance;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        Canvas.ForceUpdateCanvases();
        Rect rec = GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect;
        canvasX = rec.width;
        canvasY = rec.height;
        
        imageTransform.sizeDelta = new Vector2(canvasX, canvasY);
        maskTransform.sizeDelta = 2 * canvasX * Vector2.one;
        if (!doFadeIn) return;
        overlay.SetActive(true);
        StartCoroutine(nameof(AnimationEnterScene));
    }
    
    public void ToTitleScreen()
    {
        Debug.Log("method called");
        LocalTransitionToScene(gameScene.titleScreen);
    }
    
    public void LocalTransitionToScene(gameScene newScene)
    {
        
        SoundManager.onSceneChange(newScene);
        overlay.SetActive(true);
        imageTransform.sizeDelta = new Vector2(canvasX, canvasY);
        maskTransform.sizeDelta = Vector2.zero;
        maskTransform.DOSizeDelta(2 * canvasX * Vector2.one, 1f)
            .SetEase(Ease.InOutQuint)
            .SetUpdate(true)
            .SetDelay(0.2f)
            .OnComplete(delegate
            {
                PauseMenu.ResumeTime();
                SceneManager.LoadScene(SceneToName(newScene));
            });
    }

    public static void TransitionToScene(gameScene newScene)
    {
        instance.LocalTransitionToScene(newScene);
    }
    
    public static void TransitionToScene(PlanetData planetData)
    {
        TransitionToScene(planetData.getScene());
    }

    public static string SceneToName(gameScene scene)
    {
        return scene switch
        {
            gameScene.titleScreen => Vault.scene.TitleScreen,
            gameScene.ship => Vault.scene.Ship,
            gameScene.planetIce => Vault.scene.Planet,
            gameScene.planetDesert => Vault.scene.Planet,
            gameScene.planetMushroom => Vault.scene.Planet,
            gameScene.planetStorm => Vault.scene.Planet,
            gameScene.planetJungle => Vault.scene.Planet,
            gameScene.shop => Vault.scene.Shop,
            gameScene.win => Vault.scene.Win,
            _ => Vault.scene.TitleScreen
        };
    }
    
    

    IEnumerator AnimationEnterScene()
    {
        yield return null;
        maskTransform.DOSizeDelta(Vector2.zero, 1f).SetEase(Ease.InOutQuint);
    }

}
