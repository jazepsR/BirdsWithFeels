using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feedBack : MonoBehaviour {
	public TextMesh feedBackText;
	public bool isMain = true;
	public TextMesh LvlIndicatorText;
	public SpriteRenderer BelowBirdIndicator;
	public Bird birdScript;
	[HideInInspector]
	public Bird PlayerEnemyBird = null;
	Bird.dir dir;
	public float hideBonus;
	public float hideVal = 0.3f;
	public int myIndex;
	string toolTipText;
	public battleFeedback myBattleFeedback;
	Vector3 scale;
	[HideInInspector]
	public int feedbackVal;
	GameObject line;
	GameObject lineObj = null;
	//float factor = 0.35f;
	bool canFight = false;
	// Use this for initialization
	void Awake () {
		line = Resources.Load<GameObject>("prefabs/lightningLine");
		feedBackText.gameObject.GetComponent<Renderer>().sortingLayerName = "front";    
		if(isMain)
			scale= BelowBirdIndicator.transform.localScale;
		//myBattleFeedback = feedBackText.gameObject.GetComponent<battleFeedback>();
		myBattleFeedback.fb = this;		
		dir = birdScript.position;
		feedBackText.transform.localScale = Vector3.zero;       
	}

	//checks if the enemy has an opponent
	public bool CheckOpponent()
	{
		bool canfight = false;
		if (birdScript.enemyType == fillEnemy.enemyType.drill)
		{
			int enemyCount = 0;
			if (dir == Bird.dir.front)
			{
				for (int i = 0; i < 4; i++)
				{
					if (Var.playerPos[i, myIndex] != null)
					{
						if (!Var.playerPos[i, myIndex].isHiding)
						{
							enemyCount++;
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					if (Var.playerPos[myIndex, i] != null)
					{
						if (!Var.playerPos[myIndex, i].isHiding)
						{
							enemyCount++;
						}
					}

				}
			}
			if (enemyCount >= 2)
				canfight = true;
		}
		else
		{
			if (dir == Bird.dir.front)
			{
				for (int i = 0; i < 4; i++)
				{
					if (Var.playerPos[i, myIndex] != null)
					{
						if (!Var.playerPos[i, myIndex].isHiding)
						{
							canfight = true;
							break;
						}
					}
				}
			}
			else
			{
				for (int i = 0; i < 4; i++)
				{
					if (Var.playerPos[myIndex, i] != null)
					{
						if (!Var.playerPos[myIndex, i].isHiding)
						{
							canfight = true;
							break;
						}
					}

				}
			}
		}
		return canfight;
	}
	public void HighlightEnemy()
	{
		Debug.Log("highliting");
		birdScript.GetComponentInChildren<Animator>().SetTrigger("highlight");
	   /*foreach(SpriteRenderer sp in birdScript.GetComponentsInChildren<SpriteRenderer>())
		{
			LeanTween.color(sp.gameObject, new Color(sp.color.r + factor, sp.color.g + factor, sp.color.b - factor*2), 0.25f).setEaseOutBack();
			LeanTween.delayedCall(0.45f,DeHighlight);
		}*/
	}
	/*void DeHighlight()
	{
		foreach (SpriteRenderer sp in birdScript.GetComponentsInChildren<SpriteRenderer>())
		{
			LeanTween.color(sp.gameObject, new Color(sp.color.r - factor, sp.color.g - factor, sp.color.b + factor * 2), 0.25f);
		}
	}*/
	public void TryWizardLine(Bird player, Bird enemy)
	{
		if (lineObj != null)
			return;
		if (enemy.enemyType != fillEnemy.enemyType.wizard ) 
			return;
		if(enemy.emotion == Var.Em.Neutral)
			return;        
		switch (enemy.emotion)
		{
			case Var.Em.Solitary:
				player.wizardFrienBoos -= 4;
				break;
			case Var.Em.Social:
				player.wizardFrienBoos += 4;
				break;
			case Var.Em.Cautious:
				player.wizardConfBoos -= 4;
				break;
			case Var.Em.Confident:
				player.wizardConfBoos += 4;
				break;
			default:
				break;
		}
		lineObj = Instantiate(line);
		LineRenderer lr = lineObj.GetComponent<LineRenderer>();
		lr.sortingOrder = 0;
		lr.SetPosition(0, player.target);
		lr.SetPosition(1, enemy.target);
		lr.startColor = Helpers.Instance.GetEmotionColor(enemy.emotion);
		lr.endColor = Helpers.Instance.GetEmotionColor(enemy.emotion);
		lr.gameObject.SetActive(true);
		player.lines.activeLines.Add(lineObj);

	}
	public void HighlightTutorialTiles()
	{
		Color col;
		if (canFight)
			col = ObstacleGenerator.Instance.tiles[0].defaultColor;
		else
			col = ObstacleGenerator.Instance.tiles[0].highlightColor;

		if (dir == Bird.dir.top)
		{
			for (int i = 0; i < 4; i++)
			{
				LeanTween.color(ObstacleGenerator.Instance.tiles[i * 4 + myIndex].gameObject, col, 0.3f);
				ObstacleGenerator.Instance.tiles[i * 4 + myIndex].baseColor = col;
			}
		}else
		{
			for (int i = 0; i < 4; i++)
			{
				LeanTween.color(ObstacleGenerator.Instance.tiles[myIndex * 4 + i].gameObject, col, 0.3f);
				ObstacleGenerator.Instance.tiles[myIndex * 4 + i].baseColor = col;
			}
		}

	}
	public void RefreshFeedback()
	{
		
		hideBonus = 0.0f;
		bool hasFeedback = false;
		bool skippedFirst = false;
		switch (dir)
		{
			case Bird.dir.top:
				for (int i = 0; i < 4; i++)
				{
					if (Var.playerPos[myIndex, i] != null)
					{
						Bird bird = Var.playerPos[myIndex, i];
						if (bird.isHiding)
						{
							hideBonus = hideVal;
						}
						else
						{
							if (isMain)
							{
								PlayerEnemyBird = Var.playerPos[myIndex, i];
								ShowFeedback(GameLogic.Instance.GetBonus(Var.playerPos[myIndex, i], birdScript), Var.playerPos[myIndex, i]);
								hasFeedback = true;
								TryWizardLine(Var.playerPos[myIndex, i], birdScript);
								break;                              
							}
							else
							{
								if (skippedFirst && birdScript.enemyType == fillEnemy.enemyType.drill)
								{
									PlayerEnemyBird = Var.playerPos[myIndex, i];
									ShowFeedback(GameLogic.Instance.GetBonus(Var.playerPos[myIndex, i], birdScript), Var.playerPos[myIndex, i]);
									hasFeedback = true;
									break;
								}
								else
									skippedFirst = true;
							}
						}
					}

				}
				canFight = hasFeedback;
				break;
			case Bird.dir.front:
				for (int i = 0; i < 4; i++)
				{

					if (Var.playerPos[3 - i, myIndex] != null)
					{
						Bird bird = Var.playerPos[3 - i, myIndex];
						if (bird.isHiding)
						{
							hideBonus = hideVal;
						}
						else
						{
							if (isMain)
							{ 
								PlayerEnemyBird = Var.playerPos[3 - i, myIndex];
								ShowFeedback(GameLogic.Instance.GetBonus(Var.playerPos[3 - i, myIndex], birdScript), Var.playerPos[3 - i, myIndex]);
								hasFeedback = true;
								TryWizardLine(Var.playerPos[3 - i, myIndex], birdScript);
								break;                              
							}
							else
							{
								if (skippedFirst && birdScript.enemyType == fillEnemy.enemyType.drill)
								{
									PlayerEnemyBird = Var.playerPos[3 - i, myIndex];
									ShowFeedback(GameLogic.Instance.GetBonus(Var.playerPos[3 - i, myIndex], birdScript), Var.playerPos[3 - i, myIndex]);
									hasFeedback = true;
									break;
								}
								else
									skippedFirst = true;
							}
					}
				}

			}
				canFight = hasFeedback;
				break;			   
		}
		
		//print(canFight);
		if(!hasFeedback)
			HideFeedBack(false);



	}

	public bool CheckResting(Bird bird)
	{
		bool skippedEnemy = false;
		switch (dir)
		{
			case Bird.dir.top:
				for (int i = 0; i < 4; i++)
				{
					if (Var.playerPos[myIndex, i] != null)
					{
						if (Var.playerPos[myIndex, i] == bird)
							return false;
						else { 
							if (birdScript.enemyType == fillEnemy.enemyType.drill)
							{
								if (skippedEnemy)
								{
									if (Var.playerPos[myIndex,i] == bird)
										return false;
									else
										break;
								}
								else
									skippedEnemy = true;
							}
							else
								break;
						}
					}
				}
				break;
			case Bird.dir.front:
				for (int i = 0; i < 4; i++)
				{
					if (Var.playerPos[3 - i, myIndex] != null)
					{
						if (Var.playerPos[3 - i, myIndex] == bird)
							return false;
						else
						{
							if (birdScript.enemyType == fillEnemy.enemyType.drill)
							{
								if (skippedEnemy)
								{
									if (Var.playerPos[3 - i, myIndex] == bird)
										return false;
									else
										break;
								}
								else
									skippedEnemy = true;
							}
							else
								break;                           
						}
					}

				}
				break;            
		}
		return true;
	}

	public void SetEnemyHoverText()
    {
        
        if (!isMain)
			return;
		BelowBirdIndicator.gameObject.SetActive(true);
		BelowBirdIndicator.color = new Color(1, 1, 1, 1f);
		string name = "<b>" + Helpers.Instance.GetName(Helpers.Instance.RandomBool()) + "</b>";
		LvlIndicatorText.text = (birdScript.data.levelRollBonus + 1).ToString();
		string strength = "None";
		string weakness = "All";
		if (Helpers.Instance.GetStenght(birdScript.emotion) != Var.Em.Neutral)
			strength = Helpers.Instance.GetHexColor(Helpers.Instance.GetOppositeEmotion(birdScript.emotion)) + Helpers.Instance.GetOppositeEmotion(birdScript.emotion).ToString() + "</color>";
		if (Helpers.Instance.GetWeakness(birdScript.emotion) != Var.Em.Neutral)
			weakness = Helpers.Instance.GetHexColor(birdScript.emotion) + birdScript.emotion.ToString() + "</color>";
		toolTipText = name + "- " + Helpers.Instance.GetHexColor(birdScript.emotion) + birdScript.emotion + "</color>";
		string abilityText = "";
		if (birdScript.enemyType == fillEnemy.enemyType.drill)
			abilityText = "<b>\nAttacks two birds!</b>";
		if (birdScript.enemyType == fillEnemy.enemyType.wizard)
			abilityText = "<b>\nInfluences emotions!</b>";
		toolTipText += abilityText;
		toolTipText += "\nStrength: " + (birdScript.data.levelRollBonus + 1).ToString();
		if (birdScript.enemyType == fillEnemy.enemyType.super)
			toolTipText += "\n Weak to: " + weakness + "\nStrong against: <b>everything</b>";
		else
			toolTipText += "\n Weak to: " + weakness + "\nStrong against: " + strength;

	}
	void ScaleDownIndicator()
	{
		LeanTween.scale(BelowBirdIndicator.gameObject, scale, 0.2f).setEase(LeanTweenType.easeInBack);       
	}
	public void ShowEnemyHoverText()
	{
		Helpers.Instance.ShowTooltip(toolTipText);
	}
	
	public void ShowFeedback(float value,Bird bird)
	{
		//print(bird.charName + " win chance: " + value+ " is main:" +isMain);
		if (isMain &&!BelowBirdIndicator.color.Equals(new Color(1f, 0, 0, 1f)))
		{          
			BelowBirdIndicator.color = new Color(1f, 0, 0, 1f);
			LeanTween.scale(BelowBirdIndicator.gameObject, scale * 1.2f, 0.15f).setEase(LeanTweenType.easeOutCubic).setOnComplete(ScaleDownIndicator);
		}
		bird.fighting = true;
		feedBackText.gameObject.SetActive(true);
		value = Mathf.Clamp01(value+hideBonus);
		float colorIndex = value;
		Color textCol = Color.Lerp(Color.red, Color.green, colorIndex);
		LeanTween.color(feedBackText.gameObject, textCol, 0.2f);
		LeanTween.scale(feedBackText.gameObject, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutBack);
		feedbackVal = Mathf.RoundToInt(value * 10)*10;
		//if (feedbackVal < 50)
		 //   Tutorial.Instance.jiggleGraph();
		feedBackText.text = feedbackVal.ToString("0");
	}
	
	public void HideFeedBack(bool forFight)
	{
		if (isMain)
		{
			if (forFight)
			{
				BelowBirdIndicator.gameObject.SetActive(false);
			}
			else
			{
				if (!BelowBirdIndicator.color.Equals(new Color(1, 1, 1, 1f)))
				{

					BelowBirdIndicator.color = new Color(1, 1, 1, 1f);
					LeanTween.scale(BelowBirdIndicator.gameObject, scale * 1.2f, 0.15f).setEase(LeanTweenType.easeOutCubic).setOnComplete(ScaleDownIndicator);
				}
			}
		}
		LeanTween.scale(feedBackText.gameObject, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutBack);

	}


}
