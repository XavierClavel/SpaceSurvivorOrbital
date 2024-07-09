using UnityEngine;
using UnityEngine.Serialization;

public class BuildScriptableObject : ScriptableObject
{
    public string buildNumber = "1";
    public string gitCommit = "";
    public string date = "";
}