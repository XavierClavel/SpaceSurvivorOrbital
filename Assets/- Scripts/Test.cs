using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Linq;
using UnityEditorInternal;


//Tells the Editor class that this will be the Editor for the WaveManager
[CustomEditor(typeof(Test))]
public class WaveManagerEditor : Editor
{
    //The array property we will edit
    SerializedProperty sfxObject;

    //The Reorderable list we will be working with
    ReorderableList list;

    private void OnEnable()
    {
        //Gets the wave property in WaveManager so we can access it. 
        sfxObject = serializedObject.FindProperty("test");
        Debug.Log(sfxObject);

        //Initialises the ReorderableList. We are creating a Reorderable List from the "wave" property. 
        //In this, we want a ReorderableList that is draggable, with a display header, with add and remove buttons        
        list = new ReorderableList(serializedObject, sfxObject, true, true, true, true);

        list.drawElementCallback = DrawListItems;
        list.drawHeaderCallback = DrawHeader;

    }

    void DrawListItems(Rect rect, int index, bool isActive, bool isFocused)
    {        
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index); //The element in the list

        // Create a property field and label field for each property. 

        // The 'mobs' property. Since the enum is self-evident, I am not making a label field for it. 
        // The property field for mobs (width 100, height of a single line)
        EditorGUI.PropertyField(
            new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight), 
            element.FindPropertyRelative("audioClip"),
            GUIContent.none
        );

        //EditorGUILayout.PropertyField(element.FindPropertyRelative("audioClip"), GUIContent.none, GUILayout.Width(8), GUILayout.ExpandWidth(true));




        // The 'quantity' property
        // The label field for quantity (width 100, height of a single line)
        EditorGUI.LabelField(new Rect(rect.x + 200, rect.y, 100, EditorGUIUtility.singleLineHeight), "Volume : ");

        //The property field for quantity (width 20, height of a single line)
        EditorGUI.PropertyField(
            new Rect(rect.x + 255, rect.y, 20, EditorGUIUtility.singleLineHeight),
            element.FindPropertyRelative("volume"),
            GUIContent.none
        );        

    }

    void DrawHeader(Rect rect)
    {
        string name = "Sfx";
        EditorGUI.LabelField(rect, name);
    }

    //This is the function that makes the custom editor work
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}

[Serializable]
public class sfxContainer : IEquatable<sfxContainer>
{
    public AudioClip audioClip;
    public float volume = 1;
    public sfxContainer(AudioClip audioClip, float volume) {
        this.audioClip = audioClip;
        this.volume = volume;
    }

    public bool Equals(sfxContainer other)
    {
        if (this.audioClip != other.audioClip) return false;
        if (this.volume != other.volume) return false;
        return true;
    }
}

public class Test : MonoBehaviour
{
    AudioSource MusicSource;
    [SerializeField] List<AudioClip> musics;
    float LowPitchRange = 0.9f;
    float HighPitchRange = 1.1f;
    AudioClip sfxClip;
    List<clip> audioIds;

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
    sfxContainer[] audioClips;// = new sfxContainer[SIZE];
    public sfxContainer[] test;
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
