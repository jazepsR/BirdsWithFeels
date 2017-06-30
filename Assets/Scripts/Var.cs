using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Var  {
    public enum Em { Neutral,Lonely,SuperLonely, Friendly, SuperFriendly,Confident,SuperConfident,Scared, SuperScared,finish};
    public static int lvl1 = 4;
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
    public static int health = 3;
    public static List<BattleData> map = new List<BattleData>();
    public static List<MapSaveData>mapSaveData = new List<MapSaveData>();
    public static GameObject selectedBird;    
    public static int currentStageID = -1;
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
