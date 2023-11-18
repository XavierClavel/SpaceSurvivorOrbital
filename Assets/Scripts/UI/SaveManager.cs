using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class SaveData
{
    public int souls = 0;
}

public static class SaveManager
{
    static SaveData saveData = null;

    public static int retrieveSouls()
    {
        return saveData.souls;
    }

    public static void updateSouls(int value)
    {
        saveData.souls = value;
    }

    private static string getDataPath()
    {
        return $"{Application.persistentDataPath}/save.fun";
        
    }

    public static void Reset()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(getDataPath(), FileMode.Create);
        
        formatter.Serialize(stream, new SaveData());
        stream.Close();
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
            saveData = formatter.Deserialize(stream) as SaveData;
            stream.Close();
        }
        else
        {
            Debug.LogWarning("Save file not found");
            saveData = new SaveData();
        }
    }
    
}
