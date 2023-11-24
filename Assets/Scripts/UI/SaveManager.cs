using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;


public static class SaveManager
{
    [Serializable]
    private class SaveData
    {
        public int souls = 0;
        public List<String> optionsUnlocked = new List<string>
        {
            "Gun"
        };
    }
    
    static SaveData saveData = null;

    public static int retrieveSouls()
    {
        return saveData.souls;
    }

    public static bool isOptionUnlocked(string key)
    {
        return saveData.optionsUnlocked.Contains(key);
    }

    public static List<String> retrieveUnlockedOptions(string key)
    {
        return saveData.optionsUnlocked;
    }

    public static void unlockOption(string key)
    {
        saveData.optionsUnlocked.Add(key);
        Save();
    }

    public static void updateSouls(int value)
    {
        saveData.souls = value;
        Save();
        Debug.Log(saveData.souls);
    }

    private static string getDataPath()
    {
        return $"{Application.persistentDataPath}/save.fun";
        
    }

    public static void Reset()
    {
        saveData = new SaveData();
        Save();
    }

    public static void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(getDataPath(), FileMode.Create);
        
        formatter.Serialize(stream, saveData);
        stream.Close();
    }

    public static void Load()
    {
        if (File.Exists(getDataPath()))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(getDataPath(), FileMode.Open);
            try
            {
                saveData = formatter.Deserialize(stream) as SaveData;
                stream.Close();
                Debug.Log("Save loaded");
            }
            catch
            {
                Debug.LogWarning("Incompatible save format, save was reset");
                stream.Close();
                Reset();
                saveData = new SaveData();
            }
            
            
        }
        else
        {
            Debug.LogWarning("Save file not found");
            saveData = new SaveData();
        }
    }
    
}
