using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Bird : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler
{
    public int portraitOrder=0;
    public int confidence=0;
    [HideInInspector]
    public int prevConf = 0;
    [HideInInspector]
    public int prevFriend = 0;
    public int friendliness = 0;
    public Var.Em emotion;
    public string charName;
    public Image src;    
    public bool inUse = true;
	public GameObject birdPrefab;
    public SpriteRenderer colorRenderer;
    public GameObject portrait;
    public Image portraitColor;
    public void OnPointerDown(PointerEventData eventData)
    {
		if (birdPrefab != null) {
			GameLogic.Instance.OnDragBird(this);
			// Update text!
			showText ();
            GuiContoler.Instance.PortraitControl(portraitOrder,emotion);
        }
    }

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (birdPrefab != null) {
			showText ();
            GuiContoler.Instance.PortraitControl(portraitOrder,emotion);
        }
	}
   
    public Bird(string name,int confidence =0,int friendliness = 0)
    {
        this.confidence = confidence;
        this.friendliness = friendliness;
        this.charName = name;
        SetEmotion();
        Debug.Log(ToString());
    }
	public override string ToString()
    {
        if (enabled)
        {
            return "Name: " + charName + "\nfriendly: " + friendliness + "\nconfidence: " + confidence + "\ntype: " + emotion.ToString();
        }else
        {
            return null;
        }

    }
    void Start()
    {
        SetEmotion();
    }
	public void SetEmotion()
    {       
        prevConf = confidence;
        prevFriend = friendliness;
        
        if(Mathf.Abs((float)confidence)<Var.lvl1 && Mathf.Abs((float)friendliness) < Var.lvl1)
        {
            //No type
            emotion = Var.Em.Neutral;
            colorRenderer.color = Helpers.Instance.neutral;
            return;
        }
        
        if (Mathf.Abs((float)confidence) > Mathf.Abs((float)friendliness))
        {
            // Confident or sad
            if (confidence > 0)
            {
                colorRenderer.color = Helpers.Instance.brave;
                //Confident
                if (confidence >= Var.lvl1)
                    emotion = Var.Em.Confident;
                //Superconfident
                if (confidence >= Var.lvl2)
                    emotion = Var.Em.SuperConfident;
            }
            else
            {
                //Scared
                colorRenderer.color = Helpers.Instance.scared;
                if (confidence <= -Var.lvl1)
                    emotion = Var.Em.Scared;
                //SuperScared
                if (confidence <= -Var.lvl2)
                    emotion = Var.Em.SuperScared;
            }

        }
        else
        {
            //Friendly or lonely
            if (friendliness > 0)
            {

                //friendly
                colorRenderer.color = Helpers.Instance.friendly;
                if (friendliness >= Var.lvl1)
                    emotion = Var.Em.Friendly;
                //SuperFriendly
                if (friendliness >= Var.lvl2)
                    emotion = Var.Em.SuperFriendly;
            }
            else
            {
                //Lonely
                colorRenderer.color = Helpers.Instance.lonely;
                if (friendliness <= -Var.lvl1)
                    emotion = Var.Em.Lonely;
                //SuperLonely
                if (friendliness <= -Var.lvl2)
                    emotion = Var.Em.SuperLonely;
            }

        }
      
    }
	
	public void showText()
    {
        if(ToString()!=null)
        Var.birdInfo.text = ToString();
    }


}
