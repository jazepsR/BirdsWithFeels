using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Helpers : MonoBehaviour {
	public static Helpers Instance { get; private set; }
	[Header("Colors")]
	public Color neutral;
	public Color brave;
	public Color scared;
	public Color friendly;
	public Color lonely;
	public Color level;
	[Header("Soft colors")]
	public Color softBrave;
	public Color softScared;
	public Color softFriendly;
	public Color softLonely;
	public string BraveHexColor;
	public string ScaredHexColor;
	public string LonelyHexColor;
	public string FriendlyHexColor;
	public string RandomHexColor;
	GameObject heartBreak;
	GameObject heartGain;
	Sprite fullHeart;
	Sprite mentalHeart;
	Sprite emptyHeart;
	Sprite emptyMentalHeart;
	public bool inMap;
	List<Image> heartsToFill = new List<Image>();
	public Transform relationshipDialogs;
    public GameObject seed;
    public GameObject seedFar;
	[HideInInspector]
	public List<LevelBits> levelBits;
	enum friendState { alone, diagonal, oneFriend, twoFriends };
	public void Awake()
	{
		heartBreak = Resources.Load<GameObject>("prefabs/heartBreak");
		heartGain = Resources.Load<GameObject>("prefabs/heartGain");
		fullHeart = Resources.Load<Sprite>("sprites/heart");
		emptyHeart = Resources.Load<Sprite>("sprites/emptyHeart");
		mentalHeart = Resources.Load<Sprite>("sprites/mentalHeart");
		emptyMentalHeart = Resources.Load<Sprite>("sprites/mentalHeart_empty");
		Instance = this;
		if (levelBits.Count ==0)
		{
			levelBits = new List<LevelBits>();
			levelBits.AddRange(Resources.LoadAll<LevelBits>("ScriptableOjbects/Cautious"));
			levelBits.AddRange(Resources.LoadAll<LevelBits>("ScriptableOjbects/Confident"));
			levelBits.AddRange(Resources.LoadAll<LevelBits>("ScriptableOjbects/Social"));
			levelBits.AddRange(Resources.LoadAll<LevelBits>("ScriptableOjbects/Solitary"));


		}
        Application.targetFrameRate = 60;
	}
	public EventScript.Character GetCharEnum(Bird bird)
	{
		try
		{
			return (EventScript.Character)Enum.Parse(typeof(EventScript.Character), bird.charName);
		}
		catch
		{
			return EventScript.Character.None;
		}
	}

	public bool VarContainsTimedEvent(string evName)
	{
		if (Var.timedEvents.Count == 0)
			return false;
		foreach(TimedEventData data in Var.timedEvents)
		{
			if (data.eventName == evName)
				return true;
		}
		return false;
	}
	public TimedEventData GetTimedEvent(string evName)
	{
		if (Var.timedEvents.Count == 0)
			return null;
		foreach (TimedEventData data in Var.timedEvents)
		{
			if (data.eventName == evName)
				return data;
		}
		return null;
	}

	public static int GetEmotionNumber(Var.Em emotion)
	{
		switch (emotion)
		{
			case Var.Em.Neutral:
				return 0;
			case Var.Em.Cautious:
				return 1;
			case Var.Em.Confident:
				return 2;
			case Var.Em.Social:
				return 3;
			case Var.Em.Solitary:
				return 4;
			default:
				return 0;
		}
	}


	public int GetEmotionValue(Bird bird, Var.Em emotion)
	{
		switch (emotion)
		{
			case Var.Em.Confident:
				return bird.data.confidence;
			case Var.Em.Cautious:
				return Mathf.Abs(bird.data.confidence);
			case Var.Em.Social:
				return bird.data.friendliness;
			case Var.Em.Solitary:
				return Mathf.Abs(bird.data.friendliness);
			default:
				return 0;
		}
	}

    public static void ApplyLevel(Levels.type type, Bird bird)
    {

        switch(type)
        {
            case Levels.type.Brave1:
                Levels.ApplyLevel(new LevelData(Levels.type.Brave1, Var.Em.Confident, Var.lvlSprites[1]), "Brave 1",bird);
                break;
            case Levels.type.Brave2:
                Levels.ApplyLevel(new LevelData(Levels.type.Brave2, Var.Em.Confident, Var.lvlSprites[5]), "Brave 2", bird);
                break;
            case Levels.type.Friend1:
                Levels.ApplyLevel(new LevelData(Levels.type.Friend1, Var.Em.Social, Var.lvlSprites[0]), "Friendly 1", bird);
                break;
            case Levels.type.Friend2:
                Levels.ApplyLevel(new LevelData(Levels.type.Friend1, Var.Em.Social, Var.lvlSprites[4]), "Friendly 2", bird);
                break;
            case Levels.type.Lonely1:
                Levels.ApplyLevel(new LevelData(Levels.type.Lonely1, Var.Em.Solitary, Var.lvlSprites[3]), "Lonely 1", bird);
                break;
            case Levels.type.Lonely2:
                Levels.ApplyLevel(new LevelData(Levels.type.Lonely2, Var.Em.Solitary, Var.lvlSprites[7]), "Lonely 2", bird);
                break;
            case Levels.type.Scared1:
                Levels.ApplyLevel(new LevelData(Levels.type.Scared1, Var.Em.Cautious, Var.lvlSprites[2]), "Scared 1", bird);
                break;
            case Levels.type.Scared2:
                Levels.ApplyLevel(new LevelData(Levels.type.Scared2, Var.Em.Cautious, Var.lvlSprites[6]), "Scared 2", bird);
                break;
            default: break;

        }


    }

    public Bird GetBirdFromEnum(EventScript.Character ch, bool useAll = false)
	{
		List<Bird> birds;
		if ( useAll)
			birds = Var.availableBirds;
		else
		{
			birds = new List<Bird>();
			birds.AddRange(FillPlayer.Instance.playerBirds);
		}
		 

		if(ch == EventScript.Character.Random)
		{
			return birds[UnityEngine.Random.Range(0, birds.Count)];
		}
		foreach (Bird bird in birds)
		{
			if (bird.charName == ch.ToString())
				return bird;
		}
		return null;


	}
	public string GetStatInfo(int confidence, int social)
	{
		string statText = "";
		if (confidence > 0)
			statText += Helpers.Instance.GetHexColor(Var.Em.Confident) + "Confidence: " + confidence + "</color>\n";
		if (confidence < 0)
			statText += Helpers.Instance.GetHexColor(Var.Em.Cautious) + "Cautions: " + Mathf.Abs(confidence) + "</color>\n";
		if (confidence == 0)
			statText += "Confidence: 0\n";
		if (social > 0)
			statText += Helpers.Instance.GetHexColor(Var.Em.Social) + "Social: " + social+ "</color>\n";
		if (social < 0)
				statText += Helpers.Instance.GetHexColor(Var.Em.Solitary) + "Solitary: " + Mathf.Abs(social) + "</color>\n";
		if (social == 0)
			statText += "Solitude: 0\n";
		statText = "<b>" + statText + "</b>";
		return statText;
	}
	public Var.Em GetOppositeEmotion(Var.Em type)
	{
		switch (type)
		{
			case Var.Em.Confident:
				return Var.Em.Cautious;
			case Var.Em.Cautious:
				return Var.Em.Confident;
			case Var.Em.Social:
				return Var.Em.Solitary;
			case Var.Em.Solitary:
				return Var.Em.Social;
			default:
				return Var.Em.Neutral;
		}
	}
	public GameObject GetEnemyVisual(Bird.dir dir, Var.Em emotion, fillEnemy.enemyType type)
	{
		int id = 0;
		if (dir == Bird.dir.top)
			id += 5;
		switch (emotion)
		{
			case Var.Em.Confident:
				id += 0;
				break;
			case Var.Em.Social:
				id += 1;
				break;
			case Var.Em.Solitary:
				id += 2;
				break;
			case Var.Em.Neutral:
				id += 3;
				break;
			case Var.Em.Cautious:
				id += 4;
				break;
		}
		switch (type)
		{
			case fillEnemy.enemyType.drill:
				return Var.drillEnemySprites[id];
			case fillEnemy.enemyType.normal:
				return Var.enemySprites[id];
			case fillEnemy.enemyType.wizard:
				return Var.wizardEnemySprites[id];
			case fillEnemy.enemyType.super:
				return Var.sueprEnemySprites[id];
		}
		return null;
		

	}
	public GameObject GetPortrait(EventScript.Character Char)
	{
		if (Char == EventScript.Character.Random)
			return GetBirdFromEnum(Char).portrait;
		else
		{
			return Resources.Load<GameObject>("prefabs/portraits/portrait_" + Char.ToString());
		}

	}
	public Var.Em RandomEmotion()
	{
		int rnd = UnityEngine.Random.Range(0, 5);
		switch (rnd)
		{
			case 0:
				return Var.Em.Neutral;
			case 1:
				return Var.Em.Cautious;
			case 2:
				return Var.Em.Confident;
			case 3:
				return Var.Em.Solitary;
			case 4:
				return Var.Em.Social;
			default:
				return Var.Em.Neutral;
		}


	}
	public Var.Em GetLevelEmotion(Levels.type type)
	{
		switch (type)
		{
			case Levels.type.Brave1:
				return Var.Em.Confident;
			case Levels.type.Brave2:
				return Var.Em.Confident;
			case Levels.type.Friend1:
				return Var.Em.Social;
			case Levels.type.Friend2:
				return Var.Em.Social;
			case Levels.type.Lonely1:
				return Var.Em.Solitary;
			case Levels.type.Lonely2:
				return Var.Em.Solitary;
			case Levels.type.Scared1:
				return Var.Em.Cautious;
			case Levels.type.Scared2:
				return Var.Em.Cautious;
			default:
				return Var.Em.Neutral;
		}
	}


	public bool RandomBool()
	{
		if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
			return true;
		else
			return false;
	}
	public string GetName(bool isMale)
	{
		if (isMale)
			return Var.maleNames[UnityEngine.Random.Range(0, Var.maleNames.Length)];
		else
			return Var.femaleNames[UnityEngine.Random.Range(0, Var.femaleNames.Length)];
	}
	void FillHearts(Sprite HeartSprite)
	{
		foreach(Image heart in heartsToFill)
		{
			heart.sprite = HeartSprite;
		}
		heartsToFill = new List<Image>();
	}
	public void setHearts(Image[] hearts, int currentHP, int MaxHP, int prevRoundHealth = -1, bool isMental = false)
	{
		Sprite heartSprite = fullHeart;
		Sprite emptyHeartSprite = emptyHeart;
		if (isMental)
		{
			heartSprite = mentalHeart;
			emptyHeartSprite = emptyMentalHeart;

		}
		for (int i = 0; i < hearts.Length; i++)
		{
			if (i < currentHP)
			{
				hearts[i].gameObject.SetActive(true);
				if (i >= prevRoundHealth && prevRoundHealth != -1)
				{
					GameObject breakObj = Instantiate(heartGain, hearts[i].transform.position, Quaternion.identity);
					breakObj.transform.parent = hearts[i].transform;
					breakObj.transform.localScale = Vector3.one;// * 0.5f;
					heartsToFill.Add(hearts[i]);
					LeanTween.delayedCall(0.46f,()=> FillHearts(heartSprite));
					Destroy(breakObj, 0.45f);
				}
				else
				{
					hearts[i].sprite = heartSprite;
					
				}
			} else
			{
				if (i < prevRoundHealth && prevRoundHealth != -1)
				{
					GameObject breakObj = Instantiate(heartBreak, hearts[i].transform.position, Quaternion.identity);
					breakObj.transform.parent = hearts[i].transform;
					breakObj.transform.localScale = Vector3.one * 0.5f;
					Destroy(breakObj, 0.45f);
				}
				
				if (i < MaxHP)
				{
					hearts[i].sprite = emptyHeartSprite;
					hearts[i].gameObject.SetActive(true);
				}
				else
				{
					hearts[i].gameObject.SetActive(false);
				}
				
			}
		}  
	}

	public String GetHexColor(Var.Em emotion)
	{
		switch (emotion)
		{
			case Var.Em.Social:
				return FriendlyHexColor;
			case Var.Em.Solitary:
				return LonelyHexColor;
			case Var.Em.Confident:
				return BraveHexColor;
			case Var.Em.Cautious:
				return ScaredHexColor;
			case Var.Em.SuperFriendly:
				return FriendlyHexColor;
			case Var.Em.SuperLonely:
				return LonelyHexColor;
			case Var.Em.SuperConfident:
				return BraveHexColor;
			case Var.Em.SuperScared:
				return ScaredHexColor;
			case Var.Em.Neutral:
				return "<color=#000000FF>";
			case Var.Em.Random:
				return RandomHexColor;
			default:
				return "<color=#000000FF>";
		}
	}

	//Neutral means weak to all
	public Var.Em GetWeakness(Var.Em Emotion)
	{
		switch (Emotion)
		{
			case Var.Em.Neutral:
				return Var.Em.Neutral;
			case Var.Em.Solitary:
				return Var.Em.Confident;
			case Var.Em.SuperLonely:
				return Var.Em.Confident;
			case Var.Em.Social:
				return Var.Em.Cautious;
			case Var.Em.SuperFriendly:
				return Var.Em.Cautious;
			case Var.Em.Confident:
				return Var.Em.Social;
			case Var.Em.SuperConfident:
				return Var.Em.Social;
			case Var.Em.Cautious:
				return Var.Em.Solitary;
			case Var.Em.SuperScared:
				return Var.Em.Solitary;
			case Var.Em.finish:
				return Var.Em.Neutral;
			default:
				return Var.Em.Neutral;
		}
	}
	public Sprite GetHatSprite(string charName)
	{
		switch (charName)
		{
			case "Kim":
				return Var.hatSprites[1];
			case "Sophie":
				return Var.hatSprites[0];
			case "Terry":
				return Var.hatSprites[2];
			case "Rebecca":
				return Var.hatSprites[4];
			case "Alexander":
				return Var.hatSprites[3];
			default:
				return null;
		}
	}

	public string GetLevelTitle(Levels.type type)
	{
	   
		switch (type)
		{
			case Levels.type.Alexander:
				return "Alexander";
			case Levels.type.Kim:
				return "Kim";
			case Levels.type.Rebecca:
				return "Rebecca";
			case Levels.type.Sophie:
				return "Sophie";
			case Levels.type.Terry:
				return "Terry";
			case Levels.type.Friend1:
				return "Healing touch";
			case Levels.type.Friend2:
				return "A great massage";
			case Levels.type.Brave1:
				return "Bird shield";
			case Levels.type.Brave2:
				return "Fake hearts";
			case Levels.type.Lonely1:
				return "Emo intensifier";
			case Levels.type.Lonely2:
				return "Time lord";
			case Levels.type.Scared1:
				return "Diagonal backstabber";
			case Levels.type.Scared2:
				return "Original backstabber";
			default:
				return "";
		}

	}

	//Neutral means weak to all
	public Var.Em GetStenght(Var.Em Emotion)
	{
		switch (Emotion)
		{
			case Var.Em.Neutral:
				return Var.Em.Neutral;
			case Var.Em.Solitary:
				return Var.Em.Cautious;
			case Var.Em.SuperLonely:
				return Var.Em.Cautious;
			case Var.Em.Social:
				return Var.Em.Confident;
			case Var.Em.SuperFriendly:
				return Var.Em.Confident;
			case Var.Em.Confident:
				return Var.Em.Solitary;
			case Var.Em.SuperConfident:
				return Var.Em.Solitary;
			case Var.Em.Cautious:
				return Var.Em.Social;
			case Var.Em.SuperScared:
				return Var.Em.Social;
			case Var.Em.finish:
				return Var.Em.Neutral;
			default:
				return Var.Em.Neutral;
		}
	}
	public List<Bird> GetAdjacentBirds(Bird bird)
	{
		try
		{
			int x = bird.x;
			int y = bird.y;
			List<Bird> list = new List<Bird>();
			int sizeY = Var.playerPos.GetLength(1) - 1;
			int sizeX = Var.playerPos.GetLength(0) - 1;
			if (y + 1 <= sizeY && Var.playerPos[x, y + 1] != null)
				list.Add(Var.playerPos[x, y + 1]);
			if (y - 1 >= 0 && Var.playerPos[x, y - 1] != null)
				list.Add(Var.playerPos[x, y - 1]);
			if (x + 1 <= sizeX && Var.playerPos[x + 1, y] != null)
				list.Add(Var.playerPos[x + 1, y]);
			if (x - 1 >= 0 && Var.playerPos[x - 1, y] != null)
				list.Add(Var.playerPos[x - 1, y]);
			if (y + 1 <= sizeY && x + 1 <= sizeX && Var.playerPos[x + 1, y + 1] != null)
				list.Add(Var.playerPos[x + 1, y + 1]);
			if (y + 1 <= sizeY && x - 1 >= 0 && Var.playerPos[x - 1, y + 1] != null)
				list.Add(Var.playerPos[x - 1, y + 1]);
			if (y - 1 >= 0 && x + 1 <= sizeX && Var.playerPos[x + 1, y - 1] != null)
				list.Add(Var.playerPos[x + 1, y - 1]);
			if (y - 1 >= 0 && x - 1 >= 0 && Var.playerPos[x - 1, y - 1] != null)
				list.Add(Var.playerPos[x - 1, y - 1]);
			return list;
		}
		catch
		{
			//Debug.LogError("Get close birds failed");
			return new List<Bird>();
		}
	}
	public string ApplyTitle(Bird name, string title)
	{
		
		if (name == null)
			return title;
		if (title != "")
			return title.Replace("<name>", name.charName);
		else
			return name.charName;
	}
	public List<Bird> GetInactiveBirds()
	{
		List<Bird> inactiveBirds = new List<Bird>();
		foreach(Bird bird in Var.availableBirds)
		{
			bool isInactive = true;
			foreach(Bird activeBird in Var.activeBirds)
			{
				if (bird.charName == activeBird.charName)
					isInactive = false;
			}
			if (isInactive)
				inactiveBirds.Add(bird);
		}
		return inactiveBirds;
	}
	public Sprite GetSkillPicture(Levels.type type)
	{
		switch (type)
		{
			case Levels.type.Brave1:
				return Var.skillIcons[0];
			case Levels.type.Brave2:
				return Var.skillIcons[1];
			case Levels.type.Friend1:
				return Var.skillIcons[6];
			case Levels.type.Friend2:
				return Var.skillIcons[7];
			case Levels.type.Lonely1:
				return Var.skillIcons[2];
			case Levels.type.Lonely2:
				return Var.skillIcons[3];
			case Levels.type.Scared1:
				return Var.skillIcons[4];
			case Levels.type.Scared2:
				return Var.skillIcons[5];
			default:
				return null;
		}
	}


	public string GetLevelUpText(string name, Levels.type type)
	{
		switch (type)
		{
			case Levels.type.Brave1:
				return "<name>'s confidence is surging! They feel like they can do anything!".Replace("<name>", name);
				//return "Successfully fending off two vultures at once, <name> feels a surge of confidence flow through them.& Nothing can stop <name> at this point! <name> promises to use their newfound confidence to shield their teammates from harm. ".Replace("<name>", name);
			case Levels.type.Brave2:
				return "<name>'s confidence is surging! They feel like they can do anything!".Replace("<name>", name);
				//return "There's no stopping <name> now! Nothing can hurt them! (As long as they think so, that is)".Replace("<name>", name);
			case Levels.type.Friend1:
				return "<name> has realized the power of friendship!".Replace("<name>", name);
			//return "<name> smacks down the enemy! Surrounded by friends, <name> realizes they'd do almost anything for them. <name> promises themselves to help their team mates in any way possible".Replace("<name>", name);
			case Levels.type.Friend2:
				return "<name> has realized the power of friendship!".Replace("<name>", name);
				//return "<name> has found a way to help their friends even more! Applying massage techniques passed down from the friendship gods, <name> can now fully heal team mates if given the time!".Replace("<name>", name);
			case Levels.type.Lonely1:
				return "Through contemplation and solitude <name> has grown stronger!".Replace("<name>", name);
				//return "Through deep solidtary meditation coupled with the rush of victory, <name> realizes that all emotions are just a result of circumstances.& <name> realizes they can intensify the emotions of their teammates!".Replace("<name>", name);
			case Levels.type.Lonely2:
				return "Through contemplation and solitude <name> has grown stronger!".Replace("<name>", name);
				//return "All this alone time was a great chance for <name> to do some serious studying! Through careful observation, <name> has percieved that life is but a series of turns, which can be manipulated with the right tools. ".Replace("<name>", name);
			case Levels.type.Scared1:
				return "<name> has found that there are more subtle ways to help his team teanmates direct combat".Replace("<name>", name);
				//return "<name> realizes the depths of their incompetence. Were they ever fit for battle? They resolve to help the team with more cunning means - by weakening the enemy.  ".Replace("<name>", name);
			case Levels.type.Scared2:
				return "<name> has found that there are more subtle ways to help his team teanmates direct combat".Replace("<name>", name);
				//return "Direct combat is definetelly not for <name>! They prefer to attack their enemies from behind and let teammates finish the job! ".Replace("<name>", name);
			default:
				return "Error in level up text";        
		}

	}
	public string GetDeathText(Levels.type type, string name)
	{
		
		string deathTxt = "";
		switch (type)
		{
			case Levels.type.Brave1:
				deathTxt = "Despite their bravery, the enemies were too much for <name>. <name> hopes they will be strong enough to make it without him. ";
				break;
			case Levels.type.Brave2:
				deathTxt = "<name> was sure nothing could hurt them. Unfortunately, <name> realized this error too late... ";
				break;
			case Levels.type.Friend1:
				deathTxt = "<name> tired to help the team, but was not able to escape the danger. Who will take care of them now? ";
				break;
			case Levels.type.Friend2:
				deathTxt = "<name> was so close... If they had just made it back to base quicker, <name> would have made it all right.";
				break;
			case Levels.type.Lonely1:
				deathTxt = "<name> kept to themselves during the past few weeks. Maybe tht was the wrong strategy?";
				break;
			case Levels.type.Lonely2:
				deathTxt = "This is why <name> didn't want to rely on the other birds! Things like this often lead to injury";
				break;
			case Levels.type.Scared1:
				deathTxt = "<name> wasn't cut out for fighting. It turned out their clever tricks could'nt portect them.";
				break;
			case Levels.type.Scared2:
				deathTxt = "For all their attempts do avoid fighting, violence eventually found <name>. ";
				break;
			default:
				deathTxt = "Ouch! <name> will be out for a while after this!";
				break;
		}
		deathTxt += "\n" + Var.deathSignoffs[UnityEngine.Random.Range(0, Var.deathSignoffs.Length)];
		deathTxt = deathTxt.Replace("<name>", name);
		return deathTxt;




	}
	public string GetLVLProgress(Levels.type type, Bird bird)
	{
		switch (type)
		{
			case Levels.type.Brave2:

			default:
				return "";
		}
	}
	public string GetLVLRequirements(Levels.type type)
	{
		switch (type)
		{
			case Levels.type.Brave1:
				return "Defeat two enemies in the same fight";
			case Levels.type.Brave2:
				return "Defeat 4 enemies in a row without losing";
			case Levels.type.Friend1:
				return "Gain + 4 or more social in one turn and and win a fight";
			case Levels.type.Friend2:
				return "Be close to a friendly bird and gain + 5 or more social in one turn"; //?
			case Levels.type.Lonely1:
				return "Be alone (no teammates in this bird's row or column). At the same time, win a fight";
			case Levels.type.Lonely2:
				return "The bird is not used for two adventures in a row. Then, spend a fight all alone";
			case Levels.type.Scared1:
				return "Lose a fight while being close to a friendly bird winning a fight";
			case Levels.type.Scared2:
				return "Rest 3 turns in a row";
			default:
				return "Error in level up text";
		}

	}
	public string GetLVLInfoText(Levels.type level)
	{
		switch (level)
		{
			case Levels.type.Brave1:
				return "Prevent a random close bird from losing health if they lose their fight. 3 turn cooldown";
			case Levels.type.Brave2:
				return "If feeling confident, when taking damage lose 2 confidence instead of 1 health";
			case Levels.type.Friend1:
				return "Once per turn, if resting, restore 1 health to a random close bird";
			case Levels.type.Friend2:
				return "After an adventure fully heal one bird of your choice";
			case Levels.type.Scared1:
				return "Give all diagonal birds minus 10% in combat (both friendly and enemy birds)";
			case Levels.type.Scared2:
				return "If in hidden, this bird does not fight enemies. Instead, all enemies crossing this birds tile have -30% in combat. Hide the bird by left clicking them.";
			case Levels.type.Lonely1:
				return "Emotional changes increased by 1 for all teammates in this birds column.";
			case Levels.type.Lonely2:
				return "If resting, the player can chosse to redo the last battle. 3 turn cooldown";
			case Levels.type.Alexander:
				return "Bird gains +1 in the dominant feeling of an adjacent bird";
			case Levels.type.Kim:
				return "Bird loses -2 confidence when resting";
			case Levels.type.Rebecca:
				return "Ground effects affect this bird twice as much";
			case Levels.type.Sophie:
				return "If resting, all adjacent birds recieve 10% chance to win fights";
			case Levels.type.Terry:
				return "All birds in the same horizontal row gain +1 confidence";

			default:
				return "Level not found error";

		}
	}

	public bool ListContainsLevel(Levels.type level, List<LevelData> list)
	{
		if (list != null)
		{
			foreach (LevelData data in list)
			{
				if (data.type == level)
					return true;
			}
			return false;
		}else
		{
			return false;
		}
	}
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            Time.timeScale = 0.25f;
        else if (Input.GetKey(KeyCode.RightShift))
            Time.timeScale = 4f;
        else
            Time.timeScale = 1;
    }
