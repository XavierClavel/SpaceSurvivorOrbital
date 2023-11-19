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
}

public class SoundManager : MonoBehaviour
{
    private static Dictionary<gameScene, AudioSource> dictSceneToMusic;
    
    float LowPitchRange = 0.9f;
    float HighPitchRange = 1.1f;
    List<clip> audioIds;

    private float sfxVolume = 1f;



    [Header("Audio Sources")]
    [SerializeField] private AudioSource titleScreenMusic;
    [SerializeField] private AudioSource shipMusic;
    [SerializeField] private AudioSource planetJungleMusic;
    [SerializeField] private AudioSource planetIceMusic;
    [SerializeField] private AudioSource planetDesertMusic;
    [SerializeField] private AudioSource planetStormMusic;
    [SerializeField] private AudioSource planetMushroomMusic;
    
    static int SIZE = Enum.GetValues(typeof(sfx)).Length;
    //[NamedArray(typeof(sfx))] public AudioClip[] audioClips = new AudioClip[SIZE];

    [HideInInspector] List<sfxContainer> controlGroup = new List<sfxContainer>();
    Dictionary<sfx, AudioClip> sfxDictionary;
    Dictionary<sfx, float> volumeDictionary;
    [Header(" ")]
    [NamedArray(typeof(sfx))] public sfxContainer[] audioClips;// = new sfxContainer[SIZE];
    //public sfxContainer[] test;
    private static AudioSource currentMusicSource;
    [SerializeField] private gameScene currentScene;

    sfxContainer CloneContainer(sfxContainer container)
    {
        return new sfxContainer(container.audioClip, container.volume);
    }



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
                { gameScene.planetDesert, planetDesertMusic},
                { gameScene.planetIce, planetIceMusic},
                { gameScene.planetMushroom, planetMushroomMusic},
                { gameScene.planetStorm, planetStormMusic},
                { gameScene.planetJungle, planetJungleMusic},
            };
            onSceneChange(currentScene);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        
        
        audioIds = new List<clip>();
        
        List<sfx> sfxList = Enum.GetValues(typeof(sfx)).Cast<sfx>().ToList();

        int i = 0;
        sfxDictionary = new Dictionary<sfx, AudioClip>();
        foreach (sfx sfxElement in sfxList)
        {
            sfxDictionary[sfxElement] = audioClips[i].audioClip;
            i++;
        }

        i = 0;
        volumeDictionary = new Dictionary<sfx, float>();
        foreach (sfx sfxElement in sfxList)
        {
            volumeDictionary[sfxElement] = audioClips[i].volume;
            i++;
        }
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

    public void PlaySfx(Transform pos, string key)
    {
        float randomPitch = UnityEngine.Random.Range(LowPitchRange, HighPitchRange);
        if (!ScriptableObjectManager.dictKeyToSfx.ContainsKey(key))
        {
            Debug.LogWarning($"Sfx key '{key}' not found");
            return;
        }
        Sfx sfx = ScriptableObjectManager.dictKeyToSfx[key];

        PlayClipAt(sfx.getClip(), pos.position, randomPitch, key, sfxVolume * sfx.getVolume());

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
