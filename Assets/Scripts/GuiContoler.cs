using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiContoler : MonoBehaviour {
    public static GuiContoler Instance { get; private set; }
    public Text infoText;
    public Text infoHeading;
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

    public GameObject graph;
    public Text winText;
    public Text winDetails;
    public Text feedbackText;
    public GameObject battlePanel;
    public GuiMap mapBirdScript;

    void Awake()
    {
        Var.birdInfo = infoText;
        Var.birdInfoHeading = infoHeading;
        if (Var.map.Count < 1)
        {
            Var.map.Add(new BattleData(Var.Em.Neutral));
            Var.map.Add(new BattleData(Var.Em.Neutral));
            Var.map.Add(new BattleData(Var.Em.Lonely));
            Var.map.Add(new BattleData(Var.Em.Friendly));            
        }
        setMapLocation(0);
        Instance = this;
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

    public void CloseGraph()
    {

        graph.SetActive(false);
        battlePanel.SetActive(true);
        mapBirdScript.MoveMapBird(mapPos * 3 + posInMapRound);
        foreach (Transform child in graph.transform.Find("ReportGraph").transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void CreateReport(int winCount)
    {
        battlePanel.SetActive(false);
        /*foreach (GameObject bird in players)
        {
            bird.GetComponent<Bird>().SetEmotion();
        }*/

        graph.SetActive(true);
        foreach (GameObject player in players)
        {
            


            Bird script = player.GetComponent<Bird>();

            //Normalize bird stats
            if (script.confidence > 12)
                script.confidence = 12;
            if (script.confidence < -12)
                script.confidence = -12;
            if (script.friendliness > 12)
                script.friendliness = 12;
            if (script.friendliness < -12)
                script.friendliness = -12;


            GameObject portrait = script.portrait;
            GameObject colorObj = portrait.gameObject.transform.Find("bird_color").gameObject;
            colorObj.GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(script.emotion);
            Graph.Instance.PlotFull(script.prevFriend, script.prevConf, script.friendliness, script.confidence, portrait);

        }
            string feedBackString = "";
            if (currentMapArea != nextMapArea)
            {
                feedBackString += nextMapArea + " birds coming in " + (roundLength - posInMapRound) + " battles!"; 
            }
            if(nextMapArea == Var.Em.finish)
        {
            feedBackString = "Victory in " + (roundLength - posInMapRound) + " battles!";
        }
            feedbackText.text = feedBackString;

            string winString = "You won!";
            if (winCount<0)
            {
                winString = "You lost :'(";
            }
            winText.text = winString;

            int winNo = (winCount-1)/2+2;

            string winDetString = winNo + " / 3 Battles won!";
            winDetails.text = winDetString;


        
            /*string reportString = "";
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
                reportString += "\nconfidence: " + script.prevConf + " -> " + script.confidence + " (" +  + ")\n"; 
            }
            
            reportText.text = reportString;
            report.SetActive(true);
            Debug.Log(reportString);*/
        }

    public void PortraitControl(int portNr,Var.Em color)
    {
        for (int i = 0; i < portraits.Length; i++)
        {
            if (i == portNr)
            {
                portraits[i].SetActive(true);
                GameObject colorObj= portraits[i].gameObject.transform.Find("bird_color").gameObject;
                colorObj.GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(color);
            }else
            {
                portraits[i].SetActive(false);
            }
        }
    }

	private int finalResult = 0;

	public void OnCompletedBattleVisual()
	{
		if (finalResult > 0)
		{
			Debug.Log("Player won!");
			foreach(GameObject bird in players)
			{
				bird.GetComponent<Bird>().confidence+= Var.confWinAll;
			}
			moveInMap();
			CreateReport(finalResult);
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
                mapPos = 0;
				loseBanner.SetActive(true);
			}
			else
			{
				moveInMap();
				CreateReport(finalResult);
			}

		}

		Reset();
	}

    public void Fight()
    {

        Debug.Log("Fight selected");
        int result = 0;
        Bird playerBird = null;

		int lineY = 0;
		GameObject birdObj = null;
       

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

						birdObj = GameLogic.Instance.OnGetArrayVisualHolder (new Vector2(j,i),true);
						lineY = i;

                        break;
                    }

                }

				int resultOfBattle = GameLogic.Instance.Fight(playerBird, Var.enemies[i]);

				// They will fight!
				BattleAction.Instance.AddBattleInfo (birdObj,Var.enemies[i].transform.gameObject,resultOfBattle,lineY);

                result += GameLogic.Instance.Fight(playerBird, Var.enemies[i]);
                
            }

    }

		// We continue after battle visualisation

		// Clear the lines from screen!
		GameLogic.Instance.OnClearFeedbackQuick();

		BattleAction.Instance.OnStartBattle();
		finalResult = result;
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
        try
        {
            currentMapArea = Var.map[index].type;
        }
        catch { }
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
                mapPos = 0;
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
