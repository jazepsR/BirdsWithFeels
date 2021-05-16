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
		Vector3 offset;
		switch (type)
		{
			case Var.PowerUps.dmg:
				offset = new Vector3(0.18f, 0.23f, 0);
				break;
            case Var.PowerUps.shield:
                offset = new Vector3(0.15f, 0.15f, 0);
                break;
            case Var.PowerUps.heal:
				offset = new Vector3(0.1f, 0.25f, 0);
				break;
			default:
				offset = Vector3.zero;
				break;
		}
		transform.position += offset;
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
			canKiss = false;
			LeanTween.delayedCall(1f, AllowKiss);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "feet" && type != Var.PowerUps.obstacle)
		{
			AudioControler.Instance.PlaySound(AudioControler.Instance.tileHighlightBirdHoverSpecial);
			/*switch (type)
			{
				case Var.PowerUps.dmg:
					AudioControler.Instance.powerTileCombat.Play();
					break;
				case Var.PowerUps.shield:
					AudioControler.Instance.powerTileShield.Play();
					break;
				case Var.PowerUps.emotion:
					if (emotion == Var.Em.Solitary || emotion == Var.Em.Cautious)
						AudioControler.Instance.powerTileNegative.Play();
					else
						AudioControler.Instance.powerTilePositive.Play();
					break;
				case Var.PowerUps.heal:
					AudioControler.Instance.powerTileHeart.Play();
					break;
				default:
					AudioControler.Instance.PlaySound(AudioControler.Instance.tileHighlightBirdHoverSpecial);
					break;
			}*/
			
		}
	}


	void AllowKiss()
	{
		canKiss = true;
	}

	void OnMouseEnter()
	{
        if (!Var.CanShowHover || GuiContoler.Instance.speechBubbleObj.activeSelf || Var.Infight || EventController.Instance.eventObject.activeSelf || Time.timeSinceLevelLoad < .3f)
            return;
        //if (Var.selectedBird != null)       
        //	return;        
        string info = "";
		switch (type)
		{
			case Var.PowerUps.dmg:
				info = "Birds on this tile recieve +10% fighting bonus";
				//AudioControler.Instance.powerTileCombat.Play();
				break;
            case Var.PowerUps.shield:
                info = "Birds on this tile are shielded from damage";
               // AudioControler.Instance.powerTileShield.Play();
                break;
            case Var.PowerUps.emotion:
				info = "Gain one extra "+Helpers.Instance.GetHexColor(emotion) + emotion.ToString()+"</color>";
				/*if(emotion == Var.Em.Solitary || emotion == Var.Em.Cautious)
					//AudioControler.Instance.powerTileNegative.Play();
				else
					//AudioControler.Instance.powerTilePositive.Play();*/
				break;
			case Var.PowerUps.heal:
				info = "Birds on this tile will heal 1 heart after the battle";
				//AudioControler.Instance.powerTileHeart.Play();
				break;
			case Var.PowerUps.obstacle:
				info = "You can't place birds here - enemies will walk through";
				AudioControler.Instance.PlaySound(AudioControler.Instance.rockMouseover);    
				if(canKiss)           
					GetComponent<Animator>().SetTrigger("kiss");
				break;

		}
		if(info != "")
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
        if(type == Var.PowerUps.shield)
        {
            bird.hasShieldBonus = true;
        }
	}
}
