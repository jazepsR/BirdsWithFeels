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

        //Debug.Log(list.Count);
        return list;

    }

    public void ApplyLevel(LevelData data, string levelName)
    {
        if (!Helpers.Instance.ListContainsLevel(data.type, LevelList))
        {
            GuiContoler.Instance.ShowMessage(myBird.charName + " Reached level " + levelName);
            myBird.AddLevel(data);
            Debug.Log(myBird.charName + " Reached level " + levelName);
        }
    }

    public void  CheckBrave1()
    {
        if (myBird.lastLevel.emotion != Var.Em.Confident && (myBird.emotion == Var.Em.Confident || myBird.emotion == Var.Em.SuperConfident))
        {
            if (myBird.winsInOneFight > 1  && myBird.confidence >= 7)
            {
                ApplyLevel(new LevelData(type.Brave2, Var.Em.Confident), "Brave 1");
            }
        }

    }
    public void CheckBrave2()
    {
        if (myBird.lastLevel.emotion != Var.Em.Confident && (myBird.emotion == Var.Em.Confident || myBird.emotion == Var.Em.SuperConfident))
        {
            if (myBird.consecutiveFightsWon >= 6 && myBird.confidence >= 10)
            {
                ApplyLevel(new LevelData(type.Brave1, Var.Em.Confident), "Brave 2");
            }
        }
    }
    public void CheckLonely1()
    {
        if (myBird.lastLevel.emotion != Var.Em.Lonely && (myBird.emotion == Var.Em.Lonely || myBird.emotion == Var.Em.SuperLonely))
        {           
            if ( myBird.friendliness <= -7 && Helpers.Instance.GetAdjacentBirds(myBird).Count == 0 && myBird.wonLastBattle>=1)
            {
                ApplyLevel(new LevelData(type.Lonely1, Var.Em.Lonely), "Lonely 1");
            }            
        }
    }

    public void CheckLonely2()
    {
        if (myBird.lastLevel.emotion != Var.Em.Lonely && (myBird.emotion == Var.Em.Lonely || myBird.emotion == Var.Em.SuperLonely))
        {
            //Fix -7 to -10
            if (myBird.friendliness <= -10 && Helpers.Instance.GetAdjacentBirds(myBird).Count == 0 && myBird.AdventuresRested>=2)
            {
                ApplyLevel(new LevelData(type.Lonely2, Var.Em.Lonely),"Lonely 2");
            }
        }
    }
    
    public void CheckFriendly1()
    {
        if (myBird.lastLevel.emotion != Var.Em.Friendly && (myBird.emotion == Var.Em.Friendly || myBird.emotion == Var.Em.SuperFriendly))
        {
            if (myBird.FriendGainedInRound >= 3  && myBird.wonLastBattle >=1 && myBird.friendliness >= 7)
            {
                ApplyLevel(new LevelData(type.Friend1, Var.Em.Friendly),"Friendly 1");
            }
        }

    }

    public void CheckFriendly2()
    {
        if (myBird.lastLevel.emotion != Var.Em.Friendly && (myBird.emotion == Var.Em.Friendly || myBird.emotion == Var.Em.SuperFriendly))
        {
            bool closeFriend = false;
            foreach(Bird bird in Helpers.Instance.GetAdjacentBirds(myBird))
            {
                if(bird.emotion == Var.Em.Friendly || bird.emotion == Var.Em.SuperFriendly )
                {
                    closeFriend = true;
                    break;
                }
            }
            if (closeFriend && myBird.FriendGainedInRound >= 5 && myBird.friendliness >= 10)
            {
                ApplyLevel(new LevelData(type.Friend1, Var.Em.Friendly),"Friendly 2");
            }
        }

    }

    public void CheckScared1()
    {
        if (myBird.lastLevel.emotion != Var.Em.Scared && (myBird.emotion == Var.Em.Scared || myBird.emotion == Var.Em.SuperScared))
        {
            bool closeWinner = false;
            foreach (Bird bird in Helpers.Instance.GetAdjacentBirds(myBird))
            {
                if (bird.wonLastBattle>=1)
                {
                    closeWinner = true;
                    break;
                }
            }

            if ((closeWinner &&myBird.wonLastBattle == 0 || closeWinner && myBird.wonLastBattle == 2) && myBird.confidence <= -7)
            {
                ApplyLevel(new LevelData(type.Scared1, Var.Em.Scared),"Scared 1");
            }
        }

    }
    public void CheckScared2()
    {
        if (myBird.lastLevel.emotion != Var.Em.Scared && (myBird.emotion == Var.Em.Scared || myBird.emotion == Var.Em.SuperScared))
        {
            
            if (myBird.roundsRested >= 4 && myBird.confidence <= -10)
            {
                ApplyLevel(new LevelData(type.Scared2, Var.Em.Scared),"Scared 2");
            }
        }

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
