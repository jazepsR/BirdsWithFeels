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
    
    public Color GetEmotionColor(Var.Em emotion)
    {
        switch (emotion)
        {
            case Var.Em.Neutral:
                return neutral;
            case Var.Em.Friendly:
                return friendly;
            case Var.Em.Lonely:
                return lonely;
            case Var.Em.Confident:
                return brave;
            case Var.Em.Scared:
                return scared;
            default:
                return neutral;
        }
    }

    public Vector3 dirToVector(Bird.dir dir)
    {
        float increment = 0.7f;
        switch (dir)
        {
            case Bird.dir.front:
                return new Vector3(increment, 0);
            case Bird.dir.bottom:
                return new Vector3(0, -increment);
            case Bird.dir.top:
                return new Vector3(0, increment);
            default:
                return new Vector3(0, 0, 0);
        }
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
