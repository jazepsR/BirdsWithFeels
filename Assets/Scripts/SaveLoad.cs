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
        DeleteSave();
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
        Var.map = data.map;
        Var.shownDialogs = data.usedDialogs;
        Var.shownEvents = data.usedEvents;
        List<Bird> activeBirds = new List<Bird>();
       /* foreach(BirdSaveData birdData in data.activeBirds)
        {
            activeBirds.Add(FillPlayer.LoadSavedBird(birdData));
        }
        Var.activeBirds = activeBirds;*/
       
        
        List<Bird> availableBirds = new List<Bird>();
        foreach (BirdSaveData birdData in data.availableBirds)
        {
            availableBirds.Add(FillPlayer.LoadSavedBird(birdData));
        }
        Var.availableBirds = availableBirds;
        //Load relationship birds
        /*foreach (BirdSaveData birdData in data.availableBirds)
        {
            if (birdData.relationshipBird != EventScript.Character.None)
            {
                foreach (Bird bird in Var.availableBirds)
                {
                    if (bird.charName == birdData.charName)
                    {
                        bird.relationshipBird = Helpers.Instance.GetBirdFromEnum(birdData.relationshipBird,true);
                    }
                }
            }
        }*/



        
    }
}
[Serializable]
public class SaveData
{
    public List<MapSaveData> mapSaveData;
    public List<BirdSaveData> activeBirds;
    public List<BirdSaveData> availableBirds;
    public List<BattleData> map;
    public List<string> usedDialogs;
    public List<string> usedEvents;
    public SaveData()
    {
        usedEvents = Var.shownEvents;
        usedDialogs = Var.shownDialogs;
        mapSaveData = Var.mapSaveData;
        activeBirds = new List<BirdSaveData>();
        foreach(Bird bird in Var.activeBirds)
        {
            activeBirds.Add(FillPlayer.SetupSaveBird(bird));
        }
        availableBirds = new List<BirdSaveData>();

        foreach (Bird bird in Var.availableBirds)
        {
            availableBirds.Add(FillPlayer.SetupSaveBird(bird));
        }        
        map = Var.map;
    }
}

[Serializable]
public class BirdSaveData
{    
    public string charName;
    public string birdPrefabName;
    public int friendliness;
    public int confidence;
    public int portraitOrder;
    public int health;
    public int maxHealth;
    public bool dead;
    public Var.Em preferredEmotion;
    public EventScript.Character relationshipBird;
    //public GameObject portrait;
    public List<LevelData> levelList;
    public Levels.type startingLVL;
    public int battleCount;
    public LevelData lastLevel;
    public int level;
    public string birdAbility;
    public int consecutiveFightsWon;
    public int battlesToNextLVL;
    public int roundsRested;
    public int AdventuresRested;
    public int CoolDownLeft;
    public int CoolDownLength;
    public Dictionary<EventScript.Character, int> relationships;
}





