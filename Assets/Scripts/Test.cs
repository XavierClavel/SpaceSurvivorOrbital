using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;
using System.Linq;



// Defines an attribute that makes the array use enum values as labels.
// Use like this:
//      [NamedArray(typeof(eDirection))] public GameObject[] m_Directions;

public class NamedArrayAttribute : PropertyAttribute
{
    public Type TargetEnum;
    public NamedArrayAttribute(Type TargetEnum)
    {
        this.TargetEnum = TargetEnum;
    }
}


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(NamedArrayAttribute))]
public class NamedArrayDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Properly configure height for expanded contents.
        return EditorGUI.GetPropertyHeight(property, label, property.isExpanded);
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Replace label with enum name if possible.
        try
        {
            var config = attribute as NamedArrayAttribute;
            var enum_names = System.Enum.GetNames(config.TargetEnum);
            int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
            var enum_label = enum_names.GetValue(pos) as string;

            // Make names nicer to read (but won't exactly match enum definition).
            enum_label = ObjectNames.NicifyVariableName(enum_label);
            label = new GUIContent(enum_label);
        }
        catch
        {
            // keep default label
        }
        EditorGUI.PropertyField(position, property, label, property.isExpanded);
    }
}
#endif


[Serializable]
public class sfxContainer : IEquatable<sfxContainer>
{
    public AudioClip audioClip;
    public float volume = 1;
    public sfxContainer(AudioClip audioClip, float volume)
    {
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
        audioIds = new List<clip>();
        //PlayMusic(musics[0]);
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
        if (laserSourcePlaying) laserSource.Play();
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
