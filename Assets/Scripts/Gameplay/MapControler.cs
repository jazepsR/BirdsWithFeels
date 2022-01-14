using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapControler : MonoBehaviour {
	public static MapControler Instance { get; private set; }
	public List<int> allIDs = new List<int>();
	[HideInInspector]
	public bool canFight= false;
	[HideInInspector]
	public bool showGraphAfterEvent = true;
	[HideInInspector]
	public bool canHeal = false;
	[TextArea(3,10)]
	public string trialDescription = "This is a <b>trial level!</b> In this level your <b>emotions are frozen</b>" +
		"and won't change. In this level, mHP will not affect your birds";
	GameObject healTrail;
	public Text title;
	public Transform centerPos;
	public bool canMove = true;
	public GameObject SelectionMenu;
	public Text SelectionText;
	public Text SelectionDescription;
	public Text SelectionTitle;
	public Button startLvlBtn;
    public GameObject restButton;
    public GameObject restButtonBeam; //the iron beam visual that is connected to the rest button
    [HideInInspector]
	public float scaleTime = 0.35f;
	public Image[] pieChart;
    Animator SelectionMenuAnimator;
  //  public Animator SelectionMenuBlurAnimator;
	[HideInInspector]
    public bool isViewingNode = false;
    [HideInInspector]
    public MapIcon SelectedIcon;
	public Text timerText;
	public List<Bird> selectedBirds;
	public Sprite trialSprite;
	TimedEventControl[] timedEvents;
	public Material disabledRoadMat;
	public Material availableRoadMat;
	public Material completedRoadMat;
	public Animator charInfoAnim;
	[HideInInspector]
	public int count = 0;
	public AudioGroup ambientSounds;
    public GraphicRaycaster restBtnRaycaster;

	[Header("Trial UI")]
	public GameObject trialUiObject;
	public Image trialUiIcon;
	public Text trialWeeksLeftText;
	public Text trialTooLateText;
	public Text trialNameText;
	public int[] chapterIDs;

	[Header("Map selection panel")]
	public GameObject selectionPanelHealthIcon;
	public GameObject selectionPanelEmoTiles;
	public GameObject selectionPanelWizards;
	public GameObject selectionPanelShields;
	public GameObject selectionPanelSwords;
	public Text enemyLevelText;
	public GameObject selectionPanelBG;
	public GameObject selectionPanelTitleBG;
	public GameObject selectionPanelTrialBG;
	public GameObject selectionPanelTrialTitleBG;



	void Awake()
	{
		Instance = this;
		Var.snapshot = null;
		trialUiObject.SetActive(false);
	}
	public void FocusOnNodeAfterLoss()
	{

		MapIcon[] icons = FindObjectsOfType<MapIcon>();
		foreach (MapIcon icon in icons)
		{
			if (icon.ID == Var.currentStageID)
			{
				icon.CenterMapNode(false);
				return;
			}
		}
	}
	public void UnlockToChapter(int targetLevel)
	{
		if (Var.loadChapterID < 0)
			return;
		MapIcon[] icons = FindObjectsOfType<MapIcon>();
		int lastID = -1;
		foreach (MapIcon icon in icons)
		{
			if (icon.ID <= targetLevel)
			{
				icon.available = true;
				icon.completed = true;
				if (icon.fogObject)
				{
					icon.fogObject.SetActive(false);
				}
				if(icon.ID > lastID)
                {
					lastID = icon.ID;
					SelectedIcon = icon;
				}
				//icon.stateSet = false;
				icon.SetState();
				icon.LoadSaveData(true);
			}			
		}
		SelectedIcon.completed = false;
		SelectedIcon.SetState();
		SelectedIcon.CenterMapNode();
		mapPan.Instance.activeFog = null;
		mapPan.Instance.scrollingEnabled = true;
		Var.loadChapterID = -1;
		SaveLoad.Save();
	}

	// Use this for initialization
	void Start () {
		
		timedEvents = FindObjectsOfType<TimedEventControl>();
		Var.isBoss = false;
		Var.freezeEmotions = false;
		timerText.text = "Week: " + Mathf.Max(0, Var.currentWeek);
		Achievements.amountOfWeeks(Var.currentWeek);
		//SelectionMenu.transform.localScale = Vector3.zero;
		canHeal = false;
		if(ambientSounds.clips.Length>0)
		{
			Var.ambientSounds = ambientSounds;
			AudioControler.Instance.AmbientSounds = ambientSounds;
		}
		foreach(Bird bird in FillPlayer.Instance.playerBirds)
		{
            if (bird.data.unlocked)
            {               
                count++;                
            }
			bool wasActive = false;
			foreach(Bird activeBird in Var.activeBirds)
			{
				if(activeBird.charName == bird.charName)
				{
					wasActive = true;
					break;
				}
			}
			if (!wasActive && bird.gameObject.activeSelf)
			{
				if(bird.data.injured)
                {
					bird.DecreaseTurnsInjured();
                }

				if(bird.data.health<bird.data.maxHealth && !bird.data.injured)
				{
					GameObject healObj = Instantiate(bird.healParticle, bird.transform);
					Destroy(healObj, 1.5F);
				}
				bird.data.health = Mathf.Min(bird.data.health + 1, bird.data.maxHealth);                
				bird.data.mentalHealth = Mathf.Min(bird.data.mentalHealth + 1, Var.maxMentalHealth);
			}

			if (count == 5) //if all 5 birds are unlocked
			{
				if (Var.maxLevel == 5 && bird.data.level == Var.maxLevel)
				{
					if (Var.birdsMaxLevelCount != 5)
					{
						Var.birdsMaxLevelCount++;
					}
				}

				Achievements.checkBirdLevelUp(bird, true);
			}
		}

        canRest();

		if (count == 3)
		{
			foreach (Bird bird in FillPlayer.Instance.playerBirds)
			{
				if (bird.data.unlocked)
				{
					if (!bird.data.injured && bird.mapHighlight)
					{
						bird.mapHighlight.SetActive(true);
						selectedBirds.Add(bird);
					}
                }
			}
			CanLoadBattle();
		}
		/*foreach(Bird bird in Var.activeBirds)
		{
			if(Helpers.Instance.ListContainsLevel(Levels.type.Friend2, bird.data.levelList) &&!Var.fled)
			{
				healTrail = Instantiate(Resources.Load("MouseHealParticle"), Input.mousePosition, Quaternion.identity) as GameObject;
				canHeal = true;
				title.text = bird.charName + " can heal one of your birds!";
			}
		}*/
		SaveLoad.Save();

		
		if (Var.loadChapterID >= 0)
		{
			LeanTween.delayedCall(0.05f, () =>
			 UnlockToChapter(chapterIDs[Var.loadChapterID]));
		}
		if(Var.fled)
		{
			FocusOnNodeAfterLoss();
		}


		if (Var.currentWeek<3)
			Var.shouldDoMapEvent = false;
		//Var.shouldDoMapEvent = true;
		if (Var.shouldDoMapEvent)
		{
			if (!EventController.Instance.tryEvent())
				DialogueControl.Instance.TryDialogue(Dialogue.Location.map);
			Var.shouldDoMapEvent = false;
		}
        //foreach (Bird bird in FillPlayer.Instance.playerBirds)
        //  bird.publicStart();
        //ProgressGUI.Instance.PortraitClick(Var.availableBirds[0]);

        SelectionMenuAnimator = SelectionMenu.GetComponent<Animator>();


	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (canHeal)
		{
			healTrail.transform.position = Camera.main.ScreenToWorldPoint( Input.mousePosition);
		}else
		{
			Destroy(healTrail);
		}
	}
	public void CanLoadBattle()
	{
		Debug.Log("selected bird count:" + selectedBirds.Count);
		
		if (selectedBirds.Count == 3)
		{
			canFight = true;
			startLvlBtn.interactable = true;            
			startLvlBtn.GetComponent<ShowTooltip>().tooltipText = "Start the adventure\nA week will pass";
		}
		else
		{
			canFight = false;
			startLvlBtn.interactable = false;
			startLvlBtn.GetComponent<ShowTooltip>().tooltipText = "You must select 3 birds for the fight";
		}		
		
	}
	public void StartLevel()
	{
		if (SelectedIcon != null)
		{
			SelectedIcon.LoadBattleScene();
			AudioControler.Instance.buttonSoundMap.Play();
			//Debug.LogError(isViewingNode);

        }
	}

    public void canRest()
    {
        if (!Var.gameSettings.shownMapTutorial)
        {
            restButton.SetActive(false);
            restButtonBeam.SetActive(false);
        }
        else
        {
            int birdsAtMaxHealth = 0;
            foreach (Bird bird in FillPlayer.Instance.playerBirds)
            {
                if (bird.data.unlocked)
                {
                    if (bird.data.health < bird.data.maxHealth || bird.data.injured || bird.data.mentalHealth < Var.maxMentalHealth)
                    {
						restButton.GetComponent<Button>().interactable = true;
						restButton.GetComponent<ShowTooltip>().tooltipText = "A week will pass. All your birds will heal one health, and your injured birds will be closer to recovery";

                    }

                    else
                    {
                        birdsAtMaxHealth++;
                    }

                    if (birdsAtMaxHealth == count)
                    {
                        birdsAtMaxHealth = 0;
						restButton.GetComponent<Button>().interactable = false;
						restButton.GetComponent<ShowTooltip>().tooltipText = "All birds at full mental and physical health! You can continue the adventure, no need to rest";
						 Helpers.Instance.HideTooltip();
                    }
                }

            }


            //Debug.Log("birds at max health: " + birdsAtMaxHealth++ + " count: " + count);

            birdsAtMaxHealth = 0;
        }
    }

	public void Rest()
	{
		Var.currentWeek++;

		AudioControler.Instance.buttonSoundMap.Play();
		LeanTween.delayedCall(0.3f, () => AudioControler.Instance.restSound.Play());
		if(Var.currentWeek >= 999)
			timerText.text = "Week: " + "999";
        else
			timerText.text = "Week: " + Var.currentWeek;

		timerText.GetComponent<Animator>().SetTrigger("Increment");
		foreach (TimedEventControl control in timedEvents)
			control.CheckStatus();
		foreach(Bird bird in FillPlayer.Instance.playerBirds)
		{
            if (bird.data.injured)
            {
                bird.DecreaseTurnsInjured();
            }
            else if (bird.data.health < bird.data.maxHealth)
            {
                bird.data.health++;
                GameObject healObj = Instantiate(bird.healParticle, bird.transform);
                Destroy(healObj, 1.5f);
            }

			if(bird.data.mentalHealth < Var.maxMentalHealth)
            {
				bird.data.mentalHealth = Mathf.Min(Var.maxMentalHealth, bird.data.mentalHealth + 1);
				if (bird.MHPParticle)
				{
					GameObject healObj = Instantiate(bird.MHPParticle, bird.transform);
					Destroy(healObj, 1.5f);
				}
			}
			bird.SetBandages();
		}

        canRest();
		//foreach (TimedEventControl timedEvent in FindObjectsOfType<TimedEventControl>())
		//	timedEvent.CheckStatus();

		Achievements.amountOfWeeks(Var.currentWeek);
	}

	public void HideSelectionMenu()
	{
        if(MapTutorial.instance && MapTutorial.instance.mapTutPopup.activeSelf)
        {
            return;
        }

        if (isViewingNode && SelectedIcon != null)
        {
            isViewingNode = false;

            foreach (MapIcon icon in FindObjectsOfType<MapIcon>())
            {

                icon.GetComponent<ShowTooltip>().enabled = true;

            }

            SelectedIcon.anim.SetBool("hover", false);
        }

		canMove = true;

		//this is seb taken off
		/*
        	LeanTween.scale(MapControler.Instance.SelectionMenu, Vector3.zero, MapControler.Instance.scaleTime).setEase(LeanTweenType.easeInBack);
        LeanTween.value(gameObject, (float alpha) => MapControler.Instance.SelectionMenu.GetComponent<CanvasGroup>().alpha = alpha, 1, 0, MapControler.Instance.scaleTime).setEase(LeanTweenType.easeInBack);
       */
		if (SelectionMenuAnimator)
		{
			SelectionMenuAnimator.SetBool("active", false); //seb change 
		}
        // SelectionMenuBlurAnimator.SetBool("active", false);

        ScaleSelectedBirds(scaleTime, Vector3.zero);        
        foreach (GuiMap map in FindObjectsOfType<GuiMap>())
			map.Clear();
        SelectedIcon = null;
		mapPan.Instance.scrollingEnabled = true;
    }
	public void SetSelectionMenuIcons(MapIcon mapIcon)
    {
		selectionPanelHealthIcon.SetActive(mapIcon.hasHealthPowerUps);
		SetEmotionTile(mapIcon);
		selectionPanelWizards.SetActive(mapIcon.hasWizards);
		selectionPanelShields.SetActive(mapIcon.hasShields);
		selectionPanelSwords.SetActive(mapIcon.hasDMGPowerUps);
	}

	private void SetEmotionTile(MapIcon mapIcon)
    {
		bool hasEmos = (mapIcon.hasLonelyPwerUps
			|| mapIcon.hasFirendlyPowerUps || mapIcon.hasScaredPowerUps || mapIcon.hasConfidentPowerUps);
		selectionPanelEmoTiles.SetActive(hasEmos);
		if (hasEmos)
		{
			selectionPanelEmoTiles.SetActive(hasEmos);
			string tooltipString = "";
			List<string> emotionsPresent = new List<string>();
			if (mapIcon.hasLonelyPwerUps)
				emotionsPresent.Add("solitary");
			if (mapIcon.hasScaredPowerUps)
				emotionsPresent.Add("cautious");
			if (mapIcon.hasConfidentPowerUps)
				emotionsPresent.Add("confident");
			if (mapIcon.hasFirendlyPowerUps)
				emotionsPresent.Add("friendly");
			tooltipString += string.Join(", " ,emotionsPresent);
			tooltipString = char.ToUpper(tooltipString[0]) + tooltipString.Substring(1);
			tooltipString += " emotional tiles will spawn";
			selectionPanelEmoTiles.GetComponentInChildren<ShowTooltip>().tooltipText = tooltipString;
		}
	}

	public void SetSelectionMenuBG(bool isTrial)
    {
		selectionPanelTrialBG.SetActive(isTrial);
		selectionPanelTrialTitleBG.SetActive(isTrial);
		selectionPanelBG.SetActive(!isTrial);
		selectionPanelTitleBG.SetActive(!isTrial);
	}

	public void ShowSelectionMenuAnimation()
    {
        SelectionMenuAnimator.SetBool("active", true);
        //      SelectionMenuBlurAnimator.SetBool("active", true);
    
        foreach (MapIcon icon in FindObjectsOfType<MapIcon>())
        {


            icon.GetComponent<ShowTooltip>().enabled = false;
            Helpers.Instance.HideTooltip();
            
        }

        isViewingNode = true;
        
    }

	
	public void ScaleSelectedBirds(float time, Vector3 to)
	{
		for (int i = 0; i < 3; i++)
		{
			if(Var.playerPos[i, 0]!= null)
			{
				LeanTween.scale(Var.playerPos[i, 0].gameObject, to, time).setEase(LeanTweenType.easeOutBack);
			}
		}
	}
}
