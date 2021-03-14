using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class MapIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public AudioMixerSnapshot nodeSnapshot;
	public AudioGroup ambientSounds;
	public bool isTrial = false;
	public GameObject fogObject;
    public GameObject[] hiddenThingsWhenTrialActive;
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
    public bool hasShields;
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
	public MeshRenderer[] splineRoads;
	bool useline;
	public int trialID;
	public Animator anim;
	[HideInInspector]
	public TimedEventControl timedEventTrigger;
	bool birdAdded = false;
	public EventScript firstCompleteEvent;
	public Dialogue firstCompleteDialogue;
	[HideInInspector] public bool stateSet = false;
	bool eventShown = false;
	bool dialogueShown = false;
	[HideInInspector] public TimedEventControl timedEvent;
    // Use this for initialization

    private void Awake()
    {
        if(!anim)
        {
			anim = GetComponent<Animator>();
        }
    }

    void Start()
	{
		if(MapControler.Instance.allIDs.Contains(ID))
		{
			Debug.LogError("Duplicate ID found! ID: " + ID + " node name: " + name);
		}
		MapControler.Instance.allIDs.Add(ID);

		length = battles.Count;
		offset = transform.position- transform.parent.position;
		if (isTrial)
			trialID = ID;
		else
			trialID = GetTargetID(this);
		LoadSaveData();
		//if (!Var.StartedNormally)
			//available = true;


		sr = GetComponent<Image>();
        sr = this.transform.Find("mapIcon_art").GetComponent<Image>();

        //Seb. Kind of dirty way to get the "lock" gameobject even if it has not been set. In case the prefab connection breaks
        if (LockedIcon == null)
        {
            LockedIcon = this.transform.Find("lock").gameObject;
            
        }

		if (isTrial)
		{
			sr.sprite = MapControler.Instance.trialSprite;
			sr.color = Helpers.Instance.GetEmotionColor(type);
			if(fogObject)
			{
				fogObject.SetActive(!completed);
				if((mapPan.Instance.activeFog== null || mapPan.Instance.activeFog.transform.position.x > fogObject.transform.position.x) && !completed)
					mapPan.Instance.activeFog = fogObject.transform;
			}

            for(int i=0;i<hiddenThingsWhenTrialActive.Length;i++)
            {

                hiddenThingsWhenTrialActive[i].SetActive(completed);
                
            }
            if(completed)
            {
                mapPan.Instance.scrollingEnabled = true;
            }

        }
		else
		{
			sr.sprite = Helpers.Instance.GetEmotionIcon(type);
		}
		//GetComponent<Button>().interactable = available;
		CompleteIcon.SetActive(completed);
		LockedIcon.SetActive(!available);        
		lr = GetComponent<LineRenderer>();
		AddNewBird();
		tooltipInfo = gameObject.AddComponent<ShowTooltip>();
		tooltipInfo.tooltipText = GetTooltipText();
		/*if (!CheckTargetsAvailable() && available && Var.StartedNormally)
			LeanTween.move(transform.parent.gameObject, MapControler.Instance.centerPos.position + (transform.parent.transform.position - transform.position) + new Vector3(-3,0,0), 0.01f);*/
			//LeanTween.delayedCall(0.1f, mapBtnClick);
		ValidateAll();
		CalculateTotals();

		if(splineRoads!=null && splineRoads.Length>0)
		{
			if (available)
			{
				if(completed)
				{
					foreach (MeshRenderer road in splineRoads)
						road.material = MapControler.Instance.completedRoadMat;
				}
				else
				{
					foreach (MeshRenderer road in splineRoads)
						road.material = MapControler.Instance.availableRoadMat;
				}
			}
			else
			{
				foreach (MeshRenderer road in splineRoads)
					road.material = MapControler.Instance.disabledRoadMat;

			}



		}
		if (unlockedRoad == null && splineRoads.Length ==0)
		{
			useline = false;
		}
		else
		{
			useline = false;
		}
		anim = GetComponent<Animator>();
		LeanTween.delayedCall(0.2f, () =>
		{
			if (EventController.Instance.eventsToShow.Count == 0 && !GuiContoler.Instance.activeSpeechBubble.activeInHierarchy)
				SetState();
		});
	}

    public void OnPointerEnter(PointerEventData eventData)
    {
        anim.SetBool("hover", true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("hover", false);
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
		if (available && !stateSet)
		{
			ExcelExport.CreateExportTable();
			ExcelExport.AddMapNode(this);
			if (completed)
			{
				if(ID == Var.currentStageID && firstCompletion)
				{
					if (firstCompleteEvent != null && !eventShown)
					{
						eventShown = true;
						EventController.Instance.CreateEvent(firstCompleteEvent);
						return;
					}
					if (firstCompleteDialogue != null && !dialogueShown)
					{
						dialogueShown = true;
						DialogueControl.Instance.CreateParticularDialog(firstCompleteDialogue);
						return;
					}
					stateSet = true;
					firstCompletion = false;
					anim.SetInteger("state", 1);
					LeanTween.delayedCall(0.2f, () => anim.SetInteger("state", 2));
                    AudioControler.Instance.PlaySound(AudioControler.Instance.mapNodeClick);
					if (unlockedRoad != null)
					{
						unlockedRoad.gameObject.SetActive(false);
						LeanTween.delayedCall(1.7f, () => unlockedRoad.gameObject.GetComponent<Animator>().SetBool("new", true));
						LeanTween.delayedCall(1.7f, () => unlockedRoad.gameObject.SetActive(true));
					}
					foreach(MapIcon icon in targets)
					{
						LeanTween.delayedCall(2.7f, () => icon.anim.SetInteger("state", 1));
					}
					CenterMapNode();
					//Debug.LogError(" moving to point: " + temp + " map can move: " + MapControler.Instance.canMove);
					LeanTween.delayedCall(3f,()=>SaveLoad.Save());
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
	public void CenterMapNode()
	{
		Vector2 dist = Camera.main.transform.position - transform.position;
		if (targets.Length > 0)
		{
			dist = Camera.main.transform.position - targets[0].transform.position;
		}
		Vector3 temp = FindObjectOfType<mapPan>().transform.position;
		temp += new Vector3(dist.x, dist.y, 0);
		FindObjectOfType<mapPan>().transform.position = temp;
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
		//Debug.LogError(levelName + " battle " + id + " starting validation");
		if(data.emotionPercentage.Count==0 && data.emotionType.Count==0)
		{
			Debug.LogError(levelName + " battle " + id + " dosent have any battles added!");
		}else
		{
		if (data.emotionPercentage.Count != data.emotionType.Count)
			Debug.LogError(levelName + " battle " + id + " dosent have the same number of percentages and types");
		float total = 0;
		foreach (float percentage in data.emotionPercentage)
			total += percentage;
		if(total != 1.0f)
			Debug.LogError(levelName + " battle " + id + " dont add up to 100%");
		}
		
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
			birdToAdd.data.unlocked = true;
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

			birdToAdd.SaveBirdData();
		}


	}

	void Update()
	{
		/*if(active)
		{
			transform.parent.position = transform.position - offset;           
		}*/
		
		if(useline)
		{
			renderLine();
		}
		else
		{
			if(completed && unlockedRoad != null)
			{
				unlockedRoad.gameObject.SetActive(true);
			}
			else
			{
				if(unlockedRoad)
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
	public void LoadSaveData(bool createNewSave = false)
	{
		bool SaveDataCreated = false;
		foreach (MapSaveData data in Var.mapSaveData)
		{
			if (data.ID == ID)
			{
				if(createNewSave)
                {
					Var.mapSaveData.Remove(data);
                }
                else
				{
					SaveDataCreated = true;
				}
				break;
			}
		}


		if (!SaveDataCreated )
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

        anim.SetTrigger("click");

        if (MapControler.Instance.SelectedIcon == this)
        {
            return;
        }
		try
		{
			AudioControler.Instance.PlaySound(AudioControler.Instance.mapNodeClick);
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
		if(timedEvent!= null && available)
		{
			timedEvent.TriggerActivationEvent();
		}
		active = true;
		foreach (GuiMap map in FindObjectsOfType<GuiMap>())
			map.Clear();
		MapControler.Instance.SelectedIcon = null;
		MapControler.Instance.startLvlBtn.gameObject.SetActive(false);
		//MapControler.Instance.SelectionMenu.transform.localScale = Vector3.zero; //seb
		MapControler.Instance.ScaleSelectedBirds(0, Vector3.zero);
		ShowAreaDetails();
        
        //LeanTween.value(gameObject, (float alpha) => MapControler.Instance.SelectionMenu.GetComponent<CanvasGroup>().alpha =alpha, 0,1, MapControler.Instance.scaleTime).setEase(LeanTweenType.easeInBack);
        //LeanTween.scale(MapControler.Instance.SelectionMenu, Vector3.one, MapControler.Instance.scaleTime).setEase(LeanTweenType.easeInBack);
        
        
        //LeanTween.move(transform.parent.gameObject, MapControler.Instance.centerPos.position+(transform.parent.transform.position-transform.position), 0.8f).setEase(LeanTweenType.easeInBack).setOnComplete(ShowAreaDetails);

    }

	public void ShowAreaDetails()
	{
		MapControler.Instance.canMove = false;
		active = false;      
		
		MapControler.Instance.SelectionMenu.SetActive(true);
		MapControler.Instance.SelectionTitle.text = levelName;
		MapControler.Instance.SelectionText.text = ToString();
		MapControler.Instance.SelectionDescription.text = levelDescription;
		AudioControler.Instance.ClickSound();
		FindObjectOfType<GuiMap>().CreateMap(CreateMap());
       // LeanTween.scale(MapControler.Instance.SelectionMenu, Vector3.one, MapControler.Instance.scaleTime).setEase(LeanTweenType.easeOutBack); //seb
        
        MapControler.Instance.ShowSelectionMenuAnimation();
		MapControler.Instance.SelectedIcon = this;
		if (available)
		{
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
		//Debug.LogError("loading battle scene");
		if (available && MapControler.Instance.canFight)
		{
			AudioControler.Instance.ClickSound();
			Var.isBoss = isBoss;
			Var.fled = false;
			Var.isTutorial = false;                       
			Var.currentBG = background;
			Var.map.Clear();
			Var.map = CreateMap();
			Var.currentStageID = ID;
			Var.activeBirds = new List<Bird>();
			if(ambientSounds.clips.Length>0)
			{
				Var.ambientSounds = ambientSounds;
			}
			Var.snapshot = nodeSnapshot;
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
			Var.freezeEmotions = isTrial;
			SaveLoad.Save();
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
        if (hasShields)
            list.Add(Var.PowerUps.shield);
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
		/*string weakness = "All";
		if (type == Var.Em.Random)
			weakness = "Unpredictable";
		else if (type != Var.Em.Neutral)
			weakness = Helpers.Instance.GetWeakness(type).ToString();*/
		stageInfo += ". Main ENEMY emotion: " + Helpers.Instance.GetHexColor(type) + type + "</color>.";
		   // " Weak to: " +	Helpers.Instance.GetHexColor(Helpers.Instance.GetWeakness(type)) + weakness + "</color>.";
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






