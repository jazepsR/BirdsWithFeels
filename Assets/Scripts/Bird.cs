using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
	public Image src;    
	public bool inUse = true;
	public GameObject birdPrefab;
	public SpriteRenderer colorRenderer;
    public GameObject bush;
	public GameObject portrait;
	public Image portraitColor;  
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
    [HideInInspector]
    public int healthBoost = 0;
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
    Levels levelControler;
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
    public Sprite startingIcon;
    string levelUpText;
    Color DefaultCol;
    Color HighlightCol;
    public bool dead = false;
    public int birdIndex = 0;
    public bool hasNewLevel = false;
    void Start()
	{
        prevRoundHealth = health;
        x = -1;
        y = -1;
        prevConf = confidence;
        prevFriend = friendliness;
        maxHealth = 3;
        if (!isEnemy)
        {            
            hatSprite = transform.Find("BIRB_sprite/hat").GetComponent<SpriteRenderer>().sprite;
            if (levelList == null)
            {
                levelList = new List<LevelData>();
                AddLevel(new LevelData(startingLVL, Var.Em.Neutral,startingIcon));
            }
            levelControler = GetComponent<Levels>();
            levelControler.ApplyStartLevel(this, levelList);       
        }
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
		SetEmotion();
        if(!isEnemy && !inMap)
            showText();
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
        if(data.emotion != Var.Em.Neutral)
            hasNewLevel = true;
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
        int superBonus = 0;
        if (Helpers.Instance.IsSuper(emotion))
            superBonus = 1;
        return levelRollBonus + PlayerRollBonus + GroundRollBonus + superBonus;
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
            print("created something!");       
        }
        else
        {
            
            confidence = savedData.confidence;
            friendliness = savedData.friendliness;
        }
    }
    
    void ResetBonuses()
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
        }else
        {
            colorRenderer.color = HighlightCol;
        }
        if(inMap)
            ProgressGUI.Instance.PortraitClick(this);
        if (!inMap && !isEnemy && levelUpText != null && !dragged)
            GuiContoler.Instance.ShowLvlText(levelUpText);
    }
	void OnMouseOver()
	{
        
        
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
        if (Input.GetMouseButtonDown(0))
		{
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
            colorRenderer.color = DefaultCol;
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
		SetEmotion();
		Debug.Log(ToString());
	}

    public void OnLevelPickup()
    {
       levelControler.ApplyLevelOnPickup(this, levelList);
    }

    public void ChageHealth(int change)
    {
        if (change > 0)
        {
            GameObject healObj = Instantiate(healParticle, transform);
            Destroy(healObj, 0.8f);
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
        }
        if (health + change > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health = health + change;
        }
        if (health <= 0)
        {
            GuiContoler.Instance.ShowDeathMenu(this);
            gameObject.SetActive(false);
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

    public void AddRoundBonuses()
    {
        if (dead)
            return;
        friendliness += friendBoost;
        confidence += confBoos;
        if (!foughtInRound)
        {
            consecutiveFightsWon = 0;
            roundsRested++;
            confidence = confidence - confLoseOnRest;
            if (health < maxHealth)
                health++;
        }else
        {
            roundsRested = 0;
        }
        if(CoolDownLeft>0)
            CoolDownLeft--;
        try
        {
            CooldownRing.fillAmount = (float)(CoolDownLength - CoolDownLeft) / (float)CoolDownLength;
        }
        catch
        {
            Debug.Log("fix cooldown rings!");
        }
        if (health < maxHealth)
        {
            health = Mathf.Min(health + healthBoost, maxHealth);
        }
        foughtInRound = false;        
        ConfGainedInRound = confidence - prevConf;
        FriendGainedInRound = friendliness - prevFriend;
        Helpers.Instance.NormalizeStats(this);
        levelControler.OnfightEndLevel(this, levelList);
        prevRoundHealth = health;       
        ResetBonuses();                
    }

	public void SetEmotion()
	{
        float factor = 0.13f;
        if (Mathf.Abs((float)confidence)<Var.lvl1 && Mathf.Abs((float)friendliness) < Var.lvl1)
		{
			//No type
			emotion = Var.Em.Neutral;
			colorRenderer.color = Helpers.Instance.neutral;
            DefaultCol = Helpers.Instance.GetEmotionColor(emotion);            
            HighlightCol = new Color(DefaultCol.r + factor, DefaultCol.g + factor, DefaultCol.b + factor);
            return;
		}
		
		if (Mathf.Abs((float)confidence) > Mathf.Abs((float)friendliness))
		{
			// Confident or sad
			if (confidence > 0)
			{
				colorRenderer.color = Helpers.Instance.brave;
				//Confident
				if (confidence >= Var.lvl1)
					emotion = Var.Em.Confident;
				//Superconfident
			    if (confidence >= Var.lvl2)
					emotion = Var.Em.SuperConfident;
			}
			else
			{
				//Scared
				colorRenderer.color = Helpers.Instance.scared;
			   if (confidence <= -Var.lvl1)
					emotion = Var.Em.Scared;
				//SuperScared
				if (confidence <= -Var.lvl2)
					emotion = Var.Em.SuperScared;
			}

		}
		else
		{
			//Friendly or lonely
			if (friendliness > 0)
			{

				//friendly
				colorRenderer.color = Helpers.Instance.friendly;
				if (friendliness >= Var.lvl1)
					emotion = Var.Em.Friendly;
				//SuperFriendly
			    if (friendliness >= Var.lvl2)
					emotion = Var.Em.SuperFriendly;
			}
			else
			{
				//Lonely
				colorRenderer.color = Helpers.Instance.lonely;
				if (friendliness <= -Var.lvl1)
					emotion = Var.Em.Lonely;
				//SuperLonely
			    if (friendliness <= -Var.lvl2)
					emotion = Var.Em.SuperLonely;
			}

		}
        DefaultCol = Helpers.Instance.GetEmotionColor(emotion);        
        HighlightCol = new Color(DefaultCol.r + factor, DefaultCol.g +factor, DefaultCol.b + factor);


    }
	
	public void showText()
	{
		if (ToString() != null)
		{
            //Var.birdInfo.text = ToString();           
            GuiContoler.Instance.selectedBird = this;
            if (!inMap)
            {
                Var.birdInfoHeading.text = Helpers.Instance.ApplyTitle(charName, lastLevel.title);
                Var.birdInfoFeeling.text = emotion.ToString();
                Var.birdInfoFeeling.color = Helpers.Instance.GetEmotionColor(emotion);
                GuiContoler.Instance.PortraitControl(portraitOrder, emotion);
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
        GuiContoler.Instance.confSlider.SetDist(confidence);
        GuiContoler.Instance.firendSlider.SetDist(friendliness);
        //set hearts
        Helpers.Instance.setHearts(GuiContoler.Instance.BirdInfoHearts, health, maxHealth);



		}
	}

	
	
	public void Update()
	{
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
			LeanTween.move(gameObject, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), 0.1f);
		}
        

    }

	public void ReleseBird(int x, int y)
	{
        
        Var.selectedBird = null;
		dragged = false;
        this.x = x;
        this.y = y;

        if (!inMap)
        {
            lines.DrawLines(x, y);
            Debug.Log("x: " + x+ " y: " + y);
            levelControler.ApplyLevelOnDrop(this, levelList);
            UpdateFeedback();          
        }
        LeanTween.move(gameObject, new Vector3(target.x, target.y, 0), 0.5f).setEase(LeanTweenType.easeOutBack);
        SetCoolDownRing(false);
        
		
	   

	}
    


}
