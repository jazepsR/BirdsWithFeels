using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiContoler : MonoBehaviour {
    public static GuiContoler Instance { get; private set; }
    public Text infoText;
    public GameObject[] players;
    public GameObject[] enemies;
    public Image[] tiles;
    public Text reportText;
    public GameObject report;
    public Image[] hearts;
    public GameObject loseBanner;
    public GameObject winBanner;
    public GameObject[] portraits;
    int roundLength = 3;
    Var.Em currentMapArea;
    Var.Em nextMapArea;
    int posInMapRound = 0;
    int mapPos = 0;


    void Awake()
    {
        Var.birdInfo = infoText;
        if (Var.map.Count < 1)
        {
            Var.map.Add(new BattleData(Var.Em.Neutral));
            Var.map.Add(new BattleData(Var.Em.Neutral));
            Var.map.Add(new BattleData(Var.Em.Lonely));
            Var.map.Add(new BattleData(Var.Em.Friendly));            
        }
        setMapLocation(0);
    }
	// Use this for initialization
	void Start () {
		
	}
	public void CloseReport()
    {
        report.SetActive(false);
    }
	void UpdateHearts(int health)
    {
        Debug.Log("Health: " + health);
        for(int i=0;i< hearts.Length; i++)
        {
            hearts[i].enabled = i < health;
        }
    }
    public void CreateReport(bool won)
    {
        string reportString = "";
        if (won)
        {
            reportString = "You won the fight!\n";
        }else
        {
            reportString = "You lost the fight :'(\n";
        }
        foreach(GameObject player in players)
        {
            Bird script = player.GetComponent<Bird>();
            reportString += script.charName + ": ";
            if ((script.confidence - script.prevConf) > 0)
                reportString += "Won\n";
            else
                reportString += "Lost\n";
            reportString += "friendliness: " + script.prevFriend + " -> " + script.friendliness +" (" +(script.friendliness - script.prevFriend) + ")";
            reportString += "\nconfidence: " + script.prevConf + " -> " + script.confidence + " (" + (script.confidence - script.prevConf) + ")\n"; 
        }
        if (currentMapArea != nextMapArea)
        {
            reportString += "Current area: " + currentMapArea + " next map area: " + nextMapArea + " in " + (roundLength - posInMapRound);
        }
        reportText.text = reportString;
        report.SetActive(true);
        Debug.Log(reportString);
    }

    public void PoitraitControl(int portNr)
    {
        for (int i = 0; i < portraits.Length; i++)
        {
            if (i == portNr)
            {
                portraits[i].SetActive(true);
            }else
            {
                portraits[i].SetActive(false);
            }
        }
    }

    public void Fight()
    {

        Debug.Log("Fight selected");
        int result = 0;
        Bird playerBird = null;

     
       

       for (int i = 0; i <Var.enemies.Length; i++)
       {
            if (enemies[i].GetComponent<Bird>().inUse)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Var.playerPos[j, i] != null)
                    {
                        playerBird = Var.playerPos[j, i];
                        playerBird.friendliness += Helpers.Instance.Findfirendlieness(j, i);
                        break;
                    }

                }
                result += GameLogic.Instance.Fight(playerBird, Var.enemies[i]);
                
            }

    }
        if (result > 0)
        {
            Debug.Log("Player won!");
            foreach(GameObject bird in players)
            {
                bird.GetComponent<Bird>().confidence+= Var.confWinAll;
            }
            moveInMap();
            CreateReport(true);
        }
        else
        {
            Debug.Log("Enemy won");
            foreach (GameObject bird in players)
            {
                bird.GetComponent<Bird>().confidence+= Var.confLoseAll;
            }
            Var.health--;
            UpdateHearts(Var.health);
            if (Var.health <= 0)
            {
                loseBanner.SetActive(true);
            }
            else
            {
                moveInMap();
                CreateReport(false);
            }
            
        }
        
        Reset();
    }

    public void Reset()
    {
        Var.playerPos = new Bird[3, 5];
        Var.enemies = new Bird[5];
        foreach(GameObject bird in players)
        {
            bird.GetComponent<Bird>().SetEmotion();
        }
        BattleData Area = Var.map[mapPos];
        GetComponent<fillEnemy>().createEnemies(Area.minConf, Area.maxConf, Area.minFriend, Area.maxFriend);
        foreach(Image img in tiles)
        {
            img.sprite = null;
        }
        
		// Main battle field button quick restart?
		GameLogic.Instance.OnRestartGame ();
    }
    void setMapLocation(int index)
    {
        currentMapArea = Var.map[index].type;
        try
        {
            nextMapArea = Var.map[index + 1].type;
        }
        catch
        {
            nextMapArea = Var.Em.finish;
        }
    }

    void moveInMap()
    {
        posInMapRound++;
        if (posInMapRound == roundLength)
        {
            if(nextMapArea== Var.Em.finish)
            {
                winBanner.SetActive(true);
            }
            posInMapRound = 0;
            mapPos++;
            setMapLocation(mapPos);
        }
    }
    
    public void ResetScene()
    {
        Reset();
        Application.LoadLevel(Application.loadedLevel);
    }

}
