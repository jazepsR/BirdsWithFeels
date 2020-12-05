using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class Levels : MonoBehaviour {
	public enum type { Alexander,Kim,Rebecca,Sophie, Terry,Friend1,Friend2,Brave1,Brave2,Lonely1,Lonely2,Scared1,Scared2, None};
	Bird myBird;	
	public GameObject Halo;
	public GameObject SadRest;
	public GameObject Rest;
	public Vector2 lastSwapPos = new Vector2(-2,-2);
	private bool TovaActivated = false;
	public GameObject impressionableIndicator;

	// Use this for initialization
	void Awake () {        
		myBird = GetComponent<Bird>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void ApplyStartLevel(Bird bird, List<LevelData> Levels)
	{

		foreach (LevelData data in Levels) {
			type level = data.type;
			switch (level)
			{
				case type.Rebecca:
					bird.confLoseOnRest = 2;
					break;
				case type.Kim:
					bird.groundMultiplier = 2;
					break;
				default:
					break;
			}


		}
	}

	public void ApplyLevelOnDrop(Bird bird)
	{

		if (GameLogic.Instance.CheckIfResting(myBird) && myBird.x >= 0)
		{
			myBird.GetComponentInChildren<Animator>().SetBool("rest", true);
            if (myBird.cautiousParticleObj == null)
            {
               // myBird.cautiousParticleObj = Instantiate(firendLine.cautiousParticles, transform);
                myBird.cautiousParticleObj.transform.localPosition = new Vector3(0.3f, 0, 0);
            }
        }
		else
        {
            myBird.GetComponentInChildren<Animator>().SetBool("rest", false);
            if (myBird.cautiousParticleObj != null)
                Destroy(myBird.cautiousParticleObj);
		}
        return;
		/*if (myBird.inMap || Var.isTutorial)
			return;
		//if (GameLogic.Instance.CheckIfResting(myBird) && myBird.x >= 0)
		//Rest.SetActive(true);

			foreach (LevelData data in Levels)
		{
			type level = data.type;
			switch (level)
			{
				case type.Lonely1:
					if ( myBird.x == -1)
						break;
					List<LayoutButton> tileCol = GetColumn();
					for (int i = 0; i < 4; i++)
					{						
						Color col = Helpers.Instance.GetEmotionColor(Var.Em.Solitary);
						col = new Color(col.r, col.g, col.b, 0.3f);
						tileCol[i].AddColor(col);						
						if (i != 0)
							tileCol[i].isInfluenced = true;
					}
					break;
				case type.Alexander:
					if (myBird.x == -1)
						break;
					List<Bird> adjacentBirds = Helpers.Instance.GetAdjacentBirds(myBird);
					if (adjacentBirds != null && adjacentBirds.Count > 0)
					{
						Var.Em emotion = Var.Em.Neutral;
						int emStr = 0;
						Bird biggestBird = null;
						foreach (Bird closeBird in adjacentBirds)
						{
							int emotionStr = (int)Mathf.Max(Mathf.Abs(closeBird.data.confidence), Mathf.Abs(closeBird.data.friendliness));
							if (emotionStr > emStr)
							{
								emotion = closeBird.emotion;
								emStr = emotionStr;
								biggestBird = closeBird;
							}
						}
						if (emotion != Var.Em.Neutral)
						{
							impressionableIndicator.SetActive(true);
							Vector3 moveDirection = biggestBird.transform.position - myBird.transform.position;
							float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg - 90f;
							impressionableIndicator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
							impressionableIndicator.GetComponent<SpriteRenderer>().color = Helpers.Instance.GetEmotionColor(emotion);
						}else
							impressionableIndicator.SetActive(false);
					}
					else
					{
						impressionableIndicator.SetActive(false);
					}
					break;
				//TODO: Maybe someday fix this skill
				/* case type.Lonely1:
					 if (myBird.x == -1)
						 break;
					 if (Var.enemies[myBird.y + 4].inUse)
					 {
						 if (!Var.enemies[myBird.y + 5].inUse)
						 {
							 Bird tempBird = Var.enemies[myBird.y + 4];
							 Var.enemies[myBird.y + 4] = Var.enemies[myBird.y + 5];
							 Var.enemies[myBird.y + 5] = tempBird;
							 LeanTween.moveLocal(tempBird.gameObject, Var.enemies[myBird.y + 4].home, 0.4f);
							 lastSwapPos = new Vector2(myBird.x, myBird.y);
							 Debug.Log("swapped on some num");
						 }                        
						 else
						 {
							 if (myBird.y == 3 && !Var.enemies[6].inUse)
							 {
								 Bird tempBird = Var.enemies[7];
								 Var.enemies[7] = Var.enemies[6];
								 Var.enemies[6] = tempBird;
								 //TODO: Get coordinates dynamically
								 LeanTween.moveLocal(tempBird.gameObject, new Vector3(1.73f,-1.22f,0f), 0.4f);
								 lastSwapPos = new Vector2(myBird.x, myBird.y);
								 Debug.Log("swapped on 3");
							 }
						 }
						 string bla = "";
						 foreach (Bird birdy in Var.enemies)
						 {
							 bla += birdy.birdBio + " ";
						 }
						//Debug.Log("Enemie ids after swap" + bla );
						 GameLogic.Instance.UpdateFeedback();

					 }
					 break;
				case type.Kim:
					if (myBird.x == -1)
						break;
					if(GameLogic.Instance.CheckIfResting(myBird))
						SadRest.SetActive(true);
					break;
				case type.Terry:
					if (myBird.x == -1 )
						break;
					List<LayoutButton> tileRow = GetRow();
					for(int i=0;i<tileRow.Count;i++)
					{
						Color col = Helpers.Instance.GetEmotionColor(Var.Em.Confident);
						col = new Color(col.r, col.g, col.b, 0.3f);
						tileRow[i].AddColor(col);
						if(i!=0)
							tileRow[i].ConfBonus = -1;
					}
					break;
				case type.Sophie:
					if (myBird.x == -1 || TovaActivated || !GameLogic.Instance.CheckIfResting(myBird))
						break;
					foreach (LayoutButton tile in GetAdjacent())
					{
						tile.PlayerRollBonus =+ 1;
					}
					Halo.SetActive(true);
					TovaActivated = true;
					break;
				case type.Scared1:
					if (myBird.x == -1 )
						break;
					List<LayoutButton> tileDiag = GetDiagonals();
					for (int i = 0; i < tileDiag.Count; i++)
					{
						Color col = Helpers.Instance.GetEmotionColor(Var.Em.Cautious);
						col = new Color(col.r, col.g, col.b, 0.3f);
						tileDiag[i].AddColor(col);
						if (myBird.x != tileDiag[i].index.x && myBird.y != tileDiag[i].index.y)
							tileDiag[i].PlayerRollBonus = -1;
					}
					List<Bird> enemies = GetDiagonalEnemies();
					foreach(Bird enemy in enemies)
					{
						
						var sprites = enemy.transform.GetComponentsInChildren<SpriteRenderer>();
						foreach (SpriteRenderer sp in sprites)
						{
							Color firstColor = sp.color;
							sp.color = new Color(firstColor.r - 0.3f, firstColor.g - 0.3f, firstColor.b - 0.3f);
						}                                                  
								 
						enemy.PlayerRollBonus = -1;
					}
					break;
				default:
					break;
			}
		}
		*/
	}

	public void ApplyLevelOnPickup(Bird bird, List<LevelDataScriptable> Levels)
	{
		if (myBird.inMap || Var.isTutorial)
			return;
        return;
		//Rest.SetActive(false);
		/*foreach (LevelData data in Levels)
		{
			type level = data.type;
			switch (level)
			{
				case type.Lonely1:
					if (myBird.x == -1)
						break;
					Color col = Helpers.Instance.GetEmotionColor(Var.Em.Solitary);
					col = new Color(col.r, col.g, col.b, 0.3f);
					foreach (LayoutButton tile in GetColumn())
					{
						tile.RemoveColor(col);
						tile.isInfluenced = false;
					}
					break;
				case type.Kim:
					if (myBird.x == -1)
						break;
					SadRest.SetActive(false);
					break;
				case type.Alexander:
					if (myBird.x == -1)
						break;
					impressionableIndicator.SetActive(false);
					break;
				case type.Terry:
					if (myBird.x == -1 )
						break;
					Color ColConf = Helpers.Instance.GetEmotionColor(Var.Em.Confident);
					ColConf = new Color(ColConf.r, ColConf.g, ColConf.b, 0.3f);
					foreach (LayoutButton tile in GetRow())
					{
						tile.RemoveColor(ColConf);
						tile.ConfBonus = 0;
					}
					break;
				case type.Scared1:
					if (myBird.x == -1 )
						break;
					Color colScared = Helpers.Instance.GetEmotionColor(Var.Em.Cautious);
					colScared = new Color(colScared.r, colScared.g, colScared.b, 0.3f);
					foreach (LayoutButton tile in GetDiagonals())
					{
						tile.RemoveColor(colScared);
						if (myBird.x != tile.index.x && myBird.y != tile.index.y)
							tile.PlayerRollBonus += 1;
					}
					List<Bird> enemies = GetDiagonalEnemies();
					foreach (Bird enemy in enemies)
					{
						
						var sprites = enemy.transform.GetComponentsInChildren<SpriteRenderer>();
						foreach (SpriteRenderer sp in sprites)
						{
							Color firstColor = sp.color;
							sp.color = new Color(firstColor.r + 0.3f, firstColor.g + 0.3f, firstColor.b + 0.3f);
						}
							
						enemy.PlayerRollBonus = 0;
						enemy.SetEmotion();
					}
					break;
				/*case type.Lonely1:
					if (myBird.x == -1 || !(new Vector2(myBird.x,myBird.y)).Equals(lastSwapPos))
						break;
					if (myBird.y == 3 && Var.enemies[6].inUse && !Var.enemies[7].inUse)
					{
						Bird tempBird = Var.enemies[6];
						Var.enemies[6] = Var.enemies[7];
						Var.enemies[7] = tempBird;
						LeanTween.moveLocal(tempBird.gameObject, tempBird.home, 0.4f);
					}
					else
					{

						if (Var.enemies[myBird.y + 5].inUse && !Var.enemies[myBird.y + 4].inUse)
						{
							Bird tempBird = Var.enemies[myBird.y + 5];
							Var.enemies[myBird.y + 5] = Var.enemies[myBird.y + 4];
							Var.enemies[myBird.y + 4] = tempBird;
							LeanTween.moveLocal(tempBird.gameObject, tempBird.home, 0.4f);

						}
					}
					string bla = "";
					foreach(Bird birdy in Var.enemies)
					{
						bla += birdy.birdBio + " ";
					}
					Debug.Log("Enemie ids after return swap"+bla  );
						
						GameLogic.Instance.UpdateFeedback();                       
					
					break;
				case type.Sophie:
					if (myBird.x == -1|| !TovaActivated)
						break;
					foreach (LayoutButton tile in GetAdjacent())
					{
						tile.PlayerRollBonus -= 1;
					}
					TovaActivated = false;
					Halo.SetActive(false);
					break;
				default:
					break;
			}
		}*/



	}

	public void OnfightEndLevel(Bird bird, List<LevelDataScriptable> Levels)
	{
		lastSwapPos = new Vector2(-2, -2);
        return;
		//Rest.SetActive(false);
		/*foreach (LevelDataScriptable data in Levels)
		{
			type level = data.type;
			List<Bird> adjacent = null;
			switch (level)
			{
				case type.Alexander:
					impressionableIndicator.SetActive(false);
					adjacent = Helpers.Instance.GetAdjacentBirds(myBird);
					if (adjacent.Count > 0)
					{
						Var.Em emotion = Var.Em.Neutral;
						int emStr = 0;
						foreach(Bird closeBird in adjacent)
						{
							int emotionStr = (int)Mathf.Max(Mathf.Abs(closeBird.data.confidence), Mathf.Abs(closeBird.data.friendliness));
							if (emotionStr > emStr)
							{
								emotion = closeBird.emotion;
								emStr = emotionStr;
							}
						}
						if (emotion != Var.Em.Neutral)
						{
							Vector2 newEmotions = Helpers.Instance.ApplyEmotion(new Vector2(myBird.levelFriendBoos, myBird.levelConfBoos), emotion);
							myBird.levelFriendBoos = (int)newEmotions.x;
							myBird.levelConfBoos = (int)newEmotions.y;
							Helpers.Instance.EmitEmotionParticles(myBird.transform, emotion);
						}
					}
					break;
				case type.Friend1:
					adjacent = Helpers.Instance.GetAdjacentBirds(myBird);
					if(!bird.foughtInRound && adjacent.Count > 0)
					{
						Bird healBird = adjacent[UnityEngine.Random.Range(0, adjacent.Count - 1)];
						healBird.ChageHealth(+1);
					}
					break;
				case type.Sophie:
					TovaActivated = false;
					break;
				case type.Kim:
					if (myBird.x == -1)
						break;
					SadRest.SetActive(false);
					break;
				default:
					break;
			}
		}*/
	}

	public List<LayoutButton> GetRow()
	{
		int x = myBird.x;
		int y = myBird.y;
		List<LayoutButton> list = new List<LayoutButton>();
		for (int i = 0; i < 4; i++)
		{
			int index = y * 4 + (x + i) % 4;
			list.Add(ObstacleGenerator.Instance.tiles[index]);
		}
		return list;
	}

	public List<LayoutButton> GetColumn()
	{
		int x = myBird.x;
		int y = myBird.y;
		List<LayoutButton> list = new List<LayoutButton>();
		for (int i = 0; i < 4; i++)
		{
			int index = (y+i)%4 * 4 + x;
			list.Add(ObstacleGenerator.Instance.tiles[index]);
		}
		return list;
	}

	int GetColumnCount()
	{
		int count = 0;
		foreach(LayoutButton btn in GetColumn())
		{
			if (btn.hasBird)
				count++;
		}
		return count;
	}

	int GetRowCount()
	{
		int count = 0;
		foreach (LayoutButton btn in GetRow())
		{
			if (btn.hasBird)
				count++;
		}
		return count;
	}

	public List<LayoutButton> GetAdjacent()
	{
		int x = myBird.x;
		int y = myBird.y;
		int sizeY = Var.playerPos.GetLength(1) - 1;
		int sizeX = Var.playerPos.GetLength(0) - 1;
		List<LayoutButton> list = new List<LayoutButton>();
		List<LayoutButton> tiles = ObstacleGenerator.Instance.tiles;
		if (y + 1 <= sizeY )
		{
			list.Add(tiles[(y + 1) * 4 + x]);
		}
		if (y - 1 >= 0 )
		{
			list.Add(tiles[(y -1) * 4 + x]);
		}
		if (x + 1 <= sizeX )
		{
			list.Add(tiles[(y) * 4 + x+1]);
		}
		if (x - 1 >= 0)
		{
			list.Add(tiles[(y) * 4 + x-1]);
		}
		if (y + 1 <= sizeY && x + 1 <= sizeX )
		{
			list.Add(tiles[(y + 1) * 4 + x + 1]);
		}
		if (y + 1 <= sizeY && x - 1 <= sizeX)
		{
			list.Add(tiles[(y + 1) * 4 + x - 1]);
		}
		if (y - 1 >= 0 && x + 1 <= sizeX)
		{
			list.Add(tiles[(y - 1) * 4 + x + 1]);
		}
	   
		if (y - 1 >= 0 && x - 1 >= 0)
		{
			list.Add(tiles[(y - 1) * 4 + x - 1]);
		}

		//Debug.Log(list.Count);
		return list;

	}

	public List<LayoutButton> GetDiagonals()
	{
		int x = myBird.x;
		int y = myBird.y;
		//int sizeY = Var.playerPos.GetLength(1) - 1;
		//int sizeX = Var.playerPos.GetLength(0) - 1;
		List<LayoutButton> list = new List<LayoutButton>();
		for (int i = -3; i < 4; i++)
		{
			int index = (y + i) % 4 * 4 + (x+i)%4;                          
				if( (y+i)<4 && (x+i)<4 && (x+i)>=0 && (y+i)>=0)
					list.Add(ObstacleGenerator.Instance.tiles[index]);           
			index = (y + i) % 4 * 4 + (x - i)%4;            
				if ((y + i) < 4 && (x - i) < 4 && (x - i) >= 0 && (y + i) >= 0)
					list.Add(ObstacleGenerator.Instance.tiles[index]);            
		}
		return list;

	}


	public List<Bird> GetDiagonalEnemies()
	{
		List<Bird> list=  new List<Bird>();
		int x = myBird.x;
		int y = myBird.y;
		//top
		if ((x - (y + 1)) >= 0)
			list.Add(Var.enemies[x - (y + 1)]);
		if ((x + (y+ 1)) <= 3)
			list.Add(Var.enemies[x + (y + 1)]);
		//front
		if(x+y>=4)
		   list.Add(Var.enemies[x + y]);
		if(x + y + 2 * (4 - x) <=7)
			list.Add(Var.enemies[x+y + 2*(4-x)]);	   
		return list;
	}





	//Level triggers
	public static void ApplyLevel(LevelDataScriptable data , Bird bird)
	{
			//GuiContoler.Instance.ShowMessage(Helpers.Instance.GetLevelUpText(myBird.charName,data.type));
			bird.AddLevel(data);
			levelPopupScript.Instance.Setup(bird, bird.data.lastLevel);
			AudioControler.Instance.PlaySound(AudioControler.Instance.applause);
	}

	/*public string CheckBrave1(bool tryingToApply = true)
	{
		if ( (myBird.emotion == Var.Em.Confident || myBird.emotion == Var.Em.SuperConfident) && myBird.confidence >= 7 && myBird.bannedLevels != Var.Em.Confident)
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Brave1, myBird.levelList) && Var.Em.Confident != myBird.bannedLevels)
			{
				return "First "+ Helpers.Instance.BraveHexColor + "brave</color> level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Brave1);
			}
			if (myBird.winsInOneFight > 1 && !Helpers.Instance.ListContainsLevel(type.Brave1, myBird.levelList))
			{
				ApplyLevel(new LevelData(type.Brave1, Var.Em.Confident, Var.lvlSprites[1]), "Brave 1");
				return "did LVL";
			}
		}
		return null;

	}
	public string CheckBrave2(bool tryingToApply = true)
	{
		if ( (myBird.emotion == Var.Em.Confident || myBird.emotion == Var.Em.SuperConfident) && myBird.confidence >= 10 && myBird.bannedLevels != Var.Em.Confident)
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Brave2, myBird.levelList) && Var.Em.Confident != myBird.bannedLevels)
			{
				return "Second " + Helpers.Instance.BraveHexColor + "brave</color> level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Brave2) + " Currently won: " + myBird.consecutiveFightsWon + " fights in a row";
			}
			if (myBird.consecutiveFightsWon >= 4  && Helpers.Instance.ListContainsLevel(Levels.type.Brave1,myBird.levelList) && !Helpers.Instance.ListContainsLevel(type.Brave2, myBird.levelList))
			{
				ApplyLevel(new LevelData(type.Brave2, Var.Em.Confident, Var.lvlSprites[5]), "Brave 2");
				return "did LVL";
			}
		}
		return null;
	}
	public string CheckLonely1(bool tryingToApply = true)
	{
		if ( (myBird.emotion == Var.Em.Solitary || myBird.emotion == Var.Em.SuperLonely) && myBird.friendliness <= -7 && myBird.bannedLevels != Var.Em.Solitary)
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Lonely1, myBird.levelList) && Var.Em.Solitary != myBird.bannedLevels)
			{
				return "First " + Helpers.Instance.LonelyHexColor + "Lonely</color> level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Lonely1);
			}
			print("Rowcount: " + GetRowCount() + " ColumnCount: " + GetColumnCount() + " name: " + myBird.charName);
			if (GetRowCount()==1 && GetColumnCount() == 1 && myBird.wonLastBattle>=1 && !Helpers.Instance.ListContainsLevel(type.Lonely1, myBird.levelList))
			{
				ApplyLevel(new LevelData(type.Lonely1, Var.Em.Solitary, Var.lvlSprites[3]), "Lonely 1");
				return "did LVL";
			}            
		}
		return null;
	}

	public string CheckLonely2(bool tryingToApply = true)
	{
		if ((myBird.emotion == Var.Em.Solitary || myBird.emotion == Var.Em.SuperLonely) && myBird.friendliness <= -10 && myBird.bannedLevels != Var.Em.Solitary)
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Lonely2, myBird.levelList) && Var.Em.Solitary != myBird.bannedLevels)
			{
				return "Second " + Helpers.Instance.LonelyHexColor + "Lonely</color> level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Lonely2);
			}
			if (Helpers.Instance.GetAdjacentBirds(myBird) != null && Helpers.Instance.GetAdjacentBirds(myBird).Count == 0 && myBird.AdventuresRested>=2 && Helpers.Instance.ListContainsLevel(Levels.type.Lonely1, myBird.levelList) && !Helpers.Instance.ListContainsLevel(type.Lonely2, myBird.levelList))
			{
				ApplyLevel(new LevelData(type.Lonely2, Var.Em.Solitary, Var.lvlSprites[7]),"Lonely 2");
				return "did LVL";
			}
		}
		return null;
	}
	
	public string CheckFriendly1(bool tryingToApply = true)
	{
		if ((myBird.emotion == Var.Em.Social || myBird.emotion == Var.Em.SuperFriendly) && myBird.friendliness >= 7 && myBird.bannedLevels != Var.Em.Social)
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Friend1, myBird.levelList) && Var.Em.Social != myBird.bannedLevels)
			{
				return "First " + Helpers.Instance.FriendlyHexColor + "Friendly</color> level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Friend1);
			}
			int gain = myBird.friendBoost + myBird.groundFriendBoos + myBird.levelFriendBoos + myBird.wizardFrienBoos + (int)myBird.ApplyInfluence(false).y; ;
			if (gain>= 4  && myBird.wonLastBattle >=1 && !Helpers.Instance.ListContainsLevel(type.Friend1, myBird.levelList))
			{
				ApplyLevel(new LevelData(type.Friend1, Var.Em.Social, Var.lvlSprites[0]),"Friendly 1");
				return "did LVL";
			}
		}
		return null;

	}

	public string CheckFriendly2(bool tryingToApply = true)
	{
		if ((myBird.emotion == Var.Em.Social || myBird.emotion == Var.Em.SuperFriendly) && myBird.friendliness >= 10 && myBird.bannedLevels != Var.Em.Social)
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Friend2, myBird.levelList) && Var.Em.Social != myBird.bannedLevels)
			{
				return "Second "+Helpers.Instance.FriendlyHexColor +"Friendly</color> level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Friend2);
			}
			bool closeFriend = false;
			List<Bird> birds = Helpers.Instance.GetAdjacentBirds(myBird);
			if (birds != null && birds.Count>0)
			{
				foreach (Bird bird in birds)
				{
					if (bird.emotion == Var.Em.Social || bird.emotion == Var.Em.SuperFriendly && Helpers.Instance.ListContainsLevel(Levels.type.Friend1, myBird.levelList))
					{
						closeFriend = true;
						break;
					}
				}
			}
			int gain = myBird.friendBoost + myBird.groundFriendBoos + myBird.levelFriendBoos + myBird.wizardFrienBoos + (int)myBird.ApplyInfluence(false).y;
			if (closeFriend && gain >= 5 && !Helpers.Instance.ListContainsLevel(type.Friend2, myBird.levelList))
			{
				ApplyLevel(new LevelData(type.Friend1, Var.Em.Social, Var.lvlSprites[4]),"Friendly 2");
				return "did LVL";
			}
		}
		return null;

	}

	public string CheckScared1(bool tryingToApply = true)
	{
		if ((myBird.emotion == Var.Em.Cautious || myBird.emotion == Var.Em.SuperScared) && myBird.confidence <= -7 && myBird.bannedLevels != Var.Em.Cautious)
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Scared1, myBird.levelList) && Var.Em.Cautious != myBird.bannedLevels)
				return "First " +Helpers.Instance.ScaredHexColor +"Scared </color>level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Scared1);
			bool closeWinner = false;

			List<Bird> birds = Helpers.Instance.GetAdjacentBirds(myBird);
			if (birds != null && birds.Count>0)
			{
				foreach (Bird bird in birds)
				{
					if (bird.wonLastBattle >= 1)
					{
						closeWinner = true;
						break;
					}
				}
			}

			if ((closeWinner &&myBird.wonLastBattle == 0) || (closeWinner && myBird.wonLastBattle == 2) && !Helpers.Instance.ListContainsLevel(type.Scared1, myBird.levelList))
			{
				ApplyLevel(new LevelData(type.Scared1, Var.Em.Cautious, Var.lvlSprites[2]),"Scared 1");
				return "did LVL";
			}
		}
		return null;

	}
	public string CheckScared2(bool tryingToApply = true)
	{
		if ( (myBird.emotion == Var.Em.Cautious || myBird.emotion == Var.Em.SuperScared) && myBird.confidence <= -10 && myBird.bannedLevels != Var.Em.Cautious)
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Scared2, myBird.levelList) && Var.Em.Cautious != myBird.bannedLevels)
			{
				return "Second " +Helpers.Instance.ScaredHexColor +"Scared</color> level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Scared2);
			}
			if (myBird.roundsRested >= 3  && !Helpers.Instance.ListContainsLevel(type.Scared2, myBird.levelList))
			{
				ApplyLevel(new LevelData(type.Scared2, Var.Em.Cautious, Var.lvlSprites[6]),"Scared 2");
				return "did LVL";
			}
		}
		return null;
	}*/

}
[Serializable]
public class LevelData{
	public Levels.type type;
	public Var.Em emotion;
	[NonSerialized]
	public Sprite LVLIcon;
	public string levelInfo;
	public string title;

   public LevelData(Levels.type type, Var.Em emotion, Sprite icon)
	{
		this.emotion = emotion;
		LVLIcon = icon;
		this.type = type;
		levelInfo ="<b>" + Helpers.Instance.GetLevelTitle(type)+"</b>\n"+ Helpers.Instance.GetLVLInfoText(type);
		title = Helpers.Instance.GetLVLTitle(type);
	}



}
