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
	public int friendliness = 0;
	public int health = 3;
	[HideInInspector]
	public bool foughtInRound = false;
   // [HideInInspector]
	public int maxHealth = 3;
	public int x = -1;
	public int y = -1;
	public Var.Em emotion;
	public string charName;	
	public bool inUse = true;	
	public SpriteRenderer colorRenderer;    
	List<SpriteRenderer> colorSprites;
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
	public bool isEnemy = true;
	[HideInInspector]
	public int friendBoost = 0;
	[HideInInspector]
	public int confBoos = 0;
	//[HideInInspector]
	public int healthBoost = 0;
	int roundHealthChange = 0;
	[HideInInspector]
	public int GroundRollBonus = 0;
	public bool inMap = false;
	[HideInInspector]
	public Sprite hatSprite;
	public Levels.type startingLVL;
	[HideInInspector]
	public List<LevelData> levelList;
	[HideInInspector]
	public int confLoseOnRest = 1;
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
	public int PlayerRollBonus = 0;
	public int CoolDownLeft= 3;
	public int CoolDownLength = 3;
	public Image CooldownRing;
	public bool isHiding = false;
	public int prevRoundHealth;
	public int levelRollBonus = 0;
	public int relationshipBonus = 0;
	[HideInInspector]
	public string levelUpText;
	Color DefaultCol;
	Color HighlightCol;
	public bool dead = false;
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
	[HideInInspector]
	public GameObject EnemyArt = null;
	public GameObject GroundBonus;
    public GameObject RelationshipParticles;
    public GameObject CrushParticles;
	void Start()
	{
		if (!isEnemy && portrait == null)
			portrait = Resources.Load<GameObject>("prefabs/portrait_" + charName);
	   /* if (!isEnemy && !inMap)
		{
			GuiContoler.Instance.ShowSpeechBubble(transform.Find("mouth").transform, "hi!");
			GuiContoler.Instance.ShowSpeechBubble(transform.Find("mouth").transform, "hi2!");
			GuiContoler.Instance.ShowSpeechBubble(transform.Find("mouth").transform, "hi3!");
		}*/
		
		prevRoundHealth = health;
		x = -1;
		y = -1;
		prevConf = confidence;
		prevFriend = friendliness;
	  
 
		if (!isEnemy)
		{

			if (relationships == null)
			{
				relationships = new Dictionary<EventScript.Character, int>();
				relationships.Add(EventScript.Character.Kim, 0);
				relationships.Add(EventScript.Character.Terry, 0);
				relationships.Add(EventScript.Character.Toby, 0);
				relationships.Add(EventScript.Character.Tova, 0);
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
            SetRealtionshipParticles();
            var BirdArt = Resources.Load("prefabs/" + birdPrefabName);
			GameObject birdArtObj = Instantiate(BirdArt, transform) as GameObject;
			birdArtObj.transform.localPosition = new Vector3(0.23f, -0.3f, 0);
       
            if (levelList.Count == 0)
			{
				levelList = new List<LevelData>();
				Sprite icon = Helpers.Instance.GetLVLSprite(startingLVL);               
				AddLevel(new LevelData(startingLVL, Var.Em.Neutral,icon));
			}
			levelControler = GetComponent<Levels>();
			levelControler.ApplyStartLevel(this, levelList);       
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
		SetCoolDownRing(false);
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
		prevEmotion = Var.Em.finish;

        if (dead)
        {
            Animator anim = GetComponentInChildren<Animator>();
            anim.SetBool("dead", true);
        }
    }

	public void publicStart()
	{
		Start();
	}
    void SetRealtionshipParticles()
    {
        if (inMap)
            return;
        RelationshipParticles.SetActive(relationshipBonus > 0);
        CrushParticles.SetActive(relationshipBonus < 0);

    }
	
	public void Speak(string text)
	{
		GuiContoler.Instance.ShowSpeechBubble(transform.Find("mouth").transform, text);
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
					CooldownRing.color = Helpers.Instance.GetEmotionColor(Var.Em.Lonely);
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
		levelList.Add(data);
		level = levelList.Count;       
		battlesToNextLVL = level * 3;
		levelUpText = null;
		if (data.emotion == Var.Em.Scared || data.emotion == Var.Em.Lonely)
			levelRollBonus++;
		if(data.emotion == Var.Em.Confident || data.emotion == Var.Em.Friendly)
		{
			maxHealth++;
			health++;
		}

	}
	public float getBonus()
	{
		if (y != -1)
		{
			ResetBonuses();
			ObstacleGenerator.Instance.tiles[y * 4 + x].GetComponent<LayoutButton>().ApplyPower(this);
		}
		relationshipBonus = GetRelationshipBonus();
		return levelRollBonus + PlayerRollBonus + GroundRollBonus + relationshipBonus;
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
        if (relationshipBonus != 0)
            bonusText += "\nFrom relationships: " + (relationshipBonus * 10).ToString("+#;-#;0");
        if (bonusText != "")
            bonusText = bonusText.Substring(1);
        return bonusText;

    }


	public int GetRelationshipBonus()
	{
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
		/*
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
		return 0;*/
	}
	void LoadStats()
	{
		if (dead)
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
		confBoos = 0;
		friendBoost = 0;
		GroundRollBonus = 0;
		PlayerRollBonus = 0;
		healthBoost = 0;
	}
	public void UpdateBattleCount()
	{
		if (dead)
			return;
		
		if (battleCount >= battlesToNextLVL)
		{
			CheckLevels();
		}
		if(!Var.isTutorial)
			battleCount++;
		//Reset per battle level variables

	}
	public void ResetAfterLevel()
	{
		if (dead)
			return;
		winsInOneFight = 0;
		wonLastBattle = -1;
		x = -1;
		y = -1;
		levelControler.ApplyLevelOnPickup(this, levelList);
		if (Helpers.Instance.ListContainsLevel(Levels.type.Tova, levelList))       
			levelControler.Halo.SetActive(false);
		showText();
	}
	public string CheckLevels(bool toApply = true)
	{
		//TODO: make this look nice
		string st = "";
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
	}
	void OnMouseEnter()
	{
	   
	   
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
		if(inMap)
			ProgressGUI.Instance.PortraitClick(this);
		if (!inMap && !isEnemy && levelUpText != null && levelUpText != "" && !dragged)
			GuiContoler.Instance.ShowLvlText(levelUpText);
	}
	void OnMouseOver()
	{
		if (GuiContoler.Instance.speechBubbleObj.activeSelf)
			return;
		if (Var.Infight)
			return;
		if (isEnemy)
			return;
		SetCoolDownRing(true);
	  
		if (Input.GetMouseButtonUp(1))
		{
			if(!inMap && Helpers.Instance.ListContainsLevel(Levels.type.Scared2, levelList))
			{
				bush.SetActive(!bush.activeSelf);
				isHiding = bush.activeSelf;
				GameLogic.Instance.CanWeFight();
				GameLogic.Instance.UpdateFeedback();
				
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
            foreach (SpriteRenderer child in transform.GetComponentsInChildren<SpriteRenderer>(true))
            {
                child.sortingLayerName = "Default";
            }
            GetComponentInChildren<Animator>().SetBool("lift", false);
		}
		if (Input.GetMouseButtonDown(0))
		{
			if (Var.Infight || dead)
				return;
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
			ResetBonuses();
			dragged = true;
			Var.selectedBird = gameObject;
			
			if (!inMap)
			{                
				lines.RemoveLines();
				UpdateFeedback();
				GuiContoler.Instance.HideLvlText();
				GroundBonus.SetActive(false);               
            }
			levelControler.ApplyLevelOnPickup(this, levelList);
			// RemoveAllFeedBack();
		}
		// 1 frame delay		
	}
	void OnMouseExit()
	{
		
		if (!inMap && !isEnemy)
			GuiContoler.Instance.HideLvlText();
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
		if (Helpers.Instance.ListContainsLevel(Levels.type.Tova,levelList))
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
		if (dead)
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
		}else
		{
			if (Helpers.Instance.ListContainsLevel(Levels.type.Brave2, levelList) && (emotion == Var.Em.Confident || emotion == Var.Em.SuperConfident))
			{
				confBoos = confBoos - 2;
				GameObject shield = Resources.Load("shieldEffect") as GameObject;
				Instantiate(shield, transform);

			}
			else
			{
				try
				{
					List<Bird> birds = Helpers.Instance.GetAdjacentBirds(this);
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
				catch { }
			}
		}
	   
		roundHealthChange+= change;
        Debug.Log(charName + " health change " + change);

        if (health+ roundHealthChange <= 0)
		{
            dead = true;
            GetComponentInChildren<Animator>().SetBool("dead", true);
            	
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
	public void AddRoundBonuses(bool doFightStuff= true)
	{
		//print(charName + " doing round bonus. HealthGain " + roundHealthChange);
		if (dead)
			return;
		prevRoundHealth = health;
		friendliness += friendBoost;
		confidence += confBoos;
		if (doFightStuff)
		{
            
            RelationshipScript.applyRelationship(this);
            relationshipBonus = GetRelationshipBonus();
            SetRealtionshipParticles();
            setRelationshipDialogs();
			if (!foughtInRound)
			{
				consecutiveFightsWon = 0;
				roundsRested++;
				confidence = confidence - confLoseOnRest;
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
		}
		
		health = Mathf.Min(health + healthBoost + roundHealthChange, maxHealth);
        print(charName + " healthboost " + healthBoost + " round change " + roundHealthChange);			 
		roundHealthChange = 0;
		foughtInRound = false;        
		ConfGainedInRound = confidence - prevConf;
		FriendGainedInRound = friendliness - prevFriend;
		Helpers.Instance.NormalizeStats(this);
		levelControler.OnfightEndLevel(this, levelList);           
		ResetBonuses();                
	}

	public void SetEmotion()
	{
		float factor = 0.13f;
		float transitionTime = 1.9f;
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
					emotion = Var.Em.Scared;				
			}

		}
		else
		{
			//Friendly or lonely
			if (friendliness > 0)
			{

				//friendly				
				if (friendliness >= Var.lvl1)
					emotion = Var.Em.Friendly;				
			}
			else
			{
				//Lonely				
				if (friendliness <= -Var.lvl1)
					emotion = Var.Em.Lonely;				
			}

		}
		try
		{
			foreach (SpriteRenderer sp in colorSprites)
				LeanTween.color(sp.gameObject, Helpers.Instance.GetEmotionColor(emotion), transitionTime);
		}
		catch { }        
		DefaultCol = Helpers.Instance.GetEmotionColor(emotion);        
		HighlightCol = new Color(DefaultCol.r + factor, DefaultCol.g +factor, DefaultCol.b + factor);
	}
	
	public void showText()
	{
        if (ToString() != null)
        {
            //Var.birdInfo.text = ToString();           
            GuiContoler.Instance.selectedBird = this;
            SetRelationshipSliders(GuiContoler.Instance.relationshipSliders);
            Var.birdInfoFeeling.text = emotion.ToString();
            Var.birdInfoFeeling.color = Helpers.Instance.GetEmotionColor(emotion);
            if (!inMap)
            {
                Var.birdInfoHeading.text = Helpers.Instance.ApplyTitle(this, lastLevel.title);              
                GuiContoler.Instance.PortraitControl(portraitOrder, emotion);
                GuiContoler.Instance.BirdCombatStr.text = "Combat strength: " + (getBonus()* 10f).ToString("+#;-#;0") + "%";
                GuiContoler.Instance.BirdCombatStr.gameObject.GetComponent<ShowTooltip>().tooltipText = GetBonusText();
                //set progress to level bar
                if (battleCount >= battlesToNextLVL)
                {
                    battleCount = battlesToNextLVL;
                    Var.powerText.text = "Ready to level up!";
                    levelUpText = CheckLevels(false);
                    Var.powerBar.color = Helpers.Instance.GetSoftEmotionColor(emotion);
                    Var.powerBar.fillAmount = 1;
                }
                else
                {
                    Var.powerBar.color = Color.white;
                    Var.powerText.text = null;
                    Var.powerText.text = "Leveling available in " + (battlesToNextLVL - battleCount) + " battles!";
                    Var.powerBar.fillAmount = (float)battleCount % 3 / (float)3;
                }

                int index = 0;
                GuiContoler.Instance.levelNumberText.text = level.ToString();
                //Set Relationship bars
                

                ///Set level icons
                foreach (LVLIconScript icon in GuiContoler.Instance.lvlIcons)
                {
                    if (levelList.Count > index)
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
            }
            //set emotion bars
            GuiContoler.Instance.confSlider.SetDist(confidence, this);
            GuiContoler.Instance.firendSlider.SetDist(friendliness, this);
            //set hearts
            SetRelationshipText(GuiContoler.Instance.relationshipPortrait, GuiContoler.Instance.relationshipText);
            if (!inMap)
            {
                try
                {
                    Helpers.Instance.setHearts(GuiContoler.Instance.BirdInfoHearts, health, maxHealth);

                }
                catch { }

            }
        }
	}

    public void SetRelationshipSliders(GameObject sliderParent)
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

    }



	public void SetRelationshipText(GameObject portrait, Text RelationshipText)
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
	}
	
	
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

	public void ReleseBird(int x, int y)
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
        if (!inMap)
		{
			lines.DrawLines(x, y);
			//Debug.Log("x: " + x+ " y: " + y);
			levelControler.ApplyLevelOnDrop(this, levelList);
            showText();
			UpdateFeedback();          

		}
		LeanTween.move(gameObject, new Vector3(target.x, target.y, 0), 0.5f).setEase(LeanTweenType.easeOutBack);
		SetCoolDownRing(false);
		if(inMap)
		{
			MapControler.Instance.CanLoadBattle();
		}


	}
	


}
