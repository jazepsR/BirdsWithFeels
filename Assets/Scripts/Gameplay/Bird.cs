﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class BirdSound
{
	public AudioGroup birdSelect;
    public AudioGroup mouseOverBird;
	public AudioGroup considerSound;
	public AudioGroup BirdSitDown;
	public AudioGroup pickupBird;
	public AudioGroup dropBird;
	public AudioGroup birdDialogueTalk;
    public AudioGroup birdBattleConversations;
    public AudioGroup birdSelectSound;
	public AudioGroup birdWinSound;
	public EventAudio eventAudio;
	

	public AudioGroup GetTalkGroup(Var.Em emotion)
	{
		AudioGroup talkGroup = null;		
		talkGroup = birdDialogueTalk;
		if(talkGroup.clips.Length == 0)
		{
			talkGroup = AudioControler.Instance.DefaultBirdSound.birdDialogueTalk;
		}
		return talkGroup;

	}
}
[Serializable]
public class EventAudio
{

	[Range(0, 2f)]
	public float volume = 1f;
	public EventScript.Character character;
	public AudioClip[] sylables;
}

[Serializable]
public class Bird : MonoBehaviour
{
	public string charName;
	public bool loadDataFromInspector = false;
	public string birdPrefabName;
	bool canGrab = true;
	public int portraitOrder = 1;
	public BirdData data;
	public string birdBio;
	//[HideInInspector]
	public int prevConf = 0;
	//[HideInInspector]
	public int prevFriend = 0;
	[HideInInspector]
	public int totalFriendliness = 0;
	[HideInInspector]
	public int totalConfidence = 0;
	[HideInInspector]
	public bool GainedLVLHealth = false;
	[HideInInspector]
	public bool foughtInRound = false;
	[HideInInspector]
	public bool hadMentalPain = false;
	public int x = -1;
	public int y = -1;
	public Var.Em emotion;
	public bool inUse = true;
	[NonSerialized]
	public SpriteRenderer colorRenderer;
	[HideInInspector, NonSerialized]
	public List<SpriteRenderer> colorSprites;
	[NonSerialized]
	public GameObject bush;
	public GameObject portrait, portraitTiny;
	[HideInInspector]
	public Vector3 target;
	public Vector3 home;
	[HideInInspector]
	public bool dragged = false;
	//[HideInInspector]
	[NonSerialized]
	public firendLine lines;
	bool needsReset = false;
	public enum dir { top, front, bottom };
	public dir position;
	public fillEnemy.enemyType enemyType = fillEnemy.enemyType.normal;
	public bool isEnemy = true;
	[HideInInspector]
	public int friendBoost = 0;
	public int groundFriendBoos = 0;
	[HideInInspector]
	public int wizardFrienBoos = 0;
	[HideInInspector]
	public int levelFriendBoos = 0;
	[HideInInspector]
	public int battleConfBoos = 0;
	public int groundConfBoos = 0;
	[HideInInspector]
	public int wizardConfBoos = 0;
	[HideInInspector]
	public int levelConfBoos = 0;
	//[HideInInspector]
	public bool isInfluenced = false;
	//[HideInInspector]
	public int healthBoost = 0;
	[HideInInspector]
	public int roundHealthChange = 0;
	//[HideInInspector]
	public int GroundRollBonus = 0;
	public bool hasShieldBonus = false;
	public bool inMap = false;
	[HideInInspector]
	public Sprite hatSprite;
	[HideInInspector]
	public int confLoseOnRest = 2;
	public int groundMultiplier = 1;
	public GameObject healParticle;
	public GameObject MHPParticle;
	[HideInInspector, NonSerialized]
	public Levels levelControler;
	[HideInInspector]
	public bool fighting = false;
	[HideInInspector]
	public int winsInOneFight = 0;
	[HideInInspector]
	public int ConfGainedInRound = 0;
	[HideInInspector]
	public int FriendGainedInRound = 0;
	// -1 = no fight, 0 = lost, 1 = won, 2 = won and lost
	[HideInInspector]
	public int wonLastBattle = -1;
	public int PlayerRollBonus = 0;
	public Image CooldownRing;
	public bool isHiding = false;
	public int prevRoundHealth;
	public int prevRoundMentalHealth;
	public string levelUpText;
	Color DefaultCol;
	Color HighlightCol;
	public int birdIndex = 0;
	public bool hasNewLevel = false;
	public Var.Em prevEmotion = Var.Em.finish;
	bool started = false;
	public GameObject EnemyArt = null;
	public GameObject GroundBonus;
	public GameObject mapHighlight;
	GameObject birdArtObj;
	public Image coolDownRing;
	GameObject selectionEffect;
	bool selectionBeingDestroyed = false;
	[HideInInspector]
	public GameObject cautiousParticleObj = null;
	public Transform restingMouth;
	bool mouseHeld = false;
	float clickTime = 0;
	public BirdSound birdSounds;
	public EmoIndicator indicator;
	public List<LevelDataScriptable> levelList = new List<LevelDataScriptable>();
	[HideInInspector] public bool seedCollectedInRound = false;
	void Awake()
	{
		if (!isEnemy && !Var.isTutorial)
			LoadBirdData();
	}
	void Start()
	{
		canGrab = true;
		birdSounds = new BirdSound();
		wonLastBattle = -1;
		if (!isEnemy && portrait == null)
		{
			portrait = Resources.Load<GameObject>("prefabs/portraits/portrait_" + charName);
			portraitTiny = Resources.Load<GameObject>("prefabs/portraits/tiny_portrait_" + charName);
			birdSounds = AudioControler.Instance.GetBirdSoundGroup(charName);

		}
		if (isEnemy)
		{

			birdSounds.birdDialogueTalk = AudioControler.Instance.VultureDialogueSounds;

		}
		/* if (!isEnemy && !inMap)
		 {
			 GuiContoler.Instance.ShowSpeechBubble(transform.Find("mouth").transform, "hi!");
			 GuiContoler.Instance.ShowSpeechBubble(transform.Find("mouth").transform, "hi2!");
			 GuiContoler.Instance.ShowSpeechBubble(transform.Find("mouth").transform, "hi3!");
		 }*/

		prevRoundHealth = data.health;
		prevRoundMentalHealth = data.mentalHealth;
		x = -1;
		y = -1;
		prevConf = data.confidence;
		prevFriend = data.friendliness;


		if (!isEnemy)
		{

			/*if (relationships == null)
			{
				relationships = new Dictionary<EventScript.Character, int>();
				relationships.Add(EventScript.Character.Kim, 0);
				relationships.Add(EventScript.Character.Terry, 0);
				relationships.Add(EventScript.Character.Alexander, 0);
				relationships.Add(EventScript.Character.Sophie, 0);
				relationships.Add(EventScript.Character.Rebecca, 0);
				try
				{
					relationships.Remove(Helpers.Instance.GetCharEnum(this));
				}
				catch
				{
					print("error setting up realtionships");
				}
			}
		   
			RelationshipScript.applyRelationship(this, false);
			SetRealtionshipParticles();*/

			var BirdArt = Resources.Load("prefabs/" + birdPrefabName);
			birdArtObj = Instantiate(BirdArt, transform) as GameObject;
			birdArtObj.transform.localPosition = new Vector3(0.23f, -0.3f, 0);
			birdArtObj.transform.SetAsFirstSibling();

			if (levelList.Count == 0 && !Var.isTutorial)
			{
				levelList = new List<LevelDataScriptable>();
				/*Sprite icon = Helpers.Instance.GetLVLSprite(startingLVL);     
				if(startingLVL != Levels.type.None)          
					AddLevel(new LevelData(startingLVL, Var.Em.Neutral,icon));*/
			}
			levelControler = GetComponent<Levels>();
			/*if (startingLVL != Levels.type.None)
				levelControler.ApplyStartLevel(this, levelList);      */
		}
		colorSprites = new List<SpriteRenderer>();
		foreach (SpriteRenderer child in transform.GetComponentsInChildren<SpriteRenderer>(true))
		{
			if (child.gameObject.name.Contains("flat"))
				colorSprites.Add(child);
		}
		SetEmotion();
		foreach (SpriteRenderer sp in colorSprites)
			sp.color = Helpers.Instance.GetEmotionColor(emotion);
		//SetCoolDownRing(false);
		if (CooldownRing != null)
		{
			CooldownRing.fillAmount = (float)(data.CoolDownLength - data.CoolDownLeft) / (float)data.CoolDownLength;
		}
		lines = GetComponent<firendLine>();
		if (isEnemy)
		{
			home = transform.localPosition;
		}
		else
		{
			home = transform.position;
			LoadStats();
			SetBandages();
		}
		target = transform.position;
		if (!isEnemy && (data.injured || data.health <= 0))
		{
			data.injured = true;
			Animator anim = GetComponentInChildren<Animator>();
			anim.SetBool("injured", true);
			if (inMap)
			{
				mapHighlight.SetActive(false);
				MapControler.Instance.selectedBirds.Remove(this);
				//MapControler.Instance.CanLoadBattle();
			}
		}

		if (inMap && MapControler.Instance.count != 3)
		{
			GetComponentInChildren<Animator>().SetBool("rest", true);
		}

		if (data.recievedSeeds == null)
			data.recievedSeeds = new List<string>();
		showText();

		gameObject.SetActive(data.unlocked);
		portraitOrder = Helpers.GetPortraitNumber(charName);
	}


