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
    
    public void Test(gameScene newScene) {}

    public void ToTitleScreen()
    {
        Debug.Log("method called");
        LocalTransitionToScene(gameScene.titleScreen);
    }
    
    public void LocalTransitionToScene(gameScene newScene)
    {
        SoundManager.onSceneChange(newScene);
        overlay.SetActive(true);
        imageTransform.sizeDelta = new Vector2(Camera.main.pixelWidth, Camera.main.pixelHeight);
        maskTransform.sizeDelta = Vector2.zero;
        maskTransform.DOSizeDelta(2 * Camera.main.pixelWidth * Vector2.one, 1f).SetEase(Ease.InOutQuint).SetUpdate(true)
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
            _ => Vault.scene.TitleScreen
        };
    }
    
    

    IEnumerator AnimationEnterScene()
    {
        yield return null;
        maskTransform.DOSizeDelta(Vector2.zero, 1f).SetEase(Ease.InOutQuint);
    }

}
