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
    public Image powerBarTemp;
    public Text powerTextTemp;
    public Image[] tiles;
    public LVLIconScript[] lvlIcons;
	public Text reportText;
	public GameObject report;
	public Image[] hearts;
	public GameObject loseBanner;
	public GameObject winBanner;
	public GameObject[] portraits;
	public Transform[] battleTrag;
    public GameObject rerollBox;
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
	public GameObject closeReportBtn;
	public GuiMap mapBirdScript;
	List<Bird> players = new List<Bird>();
	public bool inMap = false;
	List<string> messages;
	public GameObject messageBox;
	public Text messageText;    
	[HideInInspector]
	public int activePortrait = 0;
    public Text tooltipText;    
	void Start()
	{
		messages = new List<string>();
		Var.birdInfo = infoText;
		Var.birdInfoHeading = infoHeading;
		Var.birdInfoFeeling = infoFeeling;
        Var.powerBar = powerBarTemp;
        Var.powerText = powerTextTemp;	       
		Instance = this;
		if (!inMap)
		{
			GuiMap.Instance.CreateMap();
			setMapLocation(0);
		}        
	}


	void UpdateHearts(int health)
	{
		Debug.Log("Health: " + health);
		for(int i=0;i< hearts.Length; i++)
		{
			hearts[i].enabled = i < health;
		}
	}
    public void NoReroll()
    {
        foreach (Bird bird in FillPlayer.Instance.playerBirds)
        {
            bird.UpdateBattleCount();
            bird.AddRoundBonuses();
        }
        Instance.InitiateGraph();
        Instance.CreateBattleReport();
        rerollBox.SetActive(false);
    }
    public void YesReroll()
    {
        
        foreach (Bird bird in Var.activeBirds)
        {
            bird.SetEmotion();
            bird.gameObject.GetComponent<Animator>().SetBool("iswalking", false);
            bird.gameObject.GetComponent<Animator>().SetBool("lose", false);
            bird.gameObject.GetComponent<Animator>().SetBool("victory", false);
            bird.target = bird.home;
            bird.transform.position = bird.home;
            bird.health = bird.prevRoundHealth;
            if (Helpers.Instance.ListContainsLevel(Levels.type.Lonely2, bird.levelList) && bird.CoolDownLeft == 0 && !bird.foughtInRound)
            {
                bird.CoolDownLeft = bird.CoolDownLength;
                bird.CooldownRing.fillAmount = 0;
            }
        }
        for (int i = 0; i < Var.activeBirds.Count; i++)
        {
            FillPlayer.SetupBird(FillPlayer.Instance.playerBirds[i], Var.activeBirds[i]);
        }
        foreach(LayoutButton tile in ObstacleGenerator.Instance.tiles)
        {
            tile.Reset();
        }
        Var.playerPos = new Bird[4, 4];
        GetComponent<fillEnemy>().Reset();
        GameLogic.Instance.CanWeFight();
        GameLogic.Instance.UpdateFeedback();
        rerollBox.SetActive(false);

    }
	public void CloseGraph()
	{

        //graph.SetActive(false);
        //battlePanel.SetActive(true);
        LeanTween.moveLocal(graph, new Vector3(-1550, 0, graph.transform.position.z), 0.7f).setEase(LeanTweenType.easeOutBack);
        mapBirdScript.MoveMapBird(mapPos * 3 + posInMapRound+1);
		foreach (Transform child in graph.transform.Find("ReportGraph").transform)
		{
			Destroy(child.gameObject);
		}
		closeReportBtn.SetActive(false);
		Reset();
	}
	public void CloseBirdStats()
	{
		//graph.SetActive(false);
        LeanTween.moveLocal(graph, new Vector3(-Var.MoveGraphBy, 0, graph.transform.position.z), 0.7f).setEase(LeanTweenType.easeOutBack);
        //battlePanel.SetActive(true);
        foreach (Transform child in graph.transform.Find("ReportGraph").transform)
		{
			Destroy(child.gameObject);
		}
	}
    public void CreateGraph()
    {
        //graph.SetActive(true);        

        //battlePanel.SetActive(false);
        Graph.Instance.portraits = new List<GameObject>();
        foreach (Bird bird in Var.activeBirds)
        {
            //Normalize bird stats
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


            GameObject portrait = bird.portrait;
            GameObject colorObj = portrait.gameObject.transform.Find("bird_color").gameObject;
            colorObj.GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
            Graph.Instance.PlotFull(bird.prevFriend, bird.prevConf, bird.friendliness, bird.confidence, portrait, bird.charName);
            feedbackText.text = "";
            winText.text = "";
            winDetails.text = "";
        }
        
    }
	public void InitiateGraph()
	{
        ProgressGUI.Instance.skillArea.SetActive(false);
        ProgressGUI.Instance.Setup();
        ProgressGUI.Instance.UpdateAllHearts();
        ProgressGUI.Instance.UpdateAllLevels();
        LeanTween.moveLocal(graph, new Vector3(0, 0, graph.transform.position.z), 0.7f).setEase(LeanTweenType.easeOutBack).setOnComplete(CreateGraph);  
	}
	public void CreateBattleReport() {
		closeReportBtn.SetActive(true);
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
		activePortrait = portNr;
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
                            if (!Var.playerPos[3 - j, i % 4].isHiding)
                            {
                                playerBird = Var.playerPos[3 - j, i % 4];
                                break;
                            }
						}
					}
					if (Var.enemies[i].position == Bird.dir.top)
					{
						if (Var.playerPos[i%4, j] != null)
						{
                            if (!Var.playerPos[i % 4, j].isHiding)
                            {
                                playerBird = Var.playerPos[i % 4, j];
                                break;
                            }
						}

					}
					if (Var.enemies[i].position == Bird.dir.bottom)
					{
						if (Var.playerPos[i % 4, 3 - j] != null)
						{
                            if (!Var.playerPos[i % 4, 3 - j].isHiding)
                            {
                                playerBird = Var.playerPos[i % 4, 3 - j];
                                break;
                            }
						}

					}

				}
				playerBird.foughtInRound = true;
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
			{
				
				players.Add(bird);
			}
		}
	   foreach(Bird bird in players)
		{
			bird.friendBoost += Helpers.Instance.Findfirendlieness(bird);
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
		
		Var.enemies = new Bird[12];
		foreach (Bird bird in players)
		{			
			bird.SetEmotion();
			UpdateBirdSave(bird);		
			bird.gameObject.GetComponent<Animator>().SetBool("iswalking", false);
			bird.gameObject.GetComponent<Animator>().SetBool("lose", false);
			bird.gameObject.GetComponent<Animator>().SetBool("victory", false);
			bird.target = bird.home;
			bird.transform.position = bird.home;
            bird.prevConf = bird.confidence;
            bird.prevFriend = bird.friendliness;
        }
		//After applying levels;
		foreach(Bird bird in players)
		{
			bird.ResetAfterLevel();
		}
        Var.playerPos = new Bird[4, 4];
        foreach (LayoutButton tile in ObstacleGenerator.Instance.tiles)
		{
			tile.Reset();
		}

		finalResult = 0;
		players = new List<Bird>();


		moveInMap();
		BattleData Area = Var.map[mapPos];
		if (Area.type != Var.Em.finish)
		{
			GetComponent<fillEnemy>().createEnemies(Area.minConf, Area.maxConf, Area.minFriend, Area.maxFriend, Area.birdLVL, Area.dirs);
			GameLogic.Instance.CanWeFight();
			ObstacleGenerator.Instance.clearObstacles();
			ObstacleGenerator.Instance.GenerateObstacles();
		}

	}

	void UpdateBirdSave(Bird bird)
	{
		for (int i = 0; i < Var.activeBirds.Count; i++)
		{
			if (Var.activeBirds[i].charName == bird.charName)
			{
				Var.activeBirds[i] = bird;
				break;
			}
		}

		if (Var.availableBirds.Count > 0)
		{
			for (int i = 0; i < Var.availableBirds.Count; i++)
			{
				if (Var.availableBirds[i].charName == bird.charName)
				{
					Var.availableBirds[i] = bird;
					break;
				}
			}
		}
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
            posInMapRound = 0;
            mapPos++;
            if (nextMapArea== Var.Em.finish)
			{
				winBanner.SetActive(true);
				mapPos = 0;
			}
			
			setMapLocation(mapPos);
		}
		
	}
	
	public void ShowMessage(string message)
	{
		messages.Add(message);
		if (!messageBox.activeSelf)
		{
			messageText.text = message;
		}
		messageBox.SetActive(true);

	}	
	public void HideMessage()
	{
		messages.RemoveAt(0);
		if (messages.Count == 0)
		{
			messageText.text = "";
			messageBox.SetActive(false);
		}else
		{
			messageText.text = messages[0];
		}

	}
}
