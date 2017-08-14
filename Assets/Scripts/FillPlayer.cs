using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillPlayer : MonoBehaviour {
    public Bird[] playerBirds;
    public Bird[] deadBirds;
    public bool inMap = false;
    public static FillPlayer Instance { get; private set; }
	// Use this for initialization
	void Awake ()
    {
        if (Var.lvlSprites == null)
            Var.lvlSprites = Resources.LoadAll<Sprite>("Icons/NewIcons");
        if (Var.skillIcons == null)
            Var.skillIcons = Resources.LoadAll<Sprite>("sprites/skill_pictures");
        if(Var.startingLvlSprites == null)
            Var.startingLvlSprites = Resources.LoadAll<Sprite>("Icons/icons_startingabilties");
        if(Var.hatSprites == null)
        {
            Var.hatSprites = new List<Sprite>();
            Var.hatSprites.AddRange(Resources.LoadAll<Sprite>("sprites/hat_spriteSheet"));
            Var.hatSprites.Add(Resources.Load<Sprite>("sprites/hat_spriteSheet_3"));
            Var.hatSprites.Add(Resources.Load<Sprite>("sprites/hat_spriteSheet_4"));
            print(Var.hatSprites.Count);
        }
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

            Var.availableBirds.AddRange(playerBirds);

        }
        /*if (!inMap && Var.activeBirds.Count < 1)
        {
            Var.activeBirds.AddRange(playerBirds);
        }*/
        
        if (!inMap && Var.activeBirds.Count > 0)
        {
            for (int i = 0; i < Var.activeBirds.Count; i++)
            {
                SetupBird(playerBirds[i], Var.activeBirds[i]);
            }
            List<Bird> inactive = Helpers.Instance.GetInactiveBirds();
            for(int i=0; i < inactive.Count; i++)
            {
                SetupBird(deadBirds[i], inactive[i]);
            }
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
	
	public static void SetupBird(Bird target, Bird template)
    {
        target.charName = template.charName;
        target.friendliness = template.friendliness;
        target.confidence = template.confidence;
        target.portraitOrder = template.portraitOrder;
        target.health = template.health;
        if (template.health < 1)
            target.gameObject.GetComponent<Animator>().SetBool("dead", true);
        target.portrait = template.portrait;
        target.maxHealth = template.maxHealth;
        target.levelList = template.levelList;
        target.startingLVL = template.startingLVL;
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
       // target.transform.Find("BIRB_sprite/hat").GetComponent<SpriteRenderer>().sprite = template.hatSprite;
    }


    public static BirdSaveData SetupSaveBird(Bird template)
    {
        BirdSaveData target = new BirdSaveData();
        target.charName = template.charName;
        target.friendliness = template.friendliness;
        target.confidence = template.confidence;
        target.portraitOrder = template.portraitOrder;
        target.health = template.health;        
        //target.portrait = template.portrait;
        target.levelList = template.levelList;
        target.startingLVL = template.startingLVL;
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

        return target;       
    }
    public static Bird LoadSavedBird(BirdSaveData template)
    {
        Bird target = new Bird("steve");
        target.charName = template.charName;
        target.friendliness = template.friendliness;
        target.confidence = template.confidence;
        target.portraitOrder = template.portraitOrder;
        target.health = template.health;
        //target.portrait = template.portrait;
        target.levelList = template.levelList;
        target.startingLVL = template.startingLVL;
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
