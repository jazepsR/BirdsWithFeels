using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerTile : MonoBehaviour {
	public SpriteRenderer sr;
	Var.Em emotion;
	public Var.PowerUps type;
	public Color secondColor;
	public Color firstColor;
    bool canKiss = true;
	// Use this for initialization
	void Start () {
		
		
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
   
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "feet" && Var.selectedBird == null && type == Var.PowerUps.obstacle)
        {
            GetComponent<Animator>().SetTrigger("grump");
            print("gtumper");
            canKiss = false;
            LeanTween.delayedCall(1f, AllowKiss);
        }
    }

    void AllowKiss()
    {
        canKiss = true;
    }

    void OnMouseEnter()
	{
        if (Var.selectedBird != null)       
            return;        
		string info = "";
		switch (type)
		{
			case Var.PowerUps.dmg:
				info = "Birds on this tile recieve +10% fighting bonus";
				break;
			case Var.PowerUps.emotion:
				info = "Gain one extra "+Helpers.Instance.GetHexColor(emotion) + emotion.ToString()+"</color>";
				break;
			case Var.PowerUps.heal:
				info = "Birds on this tile will heal 1 heart after the battle";
				break;
			case Var.PowerUps.obstacle:
				info = "You can't place birds here - enemies will walk through";     
                if(canKiss)           
                    GetComponent<Animator>().SetTrigger("kiss");
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
					bird.groundConfBoos = 1 * bird.groundMultiplier;
					break;
				case Var.Em.Social:
					bird.groundFriendBoos= 1 * bird.groundMultiplier;
					break;
				case Var.Em.Solitary:
					bird.groundFriendBoos = -1 * bird.groundMultiplier;
					break;
				case Var.Em.Cautious:
					bird.groundConfBoos = -1 * bird.groundMultiplier;
					break;
			}

			Debug.Log("Applying tile"+ bird.charName+ " ground emotion: " + emotion + " conf change: " + bird.groundConfBoos + " friend change: " + bird.groundFriendBoos);
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
