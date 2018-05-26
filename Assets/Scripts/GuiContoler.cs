using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System.Linq;
using UnityEngine.SceneManagement;

public class GuiContoler : MonoBehaviour {
	public static GuiContoler Instance { get; private set; }
	public Image dangerFollowHighlight;
	public Image[] vingette;
	public GameObject kingMouth;
	public GameObject playerMouth;
	public Text EmotionChangeFeedback;
	public Text EmotionChangeHeading;
	public Text ToggleRelationPanelText;
	public GameObject relationshipPortrait;
	public Text relationshipText;
	public GameObject reportRelationshipPortrait;
	public Text reportRelationshipText;
	public GameObject statPanel;
	public GameObject relationshipPanel;
	public RectTransform canvasRect;
	public Text BirdLVLUpText;
	public Text BirdCombatStr;
	public Text infoText;    
	public Text infoHeading;
	public Text infoFeeling;
	public Image powerBarTemp;
	public Text powerTextTemp;	
	public LVLIconScript[] lvlIcons;
	public Text reportText;
	public GameObject report;
	public Image[] hearts;
	public GameObject loseBanner;
	public GameObject winBanner;
	public GameObject[] portraits;
	public Transform[] battleTrag;
	public GameObject rerollBox;	
	Var.Em currentMapArea;
	public Var.Em nextMapArea;
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
	public Image[] BirdMentalHearts;
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
	[HideInInspector]
	public int currentGraph = -1;
	public Button nextGraph;
	public Button prevGraph;
	public Button HideSmallGraph;
	public GameObject relationshipSliders;
	public GameObject reportRelationshipSliders;
	public float levelinfoHideXValue;
	public float levelinfoshowXValue;
	public Graph smallGraph;
	int maxGraph = 3;
	public GameObject boss;
	Transform lastSpeechPos = null;
	public Animator graphAnime;
	public GameObject minimap;
	public GameObject dangerZoneBorder;
	bool canChangeGraph = true;
	bool GraphActive = false;
	void Awake()
	{
		if (!Var.StartedNormally)
			Var.gameSettings.shownLevelTutorial = true;
		LeanTween.init(1000);
		Instance = this;
		maxGraph = 3;
		Var.birdInfo = infoText;
		Var.birdInfoHeading = infoHeading;
		Var.birdInfoFeeling = infoFeeling;
		Var.powerBar = powerBarTemp;
		Var.powerText = powerTextTemp;
	}
	void Start()
	{
		Var.Infight = false;
		if (Var.emotionParticles == null)
			Var.emotionParticles = Resources.Load("EmotionParticle") as GameObject;
		if (Var.isTutorial)
		{
			try
			{
				Tutorial.Instance.enabled = true;
			}
			catch { }
		}
		messages = new List<string>();
		   
		if (!inMap)
		{
			foreach(GuiMap map in FindObjectsOfType<GuiMap>())
				map.CreateMap(Var.map);            
			setMapLocation(0);
			LeanTween.delayedCall(0.05f,tryDialog);
			boss.SetActive(Var.isBoss);
		}        
	}

	void tryDialog()
	{
		if (Var.currentStageID != 1 && Var.currentStageID != 0)       
			DialogueControl.Instance.TryDialogue(Dialogue.Location.battle);       
	}
	
	
	
