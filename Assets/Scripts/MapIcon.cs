using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapIcon : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
	public bool isTrial = false;
	public GameObject CompleteIcon;
	public GameObject LockedIcon;
	public int ID;    
	public bool completed = false;
	public string levelName;
	[TextArea(3, 20)]
	public string levelDescription = "";
	public bool available;
	[Header("Level configuration")]
	public Var.Em type;
	public int background = 0;   
	public int birdLVL = 1;
	int length = 0;
	public int minEnemies = 3;
	public int maxEnemies = 4;
	[Range(0.0f, 1.0f)]
	public float mainEmotionSpawnRate = 0.8f;
	public bool isBoss = false;
	[Header("Battle list")]
	public List<MapBattleData> battles;
	[Header("Tile Configuration")]
	public bool hasObstacles;
	public bool hasScaredPowerUps;
	public bool hasFirendlyPowerUps;
	public bool hasConfidentPowerUps;
	public bool hasLonelyPwerUps;
	public bool hasHealthPowerUps;
	public bool hasDMGPowerUps;
	[Header("References")]
   // [HideInInspector]
	public MapIcon[] targets;    
	MapSaveData mySaveData;
	LineRenderer lr;
	Image sr;
	bool active = false;
	Vector3 offset;
	public bool addBirdOnComplete = false;
	public bool hasWizards = false;
	public bool hasDrills = false;
	public bool hasSuper = false;
	public bool firstCompletion = true;
	public Bird birdToAdd;
	public EventScript birdToAddScript;
	ShowTooltip tooltipInfo;
	List<Var.Em> totalEmotions;
	List<float> totalPercentages;
	public Image unlockedRoad;
	bool useline;
	public int trialID;
	[HideInInspector]
	public Animator anim;
	//[HideInInspector]
	public TimedEventControl timedEventTrigger;
	bool birdAdded = false;
	public EventScript firstCompleteEvent;
	public Dialogue firstCompleteDialogue;
	// Use this for initialization
	void Start()
	{
		length = battles.Count;
		offset = transform.position- transform.parent.position;
		if (isTrial)
			trialID = ID;
		else
			trialID = GetTargetID(this);
		LoadSaveData();
		if (!Var.StartedNormally)
			available = true;
		sr = GetComponent<Image>();
		if (isTrial)
			sr.sprite = MapControler.Instance.trialSprite;
		sr.color = Helpers.Instance.GetEmotionColor(type);
		//GetComponent<Button>().interactable = available;
		CompleteIcon.SetActive(completed);
		LockedIcon.SetActive(!available);        
		lr = GetComponent<LineRenderer>();
		AddNewBird();
		tooltipInfo = gameObject.AddComponent<ShowTooltip>();
		tooltipInfo.tooltipText = GetTooltipText();
		if (!CheckTargetsAvailable() && available && Var.StartedNormally)
			LeanTween.move(transform.parent.gameObject, MapControler.Instance.centerPos.position + (transform.parent.transform.position - transform.position) + new Vector3(-3,0,0), 0.01f);
			//LeanTween.delayedCall(0.1f, mapBtnClick);
		ValidateAll();
		CalculateTotals();

		if (unlockedRoad == null)
		{
			useline = true;
		}
		else
		{
			useline = false;
		}
		anim = GetComponent<Animator>();
		SetState();
	}
	int GetTargetID(MapIcon data)
	{
		if (data.targets.Length == 0)
			return -1;
		if (data.targets[0].isTrial)
			return data.targets[0].ID;
		else
			return GetTargetID(data.targets[0]);
	}

	public void SetState()
	{
		if (available)
		{
			if (completed)
			{
				if(ID == Var.currentStageID && firstCompletion)
				{
					if(firstCompleteEvent!= null)
						EventController.Instance.CreateEvent(firstCompleteEvent);
					if(firstCompleteDialogue != null)
						DialogueControl.Instance.CreateParticularDialog(firstCompleteDialogue);
					firstCompletion = false;
					anim.SetInteger("state", 1);
					unlockedRoad.gameObject.SetActive(false);
					LeanTween.delayedCall(0.2f, () => anim.SetInteger("state", 2));
					LeanTween.delayedCall(1.7f,()=> unlockedRoad.gameObject.GetComponent<Animator>().SetBool("new", true));
					LeanTween.delayedCall(1.7f, () => unlockedRoad.gameObject.SetActive(true));
					foreach(MapIcon icon in targets)
					{
						LeanTween.delayedCall(2.7f, () => icon.anim.SetInteger("state", 1));
					}
					LeanTween.delayedCall(3f,SaveLoad.Save);
				}
				else
				{
					anim.SetInteger("state", 2);
				}
			}else
			{
				anim.SetInteger("state", 1);
			}
		}else
		{
			anim.SetInteger("state", 0);
		}

	}

	void ValidateAll()
	{
		int i = 1;
		foreach(MapBattleData data in battles)
		{
			Validate(i, data);
			i++;
		}

	}
		
	void Validate(int id, MapBattleData data)
	{
		if (data.emotionPercentage.Count != data.emotionType.Count)
			Debug.LogError(levelName + " battle " + id + " dosent have the same number of percentages and types");
		float total = 0;
		foreach (float percentage in data.emotionPercentage)
			total += percentage;
		if(total != 1.0f)
			Debug.LogError(levelName + " battle " + id + " dont add up to 100%");
		
	}
		
	void CalculateTotals()
	{
		totalEmotions = new List<Var.Em>();
		totalPercentages = new List<float>();
		foreach(MapBattleData data in battles)
		{
			for( int i=0;i<data.emotionType.Count;i++)
			{
				if (!totalEmotions.Contains(data.emotionType[i]))
				{
					totalEmotions.Add(data.emotionType[i]);
					float percentage = data.emotionPercentage[i]/ battles.Count;
					totalPercentages.Add(percentage);
				}else
				{
					int index =totalEmotions.IndexOf(data.emotionType[i]);
					float percentage = data.emotionPercentage[i]/ battles.Count;
					totalPercentages[index] = totalPercentages[index] + percentage;
				}                
			}
		}
	}
	string GetTooltip()
	{

		string tooltip = "";
		int total = 0;
		for (int i = 0; i < totalEmotions.Count; i++)
		{
			int val = (int)(totalPercentages[i] * 100);
			total += val;
			if (i == totalEmotions.Count - 1)
				val += 100 - total;
			tooltip += "\n" + Helpers.Instance.GetHexColor(totalEmotions[i]) + totalEmotions[i] + " " + val + " %</color>";
		}
		tooltip = tooltip.Substring(1);
		return tooltip;
	}
	void SetupPieGraph()
	{
		float fill = 0;
		for(int i = 0; i < totalEmotions.Count; i++)
		{
			fill += totalPercentages[i];
			MapControler.Instance.pieChart[i].fillAmount = fill;
			MapControler.Instance.pieChart[i].color = Helpers.Instance.GetEmotionColor(totalEmotions[i]);
		}
		try
		{
		  
			MapControler.Instance.pieChart[0].gameObject.GetComponent<ShowTooltip>().tooltipText = GetTooltip();
		}
		catch
		{
			print("Could not assign pie chart tooltip");
		}
	}
	string GetTooltipText()
	{
		string tooltipText ="";
		if (isTrial)
			tooltipText += "<b>TRIAL</b>";
		tooltipText = "<b>"+levelName +"</b>\n";
		
		string stageState = "<color=#c33b24ff>Not available</color>";
		if (completed)
			stageState = "<color=#EEDE00FF>Completed</color>";
		if (available)
			stageState = "<color=#2bd617ff>Available</color>";
		tooltipText += stageState;
		tooltipText += "\nLength: <b>" + length +"</b>";
		tooltipText += "\nMain ENEMY emotion: "+ Helpers.Instance.GetHexColor(type) + type.ToString() +"</color>";

		return tooltipText;
	}


	void AddNewBird()
	{
		if(completed && addBirdOnComplete && birdToAdd!= null && !birdAdded)
		{
			birdAdded = true;
			bool canAdd = true;
			foreach(Bird bird in Var.availableBirds)
			{
				if(bird.charName == birdToAdd.charName)
				{
					canAdd = false;
					break;
				}
			}
			if (canAdd && birdToAdd != null && birdToAddScript != null)
			{
				birdToAdd.gameObject.SetActive(true);
				Var.availableBirds.Add(birdToAdd);
				EventController.Instance.CreateEvent(birdToAddScript);
			}


		}


	}

	void Update()
	{
		if(active)
		{
			transform.parent.position = transform.position - offset;           
		}
	 
		if(useline)
		{
			renderLine();
		}
		else
		{
			if(completed)
			{
				unlockedRoad.gameObject.SetActive(true);
			}
			else
			{
				unlockedRoad.gameObject.SetActive(false);
			}
		}
	}

	void renderLine()
	{
		int i = 0;
		lr.positionCount = targets.Length * 2;
		lr.sortingOrder = 2;
		if (targets.Length > 0)
		{
			foreach (MapIcon pos in targets)
			{

				lr.SetPosition(i++, pos.gameObject.transform.position);
				lr.SetPosition(i++, transform.position);
			}
		}
		if (!completed)
		{
			lr.endColor = Color.gray;
			lr.startColor = Color.gray;
		}
	}

	public void RemoveLock()
	{
	   // LockedIcon.SetActive(false);
	}
	void LoadSaveData()
	{
		bool SaveDataCreated = false;
		foreach (MapSaveData data in Var.mapSaveData)
		{
			if (data.ID == ID)
			{
				SaveDataCreated = true;
				break;
			}
		}


		if (!SaveDataCreated)
		{
			List<int> targIDs = new List<int>();
			foreach (MapIcon targ in targets)
			{
				targIDs.Add(targ.ID);
			}
			mySaveData = new MapSaveData(completed, available,firstCompletion, ID, targIDs,type, trialID,levelName);
			Var.mapSaveData.Add(mySaveData);
		}
		else
		{
			foreach (MapSaveData data in Var.mapSaveData)
			{
				if (data.ID == ID)
				{
					completed = data.completed;
					available = data.available;
					break;
				}

			}
		}
	}


	public void mapBtnClick()
	{
	 


		if (MapControler.Instance.SelectedIcon == this)
			return;
		try
		{
			AudioControler.Instance.ClickSound();
		}
		catch { }
		//map tutorial
		if (!Var.gameSettings.shownMapTutorial)
		{
			MapTutorial tut = FindObjectOfType<MapTutorial>();
			tut.tutorialHighlight.SetTrigger("off");
			DialogueControl.Instance.CreateParticularDialog(tut.mapTutorialDialog2);
			Var.gameSettings.shownMapTutorial = true;
		}
		SetupPieGraph();
		active = true;
		foreach (GuiMap map in FindObjectsOfType<GuiMap>())
			map.Clear();
		MapControler.Instance.SelectedIcon = null;
		MapControler.Instance.startLvlBtn.gameObject.SetActive(false);
		MapControler.Instance.SelectionMenu.transform.localScale = Vector3.zero;
		MapControler.Instance.selectionTiles.transform.localScale = Vector3.zero;
		MapControler.Instance.ScaleSelectedBirds(0, Vector3.zero);
		ShowAreaDetails();
		//LeanTween.move(transform.parent.gameObject, MapControler.Instance.centerPos.position+(transform.parent.transform.position-transform.position), 0.8f).setEase(LeanTweenType.easeInBack).setOnComplete(ShowAreaDetails);

	}

	public void ShowAreaDetails()
	{
		MapControler.Instance.canMove = false;
		active = false;      
		
		MapControler.Instance.SelectionMenu.SetActive(true);
		MapControler.Instance.selectionTiles.SetActive(true);
		MapControler.Instance.SelectionTitle.text = levelName;
		MapControler.Instance.SelectionText.text = ToString();
		MapControler.Instance.SelectionDescription.text = levelDescription;
		AudioControler.Instance.ClickSound();
		FindObjectOfType<GuiMap>().CreateMap(CreateMap());
		LeanTween.scale(MapControler.Instance.SelectionMenu, Vector3.one, MapControler.Instance.scaleTime).setEase(LeanTweenType.easeOutBack);
		MapControler.Instance.SelectedIcon = this;
		if (available)
		{
			LeanTween.scale(MapControler.Instance.selectionTiles, Vector3.one, MapControler.Instance.scaleTime).setEase(LeanTweenType.easeOutBack);
			MapControler.Instance.ScaleSelectedBirds(MapControler.Instance.scaleTime, Vector3.one * 0.25f);            
			MapControler.Instance.startLvlBtn.gameObject.SetActive(true);
			
		}
	}
	bool CheckTargetsAvailable()
	{
		bool targetAvailable = false;
		foreach(MapIcon target in targets)
		{
			if (target.available)
				targetAvailable = true;
		}
		return targetAvailable;
	}


	public void LoadBattleScene()
	{
		if (timedEventTrigger != null && !timedEventTrigger.data.activationEventShown)
		{
			timedEventTrigger.TriggerActivationEvent();
			return;
		}
		if (available && MapControler.Instance.canFight)
		{
			SaveLoad.Save();
			AudioControler.Instance.ClickSound();
			Var.isBoss = isBoss;
			Var.fled = false;
			Var.isTutorial = false;                       
			Var.currentBG = background;
			Var.map.Clear();
			Var.map = CreateMap();
			Var.currentStageID = ID;
			Var.activeBirds = new List<Bird>();
			GuiContoler.mapPos = 0;
			for (int i = 0; i < 3; i++)
			{
				Var.activeBirds.Add(MapControler.Instance.selectedBirds[i]);
			}
			Var.availableBirds = new List<Bird>();
			foreach (Bird bird in FillPlayer.Instance.playerBirds)
			{
				if (bird.gameObject.activeSelf)
					Var.availableBirds.Add(bird);
			}
			SceneManager.LoadScene("NewMain");
		}

	}
	Var.Em FindTopEmotion(MapBattleData data)
	{
		float biggestTotal = 0.0f;
		Var.Em toReturn = Var.Em.finish;
		for (int i = 0; i < data.emotionPercentage.Count; i++)
		{
			if (data.emotionPercentage[i] > biggestTotal)
			{ 
				toReturn = data.emotionType[i];
				biggestTotal = data.emotionPercentage[i];
			}

		}
		return toReturn;
	}
	List<BattleData> CreateMap()
	{        
		List<BattleData> map = new List<BattleData>();
		for (int i = 0; i < length; i++)
		{
			AddStageToLevel(battles[i],map);
		}
		map.Add(new BattleData(Var.Em.finish, hasObstacles, new List<Var.Em>(), null));
		return map;
	}
	void AddStageToLevel(MapBattleData mapData, List<BattleData> map)
	{
		BattleData data = new BattleData(FindTopEmotion(mapData), hasObstacles, EmPowerList(),mapData, birdLVL, CreateDirList(), PowerList(),hasWizards,hasDrills,hasSuper);
		data.maxEnemies = maxEnemies;
		data.minEnemies = minEnemies;
		map.Add(data);
	}

	List<Bird.dir> CreateDirList()
	{
		List<Bird.dir> dirList = new List<Bird.dir>();
		dirList.Add(Bird.dir.front);
		dirList.Add(Bird.dir.top);       
		return dirList;
	}

	List<Var.Em> EmPowerList()
	{
		List<Var.Em> list = new List<Var.Em>();
		if (hasFirendlyPowerUps)
			list.Add(Var.Em.Social);
		if (hasConfidentPowerUps)
			list.Add(Var.Em.Solitary);
		if (hasLonelyPwerUps)
			list.Add(Var.Em.Solitary);
		if (hasScaredPowerUps)
			list.Add(Var.Em.Cautious);
		return list;
	}
	List<Var.PowerUps> PowerList()
	{
		List<Var.PowerUps> list = new List<Var.PowerUps>();
		if (hasDMGPowerUps)
			list.Add(Var.PowerUps.dmg);
		if (hasHealthPowerUps)
			list.Add(Var.PowerUps.heal);
		return list;
	}



	/*public void OnPointerEnter(PointerEventData eventData)
	{
	   


		Helpers.Instance.ShowTooltip(ToString());
	}
   
	public void OnPointerExit(PointerEventData eventData)
	{
		Helpers.Instance.HideTooltip();
	}*/
	public override string ToString()
	{
		string stageInfo = "";
		string stageState = "not available";
		if (completed)
			stageState = "completed";
		if (available)
			stageState = "available";
		stageInfo += "Stage " + (ID + 1) + ": " + stageState;
		string weakness = "All";
		if (type == Var.Em.Random)
			weakness = "Unpredictable";
		else if (type != Var.Em.Neutral)
			weakness = Helpers.Instance.GetWeakness(type).ToString();
		stageInfo += ". Main ENEMY emotion: " +Helpers.Instance.GetHexColor(type)+ type + "</color>. Weak to: " +
			Helpers.Instance.GetHexColor(Helpers.Instance.GetWeakness(type)) + weakness + "</color>.";
		/*stageInfo += "\nEnemies attack from the: ";
		if (hasFrontEnemyRow)
			stageInfo += "\u2022Front ";
		if (hasTopEnemyRow)
			stageInfo += "\u2022Top ";      
		if (hasObstacles)
			stageInfo += "\nHas obstacles";
		if (hasHealthPowerUps)
			stageInfo += "\nHas healing tiles";
		if (hasDMGPowerUps)
			stageInfo += "\nHas bonus damage tiles";*/
		return stageInfo;
	}

}






