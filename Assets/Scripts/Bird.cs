using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
[Serializable]
public class Bird : MonoBehaviour
{
	public string birdBio;
	public string birdAbility;
	public int portraitOrder=0;
	public int confidence=0;
	[HideInInspector]
	public int prevConf = 0;
	[HideInInspector]
	public int prevFriend = 0;
	[HideInInspector]
	public int totalFriendliness = 0;
	[HideInInspector]
	public int totalConfidence = 0;
	public int friendliness = 0;
	public int health = 3;
	public int mentalHealth = 3;
	[HideInInspector]
	public bool GainedLVLHealth = false;
	[HideInInspector]
	public bool foughtInRound = false;
   // [HideInInspector]
	public int maxHealth = 3;
	[HideInInspector]
	public bool hadMentalPain = false;
	public int x = -1;
	public int y = -1;
	public Var.Em emotion;
	public Var.Em bannedLevels = Var.Em.finish;
	public string charName;	
	public bool inUse = true;	
	public SpriteRenderer colorRenderer;    
	[HideInInspector]
	public List<SpriteRenderer> colorSprites;
	public GameObject bush;    
	[HideInInspector]
	public GameObject portrait;	
	[HideInInspector] 
	public Vector3 target;
	public Vector3 home;
	[HideInInspector]
	public bool dragged = false;
	//[HideInInspector]
	public firendLine lines;
	public int level = 1;
	bool needsReset = false; 
	public enum dir { top,front,bottom};
	public dir position;
	public fillEnemy.enemyType enemyType = fillEnemy.enemyType.normal;	
	public bool isEnemy = true;
	[HideInInspector]
	public int friendBoost = 0;
	[HideInInspector]
	public int groundFriendBoos = 0;
	[HideInInspector]
	public int wizardFrienBoos = 0;
	[HideInInspector]
	public int levelFriendBoos = 0;
	[HideInInspector]
	public int battleConfBoos = 0;
	[HideInInspector]
	public int groundConfBoos = 0;
	[HideInInspector]
	public int wizardConfBoos = 0;
	[HideInInspector]
	public int levelConfBoos = 0;
	//[HideInInspector]
	public bool isInfluenced = false;
	//[HideInInspector]
	public int healthBoost = 0;
	int roundHealthChange = 0;
	//[HideInInspector]
	public int GroundRollBonus = 0;
	public bool inMap = false;
	[HideInInspector]
	public Sprite hatSprite;
	//public Levels.type startingLVL;
	[HideInInspector]
	public List<LevelData> levelList;
	[HideInInspector]
	public int confLoseOnRest = 2;
	public int groundMultiplier = 1;
	public GameObject healParticle;
	[HideInInspector]
	public Levels levelControler;
	[HideInInspector]
	public LevelData lastLevel;
	public int battleCount = 0;
	[HideInInspector]
	public bool fighting = false;
	[HideInInspector]
	public int battlesToNextLVL = 5;
	[HideInInspector]
	public int consecutiveFightsWon = 0;
	[HideInInspector]
	public int winsInOneFight = 0;
	[HideInInspector]
	public int ConfGainedInRound = 0;
	[HideInInspector]
	public int FriendGainedInRound = 0;
	// -1 = no fight, 0 = lost, 1 = won, 2 = won and lost
	[HideInInspector]
	public int wonLastBattle = -1;
	[HideInInspector]
	public int roundsRested = 0;
	//[HideInInspector]
	public int AdventuresRested = 0;
	public int TurnsInjured = 0;
	public int PlayerRollBonus = 0;
	public int CoolDownLeft= 3;
	public int CoolDownLength = 3;
	public Image CooldownRing;
	public bool isHiding = false;
	public int prevRoundHealth;
	public int prevRoundMentalHealth;
	public int levelRollBonus = 0;
	public int relationshipBonus = 0;
	[HideInInspector]
	public string levelUpText;
	Color DefaultCol;
	Color HighlightCol;
	public bool injured = false;
	public Var.Em preferredEmotion;
	public int birdIndex = 0;
	public bool hasNewLevel = false;
	//[HideInInspector]
	public Bird relationshipBird;
	[HideInInspector]
	public Var.Em prevEmotion=  Var.Em.finish;
	bool started = false;
	public Dictionary<EventScript.Character, int> relationships;
	[HideInInspector]
	public List<Dialogue> relationshipDialogs;
	[HideInInspector]
	public bool newRelationship = false;
	public string birdPrefabName;
	public GameObject EnemyArt = null;
	public GameObject GroundBonus;
	public GameObject RelationshipParticles;
	public GameObject CrushParticles;
	public GameObject mapHighlight;
    public List<string> recievedSeeds;
	GameObject birdArtObj;
	void Start()
	{

		wonLastBattle = -1;
		if (!isEnemy && portrait == null)
			portrait = Resources.Load<GameObject>("prefabs/portrait_" + charName);
	   /* if (!isEnemy && !inMap)
		{
			GuiContoler.Instance.ShowSpeechBubble(transform.Find("mouth").transform, "hi!");
			GuiContoler.Instance.ShowSpeechBubble(transform.Find("mouth").transform, "hi2!");
			GuiContoler.Instance.ShowSpeechBubble(transform.Find("mouth").transform, "hi3!");
		}*/
		
		prevRoundHealth = health;
		prevRoundMentalHealth = mentalHealth;
		x = -1;
		y = -1;
		prevConf = confidence;
		prevFriend = friendliness;
	  
 
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
	   
			if (levelList.Count == 0 && !Var.isTutorial)
			{
				levelList = new List<LevelData>();
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
			CooldownRing.fillAmount = (float)(CoolDownLength - CoolDownLeft) / (float)CoolDownLength;
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
		}
		target = transform.position;
		if (!isEnemy && (injured || health<=0))
		{
			injured = true;
			Animator anim = GetComponentInChildren<Animator>();
			anim.SetBool("injured", true);
			if (inMap)
			{
				mapHighlight.SetActive(false);
				MapControler.Instance.selectedBirds.Remove(this);
				MapControler.Instance.CanLoadBattle();
			}
		}

		if (inMap)
			GetComponentInChildren<Animator>().SetBool("rest", true);

        if (recievedSeeds == null)
            recievedSeeds = new List<string>();
        showText();

	}

