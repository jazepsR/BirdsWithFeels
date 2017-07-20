using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerTile : MonoBehaviour {
    public SpriteRenderer sr;
	Var.Em emotion;
    public Var.PowerUps type;
	public Color secondColor;
	public Color firstColor;
	// Use this for initialization
	void Start () {
		
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void SetColor(Var.Em emotion)
	{
		this.emotion = emotion;
		firstColor = Helpers.Instance.GetEmotionColor(emotion);
        secondColor = new Color(firstColor.r - 0.3f, firstColor.g - 0.3f, firstColor.b - 0.3f);
		sr.color = firstColor;
		TweenToSecond();
	}

	void TweenToFirst()
	{        
			LeanTween.color(gameObject, firstColor, 1f).setEase(LeanTweenType.linear).setOnComplete(TweenToSecond);
	  
	}
	void TweenToSecond()
	{
		LeanTween.color(gameObject, secondColor, 1f).setEase(LeanTweenType.linear).setOnComplete(TweenToFirst);

	}

    void OnMouseEnter()
    {
        string info = "";
        switch (type)
        {
            case Var.PowerUps.dmg:
                info = "Birds on this tile recieve +10% fighting bouns";
                break;
            case Var.PowerUps.emotion:
                info = "Birds on this tile recieve +1 to " + emotion.ToString();
                break;
            case Var.PowerUps.heal:
                info = "Birds on this tile will heal 1 heart after the battle";
                break;
            
        }

        Helpers.Instance.ShowTooltip(info);

    }

    void OnMouseExit()
    {
        Helpers.Instance.HideTooltip();
    }

    public void ApplyPower(Bird bird)
    {
        if (type == Var.PowerUps.emotion)
        {
            switch (emotion)
            {
                case Var.Em.Confident:
                    bird.confBoos = 1 * bird.groundMultiplier;
                    break;
                case Var.Em.Friendly:
                    bird.friendBoost = 1 * bird.groundMultiplier;
                    break;
                case Var.Em.Lonely:
                    bird.friendBoost = -1 * bird.groundMultiplier;
                    break;
                case Var.Em.Scared:
                    bird.confBoos = -1 * bird.groundMultiplier;
                    break;
            }
        }
        if( type == Var.PowerUps.heal)
        {
            bird.healthBoost = 1*bird.groundMultiplier;
        }
        if (type == Var.PowerUps.dmg)
        {
            bird.GroundRollBonus = 1 * bird.groundMultiplier;
        }
    }
}