	void LoadBirdData()
	{
		string path = Application.persistentDataPath + "/" + Var.currentSaveSlot + "/" + charName + ".dat";
#if !UNITY_EDITOR
		loadDataFromInspector = false;
#endif
		if (File.Exists(path) && !loadDataFromInspector)
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(path, FileMode.Open);
			data = (BirdData)bf.Deserialize(file);
			file.Close();
			levelList = new List<LevelDataScriptable>();
			levelList = new List<LevelDataScriptable>();
			if (data.level > 1)
			{
				if (Helpers.Instance.levels == null)
				{
					Helpers.Instance.LoadLevelData();
				}
				for (int i = 0; i < data.level-1; i++)
				{
					levelList.Add(Helpers.Instance.levels[i]);
					if (i + 2 == data.level)
					{
						data.lastLevel = Helpers.Instance.levels[i];
					}
				}
			}
			//Debug.LogError(data.charName + " current level: " + data.level);
		}
		portraitOrder = Helpers.GetPortraitNumber(charName);
		if (!isEnemy)
			birdPrefabName = Helpers.GetBirdArtName(charName);

	}
	public void SaveBirdData()
	{
		if (!isEnemy)
		{
			string path = Application.persistentDataPath + "/" + Var.currentSaveSlot + "/" + charName + ".dat";
			if (File.Exists(path))
			{
				File.Delete(path);
			}
			Debug.Log("savePath: " + path);
			if (charName.ToLower() == "Kim")
			{
				data.unlocked = Var.KimUnlocked;
			}
			if (charName.ToLower() == "Sophie")
			{
				data.unlocked = Var.SophieUnlocked;
			}
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(path);
			bf.Serialize(file, data);
			file.Close();
		}
	}
	public void publicStart()
	{
		Start();
	}
	public void DecreaseTurnsInjured(int reduceBy = 1)
	{
		if (data.injured)
		{
			data.TurnsInjured-=reduceBy;
			if (data.TurnsInjured <= 0)
			{
				data.injured = false;
				GetComponentInChildren<Animator>().SetBool("injured", false);
				data.health = data.maxHealth;
			}
		}
	}
	/*void SetRealtionshipParticles()
	{
		if (inMap)
			return;
		RelationshipParticles.SetActive(relationshipBonus > 0);
		CrushParticles.SetActive(relationshipBonus < 0);

	}*/

	public void Speak(string text)
	{
		string str = charName;
		GuiContoler.Instance.ShowSpeechBubble(GetMouthTransform(), text, birdSounds.GetTalkGroup(emotion), !isEnemy);
	}

	public Transform GetMouthTransform()
	{
		Transform mouth = transform;
		if (isEnemy)
		{
			mouth = EnemyArt.transform.Find("mouth");
			if (mouth != null)
			{
				return mouth;
			}
		}
		if (GetComponentInChildren<Animator>().GetBool("rest"))
		{
			return restingMouth;
		}
		try
		{
			birdArtObj.transform.Find("neutral").SetAsLastSibling();

			for (int i = 0; i < birdArtObj.transform.childCount; i++)
			{
				if (birdArtObj.transform.GetChild(i).name == emotion.ToString().ToLower())
				{
					mouth = birdArtObj.transform.GetChild(i).Find("mouth");

					break;
				}
			}
		}
		catch
		{
			mouth = transform;
		}
		return mouth;
	}
	public void SetBandages()
	{
		//bird
		var bandageSwitchers = birdArtObj.GetComponentsInChildren<BandageSwitcher>();
		foreach (BandageSwitcher switcher in bandageSwitchers)
		{
			switcher.SetBandages(data.health <= 1);
		}
	}
	public void SetCoolDownRing(bool active)
	{
		/*if (CooldownRing != null)
			if (Helpers.Instance.ListContainsLevel(Levels.type.Brave1, data.levelList))
			{
				CooldownRing.color = Helpers.Instance.GetEmotionColor(Var.Em.Confident);
				CooldownRing.gameObject.SetActive(active);
			}else
			{
				if(Helpers.Instance.ListContainsLevel(Levels.type.Lonely2, data.levelList))
				{
					CooldownRing.color = Helpers.Instance.GetEmotionColor(Var.Em.Solitary);
					CooldownRing.gameObject.SetActive(active);
				}
				else
				{
					CooldownRing.gameObject.SetActive(false);
				}
				
			}*/
	}
	public void AddLevel(LevelDataScriptable levelData)
	{
		hasNewLevel = true;
		//Helpers.Instance.EmitEmotionParticles(transform, Var.Em.finish);
		//Helpers.Instance.EmitEmotionParticles(transform, emotion, false);


		data.lastLevel = levelData;

		data.levelRollBonus++;
		//Reset Emotions
		/*prevConf = confidence;
		prevFriend = friendliness;
		switch (data.emotion)
		{
			case Var.Em.Confident:
				confidence = 4;
				if (friendliness > 0)
					friendliness = (int)Mathf.Min(4, friendliness);
				else
					friendliness = (int)Mathf.Max(-4, friendliness);
				break;
			case Var.Em.Cautious:
				confidence = -4;
				if (friendliness > 0)
					friendliness = (int)Mathf.Min(4, friendliness);
				else
					friendliness = (int)Mathf.Max(-4, friendliness);
				break;
			case Var.Em.Social:
				friendliness = 4;
				if (confidence > 0)
					confidence = (int)Mathf.Min(4, confidence);
				else
					confidence = (int)Mathf.Max(-4, confidence);
				break;
			case Var.Em.Solitary:
				friendliness = -4;
				if (confidence > 0)
					confidence = (int)Mathf.Min(4, confidence);
				else
					confidence = (int)Mathf.Max(-4, confidence);
				break;
			default:
				break;
		}*/
		ResetBonuses();
		levelList.Add(levelData);
		data.level = levelList.Count + 1;
		levelUpText = null;

		Achievements.checkBirdLevelUp(this, false);
	}
	public float getBonus()
	{
		if (y != -1)
		{
			//ResetBonuses();
			//ObstacleGenerator.Instance.tiles[y * 4 + x].GetComponent<LayoutButton>().ApplyPower(this);
		}
		//relationshipBonus = GetRelationshipBonus();
		//print(charName+ " playerRollBonus: "+ PlayerRollBonus + " GroundRollBonus: "+ GroundRollBonus);
		return data.levelRollBonus + PlayerRollBonus + GroundRollBonus;// + relationshipBonus;
	}

	public string GetBonusText()
	{
		string bonusText = "";
		if (data.levelRollBonus != 0)
			bonusText += "\nFrom levels: " + (data.levelRollBonus * 10).ToString("+#;-#;0");
		if (PlayerRollBonus != 0)
			bonusText += "\nFrom other birds: " + (PlayerRollBonus * 10).ToString("+#;-#;0");
		if (GroundRollBonus != 0)
			bonusText += "\nFrom the current tile: " + (GroundRollBonus * 10).ToString("+#;-#;0");
		/*if (relationshipBonus != 0)
			bonusText += "\nFrom relationships: " + (relationshipBonus * 10).ToString("+#;-#;0");*/
		if (bonusText != "")
			bonusText = bonusText.Substring(1);
		return bonusText;

	}


	/*public int GetRelationshipBonus()
	{
		return 0;
		try
		{
			if (relationshipBird.relationshipBird.charName == charName)
				return 2;
			else
				return -2;

		}
		catch
		{
			try
			{
				if (relationshipBird.charName != null)
					return -2;
				else
					return 0;
			}
			catch
			{
				return 0;
			}
		}
		
		if (relationshipBird!= null)
		{
			if(relationshipBird.relationshipBird !=null && relationshipBird.relationshipBird.charName == charName)
			{
				return 2;
			}else
			{
				return -2;
			}            
		}
		return 0;
	}*/



	void LoadStats()
	{
		if (data.injured)
			return;
		bool SaveDataCreated = false;
		Bird savedData = null;
		foreach (Bird data in Var.activeBirds)
		{
			if (data.charName == charName)
			{
				SaveDataCreated = true;
				savedData = data;
				break;
			}
		}


		if (!SaveDataCreated && (Var.activeBirds.Count <= 3 || inMap))
		{
			//Var.activeBirds.Add(this);
			// print("created something!");       
		}
		else
		{
			//TODO: fix this
			/*
			confidence = savedData.confidence;
			friendliness = savedData.friendliness;*/
		}
	}

	public void ResetBonuses()
	{
		Debug.Log("Reset: " + charName + " ground bonus friend: " + groundFriendBoos + " conf: " + groundConfBoos);
		battleConfBoos = 0;
		friendBoost = 0;
		groundConfBoos = 0;
		groundFriendBoos = 0;
		wizardConfBoos = 0;
		wizardFrienBoos = 0;
		GroundRollBonus = 0;
		PlayerRollBonus = 0;
		levelFriendBoos = 0;
		levelConfBoos = 0;
		healthBoost = 0;
		hasShieldBonus = false;
		isInfluenced = false;
		Destroy(cautiousParticleObj);
	}

	public int GetFriendlinessBonus()
	{
		return friendBoost + groundFriendBoos + wizardFrienBoos + levelFriendBoos;
	}
	public int GetConfBoost()
	{
		return battleConfBoos + groundConfBoos + wizardConfBoos + levelConfBoos;
	}
	public void TryLevelUp()
	{
		if (data.injured)
			return;
	}
	public void ResetAfterLevel()
	{
		if (data.injured)
			return;
		winsInOneFight = 0;
		wonLastBattle = -1;
		x = -1;
		y = -1;
		try
		{
			levelControler.ApplyLevelOnPickup(this, levelList);
			/*if (Helpers.Instance.ListContainsLevel(Levels.type.Sophie, data.levelList))
				levelControler.Halo.SetActive(false);*/
			showText();
		}
		catch
		{
			Debug.LogError("failed to reset!");
		}

	}
	/*public string CheckLevels(bool toApply = true)
	{
		//TODO: make this look nice
		string st = "";
		if (!Var.gameSettings.shownLevelTutorial)
			return st;
		st= levelControler.CheckBrave1(toApply);
		if (st != null)
			return st;
		st = levelControler.CheckLonely1(toApply);
		if (st != null)
			return st;
		st = levelControler.CheckFriendly1(toApply);
		if (st != null)
			return st;
		st = levelControler.CheckScared1(toApply);
		if (st != null)
			return st;
		st = levelControler.CheckBrave2(toApply);
		if (st != null)
			return st;
		st = levelControler.CheckLonely2(toApply);
		if (st != null)
			return st;
		st = levelControler.CheckFriendly2(toApply);
		if (st != null)
			return st;
		st = levelControler.CheckScared2(toApply);
		if (st != null)
			return st;

		return null;
	}*/
	void OnMouseEnter()
	{

		// Debug.LogError("friendly: " + prevFriend + " gain friendly " + GetFriendlinessBonus() + " conf: " + prevConf + " prev conf: " + GetFriendlinessBonus());
		//if(!Var.Infight)
		if (!Var.CanShowHover || GuiContoler.Instance.speechBubbleObj.activeSelf || Var.Infight || EventController.Instance.eventObject.activeSelf)
			return;
		if (Var.CanShowHover)
			showText();
		if (isEnemy)
		{
			GetComponent<feedBack>().ShowEnemyHoverText();
			AudioControler.Instance.EnemySound();
		}
		else
		{

			if (Time.timeSinceLevelLoad > 1)
			{

				if (inMap)
				{
					MapControler.Instance.charInfoAnim.SetBool("show", true);
					MapControler.Instance.charInfoAnim.SetBool("hide", false);
				}
				if (!dragged && birdSounds.mouseOverBird.clips.Length > 0)
					birdSounds.mouseOverBird.Play();
			}
			foreach (SpriteRenderer sp in colorSprites)
				sp.color = HighlightCol;

		}
		//if(inMap)
		//ProgressGUI.Instance.PortraitClick(this);
		if (!inMap && !isEnemy && levelUpText != null && levelUpText != "" && !dragged && !Var.Infight && Var.CanShowHover)
			GuiContoler.Instance.ShowLvlText(levelUpText);
	}
	void OnMouseOver()
	{
		if (Var.Infight || EventController.Instance.eventObject.activeSelf)
			return;
		if (isEnemy || GuiContoler.Instance.speechBubbleObj.activeSelf)
			return;
		if (GuiContoler.Instance.speechBubbleObj.activeSelf)
			showText();
		SetCoolDownRing(true);
		if (selectionEffect == null && Time.timeSinceLevelLoad > 1f && Helpers.Instance.selectionEffect != null && !isEnemy && !dragged)
			selectionEffect = Instantiate(Helpers.Instance.selectionEffect, transform);
		Var.selectedBird = gameObject;
		if (Input.GetMouseButtonUp(1) && !dragged)
		{
			GuiContoler.Instance.GraphButton();
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (Time.timeSinceLevelLoad - clickTime > 0.25f && !isEnemy && !inMap && !Var.isDragControls)
			{
				birdSounds.pickupBird.Play();
				GetComponentInChildren<Animator>().SetBool("lift", true);
				foreach (SpriteRenderer child in transform.GetComponentsInChildren<SpriteRenderer>(true))
				{
					child.sortingLayerName = "Front";
				}
				indicator.ResetLayer();

				for (int i = 0; i < Var.playerPos.GetLength(0); i++)
				{
					for (int j = 0; j < Var.playerPos.GetLength(1); j++)
					{

						if (Var.playerPos[i, j] == this)
						{
							Var.playerPos[i, j] = null;
							break;
						}
					}
				}

				dragged = true;
				if (selectionEffect != null)
					DestroySelection();
				ResetOnSelection();
			}
			else
			{
				if (!dragged && !Var.isDragControls)
				{
					if (inMap)
						return;
					foreach (LayoutButton btn in ObstacleGenerator.Instance.tiles)
					{
						LeanTween.delayedCall((btn.index.x + btn.index.y) * 0.05f + 0.05f, () => btn.ShowHighlight());
						btn.gameObject.layer = LayerMask.NameToLayer(charName.Substring(0, 1).ToUpper() + charName.Substring(1));
					}

					ResetOnSelection();
					Var.clickedBird = this;
					foreach (Bird bird in Var.activeBirds)
						bird.DestroySelection();
				}
				else
				{


					foreach (SpriteRenderer child in transform.GetComponentsInChildren<SpriteRenderer>(true))
					{
						child.sortingLayerName = charName.Substring(0, 1).ToUpper() + charName.Substring(1);
					}
					if (indicator)
					{
						indicator.ResetLayer();
					}
					GetComponentInChildren<Animator>().SetBool("lift", false);
					if (Var.isTutorial && !GuiContoler.Instance.inMap)
					{
						foreach (LayoutButton tile in ObstacleGenerator.Instance.tiles)
						{
							tile.baseColor = tile.defaultColor;
							LeanTween.color(tile.gameObject, tile.defaultColor, 0.3f);
						}
					}
				}
			}
		}
		if (Input.GetMouseButtonUp(2))
		{
			/*if (!inMap && Helpers.Instance.ListContainsLevel(Levels.type.Scared2, data.levelList))
			{
				bush.SetActive(!bush.activeSelf);
				isHiding = bush.activeSelf;
				GameLogic.Instance.CanWeFight();
				GameLogic.Instance.UpdateFeedback();

			}*/
		}
		if (Input.GetMouseButtonDown(0))
		{
			if (Var.Infight || data.injured || GuiContoler.Instance.speechBubbleObj.activeSelf)
				return;
			if (!inMap)
			{
				if (birdSounds.pickupBird.clips.Length > 0)
				{
					birdSounds.pickupBird.Play();
				}
				else
				{
					AudioControler.Instance.DefaultBirdSound.birdSelect.Play();
				}
			}
			if (inMap)
			{
				if (MapControler.Instance.selectedBirds.Contains(this))
				{
					mapHighlight.SetActive(false);
					GetComponentInChildren<Animator>().SetBool("rest", true);
					AudioControler.Instance.PlaySound(AudioControler.Instance.BirdSitDown);
					MapControler.Instance.selectedBirds.Remove(this);
				}
				else
				{
					mapHighlight.SetActive(true);
					GetComponentInChildren<Animator>().SetBool("rest", false);
					birdSounds.birdSelect.Play();
					MapControler.Instance.selectedBirds.Add(this);
				}
				//MapControler.Instance.CanLoadBattle();
				return;
				if (MapControler.Instance.canHeal)
				{
					ChageHealth(data.maxHealth);
					MapControler.Instance.canHeal = false;
					MapControler.Instance.title.text = "Adventure map";
					showText();
					return;
				}
			}
			if (Var.isDragControls && canGrab)
			{
				birdSounds.pickupBird.Play();
				GetComponentInChildren<Animator>().SetBool("lift", true);
				foreach (SpriteRenderer child in transform.GetComponentsInChildren<SpriteRenderer>(true))
				{
					child.sortingLayerName = "Front";
				}
				indicator.ResetLayer();

				for (int i = 0; i < Var.playerPos.GetLength(0); i++)
				{
					for (int j = 0; j < Var.playerPos.GetLength(1); j++)
					{

						if (Var.playerPos[i, j] == this)
						{
							Var.playerPos[i, j] = null;
							break;
						}
					}
				}

				dragged = true;
				if (selectionEffect != null)
					DestroySelection();
				if (!inMap)
				{
					if (!Var.Infight)
						ResetBonuses();
					lines.RemoveLines();
					if (cautiousParticleObj != null)
						Destroy(cautiousParticleObj);
					GuiContoler.Instance.HideLvlText();
					GroundBonus.SetActive(false);
					try
					{
						foreach (Bird bird in FillPlayer.Instance.playerBirds)
						{
							if (bird.charName != charName)
							{
								bird.levelControler.ApplyLevelOnPickup(bird, bird.levelList);
								bird.levelControler.ApplyLevelOnDrop(bird);//, bird.data.levelList);
							}
						}
					}
					catch
					{
						levelControler.ApplyLevelOnPickup(this, levelList);
					}
					levelControler.ApplyLevelOnPickup(this, levelList);
					UpdateFeedback();
				}
			}
			else
			{
				clickTime = Time.timeSinceLevelLoad;
			}
		}
	}


	void ResetOnSelection()
	{
		if (!inMap)
		{
			if (!Var.Infight)
				ResetBonuses();
			lines.RemoveLines();
			if (cautiousParticleObj != null)
				Destroy(cautiousParticleObj);
			GuiContoler.Instance.HideLvlText();
			GroundBonus.SetActive(false);
			try
			{
				foreach (Bird bird in FillPlayer.Instance.playerBirds)
				{
					if (bird.charName != charName)
					{
						bird.levelControler.ApplyLevelOnPickup(bird, bird.levelList);
						bird.levelControler.ApplyLevelOnDrop(bird);//, bird.data.levelList);
					}
				}
			}
			catch
			{
				levelControler.ApplyLevelOnPickup(this, levelList);
			}
			levelControler.ApplyLevelOnPickup(this, levelList);
			UpdateFeedback();
		}
	}
	void OnMouseExit()
	{

		if (!inMap && !isEnemy)
		{
			GuiContoler.Instance.HideLvlText();

			//AudioControler.Instance.smallGraphDisappear.Play();
		}
		/*if (inMap && !EventController.Instance.eventObject.activeSelf)
		{
			MapControler.Instance.charInfoAnim.SetBool("hide", true);
			MapControler.Instance.charInfoAnim.SetBool("show", false);
		}*/
		if (!dragged)
		{
			SetCoolDownRing(false);
			if (isEnemy)
			{

			}
			else
			{
				GetComponentInChildren<Animator>().SetBool("lift", false);
				foreach (SpriteRenderer sp in colorSprites)
					sp.color = DefaultCol;
			}

		}
		if (isEnemy)
		{
			GuiContoler.Instance.tooltipText.transform.parent.gameObject.SetActive(false);
		}
		else if (selectionEffect != null)
			DestroySelection();
	}
	void DestroySelection()
	{
		if (this == Var.clickedBird)
			return;
		if (selectionEffect != null)
		{
			selectionEffect.GetComponent<Animator>().SetTrigger("deselected");
			selectionBeingDestroyed = true;
			LeanTween.delayedCall(0.2f, () => { if (selectionEffect != null) Destroy(selectionEffect); selectionBeingDestroyed = false; });
		}

	}

	void UpdateFeedback()
	{

		fighting = false;
		/*if (Helpers.Instance.ListContainsLevel(Levels.type.Sophie, data.levelList) )
		{
			if (GameLogic.Instance.CheckIfResting(this)&&!dragged)
			{
				levelControler.ApplyLevelOnDrop(this, data.levelList);
			   
			}
			else
			{
				Debug.Log("not resting " + charName);
				levelControler.ApplyLevelOnPickup(this, data.levelList);
				if (cautiousParticleObj != null)
					Destroy(cautiousParticleObj);

			}
		}*/

		if (indicator)
		{

			if (dragged || (target == home && !isEnemy))
			{
				indicator.Hide();
			}
			else
			{
				GetFriendlinessBonus();
				GetConfBoost();

			}
		}
		GameLogic.Instance.UpdateFeedback();

	}
	public Bird(string name, int confidence = 0, int friendliness = 0)
	{
		this.data = new BirdData();
		this.data.confidence = confidence;
		this.data.friendliness = friendliness;
		this.charName = name;
		//SetEmotion();
		//Debug.Log(ToString());
	}

	public void OnLevelPickup()
	{
		levelControler.ApplyLevelOnPickup(this, levelList);
	}
	public void HealFully(bool reviveFromInjury)
	{
		if (reviveFromInjury)
		{
			DecreaseTurnsInjured(5);
		}
		ChageHealth(data.maxHealth);
		data.mentalHealth = Var.maxMentalHealth;
		prevRoundMentalHealth = Var.maxMentalHealth;

	}
	public void ChageHealth(int change)
	{
		try
		{
			if (data.injured)
				return;
			if (data.health + roundHealthChange <= 0)
				return;
			if (change > 0)
			{
				if (data.health != data.maxHealth && !data.injured)
				{
					GameObject healObj = Instantiate(healParticle, transform);
					Destroy(healObj, 1.5f);
				}
			}
			else if (hasShieldBonus)
			{
				//Add Shield Effects Here
				return;
			}
			roundHealthChange += change;
			Debug.Log(charName + " health change " + change);
			if (GuiContoler.Instance.selectedBird == this)
			{
				showText();
			}
			if (data.health + roundHealthChange <= 0)
			{
				data.injured = true;
				battleConfBoos -= 5;
				GetComponentInChildren<Animator>().SetBool("injured", true);
				data.TurnsInjured = 4;
			}
		}
		catch (Exception ex)
		{
			print("Error in add health: " + ex.Message);
		}

	}

	public override string ToString()
	{
		if (enabled && !isEnemy)
		{
			return '\u2022' + "friendly: " + data.friendliness + " " + '\u2022' + "brave: " + data.confidence +
				"\n" + '\u2022' + "level: " + data.level + " " + '\u2022' + "health: " + data.health + "\n" + data.birdAbility;
		}
		else
		{
			return null;
		}

	}

	public string GetHeading()
	{
		if (enabled)
		{
			return charName + "\n" + emotion.ToString();
		}
		else
		{
			return null;
		}
	}
	public Vector2 ApplyInfluence(bool shouldApply = true)
	{

		//x is confidence, y is friendly
		Vector2 toReturn = new Vector2(0, 0);
		if (isInfluenced)
		{
			if (friendBoost + wizardFrienBoos + groundFriendBoos + levelFriendBoos > 0)
			{
				if (shouldApply)
					levelFriendBoos += 1;
				toReturn.y = 1;
				print(charName + " got influenced to friendly");
			}
			else
			{
				if (friendBoost + wizardFrienBoos + groundFriendBoos + levelFriendBoos < 0)
				{
					levelFriendBoos -= 1;
					toReturn.y = -1;
					print(charName + " got influenced to lonely");
				}
			}
			if (battleConfBoos + groundConfBoos + wizardConfBoos + levelConfBoos > 0)
			{
				if (shouldApply)
					levelConfBoos += 1;
				toReturn.x = 1;
				print(charName + " got influenced to confident");

			}
			else
			{
				if (battleConfBoos + groundConfBoos + wizardConfBoos + levelConfBoos < 0)
				{
					if (shouldApply)
						levelConfBoos -= 1;
					toReturn.x = -1;
					print(charName + " got influenced to scared");
				}
			}
		}
		return toReturn;
	}
	public void AddRoundBonuses(bool doFightStuff = true)
	{
		print(charName + " doing round bonus. HealthGain " + roundHealthChange);
		if (data.injured)
			return;
		ApplyInfluence();

		prevRoundHealth = data.health;
		prevRoundMentalHealth = data.mentalHealth;



		if (doFightStuff)
		{

			/*RelationshipScript.applyRelationship(this);
			relationshipBonus = GetRelationshipBonus();
			SetRealtionshipParticles();
			setRelationshipDialogs();*/
			if (!foughtInRound)
			{
				data.consecutiveFightsWon = 0;
				data.roundsRested++;
				battleConfBoos -= 2;
			}
			else
			{
				data.roundsRested = 0;
			}
			if (data.CoolDownLeft > 0)
				data.CoolDownLeft--;
			try
			{
				CooldownRing.fillAmount = (float)(data.CoolDownLength - data.CoolDownLeft) / (float)data.CoolDownLength;
			}
			catch
			{
				Debug.Log("fix cooldown rings!");
			}
			if (!foughtInRound)
				ChageHealth(1);
			try
			{
				levelControler.OnfightEndLevel(this, levelList);
			}
			catch
			{

			}
		}
		if (!hasNewLevel && !Var.freezeEmotions)
		{
			data.friendliness += friendBoost + wizardFrienBoos + groundFriendBoos + levelFriendBoos;
			data.confidence += battleConfBoos + groundConfBoos + wizardConfBoos + levelConfBoos;
			ConfGainedInRound = data.confidence - prevConf;
			FriendGainedInRound = data.friendliness - prevFriend;
		}
		data.health = Mathf.Min(data.health + healthBoost + roundHealthChange, data.maxHealth);
		//Mental health
		if ((Mathf.Abs(data.confidence) >= Var.DangerZoneStart || Mathf.Abs(data.friendliness) >= Var.DangerZoneStart) && (Mathf.Abs(prevConf) >= Var.DangerZoneStart || Mathf.Abs(prevFriend) >= Var.DangerZoneStart) && !Var.freezeEmotions && !inMap && Var.gameSettings.useMHP)
		{//In danger zone

			if (data.mentalHealth == 0)
			{
				//dont kill the player
				if (data.health > 1)
				{//Effects from having no mental health left
				 //mentalHealth = Var.maxMentalHealth;					
					hadMentalPain = true;
					data.health--;
				}
				//data.mentalHealth = 1;
			}
			data.mentalHealth = Math.Max(data.mentalHealth - 1, 0);

		}
		else// if (Mathf.Abs(data.confidence) < Var.DangerZoneStart && Mathf.Abs(data.friendliness) < Var.DangerZoneStart && (Mathf.Abs(prevConf) < Var.DangerZoneStart && Mathf.Abs(prevFriend) < Var.DangerZoneStart))
		{//In comfort zone
			data.mentalHealth = Mathf.Min(Var.maxMentalHealth, data.mentalHealth + 1);
		}
		//Debug.LogError("Updating MPH for" + charName + " mental health: " + data.mentalHealth);
		roundHealthChange = 0;
		foughtInRound = false;

		Helpers.Instance.NormalizeStats(this);
		if (this == GuiContoler.Instance.selectedBird)
			showText();
	}
	public void ResetCanGrab()
	{
		StartCoroutine(ResetGrabDelay());
	}

	private IEnumerator ResetGrabDelay()
	{
		canGrab = false;
		yield return new WaitForSeconds(0.28f);
		canGrab = true;
	}
	public void SetEmotion()
	{
		float factor = 0.13f;
		float transitionTime = 1.9f;
		if (prevEmotion.Equals(Var.Em.finish) || isEnemy)
		{
			transitionTime = 0.0f;
		}
		prevEmotion = emotion;
		if (Mathf.Abs((float)data.confidence) < Var.lvl1 && Mathf.Abs((float)data.friendliness) < Var.lvl1)
		{
			//No type
			emotion = Var.Em.Neutral;
			try
			{
				foreach (SpriteRenderer sp in colorSprites)
					LeanTween.color(sp.gameObject, Helpers.Instance.GetEmotionColor(emotion), transitionTime);
			}
			catch { }
			DefaultCol = Helpers.Instance.GetEmotionColor(emotion);
			HighlightCol = new Color(DefaultCol.r + factor, DefaultCol.g + factor, DefaultCol.b + factor);
		}
		else if (Mathf.Abs((float)data.confidence) > Mathf.Abs((float)data.friendliness))
		{
			if (data.confidence > 0)
			{

				//Confident
				if (data.confidence >= Var.lvl1)
					emotion = Var.Em.Confident;

			}
			else
			{
				//Scared
				if (data.confidence <= -Var.lvl1)
					emotion = Var.Em.Cautious;
			}

		}
		else
		{
			//Friendly or lonely
			if (data.friendliness > 0)
			{

				//friendly				
				if (data.friendliness >= Var.lvl1)
					emotion = Var.Em.Social;
			}
			else
			{
				//Lonely				
				if (data.friendliness <= -Var.lvl1)
					emotion = Var.Em.Solitary;
			}

		}
		try
		{
			foreach (SpriteRenderer sp in colorSprites)
				LeanTween.color(sp.gameObject, Helpers.Instance.GetEmotionColor(emotion), transitionTime);
		}
		catch { }
		DefaultCol = Helpers.Instance.GetEmotionColor(emotion);
		HighlightCol = new Color(DefaultCol.r + factor, DefaultCol.g + factor, DefaultCol.b + factor);
		if (!isEnemy)
		{
			float delay = 0;
			if (Var.Infight)
				delay = 6f;
			LeanTween.delayedCall(delay, () => SetAnimation(emotion));
		}
		if (prevEmotion.Equals(Var.Em.finish))
			prevEmotion = emotion;


	}

	public void SetAnimation(Var.Em emotionNum)
	{
		try
		{
			Animator anim = GetComponentInChildren<Animator>();
			anim.SetInteger("emotion", Helpers.GetEmotionNumber(emotion));
		}
		catch
		{
			print("couldnt set emotion");
		}
	}

	public void showText()
	{
		if (ToString() != null)
		{
			//Var.birdInfo.text = ToString();         

			GuiContoler.Instance.clearSmallGraph();
			GuiContoler.Instance.smallGraph.PlotFull(this, false);

			GuiContoler.Instance.selectedBird = this;
			//SetRelationshipSliders(GuiContoler.Instance.relationshipSliders);
			Var.birdInfoFeeling.text = emotion.ToString();
			Var.birdInfoFeeling.color = Helpers.Instance.GetEmotionColor(emotion);
			if (data.lastLevel != null)
				Var.birdInfoHeading.text = Helpers.Instance.ApplyTitle(this, data.lastLevel.birdTitle);
			else
				Var.birdInfoHeading.text = charName;
			GuiContoler.Instance.PortraitControl(portraitOrder, emotion);
			GuiContoler.Instance.BirdCombatStr.text = "Combat strength: " + (getBonus() * 10f).ToString("+#;-#;0") + "%";
			GuiContoler.Instance.BirdCombatStr.gameObject.GetComponent<ShowTooltip>().tooltipText = GetBonusText();
			//levelUpText = CheckLevels(false);
			//set progress to level bar
			/*if (battleCount >= battlesToNextLVL)
			{
				battleCount = battlesToNextLVL;
				Var.powerText.text = "Ready to level up!";
				Var.powerBar.color = Helpers.Instance.GetSoftEmotionColor(emotion);
				Var.powerBar.fillAmount = 1;
			}
			else
			{
				Var.powerBar.color = Color.white;
				Var.powerText.text = null;
				Var.powerText.text = "Leveling available in " + (battlesToNextLVL - battleCount) + " battles!";
				Var.powerBar.fillAmount = (float)battleCount % 3 / (float)3;
			}*/

			int index = 0;
			GuiContoler.Instance.levelNumberText.text = data.level.ToString();
			//Set Relationship bars


			///Set level icons
			///Currently removed
			/*
			foreach (LVLIconScript icon in GuiContoler.Instance.lvlIcons)
			{
				
				if (data.levelList.Count > index && !Var.isTutorial)
				{
					icon.gameObject.SetActive(true);
					icon.GetComponent<Image>().sprite = data.levelList[index].LVLIcon;
					icon.textToDsiplay = data.levelList[index].levelInfo;
				}
				else
				{
					icon.gameObject.SetActive(false);
				}
				index++;
			}
			*/
			//set emotion bars, commented out by Seb since emotion bars are removed now
			//GuiContoler.Instance.confSlider.SetDist(confidence + battleConfBoos, this); 
			//GuiContoler.Instance.firendSlider.SetDist(friendliness, this);
			//set hearts
			//SetRelationshipText(GuiContoler.Instance.relationshipPortrait, GuiContoler.Instance.relationshipText);			
			try
			{
				if (data.injured)
				{
					Helpers.Instance.setHearts(GuiContoler.Instance.BirdInfoHearts, 0, data.maxHealth);
				}
				else
				{
					Helpers.Instance.setHearts(GuiContoler.Instance.BirdInfoHearts, data.health + roundHealthChange, data.maxHealth);
				}
				if (Var.gameSettings.useMHP)
				{
					Helpers.Instance.setHearts(GuiContoler.Instance.BirdMentalHearts, data.mentalHealth, Var.maxMentalHealth, -1, true);
				}
				GuiContoler.Instance.BirdMentalHearts[0].transform.parent.gameObject.SetActive(Var.gameSettings.useMHP);
			}
			catch
			{
				Debug.Log("failed to set hearts");
			}
		}
	}

	/*public void SetRelationshipSliders(GameObject sliderParent)
	{
		var keys = new List<EventScript.Character>(relationships.Keys);
		foreach (EventScript.Character birdFriend in keys)
		{
			GameObject slider = sliderParent.transform.Find(birdFriend.ToString()).gameObject;
			slider.SetActive(false);
			slider.GetComponent<Slider>().value = relationships[birdFriend];
			try
			{
				if (Helpers.Instance.GetBirdFromEnum(birdFriend, true).charName != null && Helpers.Instance.GetBirdFromEnum(birdFriend, true).health > 0)
					slider.SetActive(true);
			}
			catch { }

		}
		sliderParent.transform.Find(charName).gameObject.SetActive(false);

	}*/



	/*public void SetRelationshipText(GameObject portrait, Text RelationshipText)
	{

		relationshipBonus =GetRelationshipBonus();
		string relationshipText = "Likes " + Helpers.Instance.GetHexColor(preferredEmotion) + preferredEmotion.ToString() + "</color> birds. ";
		if(relationshipBonus == 0)
		{
			relationshipText += "Single.";
			portrait.transform.parent.gameObject.SetActive(false);
		}
		if (relationshipBonus > 0)
		{
			relationshipText += "In a relationship with " +relationshipBird.charName+".";
			portrait.transform.parent.gameObject.SetActive(true);
			portrait.transform.Find("bird_color").GetComponent<Image>().sprite = relationshipBird.portrait.transform.Find("bird_color").GetComponent<Image>().sprite;
			portrait.transform.Find("bird").GetComponent<Image>().sprite = relationshipBird.portrait.transform.Find("bird").GetComponent<Image>().sprite;
			portrait.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(relationshipBird.emotion);
		}
		if (relationshipBonus < 0)
		{
			relationshipText += "Has a crush on " + relationshipBird.charName + ".";
			portrait.transform.parent.gameObject.SetActive(true);
			portrait.transform.Find("bird_color").GetComponent<Image>().sprite = relationshipBird.portrait.transform.Find("bird_color").GetComponent<Image>().sprite;
			portrait.transform.Find("bird").GetComponent<Image>().sprite = relationshipBird.portrait.transform.Find("bird").GetComponent<Image>().sprite;
			portrait.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(relationshipBird.emotion);
		}
		RelationshipText.text = relationshipText;
	}*/


	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.S) && Var.cheatsEnabled)
		{
			if (Var.selectedBird == gameObject)
			{

				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Create(charName + "saveGame.dat");
				bf.Serialize(file, data);
				file.Close();
				Debug.Log("saved bird!");
			}
		}

		if (!isEnemy && !started)
		{
			try
			{
				OnMouseEnter();
				started = true;
			}
			catch { }
		}
		if (needsReset)
		{
			needsReset = false;
			Var.selectedBird = null;
			dragged = false;
			indicator.Hide();
			LeanTween.move(gameObject, new Vector3(target.x, target.y, 0), 0.5f).setEase(LeanTweenType.easeOutBack);

		}
		if (Input.GetMouseButtonUp(0) && dragged)
			needsReset = true;

		if (dragged)
		{
			LeanTween.move(gameObject, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), 0.02f);
		}
	}
	void drawLines()
	{
		if (!Var.freezeEmotions)
		{
			lines.DrawLines();
		}

		if (indicator)
		{
			Var.Em emo1 = (Helpers.Instance.GetAdjacentBirds(this).Count == 0 ? Var.Em.Solitary : Var.Em.Social);
			Var.Em emo2 = fighting ? Var.Em.Neutral : Var.Em.Cautious;
			if (emo1 == Var.Em.Social)
			{
				emo1 = Helpers.Instance.getFriendState(this) == Helpers.friendState.diagonal ? Var.Em.Neutral : Var.Em.Social;
			}
			//Debug.Log("draw lines setting emotion: emo1: " + emo1 + "emo2: " + emo2);
			indicator.SetEmotions(emo1, emo2);
		}
	}
	public void ReleaseBird(int x, int y)
	{

		Var.selectedBird = null;
		dragged = false;
		this.x = x;
		this.y = y;
		GetComponentInChildren<Animator>().SetBool("lift", false);
		foreach (SpriteRenderer child in transform.GetComponentsInChildren<SpriteRenderer>(true))
		{
			child.sortingLayerName = charName.Substring(0, 1).ToUpper() + charName.Substring(1);
		}
		indicator.ResetLayer();
		GameObject dustObj = Instantiate(Var.dustCloud, transform.Find("feet"));
		dustObj.transform.localPosition = Vector3.zero;
		Destroy(dustObj, 1.0f);
		if (!inMap)
		{			
			LeanTween.delayedCall(0.12f, drawLines);
			birdSounds.dropBird.Play();
			//lines.DrawLines();
			showText();
			try
			{
				levelControler.ApplyLevelOnDrop(this);
				foreach (Bird bird in FillPlayer.Instance.playerBirds)
				{
					if (bird.charName != charName)
					{
						bird.levelControler.ApplyLevelOnPickup(bird, bird.levelList);
						bird.levelControler.ApplyLevelOnDrop(bird);
					}
				}
				foreach (Bird bird in FillPlayer.Instance.playerBirds)
				{
					if (bird.x >= 0 && bird.y >= 0)
						ObstacleGenerator.Instance.tiles[bird.x + 4 * bird.y].ApplyPower(bird);

				}
				UpdateFeedback();
			}
			catch
			{
				UpdateFeedback();
			}
		}
		if (Var.isTutorial)
		{
			foreach (LayoutButton tile in ObstacleGenerator.Instance.tiles)
			{
				tile.baseColor = tile.defaultColor;
				LeanTween.color(tile.gameObject, tile.defaultColor, 0.3f);
			}
		}
		LeanTween.move(gameObject, new Vector3(target.x, target.y, 0), 0.5f).setEase(LeanTweenType.easeOutBack);
		SetCoolDownRing(false);
		if (inMap)
		{
			//MapControler.Instance.CanLoadBattle();
		}
	}
}