	public void publicStart()
	{
		Start();
	}
	public void DecreaseTurnsInjured()
	{
		if (injured)
		{
			TurnsInjured--;
			if (TurnsInjured<=0)
			{
				injured = false;
				GetComponentInChildren<Animator>().SetBool("injured", false);
				health = 3;
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
		GuiContoler.Instance.ShowSpeechBubble(GetMouthTransform(), text);
	}

	public Transform GetMouthTransform()
	{
		Transform mouth = transform;
		try
		{
			birdArtObj.transform.Find("neutral").SetAsLastSibling();
		
		for (int i = 0; i < birdArtObj.transform.childCount; i++)
		{
			if (birdArtObj.transform.GetChild(i).gameObject.activeSelf)
			{
				mouth = birdArtObj.transform.GetChild(i).Find("mouth");
				print(charName + " has emotion " + birdArtObj.transform.GetChild(i).name);
				break;
			}
		}
		}
		catch {
			mouth = transform;
		}
		return mouth;
	}

	public void SetCoolDownRing(bool active)
	{
		if (CooldownRing != null)
			if (Helpers.Instance.ListContainsLevel(Levels.type.Brave1, levelList))
			{
				CooldownRing.color = Helpers.Instance.GetEmotionColor(Var.Em.Confident);
				CooldownRing.gameObject.SetActive(active);
			}else
			{
				if(Helpers.Instance.ListContainsLevel(Levels.type.Lonely2, levelList))
				{
					CooldownRing.color = Helpers.Instance.GetEmotionColor(Var.Em.Solitary);
					CooldownRing.gameObject.SetActive(active);
				}
				else
				{
					CooldownRing.gameObject.SetActive(false);
				}
				
			}
	}
	public void AddLevel(LevelData data)
	{
		if (data.emotion != Var.Em.Neutral)
		{
			hasNewLevel = true;            
			Helpers.Instance.EmitEmotionParticles(transform, Var.Em.finish);
			Helpers.Instance.EmitEmotionParticles(transform, data.emotion, false);
		}
		lastLevel = data;
		List<Var.Em> currentEmotions = new List<Var.Em>();
		foreach (LevelData lvlData in levelList)
		{
			if (!currentEmotions.Contains(lvlData.emotion))
				currentEmotions.Add(lvlData.emotion);

		}
		if (data.emotion != Var.Em.Neutral)
		{
			if (!currentEmotions.Contains(data.emotion))
			{				
				GainedLVLHealth = true;
			}
			levelRollBonus++;
			//Reset Emotions
			prevConf = confidence;
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
			}
			ResetBonuses();
		}
		levelList.Add(data);
		level = levelList.Count+1;       
		battlesToNextLVL = level * 3;
		levelUpText = null;
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
		return levelRollBonus + PlayerRollBonus + GroundRollBonus;// + relationshipBonus;
	}

	public string GetBonusText()
	{
		string bonusText = "";
		if (levelRollBonus != 0)
			bonusText += "\nFrom levels: " + (levelRollBonus * 10).ToString("+#;-#;0");
		if(PlayerRollBonus !=0)
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
		if (injured)
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


		if (!SaveDataCreated && (Var.activeBirds.Count<=3 || inMap))
		{
			Var.activeBirds.Add(this);
		   // print("created something!");       
		}
		else
		{
			
			confidence = savedData.confidence;
			friendliness = savedData.friendliness;
		}
	}
	
	public void ResetBonuses()
	{		
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
		isInfluenced = false;
	}
	public void TryLevelUp()
	{
		if (injured)
			return;
	}
	public void ResetAfterLevel()
	{
		if (injured)
			return;
		winsInOneFight = 0;
		wonLastBattle = -1;
		x = -1;
		y = -1;
		try
		{
			levelControler.ApplyLevelOnPickup(this, levelList);
			if (Helpers.Instance.ListContainsLevel(Levels.type.Sophie, levelList))
				levelControler.Halo.SetActive(false);
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

		//if(!Var.Infight)
		if(GuiContoler.Instance.speechBubbleObj.activeSelf)
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
			foreach (SpriteRenderer sp in colorSprites)
				sp.color = HighlightCol;			
			if (!dragged)
				AudioControler.Instance.PlaySoundWithPitch(AudioControler.Instance.mouseOverBird);
		}
		//if(inMap)
			//ProgressGUI.Instance.PortraitClick(this);
		if (!inMap && !isEnemy && levelUpText != null && levelUpText != "" && !dragged && !Var.Infight && Var.CanShowHover)
			GuiContoler.Instance.ShowLvlText(levelUpText);
	}
	void OnMouseOver()
	{
		
		if (Var.Infight)
			return;
		if (isEnemy || GuiContoler.Instance.speechBubbleObj.activeSelf)
			return;
		if (GuiContoler.Instance.speechBubbleObj.activeSelf)
			showText();
		SetCoolDownRing(true);
	  
		if (Input.GetMouseButtonUp(1))
		{			
			GuiContoler.Instance.GraphButton();
		}
		if (Input.GetMouseButtonUp(0))
		{
			foreach (SpriteRenderer child in transform.GetComponentsInChildren<SpriteRenderer>(true))
			{
				child.sortingLayerName = "Default";
			}
			GetComponentInChildren<Animator>().SetBool("lift", false);
			if (Var.isTutorial)
			{
				foreach (LayoutButton tile in ObstacleGenerator.Instance.tiles)
				{
					tile.baseColor = tile.defaultColor;
					LeanTween.color(tile.gameObject, tile.defaultColor, 0.3f);
				}
			}
		}
		if (Input.GetMouseButtonUp(2))
		{
			if(!inMap && Helpers.Instance.ListContainsLevel(Levels.type.Scared2, levelList))
			{
				bush.SetActive(!bush.activeSelf);
				isHiding = bush.activeSelf;
				GameLogic.Instance.CanWeFight();
				GameLogic.Instance.UpdateFeedback();
				
			}
		}
		if (Input.GetMouseButtonDown(0))
		{
			if (Var.Infight || injured || GuiContoler.Instance.speechBubbleObj.activeSelf)
				return;
			if (inMap)
			{
				if (MapControler.Instance.selectedBirds.Contains(this))
				{
					mapHighlight.SetActive(false);
					GetComponentInChildren<Animator>().SetBool("rest", true);
					MapControler.Instance.selectedBirds.Remove(this);
				}else
				{
					mapHighlight.SetActive(true);
					GetComponentInChildren<Animator>().SetBool("rest",false);
					MapControler.Instance.selectedBirds.Add(this);
				}
				MapControler.Instance.CanLoadBattle();
				return;
			}
			AudioControler.Instance.PlaySoundWithPitch(AudioControler.Instance.pickupBird);
			GetComponentInChildren<Animator>().SetBool("lift", true);
			foreach (SpriteRenderer child in transform.GetComponentsInChildren<SpriteRenderer>(true))
			{
				child.sortingLayerName = "Front";
			}

			if (inMap)
			{
				if (MapControler.Instance.canHeal)
				{
					ChageHealth(maxHealth);
					MapControler.Instance.canHeal = false;
					MapControler.Instance.title.text = "Adventure map";
					showText();
					return;
				}
			}
			for(int i = 0; i < Var.playerPos.GetLength(0); i++)
			{
				for(int j =0; j< Var.playerPos.GetLength(1); j++)
				{

					if(Var.playerPos[i,j] == this)
					{						
						Var.playerPos[i, j] = null;                     
						break;
					}
				}
			}
			
			dragged = true;
			Var.selectedBird = gameObject;
			
			if (!inMap)
			{
				if (!Var.Infight)
					ResetBonuses();
				lines.RemoveLines();				
				GuiContoler.Instance.HideLvlText();
				GroundBonus.SetActive(false);
				try
				{
					foreach (Bird bird in FillPlayer.Instance.playerBirds)
					{
						if (bird.charName != charName)
						{
							bird.levelControler.ApplyLevelOnPickup(bird, bird.levelList);
							bird.levelControler.ApplyLevelOnDrop(bird, bird.levelList);
						}			
					}
				}
				catch {
					levelControler.ApplyLevelOnPickup(this, levelList);
				}
				levelControler.ApplyLevelOnPickup(this, levelList);
				UpdateFeedback();
			}
		}	
	}
	void OnMouseExit()
	{

		if (!inMap && !isEnemy)
		{
			GuiContoler.Instance.HideLvlText();
			
		}

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
	}
	void UpdateFeedback()
	{
		fighting = false;
		if (Helpers.Instance.ListContainsLevel(Levels.type.Sophie,levelList) )
		{
			if (GameLogic.Instance.CheckIfResting(this)&&!dragged)
			{
				levelControler.ApplyLevelOnDrop(this, levelList);
			}
			else
			{
				levelControler.ApplyLevelOnPickup(this, levelList);
			}
		}       
		GameLogic.Instance.UpdateFeedback();
		
	}
	public Bird(string name,int confidence =0,int friendliness = 0)
	{
		this.confidence = confidence;
		this.friendliness = friendliness;
		this.charName = name;
		//SetEmotion();
		//Debug.Log(ToString());
	}

	public void OnLevelPickup()
	{
	   levelControler.ApplyLevelOnPickup(this, levelList);
	}

	public void ChageHealth(int change)
	{
		try
		{
			if (injured)
				return;
			if (health + roundHealthChange <= 0)
				return;
			if (change > 0)
			{
				if (health != maxHealth)
				{
					GameObject healObj = Instantiate(healParticle, transform);
					Destroy(healObj, 1.5f);
				}
			}
			else
			{
				if (Helpers.Instance.ListContainsLevel(Levels.type.Brave2, levelList) && (emotion == Var.Em.Confident || emotion == Var.Em.SuperConfident))
				{
					levelConfBoos -= 2;
					GameObject shield = Resources.Load("shieldEffect") as GameObject;
					Instantiate(shield, transform);

				}
				else
				{
					try
					{
						List<Bird> birds = Helpers.Instance.GetAdjacentBirds(this);
						if (birds != null && birds.Count > 0)
						{
							foreach (Bird bird in birds)
							{
								if (Helpers.Instance.ListContainsLevel(Levels.type.Brave1, bird.levelList) && bird.CoolDownLeft == 0)
								{
									bird.CoolDownLeft = bird.CoolDownLength;
									change = 0;
									GameObject shield = Resources.Load("shieldEffect") as GameObject;
									Instantiate(shield, transform);
								}
							}
						}
					}
					catch { }
				}
			}

			roundHealthChange += change;
			Debug.Log(charName + " health change " + change);
			if (GuiContoler.Instance.selectedBird == this)
			{
				showText();
			}
			if (health + roundHealthChange <= 0)
			{
				injured = true;
				battleConfBoos -= 5;
				GetComponentInChildren<Animator>().SetBool("injured", true);
				TurnsInjured = 4;
			}
		}
		catch(Exception ex)
		{
			print("Error in add health: " + ex.Message);
		}
		
	}

	public override string ToString()
	{
		if (enabled && !isEnemy)
		{
			return '\u2022'+"friendly: " + friendliness +" "+ '\u2022' + "brave: " + confidence + 
				"\n"+ '\u2022' + "level: " + level+ " "+'\u2022' + "health: " +health +"\n" + birdAbility;
		}else
		{
			return null;
		}

	}

	public string GetHeading()
	{
		if (enabled)
		{
			return charName + "\n" +emotion.ToString();
		}
		else
		{
			return null;
		}
	}
	void setRelationshipDialogs()
	{
		if (relationshipBird == null)
		{
			relationshipDialogs = null;
		}else
		{
		   Transform relationshipTransform= Helpers.Instance.relationshipDialogs.Find(charName).transform.Find(relationshipBird.charName);
		   relationshipDialogs = new List<Dialogue>(relationshipTransform.GetComponentsInChildren<Dialogue>());
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
				if(shouldApply)
					levelFriendBoos += 2;
				toReturn.y = 2;
				print(charName + " got influenced to friendly");
			}
			else
			{
				if (friendBoost + wizardFrienBoos + groundFriendBoos + levelFriendBoos < 0)
				{
					levelFriendBoos -= 2;
					toReturn.y = -2;
					print(charName + " got influenced to lonely");
				}
			}
			if (battleConfBoos + groundConfBoos + wizardConfBoos + levelConfBoos > 0)
			{
				if (shouldApply)
					levelConfBoos += 2;
				toReturn.x = 2;
				print(charName + " got influenced to confident");

			}
			else
			{
				if (battleConfBoos + groundConfBoos + wizardConfBoos + levelConfBoos < 0)
				{
					if (shouldApply)
						levelConfBoos -= 2;
					toReturn.x = -2;
					print(charName + " got influenced to scared");
				}
			}
		}
		return toReturn;
	}
	public void AddRoundBonuses(bool doFightStuff= true)
	{
		print(charName + " doing round bonus. HealthGain " + roundHealthChange);
		if (injured)
			return;
		ApplyInfluence();

		prevRoundHealth = health;
		prevRoundMentalHealth = mentalHealth;
		


		if (doFightStuff)
		{
			
			/*RelationshipScript.applyRelationship(this);
			relationshipBonus = GetRelationshipBonus();
			SetRealtionshipParticles();
			setRelationshipDialogs();*/
			if (!foughtInRound)
			{
				consecutiveFightsWon = 0;
				roundsRested++;				
				battleConfBoos -= 2;
			}
			else
			{
				roundsRested = 0;
			}
			if (CoolDownLeft > 0)
				CoolDownLeft--;
			try
			{
				CooldownRing.fillAmount = (float)(CoolDownLength - CoolDownLeft) / (float)CoolDownLength;
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
		if (!hasNewLevel)
		{
			friendliness += friendBoost + wizardFrienBoos + groundFriendBoos + levelFriendBoos;
			confidence += battleConfBoos + groundConfBoos + wizardConfBoos + levelConfBoos;
			ConfGainedInRound = confidence - prevConf;
			FriendGainedInRound = friendliness - prevFriend;
		}
		health = Mathf.Min(health + healthBoost + roundHealthChange, maxHealth);
		//Mental health
		if ((Mathf.Abs(confidence) >= 12 || Mathf.Abs(friendliness) >= 12) && (Mathf.Abs(prevConf) >= 12 || Mathf.Abs(prevFriend) >= 12))
		{//In danger zone
			mentalHealth = Math.Max(mentalHealth - 1, 0);
			if (mentalHealth == 0)
			{
				//dont kill the player
				if (health > 1)
				{//Effects from having no mental health left
					//mentalHealth = Var.maxMentalHealth;					
					hadMentalPain = true;
					health--;
				}

				mentalHealth = Var.maxMentalHealth;
			}

		}
		else if(Mathf.Abs(confidence)< 12 && Mathf.Abs(friendliness) < 12 && (Mathf.Abs(prevConf) < 12 && Mathf.Abs(prevFriend) <12))
		{//In comfort zone
			mentalHealth = Mathf.Min(Var.maxMentalHealth, mentalHealth + 1);
		}


		print(charName + " healthboost " + healthBoost + " round change " + roundHealthChange);			 
		roundHealthChange = 0;
		foughtInRound = false;        
		
		Helpers.Instance.NormalizeStats(this);        
		if (this == GuiContoler.Instance.selectedBird)
			showText();    
	}

	public void SetEmotion()
	{
		float factor = 0.13f;
		float transitionTime = 1.9f;
		if(!isEnemy)
			print(charName + " applied emotion");
		if (prevEmotion.Equals( Var.Em.finish)|| isEnemy)
		{
			transitionTime = 0.0f;
		}
		prevEmotion = emotion;
		if (Mathf.Abs((float)confidence)<Var.lvl1 && Mathf.Abs((float)friendliness) < Var.lvl1)
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
			return;
		}
		
		if (Mathf.Abs((float)confidence) > Mathf.Abs((float)friendliness))
		{
			if (confidence > 0)
			{
				
				//Confident
				if (confidence >= Var.lvl1)
					emotion = Var.Em.Confident;
			
			}
			else
			{
				//Scared
			   if (confidence <= -Var.lvl1)
					emotion = Var.Em.Cautious;				
			}

		}
		else
		{
			//Friendly or lonely
			if (friendliness > 0)
			{

				//friendly				
				if (friendliness >= Var.lvl1)
					emotion = Var.Em.Social;				
			}
			else
			{
				//Lonely				
				if (friendliness <= -Var.lvl1)
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
			GuiContoler.Instance.smallGraph.PlotFull(this);
			
			GuiContoler.Instance.selectedBird = this;
			//SetRelationshipSliders(GuiContoler.Instance.relationshipSliders);
			Var.birdInfoFeeling.text = emotion.ToString();
			Var.birdInfoFeeling.color = Helpers.Instance.GetEmotionColor(emotion);            
			Var.birdInfoHeading.text = Helpers.Instance.ApplyTitle(this, lastLevel.title);              
			GuiContoler.Instance.PortraitControl(portraitOrder, emotion);
			GuiContoler.Instance.BirdCombatStr.text = "Combat strength: " + (getBonus()* 10f).ToString("+#;-#;0") + "%";
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
			GuiContoler.Instance.levelNumberText.text = level.ToString();
			//Set Relationship bars
				

			///Set level icons
			foreach (LVLIconScript icon in GuiContoler.Instance.lvlIcons)
			{
				
				if (levelList.Count > index && !Var.isTutorial)
				{
					icon.gameObject.SetActive(true);
					icon.GetComponent<Image>().sprite = levelList[index].LVLIcon;
					icon.textToDsiplay = levelList[index].levelInfo;
				}
				else
				{
					icon.gameObject.SetActive(false);
				}
				index++;
			}
			
			//set emotion bars
			GuiContoler.Instance.confSlider.SetDist(confidence + battleConfBoos, this);
			GuiContoler.Instance.firendSlider.SetDist(friendliness, this);
			//set hearts
			//SetRelationshipText(GuiContoler.Instance.relationshipPortrait, GuiContoler.Instance.relationshipText);			
			try
			{
				Helpers.Instance.setHearts(GuiContoler.Instance.BirdInfoHearts, health+roundHealthChange, maxHealth);
				Helpers.Instance.setHearts(GuiContoler.Instance.BirdMentalHearts, mentalHealth, Var.maxMentalHealth,-1,true);

			}
			catch {
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
			LeanTween.move(gameObject, new Vector3(target.x, target.y, 0), 0.5f).setEase(LeanTweenType.easeOutBack);
		}
		if (Input.GetMouseButtonUp(0) &&dragged)
			needsReset = true;        
			
		if (dragged)
		{
			LeanTween.move(gameObject, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), 0.02f);
		}
		

	}
	void drawLines()
	{
		lines.DrawLines();
	}
	public void ReleaseBird(int x, int y)
	{
		
		Var.selectedBird = null;
		dragged = false;
		this.x = x;
		this.y = y;
		AudioControler.Instance.PlaySoundWithPitch(AudioControler.Instance.dropBird);
		GetComponentInChildren<Animator>().SetBool("lift", false);
		foreach (SpriteRenderer child in transform.GetComponentsInChildren<SpriteRenderer>(true))
		{
			child.sortingLayerName = "Default";
		}
		GameObject dustObj = Instantiate(Var.dustCloud, transform.Find("feet"));
		dustObj.transform.localPosition = Vector3.zero;
		Destroy(dustObj, 1.0f);
		if (!inMap)
		{
			LeanTween.delayedCall(0.15f, drawLines);
			//lines.DrawLines();
			showText();
			try
			{
				levelControler.ApplyLevelOnDrop(this, levelList);
				foreach (Bird bird in FillPlayer.Instance.playerBirds)
				{
					if (bird.charName != charName)
					{
						bird.levelControler.ApplyLevelOnPickup(bird, bird.levelList);
						bird.levelControler.ApplyLevelOnDrop(bird, bird.levelList);
					}
				}
				foreach (Bird bird in FillPlayer.Instance.playerBirds)
				{
					if (bird.x >= 0 && bird.y >= 0)
						ObstacleGenerator.Instance.tiles[bird.x + 4 * bird.y].ApplyPower(bird);

				}
				UpdateFeedback();
			}
			catch {
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
		if(inMap)
		{
			MapControler.Instance.CanLoadBattle();
		}


	}
	


}
