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
	public bool canHeal = false;
	GameObject healTrail;
	public Text title;
	public Transform centerPos;
	public bool canMove = true;
	public GameObject SelectionMenu;
	public Text SelectionText;
	public Text SelectionDescription;
	public Text SelectionTitle;
	public Button startLvlBtn;
	public float scaleTime = 0.35f;
	public Image[] pieChart;
    Animator SelectionMenuAnimator;
  //  public Animator SelectionMenuBlurAnimator;
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
	void Awake()
	{
		Instance = this;
		Var.snapshot = null;
		trialUiObject.SetActive(false);
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
				count++;
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
				if(bird.data.health<bird.data.maxHealth && !bird.data.injured)
				{
					GameObject healObj = Instantiate(bird.healParticle, bird.transform);
					Destroy(healObj, 1.5F);
				}
				bird.data.health = Mathf.Min(bird.data.health + 1, bird.data.maxHealth);                
				bird.data.mentalHealth = Mathf.Min(bird.data.mentalHealth + 1, Var.maxMentalHealth);
			}
		}
		if (count == 3)
		{
			foreach (Bird bird in FillPlayer.Instance.playerBirds)
			{
				if (bird.data.unlocked)
				{
					if (!bird.data.injured)
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
		if (selectedBirds.Count == 3)
		{
			canFight = true;
			startLvlBtn.interactable = true;            
			startLvlBtn.GetComponent<ShowTooltip>().tooltipText = "";
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
		}
	}

	public void Rest()
	{
		Var.currentWeek++;

		AudioControler.Instance.buttonSoundMap.Play();
		LeanTween.delayedCall(0.3f, () => AudioControler.Instance.restSound.Play());
		timerText.text = "Week: " + Var.currentWeek;
		timerText.GetComponent<Animator>().SetTrigger("Increment");
		foreach (TimedEventControl control in timedEvents)
			control.CheckStatus();
		foreach(Bird bird in FillPlayer.Instance.playerBirds)
		{
			if (bird.data.injured)
			{
				bird.DecreaseTurnsInjured();
				GameObject healObj = Instantiate(bird.healParticle, bird.transform);
				Destroy(healObj, 1.5f);
			}
			else if (bird.data.health < bird.data.maxHealth)
				{
					bird.data.health++;
					GameObject healObj = Instantiate(bird.healParticle, bird.transform);
					Destroy(healObj, 1.5f);
				}
			
		}
		//foreach (TimedEventControl timedEvent in FindObjectsOfType<TimedEventControl>())
		//	timedEvent.CheckStatus();
	}
	public void HideSelectionMenu()
	{
        if(MapTutorial.instance && MapTutorial.instance.mapTutPopup.activeSelf)
        {
            return;
        }

		canMove = true;

        //this is seb taken off
        /*
        	LeanTween.scale(MapControler.Instance.SelectionMenu, Vector3.zero, MapControler.Instance.scaleTime).setEase(LeanTweenType.easeInBack);
        LeanTween.value(gameObject, (float alpha) => MapControler.Instance.SelectionMenu.GetComponent<CanvasGroup>().alpha = alpha, 1, 0, MapControler.Instance.scaleTime).setEase(LeanTweenType.easeInBack);
       */
         SelectionMenuAnimator.SetBool("active", false); //seb change 
        // SelectionMenuBlurAnimator.SetBool("active", false);

        MapControler.Instance.ScaleSelectedBirds(MapControler.Instance.scaleTime, Vector3.zero);
        
        foreach (GuiMap map in FindObjectsOfType<GuiMap>())
			map.Clear();
        MapControler.Instance.SelectedIcon = null;

    }

    public void ShowSelectionMenuAnimation()
    {
        SelectionMenuAnimator.SetBool("active", true);
  //      SelectionMenuBlurAnimator.SetBool("active", true);
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
