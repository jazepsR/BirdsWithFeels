using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers : MonoBehaviour {
    public static Helpers Instance { get; private set; }
    public Color neutral;
    public Color brave;
    public Color scared;
    public Color friendly;
    public Color lonely;

    public void Awake()
    {
        Instance = this;
    }
    public int Findfirendlieness(int x,int y)
    {
        int sizeY = Var.playerPos.GetLength(1)-1;
        int sizeX = Var.playerPos.GetLength(0)-1;
        int lonelyVal = 0;
        if(y+1<= sizeY && Var.playerPos[x, y + 1] != null)
        {
            lonelyVal += 2;
        }
        if (y - 1 >=0 && Var.playerPos[x, y - 1] != null)
        {
            lonelyVal += 2;
        }
        if(y+1<=sizeY && x+1<= sizeX && Var.playerPos[x+1, y + 1] != null)
        {
            lonelyVal += 1;
        }

        if (y + 1 <= sizeY && x - 1 >= 0 && Var.playerPos[x - 1, y + 1] != null)
        {
            lonelyVal += 1;
        }

        if (y - 1 >= 0 && x + 1 <= sizeX && Var.playerPos[x + 1, y - 1] != null)
        {
            lonelyVal += 1;
        }

        if (y - 1 >= 0 && x - 1 >= 0 && Var.playerPos[x - 1, y - 1] != null)
        {
            lonelyVal += 1;
        }
        return -2 + 2 * lonelyVal;

    }
}
