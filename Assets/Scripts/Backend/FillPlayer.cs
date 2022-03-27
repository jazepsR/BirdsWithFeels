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
		Var.activeBirds = new List<Bird>();
		if (Var.activeBirds.Count < 1)
		{
			Var.activeBirds.AddRange(playerBirds);
		}
			else if(!inMap)
		{
			for (int i = 0; i < 3; i++)
			{
				playerBirds[i].charName = Var.activeBirds[i].charName;
				Var.activeBirds[i] = playerBirds[i];
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
}
