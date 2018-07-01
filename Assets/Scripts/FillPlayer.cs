using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillPlayer : MonoBehaviour {
	public Bird[] playerBirds;
	//public Bird[] deadBirds;
	public bool inMap = false;
	public static FillPlayer Instance { get; private set; }
	// Use this for initialization
	void Awake ()
	{
		LoadSprites();
		Instance = this;
		if (Var.isTutorial && !inMap)
			return;
		if (inMap && Var.availableBirds.Count>0)
		{
			foreach (Bird bird in Var.availableBirds)
			{
				bool wasActive = false;
				foreach (Bird ActiveBird in Var.activeBirds)
				{
					if (ActiveBird.charName == bird.charName)
					{
						wasActive = true;
						break;
					}
				}
				if (wasActive)
					bird.AdventuresRested = 0;
				else
				{
					if(!Var.fled)
						bird.AdventuresRested++;
				}
			}
			foreach (Bird bird in playerBirds)
			{
				bool foundBird = false;
				foreach (Bird loadBird in Var.availableBirds)
				{
					if (bird.charName == loadBird.charName)
					{
						SetupBird(bird, loadBird);
						bird.gameObject.SetActive(true);
						foundBird = true;
						break;
					}
				}
				if (!foundBird)
					bird.gameObject.SetActive(false);
			}
			
		}
		if (Var.availableBirds.Count < 1 && inMap)
		{           
			foreach(Bird bird in playerBirds)
			{
				if(bird.gameObject.activeSelf)
					Var.availableBirds.Add(bird);
			}          
		}
	 
		
		if (!inMap && Var.activeBirds.Count > 0)
		{
			for (int i = 0; i < Var.activeBirds.Count; i++)
			{
				SetupBird(playerBirds[i], Var.activeBirds[i]);
			}
			List<Bird> inactive = Helpers.Instance.GetInactiveBirds();
			/*for(int i=0; i < inactive.Count; i++)
			{
				SetupBird(deadBirds[i], inactive[i]);
			}*/
		}
	   if (!inMap && Var.activeBirds.Count < 1)
		{
			Var.activeBirds.AddRange(playerBirds);
			Var.availableBirds.AddRange(playerBirds);
		}
	}

	void Start()
	{
		if (Var.isTutorial&& !inMap)
		{
			for(int i =0;i<playerBirds.Length; i++)
			{
				if (i < Tutorial.Instance.BirdCount[0])
				{
					playerBirds[i].gameObject.SetActive(true);
				}else
				{
					playerBirds[i].gameObject.SetActive(false);
				}
			}
		}
	}
	void LoadSprites()
	{
		if (Var.dustCloud == null)
			Var.dustCloud = Resources.Load<GameObject>("prefabs/dustcloud");
		if (Var.lvlSprites == null)
			Var.lvlSprites = Resources.LoadAll<Sprite>("Icons/NewIcons");
		if (Var.skillIcons == null)
			Var.skillIcons = Resources.LoadAll<Sprite>("sprites/skill_pictures");
		if (Var.startingLvlSprites == null)
			Var.startingLvlSprites = Resources.LoadAll<Sprite>("Icons/icons_startingabilties");
		if (Var.hatSprites == null)
		{
			Var.hatSprites = new List<Sprite>();
			Var.hatSprites.AddRange(Resources.LoadAll<Sprite>("sprites/hat_spriteSheet"));
			Var.hatSprites.Add(Resources.Load<Sprite>("sprites/hat_spriteSheet_3"));
			Var.hatSprites.Add(Resources.Load<Sprite>("sprites/hat_spriteSheet_4"));
			// print(Var.hatSprites.Count);
		}
		if (Var.enemySprites == null)
		{
			Var.enemySprites = new List<GameObject>();
			Var.enemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/Visuals_side_confident"));
			Var.enemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/Visuals_side_friendly"));
			Var.enemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/Visuals_side_lonely"));
			Var.enemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/Visuals_side_neutral"));
			Var.enemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/Visuals_side_scared"));
			Var.enemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/Visuals_top_confident"));
			Var.enemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/Visuals_top_friendly"));
			Var.enemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/Visuals_top_lonely"));
			Var.enemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/Visuals_top_neutral"));
			Var.enemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/Visuals_top_scared"));
		}
		if (Var.wizardEnemySprites == null)
		{
			Var.wizardEnemySprites = new List<GameObject>();
			Var.wizardEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/magic enemies/Visuals_side_confident_MAGIC"));
			Var.wizardEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/magic enemies/Visuals_side_friendly_MAGIC"));
			Var.wizardEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/magic enemies/Visuals_side_lonely_MAGIC"));
			Var.wizardEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/magic enemies/Visuals_side_neutral_MAGIC"));
			Var.wizardEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/magic enemies/Visuals_side_scared_MAGIC"));
			Var.wizardEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/magic enemies/Visuals_top_confident_MAGIC"));
			Var.wizardEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/magic enemies/Visuals_top_friendly_MAGIC"));
			Var.wizardEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/magic enemies/Visuals_top_lonely_MAGIC"));
			Var.wizardEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/magic enemies/Visuals_top_neutral_MAGIC"));
			Var.wizardEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/magic enemies/Visuals_top_scared_MAGIC"));
		}
		if (Var.drillEnemySprites == null)
		{
			Var.drillEnemySprites = new List<GameObject>();
			Var.drillEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/drill enemies/Visuals_side_confident_DRILL"));
			Var.drillEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/drill enemies/Visuals_side_friendly_DRILL"));
			Var.drillEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/drill enemies/Visuals_side_lonely_DRILL"));
			Var.drillEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/drill enemies/Visuals_side_neutral_DRILL"));
			Var.drillEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/drill enemies/Visuals_side_scared_DRILL"));
			Var.drillEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/drill enemies/Visuals_top_confident_DRILL"));
			Var.drillEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/drill enemies/Visuals_top_friendly_DRILL"));
			Var.drillEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/drill enemies/Visuals_top_lonely_DRILL"));
			Var.drillEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/drill enemies/Visuals_top_neutral_DRILL"));
			Var.drillEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/drill enemies/Visuals_top_scared_DRILL"));
		}
		if(Var.sueprEnemySprites == null)
		{
			Var.sueprEnemySprites = new List<GameObject>();
			Var.sueprEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/super enemies/Visuals_side_confident_SUPER"));
			Var.sueprEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/super enemies/Visuals_side_friendly_SUPER"));
			Var.sueprEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/super enemies/Visuals_side_lonely_SUPER"));
			Var.sueprEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/super enemies/Visuals_side_neutral_SUPER"));
			Var.sueprEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/super enemies/Visuals_side_scared_SUPER"));
			Var.sueprEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/super enemies/Visuals_top_confident_SUPER"));
			Var.sueprEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/super enemies/Visuals_top_friendly_SUPER"));
			Var.sueprEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/super enemies/Visuals_top_lonely_SUPER"));
			Var.sueprEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/super enemies/Visuals_top_neutral_SUPER"));
			Var.sueprEnemySprites.Add(Resources.Load<GameObject>("prefabs/enemies/super enemies/Visuals_top_scared_SUPER"));
		}
	}
	public static void SetupBird(Bird target, Bird template)
	{
		target.recievedSeeds = template.recievedSeeds;
		target.charName = template.charName;
		target.friendliness = template.friendliness;
		target.confidence = template.confidence;
		target.portraitOrder = template.portraitOrder;
		target.health = template.health;
		target.mentalHealth = template.mentalHealth;
		if (template.injured)
		{
			//target.gameObject.GetComponentInChildren<Animator>().SetBool("dead", true);
			target.health = 0;            
		}
		target.levelRollBonus = template.levelRollBonus;
		target.bannedLevels = template.bannedLevels;
		target.injured = template.injured;
		target.portrait = template.portrait;
		target.portraitTiny = template.portraitTiny;
		target.maxHealth = template.maxHealth;
		target.levelList = template.levelList;
		//target.startingLVL = template.startingLVL;
		target.battleCount = template.battleCount;
		target.lastLevel = template.lastLevel;
		target.level = template.level;
		target.birdAbility = template.birdAbility;
		target.consecutiveFightsWon = template.consecutiveFightsWon;
		target.battlesToNextLVL = template.battlesToNextLVL;        
		target.roundsRested = template.roundsRested;
		target.AdventuresRested = template.AdventuresRested;
		target.TurnsInjured = template.TurnsInjured;
		target.CoolDownLeft = template.CoolDownLeft;
		target.CoolDownLength = template.CoolDownLength;
		target.preferredEmotion = template.preferredEmotion;
		target.birdPrefabName = template.birdPrefabName;
	   // target.transform.Find("BIRB_sprite/hat").GetComponent<SpriteRenderer>().sprite = template.hatSprite;
	}


	public static BirdSaveData SetupSaveBird(Bird template)
	{

		BirdSaveData target = new BirdSaveData();
        target.recievedSeeds = template.recievedSeeds;
		target.levelRollBonus = template.levelRollBonus;
		target.bannedLevels = template.bannedLevels;
		target.charName = template.charName;
		target.friendliness = template.friendliness;
		target.confidence = template.confidence;
		target.portraitOrder = template.portraitOrder;
		target.health = template.health;
		target.mentalHealth = template.mentalHealth;
		target.birdPrefabName = template.birdPrefabName;
		target.preferredEmotion = template.preferredEmotion;
		target.dead = template.injured;
		//target.portrait = template.portrait;
		target.levelList = template.levelList;
		//target.startingLVL = template.startingLVL;
		target.battleCount = template.battleCount;
		target.lastLevel = template.lastLevel;
		target.level = template.level;
		target.birdAbility = template.birdAbility;
		target.consecutiveFightsWon = template.consecutiveFightsWon;
		target.battlesToNextLVL = template.battlesToNextLVL;
		target.roundsRested = template.roundsRested;
		target.TurnsInjured = template.TurnsInjured;
		target.AdventuresRested = template.AdventuresRested;
		target.CoolDownLeft = template.CoolDownLeft;
		target.CoolDownLength = template.CoolDownLength;        
		return target;       
	}
	public static Bird LoadSavedBird(BirdSaveData template)
	{
		Bird target = new Bird("steve");
        target.recievedSeeds = template.recievedSeeds;
		target.levelRollBonus = template.levelRollBonus;
		target.charName = template.charName;
		target.friendliness = template.friendliness;
		target.confidence = template.confidence;
		target.portraitOrder = template.portraitOrder;
		target.bannedLevels = template.bannedLevels;
		target.health = template.health;		
		target.preferredEmotion = template.preferredEmotion;
		target.injured = template.dead;//target.portrait = template.portrait;
		target.levelList = template.levelList;
		//target.startingLVL = template.startingLVL;
		target.battleCount = template.battleCount;
		target.lastLevel = template.lastLevel;
		target.level = template.level;
		target.birdAbility = template.birdAbility;
		target.consecutiveFightsWon = template.consecutiveFightsWon;
		target.battlesToNextLVL = template.battlesToNextLVL;
		target.roundsRested = template.roundsRested;
		target.AdventuresRested = template.AdventuresRested;
		target.CoolDownLeft = template.CoolDownLeft;
		target.CoolDownLength = template.CoolDownLength;
		target.birdPrefabName = template.birdPrefabName;
		try
		{
			foreach (LevelData data in target.levelList)
			{
				data.LVLIcon = Helpers.Instance.GetLVLSprite(data.type);
			}
		}
		catch { }
		target.portrait = Resources.Load<GameObject>("prefabs/portrait_" + target.charName);
		return target;
	}
	// Update is called once per frame
	void Update () {
		
	}
}
