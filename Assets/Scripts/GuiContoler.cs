using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System.Linq;
using UnityEngine.SceneManagement;

public class GuiContoler : MonoBehaviour {
	public static GuiContoler Instance { get; private set; }
	public RectTransform canvasRect;
	public Text BirdLVLUpText;
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
	public int roundLength = 3;
	Var.Em currentMapArea;
	public Var.Em nextMapArea;
	public int posInMapRound = 0;
	public static int mapPos = 0;
	private int finalResult = 0;
	public GameObject graph;
	public Text winText;
	public Text winDetails;
	public Text feedbackText;
	public GameObject battlePanel;   
	public GameObject closeReportBtn;
	public Button CloseBattleReport;
	public GuiMap mapBirdScript;
	List<Bird> players = new List<Bird>();
	public bool inMap = false;
	List<string> messages;
	public GameObject messageBox;
	public Text messageText;    
	[HideInInspector]
	public int activePortrait = 0;
	public Text tooltipText;
	public GameObject speechBubble;
	List<string> speechTexts = new List<string>();
	List<Transform> speechPos = new List<Transform>();
	public Text SpeechBubbleText;
	public Text SpeechBubbleReminderText;
	public GameObject speechBubbleObj;
	public SliderSwitcher confSlider;
	public SliderSwitcher firendSlider;    
	public Image[] BirdInfoHearts;
	public Text levelNumberText;
	public GameObject pause;
	public GameObject deathMenu;
	public Text deathTitle;
	public Toggle[] deadBirdToggles;
	bool CanToggleDeathTriggers = true;
	Bird SelectedDeathBird = null;
	Bird DeadBird = null;
	[HideInInspector]
	public Bird selectedBird;
	public Button SelectDeathBirdBtn;
	int currentGraph = -1;
	public Button nextGraph;
	public Button prevGraph;
	public Button HideSmallGraph;

	public float levelInfo_yvalue;
	public float levelinfoHideXValue;
	public float levelinfoshowXValue;
	void Awake()
	{
		Instance = this;
	}
	void Start()
	{
		Var.Infight = false;
		if (Var.emotionParticles == null)
			Var.emotionParticles = Resources.Load("EmotionParticle") as GameObject;
		messages = new List<string>();
		Var.birdInfo = infoText;
		Var.birdInfoHeading = infoHeading;
		Var.birdInfoFeeling = infoFeeling;
		Var.powerBar = powerBarTemp;
		Var.powerText = powerTextTemp;	    
		if (!inMap)
		{
			GuiMap.Instance.CreateMap();
			setMapLocation(0);
		}        
	}
	public void setPause()
	{
		AudioControler.Instance.ClickSound();
		if (pause.activeSelf)
		{
			pause.SetActive(false);
			Time.timeScale = 1.0f;
		}
		else
		{
			pause.SetActive(true);
			Time.timeScale = 0.0f;
		}
	}
   
	public void QuitGame()
	{
		AudioControler.Instance.ClickSound();
		Application.Quit();
	}

	public void QuitToMap()
	{
		int deadCount = 0;        
		foreach(Bird bird in Var.availableBirds)
		{
			if (bird.health <= 0)
				deadCount++;
		}
		if (deadCount < 3)
		{
			AudioControler.Instance.ClickSound();
			Time.timeScale = 1.0f;
			Var.fled = true;
			SceneManager.LoadScene("Map");
		}else
		{
			Time.timeScale = 1.0f;
			loseBanner.SetActive(true);
		}
	}

	public void ShowDeathMenu(Bird deadBird)
	{
		DeadBird = deadBird;
		SelectDeathBirdBtn.interactable = false;
		deathTitle.text = deadBird.charName + " has died! Select a bird to replace him!";        
		for(int i=0;i<Var.availableBirds.Count;i++)
		{
			if (Var.availableBirds[i].charName == deadBird.charName)
			{
				Debug.Log("Set doude as dead");
				Var.availableBirds[i] = deadBird;
			}
		}
		deathMenu.SetActive(true);
	}

	public void DeadBirdToggle(int num)
	{
		if (!CanToggleDeathTriggers)
			return;
		CanToggleDeathTriggers = false;
		
		LeanTween.delayedCall(0.05f, allowCall);
		for(int i = 0; i < deadBirdToggles.Length; i++)
		{
			if (num != i)
				deadBirdToggles[i].isOn = false;
			if(num == i)
			{
				SelectDeathBirdBtn.interactable = deadBirdToggles[i].isOn;
				if (deadBirdToggles[i].isOn)                
					SelectedDeathBird = FillPlayer.Instance.deadBirds[num];                
				else                
					SelectedDeathBird = null;                 
			}
		}
	}
	void allowCall()
	{
		CanToggleDeathTriggers = true;
	}
	public void SelectDeathBird()
	{
		Var.availableBirds.Remove(SelectedDeathBird);
		FillPlayer.SetupBird(DeadBird, SelectedDeathBird);        
		DeadBird.gameObject.SetActive(true);
		deathMenu.SetActive(false);
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{           
			setPause();
		}
		if (Input.GetMouseButtonDown(0))
		{
			if (speechBubbleObj != null)
			{
				if (speechTexts.Count == 0)
				{
					speechBubbleObj.SetActive(false);
					if(Var.isTutorial)
						CloseBattleReport.interactable = true;
				}
				else
				{
					SpeechBubbleText.text = speechTexts[0];                    
					speechBubble.GetComponent<UIFollow>().target = speechPos[0];
					speechPos.RemoveAt(0);
					speechTexts.RemoveAt(0);
				}
			}
		}
	}

