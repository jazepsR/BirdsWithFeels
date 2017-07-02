using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System.Linq;
using UnityEngine.SceneManagement;

public class GuiContoler : MonoBehaviour {
	public static GuiContoler Instance { get; private set; }
	public Text infoText;
	public Text infoHeading;
	public Text infoFeeling;
	public Image[] tiles;
	public Text reportText;
	public GameObject report;
	public Image[] hearts;
	public GameObject loseBanner;
	public GameObject winBanner;
	public GameObject[] portraits;
	public Transform[] battleTrag;
	int roundLength = 3;
	Var.Em currentMapArea;
	Var.Em nextMapArea;
	int posInMapRound = 0;
	public static int mapPos = 0;
	private int finalResult = 0;
	public GameObject graph;
	public Text winText;
	public Text winDetails;
	public Text feedbackText;
	public GameObject battlePanel;
	public GuiMap mapBirdScript;
	List<Bird> players = new List<Bird>();
	void Start()
	{
		Var.birdInfo = infoText;
		Var.birdInfoHeading = infoHeading;
		Var.birdInfoFeeling = infoFeeling;		
        GuiMap.Instance.CreateMap();
		setMapLocation(0);
		Instance = this;        
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
		mapBirdScript.MoveMapBird(mapPos * 3 + posInMapRound+1);
		foreach (Transform child in graph.transform.Find("ReportGraph").transform)
		{
			Destroy(child.gameObject);
		}
		Reset();
	}

	public void CreateReport()
	{
		battlePanel.SetActive(false);
		

		graph.SetActive(true);
		foreach (Bird bird in players)
		{
			


			

			//Normalize bird stats
			if (bird.confidence > 12)
				bird.confidence = 12;
			if (bird.confidence < -12)
				bird.confidence = -12;
			if (bird.friendliness > 12)
				bird.friendliness = 12;
			if (bird.friendliness < -12)
				bird.friendliness = -12;


			GameObject portrait = bird.portrait;
			GameObject colorObj = portrait.gameObject.transform.Find("bird_color").gameObject;
			colorObj.GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
			Graph.Instance.PlotFull(bird.prevFriend, bird.prevConf, bird.friendliness, bird.confidence, portrait);

		}


		if (finalResult < 0)
		{
			UpdateHearts(--Var.health);
		}
		string feedBackString = "";
			if (currentMapArea != nextMapArea)
			{
				feedBackString += nextMapArea + " birds coming in " + (roundLength - posInMapRound) + " battles!"; 
			}
			if(nextMapArea == Var.Em.finish)
		{
			feedBackString = "Victory in " + (roundLength - posInMapRound-1) + " battles!";
		}
			feedbackText.text = feedBackString;

			string winString = "You won!";
			if (finalResult<0)
			{
				winString = "You lost :'(";
			}
			winText.text = winString;

			int winNo = (finalResult-1)/2+2;

			string winDetString = winNo + " / 3 Battles won!";
			winDetails.text = winDetString;
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
	
	public void Fight()
	{
        feedBack[] feedBackObj = FindObjectsOfType(typeof(feedBack)) as feedBack[];
        foreach (feedBack fb in feedBackObj)
        {
            fb.HideFeedBack();
        }
        Debug.Log("Fight selected");
		int result = 0;
		Bird playerBird = null;

	
	   

	   for (int i = 0; i <Var.enemies.Length; i++)
	   {
			if (Var.enemies[i].inUse)
			{
				for (int j = 0; j < 4; j++)
				{

                    if (Var.enemies[i].position == Bird.dir.front)
                    {
                        if (Var.playerPos[3-j, i%4] != null)
                        {
                            playerBird = Var.playerPos[3-j, i%4];
                            if (!players.Contains(playerBird))
                            {                                
                                playerBird.friendliness += Helpers.Instance.Findfirendlieness(3-j,i%4);
                            }
                            break;
                        }
                    }
                    if (Var.enemies[i].position == Bird.dir.top)
                    {
                        if (Var.playerPos[i%4, j] != null)
                        {
                            playerBird = Var.playerPos[i%4, j];
                            if (!players.Contains(playerBird))
                            {                                
                                playerBird.friendliness += Helpers.Instance.Findfirendlieness( i%4, j);
                            }                           

                            break;
                        }

                    }
                    if (Var.enemies[i].position == Bird.dir.bottom)
                    {
                        if (Var.playerPos[i % 4, 3 - j] != null)
                        {
                            playerBird = Var.playerPos[i % 4, 3 - j];
                            if (!players.Contains(playerBird))
                            {
                                playerBird.friendliness += Helpers.Instance.Findfirendlieness(i % 4, 3 - j);
                            }

                            break;
                        }

                    }

                }

				int resultOfBattle = GameLogic.Instance.Fight(playerBird, Var.enemies[i]);

				// Fight Logic
				battleAnim.Instance.AddData(playerBird, Var.enemies[i], resultOfBattle);
				result += resultOfBattle;


			}

		}
        Bird[] birds = FindObjectsOfType(typeof(Bird)) as Bird[];
        foreach(Bird bird in birds)
        {
            if (!bird.isEnemy)
                players.Add(bird);
        }
	   foreach(Bird bird in players)
		{
			bird.gameObject.GetComponent<firendLine>().RemoveLines();
		}



		battleAnim.Instance.Battle();        
		finalResult = result;
		
		

	}
	public void ReturnToMap()
    {
        if(Var.currentStageID != -1)
        {
            foreach(MapSaveData data in Var.mapSaveData)
            {
                if(data.ID == Var.currentStageID)
                {
                    data.completed = true;
                    foreach(int id in data.targets)
                    {
                        foreach(MapSaveData targ in Var.mapSaveData)
                        {
                            if(targ.ID == id)
                            {
                                targ.available = true;
                                break;
                            }
                        }
                        
                    }
                    break;
                }
            }
        }
        SceneManager.LoadScene("Map");
    }
	public void Reset()
	{
		Var.playerPos = new Bird[4, 4];
		Var.enemies = new Bird[12];
		foreach (Bird bird in players)
		{
			bird.SetEmotion();
		}
		foreach (Bird bird in players)
		{
			bird.gameObject.GetComponent<Animator>().SetBool("iswalking", false);
			bird.gameObject.GetComponent<Animator>().SetBool("lose", false);
			bird.gameObject.GetComponent<Animator>().SetBool("victory", false);
			bird.target = bird.home;
			bird.transform.position = bird.home;
		}


		finalResult = 0;
		players = new List<Bird>();


		moveInMap();
		BattleData Area = Var.map[mapPos];
		GetComponent<fillEnemy>().createEnemies(Area.minConf, Area.maxConf, Area.minFriend, Area.maxFriend,Area.birdLVL);
		GameLogic.Instance.CanWeFight();

        ObstacleGenerator.Instance.clearObstacles();
        ObstacleGenerator.Instance.GenerateObstacles();

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
		

}
