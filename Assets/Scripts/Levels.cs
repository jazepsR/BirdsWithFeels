using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour {
    public enum type { Toby,Kim,Rebecca,Tova, Terry,Friend1,Friend2,Brave1,Brave2,Lonely1,Lonely2,Scared1,Scared2};
    Bird myBird;
    List<LevelData> LevelList;
    public GameObject Halo;
	// Use this for initialization
	void Start () {        
        myBird = GetComponent<Bird>();
        LevelList = myBird.levelList;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ApplyStartLevel(Bird bird, List<LevelData> Levels)
    {

        foreach (LevelData data in Levels) {
            type level = data.type;
            switch (level)
            {
                case type.Rebecca:
                    bird.confLoseOnRest = 2;
                    break;
                case type.Kim:
                    bird.groundMultiplier = 2;
                    break;
                default:
                    break;
            }


        }
    }

    public void ApplyLevelOnDrop(Bird bird, List<LevelData> Levels)
    {
        foreach (LevelData data in Levels)
        {
            type level = data.type;
            switch (level)
            {
                case type.Toby:
                    if (myBird.x == -1)
                        break;
                    List<LayoutButton> tileCol = GetColumn();
                    for (int i = 0; i < 4; i++)
                    {
                        Color col = Helpers.Instance.GetEmotionColor(Var.Em.Lonely);
                        col = new Color(col.r, col.g, col.b, 0.3f);
                        tileCol[i].SetColor(col,false);
                        if(i!=0)
                            tileCol[i].FriendBonus = -1;

                    }
                    break;
                case type.Terry:
                    if (myBird.x == -1)
                        break;
                    List<LayoutButton> tileRow = GetRow();
                    for(int i=0;i<4;i++)
                    {
                        Color col = Helpers.Instance.GetEmotionColor(Var.Em.Scared);
                        col = new Color(col.r, col.g, col.b, 0.3f);
                        tileRow[i].SetColor(col,true);
                        if(i!=0)
                            tileRow[i].ConfBonus = -1;
                    }
                    break;
                case type.Tova:
                    if (myBird.x == -1)
                        break;
                    foreach (LayoutButton tile in GetAdjacent())
                    {
                        tile.RollBonus = 1;
                    }
                    Halo.SetActive(true);
                    break;
                default:
                    break;
            }
        }
    }

    public void ApplyLevelOnPickup(Bird bird, List<LevelData> Levels)
    {
        foreach (LevelData data in Levels)
        {
            type level = data.type;
            switch (level)
            {
                case type.Toby:
                    if (myBird.x == -1)
                        break;
                    foreach (LayoutButton tile in GetColumn())
                    {
                        tile.ResetColor(false);
                        tile.FriendBonus = 0;
                    }
                    break;
                case type.Terry:
                    if (myBird.x == -1)
                        break;
                    foreach (LayoutButton tile in GetRow())
                    {
                        tile.ResetColor(true);
                        tile.ConfBonus = 0;
                    }
                    break;
                case type.Tova:
                    if (myBird.x == -1)
                        break;
                    foreach (LayoutButton tile in GetAdjacent())
                    {
                        tile.RollBonus = 0;
                    }
                    Halo.SetActive(false);
                    break;
                default:
                    break;
            }
        }



    }


    public List<LayoutButton> GetRow()
    {
        int x = myBird.x;
        int y = myBird.y;
        List<LayoutButton> list = new List<LayoutButton>();
        for (int i = 0; i < 4; i++)
        {
            int index = y * 4 + (x + i) % 4;
            list.Add(ObstacleGenerator.Instance.tiles[index]);
        }
        return list;
    }

    public List<LayoutButton> GetColumn()
    {
        int x = myBird.x;
        int y = myBird.y;
        List<LayoutButton> list = new List<LayoutButton>();
        for (int i = 0; i < 4; i++)
        {
            int index = (y+i)%4 * 4 + x;
            list.Add(ObstacleGenerator.Instance.tiles[index]);
        }
        return list;
    }

    public List<LayoutButton> GetAdjacent()
    {
        int x = myBird.x;
        int y = myBird.y;
        int sizeY = Var.playerPos.GetLength(1) - 1;
        int sizeX = Var.playerPos.GetLength(0) - 1;
        List<LayoutButton> list = new List<LayoutButton>();
        List<LayoutButton> tiles = ObstacleGenerator.Instance.tiles;
        if (y + 1 <= sizeY )
        {
            list.Add(tiles[(y + 1) * 4 + x]);
        }
        if (y - 1 >= 0 )
        {
            list.Add(tiles[(y -1) * 4 + x]);
        }
        if (x + 1 <= sizeX )
        {
            list.Add(tiles[(y) * 4 + x+1]);
        }
        if (x - 1 >= 0)
        {
            list.Add(tiles[(y) * 4 + x-1]);
        }
        if (y + 1 <= sizeY && x + 1 <= sizeX )
        {
            list.Add(tiles[(y + 1) * 4 + x + 1]);
        }
        if (y + 1 <= sizeY && x - 1 <= sizeX)
        {
            list.Add(tiles[(y + 1) * 4 + x - 1]);
        }
        if (y - 1 >= 0 && x + 1 <= sizeX)
        {
            list.Add(tiles[(y - 1) * 4 + x + 1]);
        }
       
        if (y - 1 >= 0 && x - 1 >= 0)
        {
            list.Add(tiles[(y - 1) * 4 + x - 1]);
        }

        Debug.Log(list.Count);
        return list;

    }


}

public class LevelData{
    public Levels.type type;
    public Var.Em emotion;

   public LevelData(Levels.type type, Var.Em emotion)
    {
        this.emotion = emotion;
        this.type = type;
    }



}
