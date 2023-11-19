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
    AudioClip sfxClip;
    List<clip> audioIds;



    [Header("Audio Sources")]
    [SerializeField] private AudioSource titleScreenMusic;
    [SerializeField] private AudioSource shipMusic;
    [SerializeField] private AudioSource planetJungleMusic;
    [SerializeField] private AudioSource planetIceMusic;
    [SerializeField] private AudioSource planetDesertMusic;
    [SerializeField] private AudioSource planetStormMusic;
    [SerializeField] private AudioSource planetMushroomMusic;
    
    
    
    float volume;
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

    void OnValidate()
    {

        int j = 0;
        foreach (sfxContainer audioClip in audioClips)
        {
            if (audioClip.audioClip == null)
            {
                var tag = Enum.GetValues(typeof(sfx)).Cast<sfx>().ToList()[j];
                Debug.LogWarning("Field " + tag + " is empty");
            }
            j++;
        }

        if (controlGroup.Count != SIZE)
        {      //case where the control group is not initialized or enum was modified
            j = 0;
            controlGroup.Clear();
            foreach (sfxContainer container in audioClips)
            {
                controlGroup.Add(CloneContainer(container));
                j++;
            }
        }

        if (audioClips.Length > SIZE)   //case where the user tries to increment the list or enum was modified
        {
            Array.Resize(ref audioClips, SIZE);
        }

        else if (audioClips.Length < SIZE)
        {    //case where the user tries to decrement the list or enum was modified
            Array.Resize(ref audioClips, SIZE);
            for (int i = 0; i < controlGroup.Count; i++)
            {     //we don't know which item was deleted, so we have to clone them all back
                audioClips[i] = CloneContainer(controlGroup[i]);
            }
        }

        else
        {
            int nbDiff = 0;
            List<int> diffIndex = new List<int>();  //list of index where the control groups differs from the actual data

            for (int i = 0; i < audioClips.Length; i++)
            {     //we check every value to see which sfxElements where modified
                if (!controlGroup[i].Equals(audioClips[i]))
                {
                    nbDiff++;
                    diffIndex.Add(i);
                }
            }

            if (nbDiff > 1)
            {   //it is only possible to have more than 1 difference if the player swapped two values

                foreach (int index in diffIndex)
                {      //in which case we change back both values
                    audioClips[index] = CloneContainer(controlGroup[index]);
                }
            }
            else if (nbDiff == 1) controlGroup[diffIndex[0]] = CloneContainer(audioClips[diffIndex[0]]);
            //if the player modified only one value, we update the control goup
        }
    }

    public static SoundManager instance = null;

    struct clip
    {
        public sfx type;
        public GameObject audio;

        public clip(sfx type, GameObject audio)
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
                { gameScene.ship, titleScreenMusic},
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

    public void PlaySfx(Transform pos, sfx type)
    {
        float randomPitch = UnityEngine.Random.Range(LowPitchRange, HighPitchRange);
        sfxClip = sfxDictionary[type];
        volume = volumeDictionary[type];

        PlayClipAt(sfxClip, pos.position, randomPitch, type, volume);

    }

    void PlayClipAt(AudioClip clip, Vector3 pos, float pitch, sfx type, float volume)       //create temporary audio sources for each sfx in order to be able to modify pitch
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
