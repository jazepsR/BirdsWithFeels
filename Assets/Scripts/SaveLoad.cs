using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveLoad : MonoBehaviour
{
    public static void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/saveGame.dat");
        SaveData saveData = new SaveData();
        bf.Serialize(file, saveData);
        file.Close();
    }
    public static bool Load()
    {
        if (File.Exists(Application.persistentDataPath + "/saveGame.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/saveGame.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            ApplyLoadedFile(data);
            return true;
        }
        else
        {
            return false;
        }
    }
    public static void DeleteSave()
    {
        File.Delete(Application.persistentDataPath + "/saveGame.dat");
    }
    private static void ApplyLoadedFile(SaveData data)
    {
        Var.mapSaveData = data.mapSaveData;
        Var.activeBirds = data.activeBirds;
        Var.activeBirds = data.availableBirds;
        Var.map = data.map;
    }
}
[Serializable]
public class SaveData
{
    public List<MapSaveData> mapSaveData;
    public List<Bird> activeBirds;
    public List<Bird> availableBirds;
    public List<BattleData> map;
    public SaveData()
    {
        mapSaveData = Var.mapSaveData;
        activeBirds = Var.activeBirds;
        activeBirds = Var.availableBirds;
        map = Var.map;
    }
}
