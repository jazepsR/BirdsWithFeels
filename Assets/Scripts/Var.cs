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
    public static Bird[] enemies = new Bird[5];
    public static Bird[,] playerPos = new Bird[3,5];
    public static Dictionary<string, Sprite> spriteDict = new Dictionary<string, Sprite>();
    public static Text birdInfo;
    public static Text birdInfoHeading;
    public static int health = 3;
    public static List<BattleData> map = new List<BattleData>();
}