	public void ShowSpeechBubble(Transform pos,string text)
	{
		if (speechBubbleObj.activeSelf) {
			if (Var.isTutorial)
				CloseBattleReport.interactable = false;
			speechTexts.Add(text);
			speechPos.Add(pos);
		}
		else
		{
			SpeechBubbleText.text = text;
			speechBubbleObj.SetActive(true);
			speechBubble.GetComponent<UIFollow>().target = pos;
		   // LeanTween.delayedCall(6f, ShowSpeechBubbleReminder);
		}
	   
 
	}
	void ShowSpeechBubbleReminder()
	{
		SpeechBubbleReminderText.gameObject.SetActive(true);
	}

	public void ShowNextGraph()
	{
        if (currentGraph == 3)
        {
            CloseGraph();
            return;
        }
		currentGraph++;
		if (Var.isTutorial)
		{
			Tutorial.Instance.ShowGraphSpeech(currentGraph);
		}
		if (currentGraph == 3)
		{
			CreateGraph(-1);
		}
		else
		{
			CreateGraph(currentGraph);
			ProgressGUI.Instance.PortraitClick(Var.activeBirds[currentGraph]);
		}
		
		
		
	}
	public void ShowPrevGraph()
	{
		currentGraph--;
		CreateGraph(currentGraph);
		ProgressGUI.Instance.PortraitClick(Var.activeBirds[currentGraph]);
		
	   
		
	}