#endif
    public string GetLevelUpDialogs(Levels.type type, EventScript.Character character)
	{
		switch (character)
		{
			case EventScript.Character.Terry:
				switch (type)
				{
					case Levels.type.Friend1:
						return "Wow, I’m so glad I had you guys next to me!&Let me know if you get injured, and I’ll do what I can to help you!";
					case Levels.type.Friend2:
						return "You guys are so great!&Once we’re done with these vultures, I know just the thing to make you feel better if you need it!";
					case Levels.type.Lonely1:
						return "Why were you guys so far away from me?&If you stand a bit closer, I know how to boost your emotions now!";
					case Levels.type.Lonely2:
						return "I liked spending some time alone.&If I concentrate for a bit, I can call a do-over when fighting!";
					case Levels.type.Brave1:
						return "Did you see that? I took down two of them!&I’ll protect you if something happens!";
					case Levels.type.Brave2:
						return "I’m on a roll! If I get hit, I’ll just shrug it off!";
					case Levels.type.Scared1:
						return "I know losing isn’t all bad, but it still hurt.&I’ve figured out how to weaken everybirdy’s combat strength now.";
					case Levels.type.Scared2:
						return "I practiced hiding while resting. That the vultures won’t even see me coming!";
					default:
						return "";
				}
			case EventScript.Character.Rebecca:
				switch (type)
				{
					case Levels.type.Friend1:
						return "I’m so glad we’re all friends. Come to me if you’re injured and I’ll help you!";
					case Levels.type.Friend2:
						return "Hanging out with you guys is the best!&Once we get a moment of peace, I can fully heal one of you.";
					case Levels.type.Lonely1:
						return "Please, don’t leave me alone again. I don’t like it.&If you stand closer, I can enhance your emotional strength.";
					case Levels.type.Lonely2:
						return "I’ve never experience this deep kind of loneliness before…&"+
							"It’s strange, but I like it. I can use the negotiations I’ve practiced to let us redo a fight!";
					case Levels.type.Brave1:
						return "Wow, I never knew I had that in me! I feel like I can take on all the vultures in the world!&I’ll shield you guys if something happens!";
					case Levels.type.Brave2:
						return "I can’t believe I won against all those vultures!&Don’t worry about me losing health, I’ll just sacrifice some confidence instead!";
					case Levels.type.Scared1:
						return "Why didn’t you help me? I thought we were friends…&I know just how to return the favor.";
					case Levels.type.Scared2:
						return "That was a nice rest, and the vultures didn’t even notice me!&I know how to use that in a fight now!";
					default:
						return "";
				}

			case EventScript.Character.Alexander:
				switch (type)
				{
					case Levels.type.Friend1:
						return "Now this is a proper battle strategy!&I remember the first aid training I’ve had, let me know if you need it.";
					case Levels.type.Friend2:
						return "Once we’re through these fights, I can use my medical training to fully heal one of you.";
					case Levels.type.Lonely1:
						return "I suppose being alone makes me stronger in a way, too.&I know a strategy to enhance your emotions, just stand in an orderly line!";
					case Levels.type.Lonely2:
						return "I spent some time alone and practiced my military negotiations skill..&..so that I can give us a second chance at a fight!";
					case Levels.type.Brave1:
						return "Is everybirdy okay? Don’t worry, I’ll protect you if a vulture is headed your way!";
					case Levels.type.Brave2:
						return "Now this is true confidence! If I get hit I’ll just shake it off!";
					case Levels.type.Scared1:
						return "We can’t win them all, I suppose. That hurt a bit, though.&Reminds me of a backstabbing technique I learned long ago.";
					case Levels.type.Scared2:
						return "In the military, we practiced stealth and undercover operations.&I can use those lessons to our advantage!";
					default:
						return "";
				}

			case EventScript.Character.Sophie:
				switch (type)
				{
					case Levels.type.Friend1:
						return "I’ve never been surrounded by this many friends before.&I’ve read a bit about medicine, let me know if you guys get hurt, okay?";
					case Levels.type.Friend2:
						return "Being this close to you guys made me recall the stuff I’ve read about advanced healing!&I can help one of you once we get through these fights.";
					case Levels.type.Lonely1:
						return "I like being alone, and now I know just the thing to push everybirdy’s emotions even further!";
					case Levels.type.Lonely2:
						return "During my time alone, I researched some strategy and if I rest now, I can give you guys a second chance at fighting.";
					case Levels.type.Brave1:
						return "I never knew I could do that! I know how to protect you guys now, if you need it.";
					case Levels.type.Brave2:
						return "I can’t believe I keep winning these fights!&I’ve learned how to sacrifice some confidence instead of health now.";
					case Levels.type.Scared1:
						return "I’ve read a bit about strategy and I don’t think that was the best move…&I know how to weaken those standing diagonally to me, let me show you.";
					case Levels.type.Scared2:
						return "I never realized how nice resting can be.&I’ve figured out how to hide myself so that the vultures can pass right by me!";
					default:
						return "";
				}
			case EventScript.Character.Kim:
				switch (type)
				{
					case Levels.type.Friend1:
						return "I feel safe when you guys are around, and I hope I can help.&I think I can heal you if you get injured.";
					case Levels.type.Friend2:
						return "It’s so nice to have friends.&I can heal you later, if one of you needs it.";
					case Levels.type.Lonely1:
						return "It’s okay to be lonely, but if we all stand together, I can boost your emotions.";
					case Levels.type.Lonely2:
						return "I spent some time alone thinking about our adventure so far.&I’ve figured out a way to let us retry a fight if things go badly!";
					case Levels.type.Brave1:
						return "Oh my beak, I fought two of them and won!&I’ll shield you if a vulture is coming for you!";
					case Levels.type.Brave2:
						return "It’s nice to get some time alone every now and again.&I can give us a second attempt at fighting, if you think we need it.";
					case Levels.type.Scared1:
						return "I thought we were a team… I thought we were supposed to look after each other.&Well, I know how to backstab too.";
					case Levels.type.Scared2:
						return "All that resting gave me some time to prepare a camouflage.&The vultures won’t see me when they pass!";
					default:
						return "";
				}
			default:
				return "";
		}


	}


	public bool ListContainsEmotion(Var.Em emotion, List<LevelData> list)
	{
		if (list != null)
		{
			foreach (LevelData data in list)
			{
				if (data.emotion == emotion)
					return true;
			}
			return false;
		}
		else
		{
			return false;
		}
	}
	public void NormalizeStats(Bird bird, int treshold = 15)
	{
		
		if (bird.data.level > 1)
		{
			treshold = 15;
		}
		if (bird.data.confidence > treshold)
			bird.data.confidence = treshold;
		if (bird.data.confidence < -treshold)
			bird.data.confidence = -treshold;
		if (bird.data.friendliness > treshold)
			bird.data.friendliness = treshold;
		if (bird.data.friendliness < -treshold)
			bird.data.friendliness = -treshold;
	}

	public Color GetEmotionColor(Var.Em emotion)
	{
		switch (emotion)
		{
			case Var.Em.Neutral:
				return neutral;
			case Var.Em.Social:
				return friendly;
			case Var.Em.Solitary:
				return lonely;
			case Var.Em.Confident:
				return brave;
			case Var.Em.Cautious:
				return scared;
			case Var.Em.SuperFriendly:
				return friendly;
			case Var.Em.SuperLonely:
				return lonely;
			case Var.Em.SuperConfident:
				return brave;
			case Var.Em.SuperScared:
				return scared;
			case Var.Em.finish:
				return level;
			case Var.Em.Random:
				return level;
			default:
				return neutral;
		}
	}
	public Color GetSoftEmotionColor(Var.Em emotion)
	{
		switch (emotion)
		{
			case Var.Em.Neutral:
				return neutral;
			case Var.Em.Social:
				return softFriendly;
			case Var.Em.Solitary:
				return softLonely;
			case Var.Em.Confident:
				return softBrave;
			case Var.Em.Cautious:
				return softScared;
			case Var.Em.SuperFriendly:
				return softFriendly;
			case Var.Em.SuperLonely:
				return softLonely;
			case Var.Em.SuperConfident:
				return softBrave;
			case Var.Em.SuperScared:
				return softScared;
			default:
				return neutral;
		}
	}

	public Sprite GetLVLSprite(Levels.type type)
	{
		switch (type) {
			case Levels.type.Kim:
				return Var.startingLvlSprites[3];
			case Levels.type.Rebecca:
				return Var.startingLvlSprites[0];
			case Levels.type.Terry:
				return Var.startingLvlSprites[4];
			case Levels.type.Alexander:
				return Var.startingLvlSprites[1];
			case Levels.type.Sophie:
				return Var.startingLvlSprites[2];
			case Levels.type.Brave1:
				return Var.lvlSprites[1];
			case Levels.type.Brave2:
				return Var.lvlSprites[5];
			case Levels.type.Friend1:
				return Var.lvlSprites[0];
			case Levels.type.Friend2:
				return Var.lvlSprites[4];
			case Levels.type.Scared1:
				return Var.lvlSprites[2];
			case Levels.type.Scared2:
				return Var.lvlSprites[6];
			case Levels.type.Lonely1:
				return Var.lvlSprites[3];
			case Levels.type.Lonely2:
				return Var.lvlSprites[7];
			default:
				return null;


		}       
	}
	public string GetLVLTitle(Levels.type lvl)
	{
		switch (lvl)
		{
			case Levels.type.Brave1:
				return "<name> the protector";
			case Levels.type.Brave2:
				return "<name> the invicible";
			case Levels.type.Friend1:
				return "<name> the healer";                
			case Levels.type.Friend2:
				return "Doctor <name>";
			case Levels.type.Lonely1:
				return "Apprentice <name>";
			case Levels.type.Lonely2:
				return "Time lord <name>";
			case Levels.type.Scared1:
				return "Cunning <name>";
			case Levels.type.Scared2:
				return "<name> the rogue";
			default:
				return "";
		}
	}
	public Vector3 dirToVector(Bird.dir dir)
	{
		float increment = 0.7f;
		switch (dir)
		{
			case Bird.dir.front:
				return new Vector3(increment, 0);          
			case Bird.dir.top:
				return new Vector3(0, increment);
			default:
				return new Vector3(0, 0, 0);
		}
	}

	public void ShowTooltip(String text)
	{
		GuiContoler.Instance.tooltipText.transform.parent.gameObject.SetActive(true);
		GuiContoler.Instance.tooltipText.transform.parent.gameObject.GetComponent<Image>().enabled = false;
		GuiContoler.Instance.tooltipText.text = text;
		AudioControler.Instance.PlaySound(AudioControler.Instance.expand);       
		//LeanTween.delayedCall(0.05f, GuiContoler.Instance.tooltipText.transform.parent.gameObject.GetComponent<tooltipScript>().SetPos);
	}
	public void HideTooltip()
	{
		GuiContoler.Instance.tooltipText.transform.parent.gameObject.SetActive(false);
	}


	public void EmitEmotionParticles(Transform parent,Var.Em emotion, bool useText= true, int intensity = 1)
	{
		var emParticleObject = Instantiate(Var.emotionParticles, parent);

		emoParticleController emoParticleCtrl = emParticleObject.GetComponent<emoParticleController>();

		emoParticleCtrl.emitParticles(emotion, intensity); //let number go between 0-2 for different intensities! 

		var particleSys = emParticleObject.GetComponentInChildren<ParticleSystem>().colorOverLifetime;
		//Gradient grad = new Gradient();
		Color col = GetEmotionColor(emotion);
	  //  grad.SetKeys(new GradientColorKey[] { new GradientColorKey(col, 0.0f), new GradientColorKey(col, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
	  //  particleSys.color = grad;
		Text text = emParticleObject.transform.Find("Text").GetComponent<Text>();
		text.enabled = useText;
		text.text = emotion.ToString().Replace("Super","");
		if (text.text == "finish")
			text.text = "Level up!";
		text.color = col;
	   // LeanTween.moveLocalZ(text.gameObject, 55.0f, 1f);
	   // LeanTween.scale(text.gameObject, Vector3.one * 2f, 1.2f);
	 //   LeanTween.alphaText(text.rectTransform, 0.0f, 1.2f);
		Destroy(emParticleObject, 1.7f);
	}


	public float RandGaussian(float stdDev, float mean)
	{
		float u1 = UnityEngine.Random.Range(0.0f, 1.0f);
		float u2 = UnityEngine.Random.Range(0.0f, 1.0f);
		float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
		return  mean + stdDev * randStdNormal;
	}
	public bool IsSuper(Var.Em emotion)
	{
		if (emotion == Var.Em.SuperFriendly || emotion == Var.Em.SuperConfident || emotion == Var.Em.SuperLonely || emotion == Var.Em.SuperFriendly)
			return true;
		else
			return false;
	}

	public int Findfirendlieness(Bird bird)
	{        
		int x = 0;
		int y = 0;
		for (int i = 0; i < Var.playerPos.GetLength(0); i++)
		{
			for (int j = 0; j < Var.playerPos.GetLength(1); j++)
			{

				if (Var.playerPos[i, j] == bird)
				{
					x = i;
					y = j;
					break;
				}
			}
		}
		//TODO: Make these values global
		int sizeY = Var.playerPos.GetLength(1)-1;
		int sizeX = Var.playerPos.GetLength(0)-1;
		friendState state = friendState.alone;
		if (y + 1 <= sizeY && x + 1 <= sizeX && Var.playerPos[x + 1, y + 1] != null)
		{
			state = friendState.diagonal;
		}
		if (y + 1 <= sizeY && x - 1 >= 0 && Var.playerPos[x - 1, y + 1] != null)
		{
			state = friendState.diagonal;
		}
		if (y - 1 >= 0 && x + 1 <= sizeX && Var.playerPos[x + 1, y - 1] != null)
		{
			state = friendState.diagonal;
		}
		if (y - 1 >= 0 && x - 1 >= 0 && Var.playerPos[x - 1, y - 1] != null)
		{
			state = friendState.diagonal;
		}
		if (y+1<= sizeY && Var.playerPos[x, y + 1] != null)
		{
			if (state == friendState.oneFriend)
				state = friendState.twoFriends;
			else
				state = friendState.oneFriend;
		}
		if (y - 1 >=0 && Var.playerPos[x, y - 1] != null)
		{
			if (state == friendState.oneFriend)
				state = friendState.twoFriends;
			else
				state = friendState.oneFriend;
		}
		if (x + 1 <= sizeX && Var.playerPos[x+1, y] != null)
		{
			if (state == friendState.oneFriend)
				state = friendState.twoFriends;
			else
				state = friendState.oneFriend;
		}
		if (x - 1 >= 0 && Var.playerPos[x-1, y ] != null)
		{
			if (state == friendState.oneFriend)
				state = friendState.twoFriends;
			else
				state = friendState.oneFriend;
		}
		
		switch (state) {
			case friendState.alone:
				return Var.noFriendGain;
			case friendState.diagonal:
				return Var.friendDiagGain;
			case friendState.oneFriend:
				return Var.oneFriendStraightGain;
			case friendState.twoFriends:
				return Var.twoFriendStraightGain;
			default:
				return 0;
		}  

	}

	//Used to apply stat changes based on the emotion. first number is friendliness, second number is confidence
	public Vector2 ApplyEmotion(Vector2 currentStats, Var.Em emotion, int magnitude = 1)
	{
		if (emotion == Var.Em.Confident || emotion == Var.Em.SuperConfident)
			return new Vector2(currentStats.x, currentStats.y + magnitude);
		if (emotion == Var.Em.Cautious || emotion == Var.Em.SuperScared)
			return new Vector2(currentStats.x, currentStats.y - magnitude);
		if (emotion == Var.Em.Social || emotion == Var.Em.SuperFriendly)
			return new Vector2(currentStats.x + magnitude, currentStats.y );
		if (emotion == Var.Em.Solitary || emotion == Var.Em.SuperLonely)
			return new Vector2(currentStats.x - magnitude, currentStats.y);
		return currentStats;
	}



}
