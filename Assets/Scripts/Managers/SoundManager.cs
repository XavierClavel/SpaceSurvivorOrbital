using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Linq;
using DG.Tweening;
using UnityEngine.Serialization;

public enum sfx
{
    jump,
    shoot,
    bulletOnGround,
    ennemyExplosion,
    playerHit,
    breakResource,
    reload
};

[Serializable]
public enum gameScene
{
    titleScreen,
    ship,
    planetIce,
    planetDesert,
    planetMushroom,
    planetStorm,
    planetJungle,
    shop,
    win,
}

public class SoundManager : MonoBehaviour
{
    private static Dictionary<gameScene, AudioSource> dictSceneToMusic;
    
    static float LowPitchRange = 0.9f;
    static float HighPitchRange = 1.1f;
    List<clip> audioIds;

    private static float sfxVolume = 1f;



    [Header("Audio Sources")]
    [SerializeField] private AudioSource titleScreenMusic;
    [SerializeField] private AudioSource shipMusic;
    [SerializeField] private AudioSource planetJungleMusic;
    [SerializeField] private AudioSource planetIceMusic;
    [SerializeField] private AudioSource planetDesertMusic;
    [SerializeField] private AudioSource planetStormMusic;
    [SerializeField] private AudioSource planetMushroomMusic;
    [SerializeField] private AudioSource shopMusic;
    [SerializeField] private AudioSource winMusic;

    [Header(" ")]
    //public sfxContainer[] test;
    private static AudioSource currentMusicSource;
    [SerializeField] private gameScene currentScene;


    public static SoundManager instance = null;

    struct clip
    {
        public string type;
        public GameObject audio;

        public clip(string type, GameObject audio)
        {    //Constructor
            this.type = type;
            this.audio = audio;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            dictSceneToMusic = new Dictionary<gameScene, AudioSource>
            {
                { gameScene.titleScreen, titleScreenMusic},
                { gameScene.ship, shipMusic},
                { gameScene.shop, shopMusic},
                { gameScene.planetDesert, planetDesertMusic},
                { gameScene.planetIce, planetIceMusic},
                { gameScene.planetMushroom, planetMushroomMusic},
                { gameScene.planetStorm, planetStormMusic},
                { gameScene.planetJungle, planetJungleMusic},
                { gameScene.win, winMusic},
            };
            onSceneChange(currentScene);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        
        audioIds = new List<clip>();
        
        List<sfx> sfxList = Enum.GetValues(typeof(sfx)).Cast<sfx>().ToList();

    }

    private void Start()
    {
        LowPitchRange = ConstantsData.audioMinPitch;
        HighPitchRange = ConstantsData.audioMaxPitch;
    }

    public static void onSceneChange(gameScene newScene)
    {
        AudioSource previousMusicSource = currentMusicSource;
        previousMusicSource?.DOFade(0f, 1f).SetEase(Ease.Linear).OnComplete(previousMusicSource.Stop);
        currentMusicSource = dictSceneToMusic[newScene];
        currentMusicSource.Play();
        currentMusicSource.DOFade(1f, 1f).SetEase(Ease.Linear);
    }


    public void StopTime()
    {
        titleScreenMusic.Pause();
        if (audioIds.Count > 0)
        {
            foreach (clip audioId in audioIds)
            {
                if (audioId.audio != null)
                {
                    audioId.audio.GetComponent<AudioSource>().Pause();
                }
            }
        }
    }

    public void ResumeTime()
    {
        //MusicSource.Play();
        if (audioIds.Count > 0)
        {
            foreach (clip audioId in audioIds)
            {
                if (audioId.audio != null)
                {
                    audioId.audio.GetComponent<AudioSource>().Play();
                }
            }
        }
    }

    public static void PlaySfx(Transform pos, string key)
    {
        PlaySfx(pos.position, key);
    }

    public static void PlaySfx(string key)
    {
        PlaySfx(Vector3.zero, key);
    }

    public static void PlaySfx(Vector3 pos, string key)
    {
        float randomPitch = UnityEngine.Random.Range(LowPitchRange, HighPitchRange);
        if (!ScriptableObjectManager.dictKeyToSfx.ContainsKey(key))
        {
            Debug.LogWarning($"Sfx key '{key}' not found");
            return;
        }
        Sfx sfx = ScriptableObjectManager.dictKeyToSfx[key];

        instance.PlayClipAt(sfx.getClip(), pos, randomPitch, key, sfxVolume * sfx.getVolume());

    }

    void PlayClipAt(AudioClip clip, Vector3 pos, float pitch, string type, float volume)       //create temporary audio sources for each sfx in order to be able to modify pitch
    {
        GameObject audioContainer = new GameObject("TempAudio"); // create the temporary object
        audioContainer.transform.position = pos; // set its position to localize sound
        AudioSource aSource = audioContainer.AddComponent<AudioSource>();
        aSource.pitch = pitch;
        aSource.clip = clip;
        aSource.volume = volume;
        clip audioId = new clip(type, audioContainer);
        audioIds.Add(audioId);
        aSource.Play(); // start the sound
        StartCoroutine(SfxTimer(clip.length, audioId));
    }

    IEnumerator SfxTimer(float time, clip audioId)  //2nd argument initially aSource
    {
        yield return new WaitForSeconds(time);
        audioIds.Remove(audioId);
        Destroy(audioId.audio);
    }

}