	public void ShowLvlText(string text)
	{
		BirdLVLUpText.text = text;
		//BirdLVLUpText.transform.parent.gameObject.SetActive(true);30
		MoveLvlText();
	}
	public void MoveLvlText()
	{
		if(selectedBird.levelUpText != null)
			LeanTween.moveLocal(BirdLVLUpText.transform.parent.gameObject, new Vector3(levelinfoshowXValue, levelInfo_yvalue, 0), 0.3f).setEase(LeanTweenType.easeOutBack);

	}
	public void HideLvlText()
	{
		//BirdLVLUpText.text = "";
		LeanTween.moveLocal(BirdLVLUpText.transform.parent.gameObject, new Vector3(levelinfoHideXValue, levelInfo_yvalue, 0), 0.3f).setEase(LeanTweenType.easeInBack);
		//BirdLVLUpText.transform.parent.gameObject.SetActive(false);-340

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
			UpdateBirdSave(bird);
		}
		Instance.InitiateGraph(Var.activeBirds[0]);       
		Instance.CreateBattleReport();
		rerollBox.SetActive(false);
	}
	public void YesReroll()
	{
		
		foreach (Bird bird in Var.activeBirds)
		{
			bird.SetEmotion();
			bird.gameObject.GetComponent<Animator>().SetBool("iswalking", false);
		   // bird.gameObject.GetComponent<Animator>().SetBool("lose", false);
		   // bird.gameObject.GetComponent<Animator>().SetBool("victory", false);
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
        AudioControler.Instance.PlayPaperSound();
		battlePanel.SetActive(true);
        if (!Reset())
            return;
        LeanTween.moveLocal(graph, new Vector3(-1550, 0, graph.transform.position.z), 0.7f).setEase(LeanTweenType.easeOutBack);		
		foreach (Transform child in graph.transform.Find("ReportGraph").transform)
		{
			Destroy(child.gameObject);
		}
		closeReportBtn.SetActive(false);
		HideSmallGraph.gameObject.SetActive(true);
		
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
	public void CreateGraph(object o)
	{
		Helpers.Instance.HideTooltip();
		battlePanel.SetActive(false);       
		foreach (Transform child in graph.transform.Find("ReportGraph").transform)
		{
			Destroy(child.gameObject);
		}
		int birdNum = (int)o;
        if (birdNum == -1)
            currentGraph = 3;
        else
            currentGraph = birdNum;
        Graph.Instance.portraits = new List<GameObject>();
		List<Bird> BirdsToGraph;
		if (birdNum == -1)
			BirdsToGraph = Var.activeBirds;
		else
		{
			BirdsToGraph = new List<Bird>() { Var.activeBirds[birdNum] };
			if (Var.activeBirds[birdNum].hasNewLevel)
			{
				levelPopupScript.Instance.Setup(Var.activeBirds[birdNum], Var.activeBirds[birdNum].lastLevel, birdNum);
				AudioControler.Instance.PlaySound(AudioControler.Instance.applause);
				return;
			}
		}



		foreach (Bird bird in BirdsToGraph)
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
			//colorObj.GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
			Graph.Instance.PlotFull(bird);
			//feedbackText.text = "";
			winText.text = "";
			//winDetails.text = "";
		}
		CheckGraphNavBtns();
	} 
	
	void CheckGraphNavBtns()
	{
		int maxGraph = 3;
		if (Var.isTutorial)
			maxGraph = Tutorial.Instance.BirdCount[Tutorial.Instance.CurrentPos]-1;
		//nextGraph.interactable = (currentGraph < maxGraph);
		prevGraph.interactable = (currentGraph > 0);
		//CloseBattleReport.interactable = (currentGraph == maxGraph);
        
	}
	public void GraphButton()
	{
		InitiateGraph(selectedBird);
	}

	public void InitiateGraph(Bird bird)
	{
		int index = -1;
		for(int i= 0; i < Var.activeBirds.Count; i++)
		{
			if(bird.charName == Var.activeBirds[i].charName)
			{
				index = i;
				break;
			}
		}
		ProgressGUI.Instance.PortraitClick(bird);
		ProgressGUI.Instance.skillArea.SetActive(false);
		LeanTween.moveLocal(graph, new Vector3(0, 0, graph.transform.position.z), 0.7f).setEase(LeanTweenType.easeOutBack).setOnComplete(CreateGraph).setOnCompleteParam(index as object);
	   
			
			}
	public void CreateBattleReport() {
		feedbackText.gameObject.SetActive(true);
		feedbackText.gameObject.SetActive(true);
		closeReportBtn.SetActive(true);
		HideSmallGraph.gameObject.SetActive(false);
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
		GameLogic.Instance.FightButton.interactable = false;
		AudioControler.Instance.ClickSound();
		AudioControler.Instance.setBattleVolume(0.85f);
		AudioControler.Instance.ambientAudioSource.PlayOneShot(AudioControler.Instance.battleStart);
		feedBack[] feedBackObj = FindObjectsOfType(typeof(feedBack)) as feedBack[];
		foreach (feedBack fb in feedBackObj)
		{
			fb.HideFeedBack(true);
		}
		Debug.Log("Fight selected");
		int result = 0;
		Var.Infight = true;
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

			if (!bird.isEnemy && !bird.dead)
			{
				
				players.Add(bird);
			}
		}
	   foreach(Bird bird in players)
		{
			int friendGain = Helpers.Instance.Findfirendlieness(bird);
			if (friendGain > 0)
				Helpers.Instance.EmitEmotionParticles(bird.transform, Var.Em.Friendly);
			if(friendGain<0)
				Helpers.Instance.EmitEmotionParticles(bird.transform, Var.Em.Lonely);

			bird.friendBoost += friendGain;			
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
        Var.shouldDoMapEvent = true;
        SceneManager.LoadScene("Map");
	}

	public void LoadMainMenu()
	{
		Time.timeScale = 1.0f;
		AudioControler.Instance.ClickSound();
		SceneManager.LoadScene("MainMenu");

	}
	public bool Reset()
	{
		
		Var.enemies = new Bird[8];
		Var.Infight = false;
		foreach (Bird bird in players)
		{						
			UpdateBirdSave(bird);		
            foreach(Bird activeBird in Var.activeBirds)
            {
                if (activeBird.health <= 0)
                {
                    Time.timeScale = 0.0f;
                    QuitToMap();
                    return false;
                }
            }
			bird.gameObject.GetComponent<Animator>().SetBool("iswalking", false);
			//bird.gameObject.GetComponent<Animator>().SetBool("lose", false);
			//bird.gameObject.GetComponent<Animator>().SetBool("victory", false);
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

        mapBirdScript.MoveMapBird(mapPos * 3 + posInMapRound + 1);
        moveInMap();
		if (Var.isTutorial)
		{
			for (int i = 0; i < FillPlayer.Instance.playerBirds.Length; i++)
			{
				if (i < Tutorial.Instance.BirdCount[posInMapRound + mapPos * 3])
				{
					FillPlayer.Instance.playerBirds[i].gameObject.SetActive(true);
				}
				else
				{
					FillPlayer.Instance.playerBirds[i].gameObject.SetActive(false);
				}
			}
			Tutorial.Instance.ShowtutorialStartingText(posInMapRound + mapPos * 3);
		}
		BattleData Area = Var.map[mapPos];
		if (Area.type != Var.Em.finish)
		{
			if (Var.isTutorial)
			{
				int posInMap = posInMapRound  + mapPos * 3;
				Tutorial.Instance.SetCurrenPos(posInMap);
				GetComponent<fillEnemy>().CreateTutorialEnemies(Tutorial.Instance.TutorialMap[posInMap]);
			}
			else
			{
				GetComponent<fillEnemy>().createEnemies(Area.minConf, Area.maxConf, Area.minFriend, Area.maxFriend, Area.birdLVL, Area.dirs, Area.minEnemies, Area.maxEnemies);
				ObstacleGenerator.Instance.clearObstacles();
				ObstacleGenerator.Instance.GenerateObstacles();
                EventController.Instance.tryEvent();
			}
			GameLogic.Instance.CanWeFight();
			
		}
        return true;

	}

	public void UpdateBirdSave(Bird bird)
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
				Var.isTutorial = false;
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
