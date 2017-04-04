using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Var  {
    public enum Em { Neutral,Lonely,SuperLonely, Friendly, SuperFriendly,Confident,SuperConfident,Scared, SuperScared};
    public static int lvl1 = 4;
    public  static int lvl2 = 10;
    public static Bird[] enemies = new Bird[6];
    public static Bird[,] playerPos = new Bird[3,6];
	
}
