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
	bool needsReset = false; 
	public enum dir { top,front,bottom};
	public dir position;
	List<Bird> activeEnemies;
	void Start()
	{
		activeEnemies = new List<Bird>();
		lines = GetComponent<firendLine>();
		home = transform.position;
		target = transform.position;
		SetEmotion();
	}
	

	public float getBonus()
	{
		return 0.0f;
	}

	void OnMouseOver()
	{
        
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
			dragged = true;
			Var.selectedBird = gameObject;
			lines.RemoveLines();
			if (activeEnemies.Count > 0)
			{
				foreach (Bird enemy in activeEnemies)
				{
					enemy.GetComponent<feedBack>().HideFeedBack();
				}
				activeEnemies = new List<Bird>();
			}

            RemoveAllFeedBack();
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

		showText();
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
			return "friendliness: " + friendliness + "\nbravery: "+ confidence;
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
		lines.DrawLines(x, y);
		LeanTween.move(gameObject, new Vector3(target.x, target.y, 0), 0.5f).setEase(LeanTweenType.easeOutBack);
		checkFeedback(x,y,x, Bird.dir.top);
		checkFeedback(x, y, y + 4, dir.front);
		checkFeedback(x, y, x + 8, dir.bottom);
	   

	}
	public void checkFeedback(int x, int y,int pos, dir Dir)
	{
		Bird enemy = Var.enemies[pos];
        this.x = x;
        this.y = y;
		if (enemy.inUse)
		{
			for(int i = 0; i< 4; i++)
			{
				if (Dir == dir.top)
				{
					if (i < y && Var.playerPos[x, i] != null)
						return;
				}
				if (Dir == dir.front)
				{
					if (i > x && Var.playerPos[i, y]!= null)
						return;

				}
				if(Dir == dir.bottom)
				{
					if (i > y && Var.playerPos[x, i] != null)
						return;
				}
				
			}

            setFeedback(enemy);
			activeEnemies.Add(enemy);
		}
	}

    public void setFeedback(Bird enemy)
    {
        float feedBack = GameLogic.Instance.GetBonus(this, enemy);
        enemy.GetComponent<feedBack>().ShowFeedback(feedBack);
    }

    public void RemoveAllFeedBack()
    {
        if (x != -1)
        {
            RemoveFeedback(x, Bird.dir.top);
            RemoveFeedback(y + 4, dir.front);
            RemoveFeedback(x + 8, dir.bottom);
        }

    }
	public void RemoveFeedback(int pos, dir Dir)
    {
        bool birdInfront = false;
        //Check forward -> dont touch feedback
        for (int i = 0; i < 4; i++)
        {
            if (Dir == dir.top && i < y && Var.playerPos[x, i] != null && Var.enemies[pos].inUse)
            {                
                    birdInfront = true;
            }
            if (Dir == dir.front && i > x && Var.playerPos[i, y] != null && Var.enemies[pos].inUse)
            {
                    birdInfront = true;
            }
            if (Dir == dir.bottom && i > y && Var.playerPos[x, i] != null && Var.enemies[pos].inUse)
            {
                    birdInfront = true;
            }
        }
        //Check backward -> that sets feedback
        for (int i = 0; i < 4; i++)
        {
            if (Dir == dir.top)
            {
                if (i > y && Var.playerPos[x,  i] != null && Var.enemies[pos].inUse)
                {
                    Var.playerPos[x, i].setFeedback(Var.enemies[pos]);
                    return;
                }
            }
            if (Dir == dir.front)
            {
                if ( i < x && Var.playerPos[i, y] != null && Var.enemies[pos].inUse)
                {
                    Var.playerPos[i,y].setFeedback(Var.enemies[pos]);
                    return;
                }
            }
            if (Dir == dir.bottom)
            {
                if (i < y && Var.playerPos[x, i] != null && Var.enemies[pos].inUse)
                {
                    Var.playerPos[x, i].setFeedback(Var.enemies[pos]);
                    return;
                }
            }
        }
        // none in line, hide feedback
        if (!birdInfront)
        {
            Var.enemies[pos].GetComponent<feedBack>().HideFeedBack();
        }
    }


}