	public void StatToggle()
	{
		if (relationshipPanel.activeSelf)
		{
			relationshipPanel.SetActive(false);
			statPanel.SetActive(true);
			ToggleRelationPanelText.text = "Relationships";
		}else
		{
			relationshipPanel.SetActive(true);
			statPanel.SetActive(false);
			ToggleRelationPanelText.text = "Stats";
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
		SaveLoad.Save();
		Application.Quit();
	}

	public void QuitToMap()
	{
		if(!Var.tutorialCompleted)
		{
			Time.timeScale = 1.0f;
			SceneManager.LoadScene("MainMenu");
			return;
		}
		int livingCount = 0;
		Var.currentWeek++;
		foreach (Bird bird in Var.availableBirds)
		{
			bird.DecreaseTurnsInjured();
			if (bird.health <= 0)
			{
				livingCount++;
				bird.AddRoundBonuses();
			}
		}
		if (livingCount < 3)
		{
			AudioControler.Instance.ClickSound();
			Time.timeScale = 1.0f;
			Var.fled = true;
			LeanTween.cancelAll();
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
		/*for(int i = 0; i < deadBirdToggles.Length; i++)
		{
			if (num != i)
				deadBirdToggles[i].isOn = false;
			if(num == i)
			{
				SelectDeathBirdBtn.interactable = deadBirdToggles[i].isOn;
				if (deadBirdToggles[i].isOn)                
					//SelectedDeathBird = FillPlayer.Instance.deadBirds[num];                
				else                
					SelectedDeathBird = null;                 
			}
		}*/
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
		if (Input.GetKeyDown(KeyCode.O))
			ReturnToMap();
		if (GraphActive && Input.GetMouseButtonDown(1))
		{
			if (nextGraph.gameObject.activeInHierarchy)
				nextGraph.onClick.Invoke();
			else if (CloseBattleReport.gameObject.activeSelf)
				CloseBattleReport.onClick.Invoke();


		}
	}
	public void SpeechBubbleClicked()
	{		
			if (speechBubbleObj != null)
			{
				if (speechTexts.Count == 0)
				{
					speechBubbleObj.SetActive(false);

					lastSpeechPos = null;
					if (!Var.isBoss && !inMap)
					{
						///GameObject dustObj = Instantiate(Var.dustCloud, boss.transform.position, Quaternion.identity);
						//dustObj.transform.localPosition = Vector3.zero;                       
						//Destroy(dustObj, 1.0f);
						boss.SetActive(false);

					}

					if (!inMap)
					{
						foreach (Image img in vingette)
						{
							LeanTween.alpha(img.rectTransform, 0, 0.5f);
						}
						CheckGraphNavBtns();
						GameLogic.Instance.CanWeFight();
					}

				}
				else
				{
					AudioControler.Instance.PlayVoice();
					SpeechBubbleText.text = speechTexts[0];
					speechBubble.GetComponent<UIFollow>().target = speechPos[0];
					if (lastSpeechPos != null && lastSpeechPos.Equals(speechPos[0]))
					{
						speechBubbleObj.GetComponent<Animator>().SetTrigger("newline");
					}
					lastSpeechPos = speechPos[0];
					speechPos.RemoveAt(0);
					speechTexts.RemoveAt(0);
					if (!inMap)
					{
						prevGraph.interactable = false;
						nextGraph.interactable = false;
					}
				}
			}		
	}



	public void ShowSpeechBubble(Transform pos,string text)
	{
		if (speechBubbleObj.activeSelf) {            
			speechTexts.Add(text);
			speechPos.Add(pos);
		}
		else
		{
			if (!inMap)
			{
				prevGraph.interactable = false;
				nextGraph.interactable = false;
			}
			SpeechBubbleText.text = text;
			speechBubbleObj.SetActive(true);
			foreach (Image img in vingette)
			{
				LeanTween.alpha(img.rectTransform, 0.5f, 0.5f);
			}
			speechBubble.GetComponent<UIFollow>().target = pos;
			try
			{
				AudioControler.Instance.PlayVoice();
			}
			catch { print("voice issue"); }
		   // LeanTween.delayedCall(6f, ShowSpeechBubbleReminder);
		}
	   
 
	}
	void ShowSpeechBubbleReminder()
	{
		SpeechBubbleReminderText.gameObject.SetActive(true);
	}

	public void ShowNextGraph()
	{
		if (!canChangeGraph)
			return;
		canChangeGraph = false;
		if (currentGraph == maxGraph)
		{
			CloseGraph();			
			return;
		}
		
		currentGraph++;
		currentGraph = Mathf.Min(3, currentGraph);
		print("currentGraph : " + currentGraph);
		if (Var.isTutorial)
		{
			Tutorial.Instance.ShowGraphSpeech(currentGraph);
		}
		foreach (Transform child in graph.transform.Find("GraphParts").transform)
		{
			Destroy(child.gameObject);
		}
		if (currentGraph == 3)
		{			
			LeanTween.delayedCall(0.2f,()=>CreateGraph(-1));
			
			dangerZoneBorder.SetActive(false);
			ProgressGUI.Instance.AllPortraitClick();
		}
		else
		{			
			LeanTween.delayedCall(0.2f, () =>
			{
				CreateGraph(currentGraph);
				ProgressGUI.Instance.PortraitClick(Var.activeBirds[currentGraph]);
			});
			ProgressGUI.Instance.SetOnePortrait();
		}
		
		
		
	}
	public void ShowPrevGraph()
	{
		currentGraph--;
		CreateGraph(currentGraph);
		ProgressGUI.Instance.SetOnePortrait();
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
		{
			LeanTween.moveLocal(BirdLVLUpText.transform.parent.gameObject, new Vector3(levelinfoshowXValue, -200, 0), 0.3f).setEase(LeanTweenType.easeOutBack);
		}

	}
	public void HideLvlText()
	{
		//BirdLVLUpText.text = "";
		LeanTween.moveLocal(BirdLVLUpText.transform.parent.gameObject, new Vector3(levelinfoHideXValue, -200, 0), 0.3f).setEase(LeanTweenType.easeInBack);
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
			bird.TryLevelUp();
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
			bird.gameObject.GetComponentInChildren<Animator>().SetBool("iswalking", false);
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
		GraphActive = false;
		AudioControler.Instance.PlayPaperSound();
		canChangeGraph = true;
		battlePanel.SetActive(true);
		minimap.SetActive(true);
		if (!Reset())
			return;
		graphAnime.SetBool("open", false);
		//LeanTween.moveLocal(graph, new Vector3(0, -Var.MoveGraphBy, graph.transform.position.z), 0.7f).setEase(LeanTweenType.easeOutBack);		
		foreach (Transform child in graph.transform.Find("GraphParts").transform)
		{
			Destroy(child.gameObject);
		}
		closeReportBtn.SetActive(false);
		HideSmallGraph.gameObject.SetActive(true);
		Var.activeBirds[0].showText();
		dangerFollowHighlight.gameObject.SetActive(false);
		foreach (Bird activeBird in Var.activeBirds)
			activeBird.SetEmotion();
	}
	public void CloseBirdStats()
	{
		minimap.SetActive(true);
		graphAnime.SetBool("open", false);
		//graph.SetActive(false);
//		LeanTween.moveLocal(graph, new Vector3(0, -Var.MoveGraphBy, graph.transform.position.z), 0.7f).setEase(LeanTweenType.easeOutBack); //seb
		//battlePanel.SetActive(true);
		foreach (Transform child in graph.transform.Find("GraphParts").transform)
		{
			Destroy(child.gameObject);
		}
		foreach (Bird bird in FillPlayer.Instance.playerBirds)
			LeanTween.delayedCall(0.1f, () => bird.SetAnimation(bird.emotion));
		LeanTween.delayedCall(0.2f,()=> selectedBird.showText());
	}

	public void clearSmallGraph()
	{
		foreach (Transform child in smallGraph.graphParent.transform)
		{
			Destroy(child.gameObject);
		}
	}
	public void CreateGraph(object o,bool afterBattle = true)
	{
		GraphActive = true;
		canChangeGraph = true;
		Helpers.Instance.HideTooltip();
		battlePanel.SetActive(false);
		minimap.SetActive(false);   
		foreach (Transform child in graph.transform.Find("GraphParts").transform)
		{
			Destroy(child.gameObject);
		}
		int birdNum = (int)o;
		
		Graph.Instance.portraits = new List<GameObject>();
		List<Bird> BirdsToGraph;
		if (birdNum == -1)
		{
			BirdsToGraph = Var.activeBirds;
			ProgressGUI.Instance.ConditionsBG.transform.parent.gameObject.SetActive(false);
			dangerFollowHighlight.gameObject.SetActive(false);
		}
		else
		{
			BirdsToGraph = new List<Bird>() { Var.activeBirds[birdNum] };
			dangerZoneBorder.SetActive(true);
			if (Var.activeBirds[birdNum].hasNewLevel)
			{
				levelPopupScript.Instance.Setup(Var.activeBirds[birdNum], Var.activeBirds[birdNum].lastLevel, birdNum);
				AudioControler.Instance.PlaySound(AudioControler.Instance.applause);
				return;
			}
			dangerFollowHighlight.gameObject.SetActive(true);
		}

		if (birdNum == -1)
			currentGraph = 3;
		else
			currentGraph = birdNum;

		foreach (Bird bird in BirdsToGraph)
		{
			if (!bird.injured)
			{
				//Normalize bird stats
				Helpers.Instance.NormalizeStats(bird);
				GameObject portrait = bird.portrait;
				GameObject colorObj = portrait.gameObject.transform.Find("bird_color").gameObject;
				//colorObj.GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
				Graph.Instance.PlotFull(bird,afterBattle);
				//feedbackText.text = "";
				winText.text = "";
			}
			//winDetails.text = "";
		}
		CheckGraphNavBtns();
		if (currentGraph != 3)
		{
			//Normal case
			DialogueControl.Instance.TryDialogue(Dialogue.Location.graph, Helpers.Instance.GetCharEnum(Var.activeBirds[birdNum]));
			EmotionChangeFeedback.gameObject.SetActive(true);
			string changeText = CreateEmotionChangeText(Var.activeBirds[birdNum]);
			EmotionChangeFeedback.text = changeText;
			EmotionChangeHeading.gameObject.SetActive(changeText != "");
		}
		else
		{
			//Summary
			EmotionChangeFeedback.gameObject.SetActive(false);

		}
	} 

	 public string CreateEmotionChangeText(Bird bird)
	{
		string fbText = "";
		int ConfGainedInRound = bird.battleConfBoos + bird.groundConfBoos + bird.wizardConfBoos + bird.levelConfBoos;
		int FriendGainedInRound = bird.friendBoost + bird.wizardFrienBoos + bird.groundFriendBoos + bird.levelFriendBoos;
		//Confidence stuff
		if (ConfGainedInRound > 0)
			fbText += "\n"+Helpers.Instance.BraveHexColor+"<b>Confidence gained: " + ConfGainedInRound +"</b></color>";
		if (ConfGainedInRound < 0)
			fbText += "\n" + Helpers.Instance.ScaredHexColor + "<b>Caution gained: " + Mathf.Abs(ConfGainedInRound) + "</b></color>";
		if (bird.battleConfBoos > 0)
			fbText += Helpers.Instance.BraveHexColor + "\n\tFrom combat: " + bird.battleConfBoos.ToString("+#;-#;0") + " confidence</color>";
		if (bird.battleConfBoos < 0)
			fbText += Helpers.Instance.ScaredHexColor + "\n\tFrom combat: " + Mathf.Abs(bird.battleConfBoos).ToString("+#;-#;0") + " caution</color>";
		if(bird.injured)
			fbText += Helpers.Instance.ScaredHexColor + "\n\t(5 caution from injury)</color>";
		if (bird.groundConfBoos > 0)
			fbText += Helpers.Instance.BraveHexColor + "\n\tFrom tiles: " + bird.groundConfBoos.ToString("+#;-#;0") + " confidence</color>";
		if (bird.groundConfBoos < 0)
			fbText += Helpers.Instance.ScaredHexColor + "\n\tFrom tiles: " + Mathf.Abs(bird.groundConfBoos).ToString("+#;-#;0") + " caution</color>";
		if (bird.levelConfBoos > 0)
			fbText += Helpers.Instance.BraveHexColor + "\n\tFrom level abilities: " + bird.levelConfBoos.ToString("+#;-#;0") + " confidence</color>";
		if (bird.levelConfBoos < 0)
			fbText += Helpers.Instance.ScaredHexColor + "\n\tFrom level abilities: " + Mathf.Abs(bird.levelConfBoos).ToString("+#;-#;0") + " caution</color>";
		if (bird.wizardConfBoos > 0)
			fbText += Helpers.Instance.BraveHexColor + "\n\tFrom enemies: " + bird.wizardConfBoos.ToString("+#;-#;0") + " confidence</color>";
		if (bird.wizardConfBoos < 0)
			fbText += Helpers.Instance.ScaredHexColor + "\n\tFrom enemies: " + Mathf.Abs(bird.wizardConfBoos).ToString("+#;-#;0") + " caution</color>";






		//Friendship stuff
		if (FriendGainedInRound > 0)
			fbText += "\n" + Helpers.Instance.FriendlyHexColor + "<b>Friendliness gained: " + FriendGainedInRound + "</b></color>";
		if (FriendGainedInRound < 0)
			fbText += "\n" + Helpers.Instance.LonelyHexColor + "<b>Solitude gained: " + Mathf.Abs(FriendGainedInRound) + "</b></color>";
		if (bird.friendBoost > 0)
			fbText += Helpers.Instance.FriendlyHexColor + "\n\tFrom interactions: " + bird.friendBoost.ToString("+#;-#;0") + " friendship</color>";
		if (bird.friendBoost < 0)
			fbText += Helpers.Instance.LonelyHexColor + "\n\tFrom interactions: " + Mathf.Abs(bird.friendBoost).ToString("+#;-#;0") + " solitude</color>";
		if (bird.groundFriendBoos > 0)
			fbText += Helpers.Instance.FriendlyHexColor + "\n\tFrom tiles: " + bird.groundFriendBoos.ToString("+#;-#;0") + " friendship</color>";
		if (bird.groundFriendBoos < 0)
			fbText += Helpers.Instance.LonelyHexColor + "\n\tFrom tiles: " + Mathf.Abs(bird.groundFriendBoos).ToString("+#;-#;0") + " solitude</color>";
		if (bird.levelFriendBoos > 0)
			fbText += Helpers.Instance.FriendlyHexColor + "\n\tFrom level abilities: " + bird.levelFriendBoos.ToString("+#;-#;0") + " friendship</color>";
		if (bird.levelFriendBoos < 0)
			fbText += Helpers.Instance.LonelyHexColor + "\n\tFrom level abilities: " + Mathf.Abs(bird.levelFriendBoos).ToString("+#;-#;0") + " solitude</color>";
		if (bird.wizardFrienBoos > 0)
			fbText += Helpers.Instance.FriendlyHexColor + "\n\tFrom enemies: " + bird.wizardFrienBoos.ToString("+#;-#;0") + " friendship</color>";
		if (bird.wizardFrienBoos < 0)
			fbText += Helpers.Instance.LonelyHexColor + "\n\tFrom enemies: " + Mathf.Abs(bird.wizardFrienBoos).ToString("+#;-#;0") + " solitude</color>";
		return fbText;
	}



	/*void CreateRelationshipEvent(int birdNum)
	{
		if (Var.activeBirds[birdNum].newRelationship)
		{
			Bird relationshipBird = Var.activeBirds[birdNum].relationshipBird;
			if (Var.activeBirds[birdNum].GetRelationshipBonus() > 0)
			{
				//Relationship
				string title = "<name> is in a relationship!";
				string text = "Things are getting serious for <name> and " + relationshipBird.charName + ". They seem very happy for now, but will it last?\nBoth <name> and " + relationshipBird.charName + "gain <b>+20% combat strength<\b> while in this relationship";
				EventScript relationshipEvent = new EventScript(Helpers.Instance.GetCharEnum(Var.activeBirds[birdNum]), title, text);
				EventController.Instance.CreateEvent(relationshipEvent);
			}
			if (Var.activeBirds[birdNum].GetRelationshipBonus() < 0)
			{
				//Crush
				string title = "<name> has a crush!";
				string text = "<name> has fallen hard for " + relationshipBird.charName + "! will you help <name> get together with his paramour or drive them apart?\n<name> will have <b>-20% combat strength</b> until he gets over the crush, or achives a realtionship with their beloved.";
				EventScript relationshipEvent = new EventScript(Helpers.Instance.GetCharEnum(Var.activeBirds[birdNum]), title, text);
				EventController.Instance.CreateEvent(relationshipEvent);
			}
			if (Var.activeBirds[birdNum].GetRelationshipBonus() == 0)
			{
				//Breakup
				string title = "A break up for <name>!";
				string text = "<name> have seen their new romantic dreams evaporate in front of them! Will <name> try to pursue the previous lover once again, find a new love or just be single for a while?\n<name>'s stats have <b>returned to normal.</b>";
				EventScript relationshipEvent = new EventScript(Helpers.Instance.GetCharEnum(Var.activeBirds[birdNum]), title, text);
				EventController.Instance.CreateEvent(relationshipEvent);
			}

			Var.activeBirds[birdNum].newRelationship = false;
		}
	}*/
	void CheckGraphNavBtns()
	{
		
		if (Var.isTutorial)
			maxGraph = Tutorial.Instance.BirdCount[Tutorial.Instance.CurrentPos]-1;
		nextGraph.interactable = (currentGraph <= maxGraph);
		prevGraph.interactable = (currentGraph > 0);
		//CloseBattleReport.interactable = (currentGraph == maxGraph);
		
	}
	public void FightButtonMouseOver()
	{
		feedBack[] feedBack = FindObjectsOfType<feedBack>();
		foreach (feedBack fb in feedBack)
		{
			if (fb.isMain)
			{
				if (!fb.CheckOpponent())
				{
					fb.HighlightEnemy();
				}
			}
		}

	}

	public void GraphButton()
	{
		clearSmallGraph();
		if (LevelTutorial.shouldShowFirstBattleDialog)
		{
			LevelTutorial.shouldShowFirstBattleDialog = false;
			LeanTween.delayedCall(0.05f, CloseTutorialText);
		}               
		InitiateGraph(selectedBird,false);
	}
	void CloseTutorialText()
	{
		speechBubbleObj.SetActive(false);       
	}
	public void InitiateGraph(Bird bird,bool afterBattle = true)
	{
		if (!canChangeGraph)
			return;
		canChangeGraph = false;
		minimap.SetActive(false);
		int index = -1;
		for(int i= 0; i < Var.activeBirds.Count; i++)
		{
			if(bird.charName == Var.activeBirds[i].charName)
			{
				index = i;
				break;
			}
		}
		//LeanTween.delayedCall(0.7f, () =>
	   // {
			CreateGraph(index,afterBattle);
			ProgressGUI.Instance.PortraitClick(bird);
	   // });
		ProgressGUI.Instance.skillArea.SetActive(false);
		//LeanTween.moveLocal(graph, new Vector3(0, 0, graph.transform.position.z), 0.7f).setEase(LeanTweenType.easeOutBack).setOnComplete(CreateGraph).setOnCompleteParam(index as object);
		graphAnime.SetBool("open", true);
	}
	public void CreateBattleReport() {
		clearSmallGraph();
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
				feedBackString += nextMapArea + " birds coming in " + mapPos + " battles!"; 
			}
			if(nextMapArea == Var.Em.finish)
		{
			feedBackString = "Victory in " + mapPos + " battles!";
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
		if (Var.isTutorial)
			winDetails.text = "";
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
		Bird secondPlayerBird = null;
	
	   

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
								if (playerBird == null)
								{
									playerBird = Var.playerPos[3 - j, i % 4];
									if (Var.enemies[i].enemyType != fillEnemy.enemyType.drill)
										break;
								}
								else
								{
									secondPlayerBird = Var.playerPos[3 - j, i % 4];
									break;
								}
								
							}
						}
					}
					if (Var.enemies[i].position == Bird.dir.top)
					{
						if (Var.playerPos[i%4, j] != null)
						{
							if (!Var.playerPos[i % 4, j].isHiding)
							{
								if (playerBird == null)
								{
									playerBird = Var.playerPos[i % 4, j];
									if (Var.enemies[i].enemyType != fillEnemy.enemyType.drill)
										break;
								}
								else
								{
									secondPlayerBird = Var.playerPos[i % 4, j];
									break;
								}
							}
						}

					}
				}
				// Fight Logic
				playerBird.foughtInRound = true;
				int resultOfBattle = GameLogic.Instance.Fight(playerBird, Var.enemies[i]);
				battleAnim.Instance.AddData(playerBird, Var.enemies[i], resultOfBattle);
				result += resultOfBattle;
				if (secondPlayerBird != null)
				{
					secondPlayerBird.foughtInRound = true;
					int resultOfSecondBattle = GameLogic.Instance.Fight(secondPlayerBird, Var.enemies[i]);
					battleAnim.Instance.AddData(secondPlayerBird, Var.enemies[i], resultOfSecondBattle);
					result += resultOfSecondBattle;
				}
				playerBird = null;
				secondPlayerBird = null;
				


			}

		}
		Bird[] birds = FindObjectsOfType(typeof(Bird)) as Bird[];
		foreach(Bird bird in birds)
		{

			if (!bird.isEnemy && !bird.injured)
			{
				
				players.Add(bird);
			}
		}
	   foreach(Bird bird in players)
		{
			int friendGain = Helpers.Instance.Findfirendlieness(bird);
			if (friendGain > 0)
			{
				if(friendGain>2)
					Helpers.Instance.EmitEmotionParticles(bird.transform, Var.Em.Social,true,2);
				else
					Helpers.Instance.EmitEmotionParticles(bird.transform, Var.Em.Social);
			}
			if(friendGain<0)
				Helpers.Instance.EmitEmotionParticles(bird.transform, Var.Em.Solitary);

			bird.friendBoost += friendGain;			
			bird.gameObject.GetComponent<firendLine>().RemoveLines();
		}



		battleAnim.Instance.Battle();        
		finalResult = result;
		
		

	}

	
	public void ReturnToMap()
	{
		foreach (Bird bird in Var.availableBirds)
		{
			bird.AddRoundBonuses();
			bird.DecreaseTurnsInjured();
		}
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
		Var.currentWeek++;
		Var.shouldDoMapEvent = true;
		LeanTween.cancelAll();
		SceneManager.LoadScene("Map");
	}

	public void LoadMainMenu()
	{
		Time.timeScale = 1.0f;
		SaveLoad.Save();
		AudioControler.Instance.ClickSound();
		SceneManager.LoadScene("MainMenu");

	}
	public bool Reset()
	{
		
		Var.enemies = new Bird[8];
		Var.Infight = false;		
		ProgressGUI.Instance.SetOnePortrait();       
		foreach (Bird bird in players)
		{
			bird.ResetBonuses();          
			UpdateBirdSave(bird);		
			foreach(Bird activeBird in Var.activeBirds)
			{
				if (activeBird.injured)
				{
					Time.timeScale = 0.0f;
					QuitToMap();
					return false;
				}
			}
			//bird.gameObject.GetComponentInChildren<Animator>().SetBool("iswalking", false);

			//bird.gameObject.GetComponent<Animator>().SetBool("lose", false);
			//bird.gameObject.GetComponent<Animator>().SetBool("victory", false);
			bird.target = bird.home;
			bird.transform.position = bird.home;
			bird.prevConf = bird.confidence;
			bird.prevFriend = bird.friendliness;
			bird.totalConfidence += bird.confidence;
			bird.totalFriendliness += bird.totalFriendliness;
			bird.ResetBonuses();
			bird.GroundBonus.SetActive(false);
		}
		//After applying levels;
		/*GuiContoler.Instance.relationshipPanel.SetActive(false);
		GuiContoler.Instance.statPanel.SetActive(true);
		GuiContoler.Instance.ToggleRelationPanelText.text = "Relationships";*/
		foreach (Bird bird in players)
		{
			bird.ResetAfterLevel();
		}
		Var.playerPos = new Bird[4, 4];
		foreach (LayoutButton tile in ObstacleGenerator.Instance.tiles)
		{
			tile.gameObject.SetActive(true);
			tile.Reset();
		}

		finalResult = 0;
		players = new List<Bird>();
		moveInMap();
		mapPos++;
		foreach(GuiMap map in FindObjectsOfType<GuiMap>())
			map.MoveMapBird(mapPos);
		
		if (Var.isTutorial)
		{
			for (int i = 0; i < FillPlayer.Instance.playerBirds.Length; i++)
			{
				if (i < Tutorial.Instance.BirdCount[mapPos])
				{
					FillPlayer.Instance.playerBirds[i].gameObject.SetActive(true);
				}
				else
				{
					FillPlayer.Instance.playerBirds[i].gameObject.SetActive(false);
				}
			}
			Tutorial.Instance.ShowtutorialStartingText(mapPos);
		}
		BattleData Area = Var.map[mapPos];
		if (Area.type != Var.Em.finish)
		{
			if (Var.isTutorial)
			{				
				Tutorial.Instance.SetCurrenPos(mapPos);
				GetComponent<fillEnemy>().CreateTutorialEnemies(Tutorial.Instance.TutorialMap[mapPos]);
			}
			else
			{
				GetComponent<fillEnemy>().createEnemies(Area.battleData, Area.birdLVL, Area.dirs, Area.minEnemies, Area.maxEnemies);
				if (!EventController.Instance.tryEvent())
					DialogueControl.Instance.TryDialogue(Dialogue.Location.battle);
			}
			ObstacleGenerator.Instance.clearObstacles();
			ObstacleGenerator.Instance.GenerateObstacles();
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
		}else
		{
			Var.availableBirds.AddRange(FillPlayer.Instance.playerBirds);
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

		setMapLocation(mapPos);
		if (nextMapArea == Var.Em.finish)
		{
			mapPos = 0;
			Var.isTutorial = false;
			Var.tutorialCompleted = true;
			Var.isBoss = false;
			winBanner.SetActive(true);
			clearSmallGraph();		
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
