﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour {
	public static ObstacleGenerator Instance { get; private set; }
	public List<LayoutButton> tiles = new List<LayoutButton>();
	GameObject rock= null;
	public GameObject powerTile;
	public GameObject LonelyTile;
	public GameObject FirendTile;
	public GameObject CourageTile;
	public GameObject ScaredTile;
	public GameObject healthTile;
	public GameObject dmgTile;
    public GameObject shieldTile;
    public GameObject BattleArea;
	public List<GameObject> obstacles = new List<GameObject>();
	private LevelTutorial lvlTut;
	// Use this for initialization
	void Awake()
	{
		Instance = this;
		lvlTut = GetComponent<LevelTutorial>();
	}
	void Start () {
		if(rock == null)
		{
			rock = Resources.Load<GameObject>("prefabs/rock");
		}
		GenerateObstacles();
	}
	public void clearObstacles()
	{
		foreach (LayoutButton tile in tiles)
		{
			tile.isActive = true;
		}
		for (int i = 0; i < obstacles.Count; i++)
		{
			Destroy(obstacles[i]);
		}
	}
	// Update is called once per frame
	public void GenerateObstacles()
	{
		if (Var.isTutorial)
		{
			/*if (Tutorial.Instance.CurrentPos == 4) {
				LayoutButton tile = tiles[7];
				Vector3 pos = new Vector3(tile.transform.position.x + 0.05f, tile.transform.position.y + 0.3f, 20);
				tile.isActive = false;
				GameObject rockObj = Instantiate(rock, pos, Quaternion.identity);
				rockObj.transform.parent = BattleArea.transform;
				tile.gameObject.SetActive(false);
				obstacles.Add(rockObj);
			}*/
			return;
		}
		bool[] enemyPositions = new bool[8];
		for(int i=0;i<fillEnemy.Instance.Enemies.Length;i++)
        {
			enemyPositions[i] = fillEnemy.Instance.Enemies[i].isActiveAndEnabled;
			//Debug.Log("bird: " + (i+1) + " active: " + fillEnemy.Instance.Enemies[i].isActiveAndEnabled);

        }
		//TODO: set rock probability with float
		//TODO: set max rock count
		for(int i=0;i<tiles.Count;i++)
		{
			float rand = Random.Range(0.0f, 1.0f);
			if (Var.map[GuiContoler.mapPos].hasRocks && rand > 0.9f)
			{          
				Vector3 pos = new Vector3(tiles[i].transform.position.x+0.05f, tiles[i].transform.position.y + 0.3f, 20);
				tiles[i].isActive = false;
				GameObject rockObj = Instantiate(rock, pos, Quaternion.identity);
				rockObj.transform.parent = BattleArea.transform;
				tiles[i].gameObject.SetActive(false);
				obstacles.Add(rockObj);              
			}
			if(rand>0.8f && rand < 0.9f && Var.map[GuiContoler.mapPos].powerUps.Count>0)
			{
				List<Var.Em> powerUps = Var.map[GuiContoler.mapPos].powerUps;
				Vector3 pos = new Vector3(tiles[i].transform.position.x, tiles[i].transform.position.y, 20);
				Var.Em emotion = powerUps[Random.Range(0, powerUps.Count)];
				GameObject obj;
				switch (emotion)
				{
					case Var.Em.Confident:
						obj = CourageTile;
						break;
					case Var.Em.Social:
						obj = FirendTile;
						break;
					case Var.Em.Solitary:
						obj = LonelyTile;
						break;
					case Var.Em.Cautious:
						obj = ScaredTile;
						break;
					default:
						obj = powerTile;
						break;
				}
				GameObject powerObj = Instantiate(obj, pos, Quaternion.identity);
				lvlTut.ShowEmoTileTutorial();
				powerObj.transform.parent = BattleArea.transform;
				tiles[i].power = powerObj.GetComponent<powerTile>();
				powerObj.GetComponent<powerTile>().SetColor(emotion);
				obstacles.Add(powerObj);
			}
            if(rand>0.65f && rand < 0.8f && Var.map[GuiContoler.mapPos].powers != null)
            {
               // Var.map[GuiContoler.mapPos].powers.Add(Var.PowerUps.shield);


                try
				{
					List<Var.PowerUps> pow = Var.map[GuiContoler.mapPos].powers;
					Var.PowerUps type = pow[Random.Range(0, pow.Count)];
					GameObject powerUp = null;
					Vector3 pos = new Vector3(tiles[i].transform.position.x, tiles[i].transform.position.y, 20);
					bool enemyInRow = false;

					for(int j =0;j<enemyPositions.Length;j++)
                    {
						if(j<4)
                        {
                            //column
							if(i%4 == j && enemyPositions[j])
                            {
								enemyInRow = true;
								//Debug.LogError("enemy: " + j + " tile: " + i);
								break;
							}

                        }
                        else
                        {
							//row
							if(i<(j-3)*4 && i>= (j - 4) * 4 && enemyPositions[j])
							{
								enemyInRow = true;
								//Debug.LogError("enemy: " + j + " tile: " + i);
								break;
							}

						}

                    }


					switch (type)
					{
						case Var.PowerUps.dmg:
							if (enemyInRow)
							{
								powerUp = Instantiate(dmgTile, pos, Quaternion.identity);
								lvlTut.ShowSwordTutorial();
							}
							break;
						case Var.PowerUps.heal:
							powerUp = Instantiate(healthTile, pos, Quaternion.identity);
							lvlTut.ShowHeartTutorial();
							break;
                        case Var.PowerUps.shield:
							if (enemyInRow)
							{
								powerUp = Instantiate(shieldTile, pos, Quaternion.identity);
								lvlTut.ShowShieldTutorial();
							}
                            break;

                    }
					powerUp.transform.parent = BattleArea.transform;
					obstacles.Add(powerUp);
					tiles[i].power = powerUp.GetComponent<powerTile>();
				}
				catch
				{
					Debug.Log("failed to add powerUp");
				}
			}
		}
	}
}
