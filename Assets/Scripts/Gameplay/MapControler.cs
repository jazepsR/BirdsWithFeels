using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapControler : MonoBehaviour {
	public static MapControler Instance { get; private set; }
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
	public GameObject selectionTiles;
	public Button startLvlBtn;
	public float scaleTime = 0.35f;
	public Image[] pieChart;
	[HideInInspector]
	public MapIcon SelectedIcon;
	public Text timerText;
	public List<Bird> selectedBirds;
	public Sprite trialSprite;
	TimedEventControl[] timedEvents;
	void Awake()
	{
		Instance = this;
	}
	// Use this for initialization
	void Start () {
		timedEvents = FindObjectsOfType<TimedEventControl>();
		Var.isBoss = false;
		timerText.text = "Week: " + Mathf.Max(0, Var.currentWeek);
		selectionTiles.transform.localScale = Vector3.zero;
		SelectionMenu.transform.localScale = Vector3.zero;
		canHeal = false;
		int count = 0;
		foreach(Bird bird in FillPlayer.Instance.playerBirds)
		{
			if (bird.gameObject.activeSelf)
				count++;
		}
		if (count == 3)
		{
			foreach (Bird bird in FillPlayer.Instance.playerBirds)
			{
				if (bird.gameObject.activeSelf)
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
		foreach(Bird bird in Var.activeBirds)
		{
			if(Helpers.Instance.ListContainsLevel(Levels.type.Friend2, bird.data.levelList) &&!Var.fled)
			{
				healTrail = Instantiate(Resources.Load("MouseHealParticle"), Input.mousePosition, Quaternion.identity) as GameObject;
				canHeal = true;
				title.text = bird.charName + " can heal one of your birds!";
			}
		}
		SaveLoad.Save();
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
			
		}
	}

	public void Rest()
	{
		Var.currentWeek++;
		timerText.text = "Week: " + Var.currentWeek;
		foreach (TimedEventControl control in timedEvents)
			control.CheckStatus();
		foreach(Bird bird in FillPlayer.Instance.playerBirds)
		{
			if (bird.data.injured)
			{
				bird.DecreaseTurnsInjured();
			}else
			{
				if (bird.data.health < bird.data.maxHealth)
				{
					bird.data.health++;
					GameObject healObj = Instantiate(bird.healParticle, bird.transform);
					Destroy(healObj, 1.5f);
				}
			}
		}
		//foreach (TimedEventControl timedEvent in FindObjectsOfType<TimedEventControl>())
		//	timedEvent.CheckStatus();
	}
	public void HideSelectionMenu()
	{
		canMove = true;
		LeanTween.scale(MapControler.Instance.SelectionMenu, Vector3.zero, MapControler.Instance.scaleTime).setEase(LeanTweenType.easeInBack);
		LeanTween.scale(MapControler.Instance.selectionTiles, Vector3.zero, MapControler.Instance.scaleTime).setEase(LeanTweenType.easeInBack);
		MapControler.Instance.ScaleSelectedBirds(MapControler.Instance.scaleTime, Vector3.zero);
		foreach (GuiMap map in FindObjectsOfType<GuiMap>())
			map.Clear();
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
