using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers : MonoBehaviour {
    public static Helpers Instance { get; private set; }
    [Header("Colors")]
    public Color neutral;
    public Color brave;
    public Color scared;
    public Color friendly;
    public Color lonely;
    [Header("Soft colors")]
    public Color softBrave;
    public Color softScared;
    public Color softFriendly;
    public Color softLonely;
      
    public void Awake()
    {
       
        Instance = this;
    }
    
    public List<Bird> GetAdjacentBirds(Bird bird)
    {
        int x = bird.x;
        int y = bird.y;
        List<Bird> list = new List<Bird>();
        int sizeY = Var.playerPos.GetLength(1) - 1;
        int sizeX = Var.playerPos.GetLength(0) - 1;
        if (y + 1 <= sizeY && Var.playerPos[x, y + 1] != null)
            list.Add(Var.playerPos[x, y + 1]);
        if (y - 1 >= 0 && Var.playerPos[x, y - 1] != null)
            list.Add(Var.playerPos[x, y - 1]);
        if (x + 1 <= sizeX && Var.playerPos[x + 1, y] != null)
            list.Add(Var.playerPos[x + 1, y]);
        if (x - 1 >= 0 && Var.playerPos[x - 1, y] != null)
            list.Add(Var.playerPos[x - 1, y]);
        if (y + 1 <= sizeY && x + 1 <= sizeX && Var.playerPos[x + 1, y + 1] != null)
            list.Add(Var.playerPos[x + 1, y + 1]);
        if (y + 1 <= sizeY && x - 1 >= 0 && Var.playerPos[x - 1, y + 1] != null)
            list.Add(Var.playerPos[x - 1, y + 1]);
        if (y - 1 >= 0 && x + 1 <= sizeX && Var.playerPos[x + 1, y - 1] != null)
            list.Add(Var.playerPos[x + 1, y - 1]);
        if (y - 1 >= 0 && x - 1 >= 0 && Var.playerPos[x - 1, y - 1] != null)
            list.Add(Var.playerPos[x - 1, y - 1]);
        return list;
    }


    public bool ListContainsLevel(Levels.type level, List<LevelData> list)
    {
        if (list != null)
        {
            foreach (LevelData data in list)
            {
                if (data.type == level)
                    return true;
            }
            return false;
        }else
        {
            return false;
        }
    }
    public void NormalizeStats(Bird bird)
    {
        int treshold = Var.lvl2 - 1;
        //Super starts at 10
        if (bird.level > 1)
        {
            treshold = 12;
        }
        if (bird.confidence > treshold)
            bird.confidence = treshold;
        if (bird.confidence < -treshold)
            bird.confidence = -treshold;
        if (bird.friendliness > treshold)
            bird.friendliness = treshold;
        if (bird.friendliness < -treshold)
            bird.friendliness = -treshold;
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
            case Var.Em.SuperFriendly:
                return friendly;
            case Var.Em.SuperLonely:
                return lonely;
            case Var.Em.SuperConfident:
                return brave;
            case Var.Em.SuperScared:
                return scared;
            default:
                return neutral;
        }
    }
    public Color GetSoftEmotionColor(Var.Em emotion)
    {
        switch (emotion)
        {
            case Var.Em.Neutral:
                return neutral;
            case Var.Em.Friendly:
                return softFriendly;
            case Var.Em.Lonely:
                return softLonely;
            case Var.Em.Confident:
                return softBrave;
            case Var.Em.Scared:
                return softScared;
            case Var.Em.SuperFriendly:
                return softFriendly;
            case Var.Em.SuperLonely:
                return softLonely;
            case Var.Em.SuperConfident:
                return softBrave;
            case Var.Em.SuperScared:
                return softScared;
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



    public float RandGaussian(float stdDev, float mean)
    {
        float u1 = Random.Range(0.0f, 1.0f);
        float u2 = Random.Range(0.0f, 1.0f);
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return  mean + stdDev * randStdNormal;
    }


    public int Findfirendlieness(Bird bird)
    {        
        int x = 0;
        int y = 0;
        for (int i = 0; i < Var.playerPos.GetLength(0); i++)
        {
            for (int j = 0; j < Var.playerPos.GetLength(1); j++)
            {

                if (Var.playerPos[i, j] == bird)
                {
                    x = i;
                    y = j;
                    break;
                }
            }
        }


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
        if (x + 1 <= sizeX && Var.playerPos[x+1, y] != null)
        {
            lonelyVal += 2;
        }
        if (x - 1 >= 0 && Var.playerPos[x-1, y ] != null)
        {
            lonelyVal += 2;
        }
        if (y+1<=sizeY && x+1<= sizeX && Var.playerPos[x+1, y + 1] != null)
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
