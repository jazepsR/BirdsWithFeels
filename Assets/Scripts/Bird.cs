using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Bird : MonoBehaviour
{
	public int portraitOrder=0;
	public int confidence=0;
	[HideInInspector]
	public int prevConf = 0;
	[HideInInspector]
	public int prevFriend = 0;
	public int friendliness = 0;
    public int health = 3;
    int x = -1;
    int y = -1;
    public Var.Em emotion;
	public string charName;
	public Image src;    
	public bool inUse = true;
	public GameObject birdPrefab;
	public SpriteRenderer colorRenderer;
	public GameObject portrait;
	public Image portraitColor;  
	[HideInInspector] 
	public Vector3 target;
	public Vector3 home;
	bool dragged = false;
	[HideInInspector]
	public firendLine lines;
    public float level = 1;
	bool needsReset = false; 
	public enum dir { top,front,bottom};
	public dir position;	
    public bool isEnemy = true;
    [HideInInspector]
    public int friendBoost = 0;
    [HideInInspector]
    public int confBoos = 0;
    public bool inMap = false;
    [HideInInspector]
    public Sprite hatSprite;
	void Start()
	{
        if (!isEnemy)
        {
            try
            {
                hatSprite = transform.Find("BIRB_sprite/hat").GetComponent<SpriteRenderer>().sprite;
            }
            catch
            {
                Debug.Log("Couldnt get hat sprite");
            }
            }
           	
		lines = GetComponent<firendLine>();
        if (isEnemy)
        {
            home = transform.localPosition;
        }
        else
        {
            home = transform.position;
            LoadStats();
        }
		target = transform.position;
		SetEmotion();
	}
	

	public float getBonus()
	{
        return level - 1;
	}
    void LoadStats()
    {
        bool SaveDataCreated = false;
        Bird savedData = null;
        foreach (Bird data in Var.activeBirds)
        {
            if (data.charName == charName)
            {
                SaveDataCreated = true;
                savedData = data;
                break;
            }
        }


        if (!SaveDataCreated)
        {
            Var.activeBirds.Add(this);            
        }
        else
        {
            confidence = savedData.confidence;
            friendliness = savedData.friendliness;
        }
    }
        

	void OnMouseOver()
	{
        showText();
        if (Input.GetMouseButtonDown(0))
		{
			for(int i = 0; i < Var.playerPos.GetLength(0); i++)
			{
				for(int j =0; j< Var.playerPos.GetLength(1); j++)
				{

					if(Var.playerPos[i,j] == this)
					{						
						Var.playerPos[i, j] = null;                     
						break;
					}
				}
			}            
			target = home;
            confBoos = 0;
            friendBoost = 0;
			dragged = true;
			Var.selectedBird = gameObject;
            if (!inMap)
            {
                lines.RemoveLines();
                GameLogic.Instance.UpdateFeedback();
            }

           // RemoveAllFeedBack();
            x = -1;
            y = -1;
		}
		// 1 frame delay
		if (Input.GetMouseButtonUp(0))
			needsReset = true;

		if (needsReset)
		{
			needsReset = false;
			Var.selectedBird = null;
			dragged = false;
			LeanTween.move(gameObject, new Vector3(target.x, target.y, 0), 0.5f).setEase(LeanTweenType.easeOutBack);
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


    public void LoseHealth(int dmg)
    {
        health = health - dmg;
        if (health <= 0)
        {
            gameObject.SetActive(false);
        }
    }

	public override string ToString()
	{
		if (enabled)
		{
            return "friendliness: " + friendliness + "\nbravery: " + confidence + "\nlevel: " + level+"\nhealth: "+health;
		}else
		{
			return null;
		}

	}
	public string GetHeading()
	{
		if (enabled)
		{
			return charName + "\n" +emotion.ToString();
		}
		else
		{
			return null;
		}
	}
   
	public void SetEmotion()
	{       
		prevConf = confidence;
		prevFriend = friendliness;
        if (confBoos != 0)
        {
            int i = 0;
        }
        friendliness += friendBoost;
        confidence += confBoos;
        confBoos = 0;
        friendBoost = 0;
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
			   /* if (confidence >= Var.lvl2)
					emotion = Var.Em.SuperConfident;*/
			}
			else
			{
				//Scared
				colorRenderer.color = Helpers.Instance.scared;
			   if (confidence <= -Var.lvl1)
					emotion = Var.Em.Scared;
				//SuperScared
				/*if (confidence <= -Var.lvl2)
					emotion = Var.Em.SuperScared;*/
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
			   /* if (friendliness >= Var.lvl2)
					emotion = Var.Em.SuperFriendly;*/
			}
			else
			{
				//Lonely
				colorRenderer.color = Helpers.Instance.lonely;
				if (friendliness <= -Var.lvl1)
					emotion = Var.Em.Lonely;
				//SuperLonely
			   /* if (friendliness <= -Var.lvl2)
					emotion = Var.Em.SuperLonely;*/
			}

		}
        if (!isEnemy)
        {
           
      
        }
        
	  
	}
	
	public void showText()
	{
		if (ToString() != null)
		{
			Var.birdInfo.text = ToString();
			Var.birdInfoHeading.text = charName;
			Var.birdInfoFeeling.text = emotion.ToString();
			Var.birdInfoFeeling.color = Helpers.Instance.GetEmotionColor(emotion);
			GuiContoler.Instance.PortraitControl(portraitOrder, emotion);
		}
	}

	
	
	public void Update()
	{

		if (dragged)
		{
			LeanTween.move(gameObject, new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0), 0.1f);
		}
		
	}

	public void ReleseBird(int x, int y)
	{
		Var.selectedBird = null;
		dragged = false;
        if (!inMap)
        {
            lines.DrawLines(x, y);
            GameLogic.Instance.UpdateFeedback();
        }
            LeanTween.move(gameObject, new Vector3(target.x, target.y, 0), 0.5f).setEase(LeanTweenType.easeOutBack);
        
		
	   

	}
    


}
