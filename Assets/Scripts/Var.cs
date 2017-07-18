﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Var  {
    
    public enum Em { Neutral,Lonely,SuperLonely, Friendly, SuperFriendly,Confident,SuperConfident,Scared, SuperScared,finish};
    public enum PowerUps { heal,dmg,emotion};
    public static int MoveGraphBy = 1550;
    public static int lvl1 = 5;
    public  static int lvl2 = 10;
    public static int confLoseFight = -3;
    public static int confWinFight = 3;
    public static int confWinAll = 1;
    public static int confLoseAll = -1;
    public static int friendStraightGain = 2;
    public static int friendDiagGain = 1;
    public static int[] friendTable = new int[] { -2, 0, 2, 4, 6 };
    public static Bird[] enemies = new Bird[12];
    public static Bird[,] playerPos = new Bird[4,4];
    public static Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
    public static Text birdInfo;
    public static Text birdInfoHeading;
    public static Text birdInfoFeeling;
    public static Image powerBar;
    public static Text powerText;
    public static int health = 3;
    public static List<BattleData> map = new List<BattleData>();
    public static List<MapSaveData>mapSaveData = new List<MapSaveData>();
    public static GameObject selectedBird;
    public static List<Bird> activeBirds = new List<Bird>();
    public static List<Bird> availableBirds = new List<Bird>();
    public static int currentStageID = -1;
    public static GameObject emotionParticles = null;
    public static string[] maleNames = {"Noah", "Liam", "Mason", "Jacob", "William","Ethan", "James","Alexander","Michael","Benjamin","Elijah",
        "Daniel", "Aiden", "Logan", "Matthew","Lucas","Jackson","David","Oliver","Jayden","Joseph","Gabriel","Samuel","Carter","Anthony","John",
        "Dylan", "Luke", "Henry","Andrew","Isaac","Christopher","Joshua","Wyatt","Sebastian","Owen","Caleb","Nathan","Ryan","Jack","Hunter","Levi",
        "Christian","Jaxon","Julian","Landon","Grayson","Jonathan","Isaiah","Charles"};
    public static string[] femaleNames = {"Emma","Olivia","Sophia","Ava","Isabella","Mia","Abigail","Emily", "Charlotte", "Harper","Madison", "Amelia",
        "Elizabeth","Sofia","Evelyn","Chloe","Ella","Grace","Victoria","Aubrey","Scarlett","Zoey","Addison","Lily","Lillian","Natalie","Hannah","Aria","Layla"};
   
}



public class MapSaveData
{
    public int ID;
    public List<int> targets;
    public bool completed;
    public bool available;
    public MapSaveData(bool completed, bool available, int ID, List<int> targets)
    { 
        this.completed = completed;
        this.available = available;
        this.ID = ID;
        this.targets = targets;
    }
}
