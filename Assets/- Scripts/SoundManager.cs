using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditorInternal;

/*
[CustomEditor(typeof(SoundManager))]
public class SoundEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

            var enum_names = System.Enum.GetNames(typeof(sfx));
            //var enum_label = enum_names.GetValue(pos) as string;
            serializedObject.Update();

            
            // Make names nicer to read (but won't exactly match enum definition).
            SoundManager soundManager = (SoundManager)target;
            int i = 0;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("audioClips"));

            foreach (sfxContainer container in soundManager.audioClips) {
            // Make names nicer to read (but won't exactly match enum definition).
            String enum_label = ObjectNames.NicifyVariableName(enum_names[i]);
            GUILayout.Label(enum_label);
            GUILayout.BeginHorizontal();

            SerializedProperty specialProp = serializedObject.FindProperty($"audioClips[{i}]");
            specialProp.floatValue = EditorGUILayout.FloatField(1f);

            //container.volume = EditorGUILayout.Slider(1f, 0, 2);
            //container.audioClip = (AudioClip)EditorGUILayout.ObjectField(container.audioClip, typeof(AudioClip));
            //Debug.Log(container.audioClip);
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            i ++;
            }

            serializedObject.ApplyModifiedProperties();

    }
}
*/



public enum sfx {
    jump,
    shoot,
    shieldUp,
    shieldDown,
    bulletOnShield,
    bulletOnGround,
    ennemyExplosion,
    ennemyShoots,
    gravitySwitch,
    laserWarmUp,
    playerHit,
};

public class SoundManager : MonoBehaviour
{
    AudioSource MusicSource;
    [SerializeField] List<AudioClip> musics;
    float LowPitchRange = 0.9f;
    float HighPitchRange = 1.1f;
    AudioClip sfxClip;
    List<clip> audioIds;
    

    [Header("Audio Clips")]
    [SerializeField] AudioClip laserWarmUp;
    [SerializeField] AudioClip jump;
    [SerializeField] AudioClip shoot;
    [SerializeField] AudioClip shieldUp;
    [SerializeField] AudioClip shieldDown;
    [SerializeField] AudioClip bulletOnShield;
    [SerializeField] AudioClip bulletOnGround;
    [SerializeField] AudioClip ennemyExplosion;
    [SerializeField] AudioClip ennemyShoots;
    [SerializeField] AudioClip gravitySwitch;

    [Header("Audio Sources")]
    [SerializeField] AudioSource laserSource;
    [SerializeField] AudioSource musicSource;
    float volume;
    bool laserSourcePlaying = false;
    static int SIZE = System.Enum.GetValues(typeof(sfx)).Length;
    //[NamedArray(typeof(sfx))] public AudioClip[] audioClips = new AudioClip[SIZE];
    
    [HideInInspector] List<sfxContainer> controlGroup = new List<sfxContainer>();
    Dictionary<sfx, AudioClip> sfxDictionary;
    Dictionary<sfx, float> volumeDictionary;
    [Header(" ")]
    [NamedArray(typeof(sfx))] public sfxContainer[] audioClips;// = new sfxContainer[SIZE];
    //public sfxContainer[] test;

    sfxContainer CloneContainer(sfxContainer container) {
        return new sfxContainer(container.audioClip, container.volume);
    }

    void OnValidate()
    {

        int j = 0;
        foreach(sfxContainer audioClip in audioClips) {
            if(audioClip.audioClip == null) {
                var tag = Enum.GetValues(typeof(sfx)).Cast<sfx>().ToList()[j];
                Debug.LogWarning("Field " + tag + " is empty");
            }
            j ++;
        }

        if (controlGroup.Count != SIZE) {      //case where the control group is not initialized or enum was modified
            j = 0;
            controlGroup.Clear();
            foreach (sfxContainer container in audioClips) {
                controlGroup.Add(CloneContainer(container));
                j ++;
            }
        }

        if (audioClips.Length > SIZE)   //case where the user tries to increment the list or enum was modified
        {
            Array.Resize(ref audioClips, SIZE);
        }

        else if (audioClips.Length < SIZE) {    //case where the user tries to decrement the list or enum was modified
            Array.Resize(ref audioClips, SIZE);
            for (int i = 0; i < controlGroup.Count; i ++) {     //we don't know which item was deleted, so we have to clone them all back
                audioClips[i] = CloneContainer(controlGroup[i]);
            }
        }

        else {
            int nbDiff = 0;
            List<int> diffIndex = new List<int>();  //list of index where the control groups differs from the actual data

            for (int i = 0; i < audioClips.Length; i ++) {     //we check every value to see which sfxElements where modified
                if (!controlGroup[i].Equals(audioClips[i])){
                    nbDiff ++;
                    diffIndex.Add(i);
                }
            }

            if (nbDiff > 1) {   //it is only possible to have more than 1 difference if the player swapped two values

                foreach (int index in diffIndex) {      //in which case we change back both values
                    audioClips[index] = CloneContainer(controlGroup[index]);
                }
            }
            else if (nbDiff == 1) controlGroup[diffIndex[0]] = CloneContainer(audioClips[diffIndex[0]]);    
            //if the player modified only one value, we update the control goup
        }
    }

    public static SoundManager instance;

    struct clip{
        public sfx type;
        public GameObject audio;

        public clip(sfx type, GameObject audio) {    //Constructor
            this.type = type;
            this.audio = audio;
        }
    }

    private void Awake()
    {
        audioIds = new List<clip>();
        //PlayMusic(musics[0]);
        instance = this;
        List<sfx> sfxList = Enum.GetValues(typeof(sfx)).Cast<sfx>().ToList();

        int i = 0;
        sfxDictionary = new Dictionary<sfx, AudioClip>();
        foreach (sfx sfxElement in sfxList) {
            sfxDictionary[sfxElement] = audioClips[i].audioClip;
            i++;
        }

        i = 0;
        volumeDictionary = new Dictionary<sfx, float>();
        foreach (sfx sfxElement in sfxList) {
            volumeDictionary[sfxElement] = audioClips[i].volume;
            i++;
        }
    }
    

    void PlayMusic(AudioClip music)
    {
       musicSource.clip = music;
       musicSource.Play();
    }

    public void PlayLaser()
    {
        laserSource.Play();
    }

    public void StopLaser()
    {
        laserSource.Stop();
    }

    public void StopTime()
    {
        musicSource.Pause();
        laserSourcePlaying = laserSource.isPlaying;
        laserSource.Pause();
        if (audioIds.Count > 0) {
            foreach (clip audioId in audioIds) {
                if (audioId.audio != null) {
                    audioId.audio.GetComponent<AudioSource>().Pause();
                }
            }
        }
    }

    public void ResumeTime()
    {
        //MusicSource.Play();
        if (laserSourcePlaying) laserSource.Play();
        if (audioIds.Count > 0) {
            foreach (clip audioId in audioIds) {
                if (audioId.audio != null) {
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
