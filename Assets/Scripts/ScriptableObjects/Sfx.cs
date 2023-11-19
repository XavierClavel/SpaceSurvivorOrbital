using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Sfx", menuName = Vault.other.scriptableObjectMenu + "Sfx", order = 0)]
public class Sfx : ScriptableObject
{
    [SerializeField] private string key;
    [SerializeField] private AudioClip clip;
    [SerializeField] [Range(0f, 1f)] private float volume;
    
    public string getKey() { return key.Trim(); }
    public AudioClip getClip() { return clip; }

    public float getVolume()
    {
        return volume;
    }
}