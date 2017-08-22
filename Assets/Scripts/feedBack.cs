using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feedBack : MonoBehaviour {
	public TextMesh feedBackText;
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
    battleFeedback myBattleFeedback;
    Vector3 scale;  
	// Use this for initialization
	void Awake () {        
        scale= BelowBirdIndicator.transform.localScale;
        myBattleFeedback = feedBackText.gameObject.GetComponent<battleFeedback>();
        myBattleFeedback.fb = this;		
		dir = birdScript.position;
		feedBackText.transform.localScale = Vector3.zero;       
    }

	//checks if the enemy has an opponent
	public bool CheckOpponent()
	{
		bool canfight = false;
		if(dir == Bird.dir.front)
		{
			for(int i = 0; i < 4; i++)
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
		return canfight;
	}

	public void RefreshFeedback()
	{
		
		hideBonus = 0.0f;
        //print(birdScript.charName);
        bool hasFeedback = false;
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
                                PlayerEnemyBird = Var.playerPos[myIndex, i];
                                ShowFeedback(GameLogic.Instance.GetBonus(Var.playerPos[myIndex, i], birdScript), Var.playerPos[myIndex, i]);
                                hasFeedback = true;
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
                            Bird bird = Var.playerPos[3 - i, myIndex];
                            if (bird.isHiding)
                            {
                                hideBonus = hideVal;
                            }
                            else
                            {
                                PlayerEnemyBird = Var.playerPos[3 - i, myIndex];
                                ShowFeedback(GameLogic.Instance.GetBonus(Var.playerPos[3 - i, myIndex], birdScript), Var.playerPos[3 - i, myIndex]);
                                hasFeedback = true;
                                break;
                            }
                        }

                    }
                    break;
               
            }
        if(!hasFeedback)
            HideFeedBack(false);



    }

	public bool CheckResting(Bird bird)
	{
		switch (dir)
		{
			case Bird.dir.top:
				for (int i = 0; i < 4; i++)
				{
					if (Var.playerPos[myIndex, i] != null)
					{
						if (Var.playerPos[myIndex, i] == bird)
							return false;
						else
							break;
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
							break;
					}

				}
				break;
		
		}
		return true;
	}

	public void SetEnemyHoverText()
	{
        BelowBirdIndicator.gameObject.SetActive(true);
        BelowBirdIndicator.color = new Color(1, 1, 1, 1f);        
        string name = Helpers.Instance.GetName(Helpers.Instance.RandomBool());
        LvlIndicatorText.text = (birdScript.levelRollBonus + 1).ToString();
        string strength = "None";
		string weakness = "All";
		if (Helpers.Instance.GetStenght(birdScript.emotion) != Var.Em.Neutral)
			strength = Helpers.Instance.GetHexColor(Helpers.Instance.GetStenght(birdScript.emotion)) + Helpers.Instance.GetStenght(birdScript.emotion).ToString() + "</color>";
        if (Helpers.Instance.GetWeakness(birdScript.emotion) != Var.Em.Neutral)
            weakness = Helpers.Instance.GetHexColor(Helpers.Instance.GetWeakness(birdScript.emotion)) + Helpers.Instance.GetWeakness(birdScript.emotion).ToString() + "</color>";
		toolTipText = name + "- " + birdScript.emotion + "\nStrength " + (birdScript.levelRollBonus+1).ToString() + "\n Weak to: " + weakness + "\nStrong against: " + strength; 
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
        if (!BelowBirdIndicator.color.Equals(new Color(0.5f, 0, 0, 1f)))
        {          
            BelowBirdIndicator.color = new Color(0.5f, 0, 0, 1f);
            LeanTween.scale(BelowBirdIndicator.gameObject, scale * 1.2f, 0.15f).setEase(LeanTweenType.easeOutCubic).setOnComplete(ScaleDownIndicator);
        }
        bird.fighting = true;
		feedBackText.gameObject.SetActive(true);
		//float colorIndex = (value + 4.0f) / 8;
		value = Mathf.Clamp01(value+hideBonus);
		float colorIndex = value;
		Color textCol = Color.Lerp(Color.red, Color.green, colorIndex);
		LeanTween.color(feedBackText.gameObject, textCol, 0.2f);
		LeanTween.scale(feedBackText.gameObject, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutBack);
		//feedBackText.text = ((int)(value*100)).ToString("+#;-#;0") +" %";
		feedBackText.text =(Mathf.Ceil(value * 100)).ToString("0");
	}
    
	public void HideFeedBack(bool forFight)
	{
        if (forFight) {
            BelowBirdIndicator.gameObject.SetActive(false);
        } else
        {
            if (!BelowBirdIndicator.color.Equals(new Color(1, 1, 1, 1f)) )
            {
                             
                BelowBirdIndicator.color = new Color(1, 1, 1, 1f);
                LeanTween.scale(BelowBirdIndicator.gameObject, scale * 1.2f, 0.15f).setEase(LeanTweenType.easeOutCubic).setOnComplete(ScaleDownIndicator);
            }
        }

        LeanTween.scale(feedBackText.gameObject, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutBack);

	}


}
