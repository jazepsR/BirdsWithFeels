using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Levels : MonoBehaviour {
	public enum type { Toby,Kim,Rebecca,Tova, Terry,Friend1,Friend2,Brave1,Brave2,Lonely1,Lonely2,Scared1,Scared2};
	Bird myBird;
	List<LevelData> LevelList;
	public GameObject Halo;
	public Vector2 lastSwapPos = new Vector2(-2,-2);
	private bool TovaActivated = false;
	// Use this for initialization
	void Start () {        
		myBird = GetComponent<Bird>();
		LevelList = myBird.levelList;
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

	public void ApplyLevelOnDrop(Bird bird, List<LevelData> Levels)
	{
		if (myBird.inMap)
			return;
		foreach (LevelData data in Levels)
		{
			type level = data.type;
			switch (level)
			{
				case type.Lonely1:
					if (myBird.x == -1)
						break;
					List<LayoutButton> tileCol = GetColumn();
					for (int i = 0; i < 4; i++)
					{
						Color col = Helpers.Instance.GetEmotionColor(Var.Em.Lonely);
						col = new Color(col.r, col.g, col.b, 0.3f);
						tileCol[i].SetColor(col,false);
						if(i!=0)
							tileCol[i].FriendBonus = -1;

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
					break;*/
				case type.Terry:
					if (myBird.x == -1)
						break;
					List<LayoutButton> tileRow = GetRow();
					for(int i=0;i<tileRow.Count;i++)
					{
						Color col = Helpers.Instance.GetEmotionColor(Var.Em.Confident);
						col = new Color(col.r, col.g, col.b, 0.3f);
						tileRow[i].SetColor(col,true);
						if(i!=0)
							tileRow[i].ConfBonus = -1;
					}
					break;
				case type.Tova:
					if (myBird.x == -1 || TovaActivated)
						break;
					foreach (LayoutButton tile in GetAdjacent())
					{
						tile.PlayerRollBonus =+ 1;
					}
					Halo.SetActive(true);
					TovaActivated = true;
					break;
				case type.Scared1:
					if (myBird.x == -1)
						break;
					List<LayoutButton> tileDiag = GetDiagonals();
					for (int i = 0; i < tileDiag.Count; i++)
					{
						Color col = Helpers.Instance.GetEmotionColor(Var.Em.Scared);
						col = new Color(col.r, col.g, col.b, 0.3f);
						tileDiag[i].SetColor(col, true);
						if (myBird.x != tileDiag[i].index.x && myBird.y != tileDiag[i].index.y)
							tileDiag[i].PlayerRollBonus = -1;
					}
					List<Bird> enemies = GetDiagonalEnemies();
					foreach(Bird enemy in enemies)
					{
						Color firstColor = enemy.colorRenderer.color;
						enemy.colorRenderer.color = new Color(firstColor.r - 0.3f, firstColor.g - 0.3f, firstColor.b - 0.3f);
						enemy.PlayerRollBonus = -1;
					}
					break;
				default:
					break;
			}
		}
	}

	public void ApplyLevelOnPickup(Bird bird, List<LevelData> Levels)
	{
		if (myBird.inMap)
			return;
		foreach (LevelData data in Levels)
		{
			type level = data.type;
			switch (level)
			{
				case type.Lonely1:
					if (myBird.x == -1)
						break;
					foreach (LayoutButton tile in GetColumn())
					{
						tile.ResetColor(false);
						tile.FriendBonus = 0;
					}
					break;
				case type.Terry:
					if (myBird.x == -1)
						break;
					foreach (LayoutButton tile in GetRow())
					{
						tile.ResetColor(true);
						tile.ConfBonus = 0;
					}
					break;
				case type.Scared1:
					if (myBird.x == -1)
						break;
					foreach (LayoutButton tile in GetDiagonals())
					{
						tile.ResetColor(true);
						if (myBird.x != tile.index.x && myBird.y != tile.index.y)
							tile.PlayerRollBonus += 1;
					}
					List<Bird> enemies = GetDiagonalEnemies();
					foreach (Bird enemy in enemies)
					{
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
					
					break;*/
				case type.Tova:
					if (myBird.x == -1 || !TovaActivated)
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
		}



	}

	public void OnfightEndLevel(Bird bird, List<LevelData> Levels)
	{
		lastSwapPos = new Vector2(-2, -2);
		foreach (LevelData data in Levels)
		{
			type level = data.type;
			List<Bird> adjacent = null;
			switch (level)
			{
				case type.Toby:
					adjacent = Helpers.Instance.GetAdjacentBirds(myBird);
					if (adjacent.Count > 0)
					{
						Var.Em emotion = Var.Em.Neutral;
						int emStr = 0;
						foreach(Bird closeBird in adjacent)
						{
							int emotionStr = (int)Mathf.Max(Mathf.Abs(closeBird.confidence), Mathf.Abs(closeBird.friendliness));
							if (emotionStr > emStr)
							{
								emotion = closeBird.emotion;
								emStr = emotionStr;
							}
						}
						if (emotion != Var.Em.Neutral)
						{
							Vector2 newEmotions = Helpers.Instance.ApplyEmotion(new Vector2(myBird.friendBoost, myBird.confBoos), emotion);
							myBird.friendBoost = (int)newEmotions.x;
							myBird.confBoos = (int)newEmotions.y;
							Helpers.Instance.EmitEmotionParticles(myBird.transform, emotion);
						}
					}
					break;
				case type.Friend1:
					adjacent = Helpers.Instance.GetAdjacentBirds(myBird);
					if(!bird.foughtInRound && adjacent.Count > 0)
					{
						Bird healBird = adjacent[Random.Range(0, adjacent.Count - 1)];
						healBird.ChageHealth(+1);
					}
					break;
				case type.Tova:
					TovaActivated = false;
					break;

				default:
					break;
			}
		}
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
		int sizeY = Var.playerPos.GetLength(1) - 1;
		int sizeX = Var.playerPos.GetLength(0) - 1;
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
	   
		//Bottom
		if(x+y+4>=8)
			list.Add(Var.enemies[x + y+4]);
		if(x + (4 - y)+8 > 8 && x + (4 - y)+8 <12)
			list.Add(Var.enemies[x + y + 2 * (4 - x)]);
		return list;
	}





	//Level triggers
	public void ApplyLevel(LevelData data, string levelName)
	{
		if (!Helpers.Instance.ListContainsLevel(data.type, LevelList))
		{
			GuiContoler.Instance.ShowMessage(Helpers.Instance.GetLevelUpText(myBird.charName,data.type));
			myBird.battleCount = myBird.battlesToNextLVL-1;
			myBird.AddLevel(data);
			Debug.Log(myBird.charName + " Reached level " + levelName);
		}
	}

	public string CheckBrave1(bool tryingToApply = true)
	{
		if (myBird.lastLevel.emotion != Var.Em.Confident && (myBird.emotion == Var.Em.Confident || myBird.emotion == Var.Em.SuperConfident) && myBird.confidence >= 7)
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Brave1, LevelList))
			{
				return "First "+ Helpers.Instance.BraveHexColor + "brave</color> level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Brave1);
			}
			if (myBird.winsInOneFight > 1 )
			{
				ApplyLevel(new LevelData(type.Brave1, Var.Em.Confident,Resources.Load<Sprite>("Icons/Brave1Icon")), "Brave 1");
			}
		}
		return null;

	}
	public string CheckBrave2(bool tryingToApply = true)
	{
		if (myBird.lastLevel.emotion != Var.Em.Confident && (myBird.emotion == Var.Em.Confident || myBird.emotion == Var.Em.SuperConfident) && myBird.confidence >= 10)
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Brave2, LevelList))
			{
				return "Second " + Helpers.Instance.BraveHexColor + "brave</color> level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Brave2);
			}
			if (myBird.consecutiveFightsWon >= 6  && Helpers.Instance.ListContainsLevel(Levels.type.Brave1,myBird.levelList))
			{
				ApplyLevel(new LevelData(type.Brave2, Var.Em.Confident, Resources.Load<Sprite>("Icons/Brave2Icon")), "Brave 2");
			}
		}
		return null;
	}
	public string CheckLonely1(bool tryingToApply = true)
	{
		if (myBird.lastLevel.emotion != Var.Em.Lonely && (myBird.emotion == Var.Em.Lonely || myBird.emotion == Var.Em.SuperLonely) && myBird.friendliness <= -7 )
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Lonely1, LevelList))
			{
				return "First " + Helpers.Instance.LonelyHexColor + "Lonely</color> level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Lonely1);
			}
			if (  Helpers.Instance.GetAdjacentBirds(myBird).Count == 0 && myBird.wonLastBattle>=1)
			{
				ApplyLevel(new LevelData(type.Lonely1, Var.Em.Lonely, Resources.Load<Sprite>("Icons/Lonely1Icon")), "Lonely 1");
			}            
		}
		return null;
	}

	public string CheckLonely2(bool tryingToApply = true)
	{
		if (myBird.lastLevel.emotion != Var.Em.Lonely && (myBird.emotion == Var.Em.Lonely || myBird.emotion == Var.Em.SuperLonely) && myBird.friendliness <= -10)
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Lonely2, LevelList))
			{
				return "Second " + Helpers.Instance.LonelyHexColor + "Lonely</color> level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Lonely2);
			}
			if (Helpers.Instance.GetAdjacentBirds(myBird).Count == 0 && myBird.AdventuresRested>=2 && Helpers.Instance.ListContainsLevel(Levels.type.Lonely1, myBird.levelList))
			{
				ApplyLevel(new LevelData(type.Lonely2, Var.Em.Lonely, Resources.Load<Sprite>("Icons/Lonely2Icon")),"Lonely 2");
			}
		}
		return null;
	}
	
	public string CheckFriendly1(bool tryingToApply = true)
	{
		if (myBird.lastLevel.emotion != Var.Em.Friendly && (myBird.emotion == Var.Em.Friendly || myBird.emotion == Var.Em.SuperFriendly) && myBird.friendliness >= 7)
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Friend1, LevelList))
			{
				return "First " + Helpers.Instance.FriendlyHexColor + "Friendly</color> level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Friend1);
			}
			if (myBird.FriendGainedInRound >= 3  && myBird.wonLastBattle >=1 )
			{
				ApplyLevel(new LevelData(type.Friend1, Var.Em.Friendly, Resources.Load<Sprite>("Icons/Friendly1Icon")),"Friendly 1");
			}
		}
		return null;

	}

	public string CheckFriendly2(bool tryingToApply = true)
	{
		if (myBird.lastLevel.emotion != Var.Em.Friendly && (myBird.emotion == Var.Em.Friendly || myBird.emotion == Var.Em.SuperFriendly) && myBird.friendliness >= 10)
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Friend2,LevelList))
			{
				return "Second "+Helpers.Instance.FriendlyHexColor +"Friendly</color> level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Friend2);
			}
			bool closeFriend = false;
			foreach(Bird bird in Helpers.Instance.GetAdjacentBirds(myBird))
			{
				if(bird.emotion == Var.Em.Friendly || bird.emotion == Var.Em.SuperFriendly && Helpers.Instance.ListContainsLevel(Levels.type.Friend1, myBird.levelList))
				{
					closeFriend = true;
					break;
				}
			}
			if (closeFriend && myBird.FriendGainedInRound >= 5 )
			{
				ApplyLevel(new LevelData(type.Friend1, Var.Em.Friendly, Resources.Load<Sprite>("Icons/Friendly2Icon")),"Friendly 2");
			}
		}
		return null;

	}

	public string CheckScared1(bool tryingToApply = true)
	{
		if (myBird.lastLevel.emotion != Var.Em.Scared && (myBird.emotion == Var.Em.Scared || myBird.emotion == Var.Em.SuperScared) && myBird.confidence <= -7)
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Scared1, LevelList))
				return "First " +Helpers.Instance.ScaredHexColor +"Scared </color>level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Scared1);
			bool closeWinner = false;
			foreach (Bird bird in Helpers.Instance.GetAdjacentBirds(myBird))
			{
				if (bird.wonLastBattle>=1)
				{
					closeWinner = true;
					break;
				}
			}

			if ((closeWinner &&myBird.wonLastBattle == 0) || (closeWinner && myBird.wonLastBattle == 2))
			{
				ApplyLevel(new LevelData(type.Scared1, Var.Em.Scared, Resources.Load<Sprite>("Icons/Scared1Icon")),"Scared 1");
			}
		}
		return null;

	}
	public string CheckScared2(bool tryingToApply = true)
	{
		if (myBird.lastLevel.emotion != Var.Em.Scared && (myBird.emotion == Var.Em.Scared || myBird.emotion == Var.Em.SuperScared) && myBird.confidence <= -10)
		{
			if (!tryingToApply && !Helpers.Instance.ListContainsLevel(type.Scared2, LevelList))
			{
				return "Second " +Helpers.Instance.ScaredHexColor +"Scared</color> level available! To level up: " + Helpers.Instance.GetLVLRequirements(type.Scared2);
			}
			if (myBird.roundsRested >= 4  && Helpers.Instance.ListContainsLevel(Levels.type.Scared2, myBird.levelList))
			{
				ApplyLevel(new LevelData(type.Scared2, Var.Em.Scared, Resources.Load<Sprite>("Icons/Scared2Icon")),"Scared 2");
			}
		}
		return null;
	}

}

public class LevelData{
	public Levels.type type;
	public Var.Em emotion;
	public Sprite LVLIcon;
	public string levelInfo;
	public string title;

   public LevelData(Levels.type type, Var.Em emotion, Sprite icon)
	{
		this.emotion = emotion;
		LVLIcon = icon;
		this.type = type;
		levelInfo = Helpers.Instance.GetLVLInfoText(type);
		title = Helpers.Instance.GetLVLTitle(type);
	}



}
