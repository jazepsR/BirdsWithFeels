﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feedBack : MonoBehaviour {
    public TextMesh feedBackText;
    Bird birdScript;
    Bird.dir dir;

    public int myIndex;
	// Use this for initialization
	void Start () {
        birdScript = GetComponent<Bird>();
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
                    canfight = true;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < 4; i++)
            {
                if (Var.playerPos[myIndex, i] != null)
                {
                    canfight = true;
                    break;
                }

            }
        }
        return canfight;
    }
    public void RefreshFeedback()
    {
        HideFeedBack();
        switch (dir)
        {
            case Bird.dir.top:
                for (int i = 0; i < 4; i++)
                {
                   if( Var.playerPos[myIndex, i] != null)
                    {
                        ShowFeedback(GameLogic.Instance.GetBonus(Var.playerPos[myIndex, i], birdScript), Var.playerPos[myIndex, i]);
                        break;
                    }

                }
                    break;
            case Bird.dir.front:
                for (int i = 0; i < 4; i++)
                {
                    if (Var.playerPos[3-i, myIndex] != null)
                    {
                        ShowFeedback(GameLogic.Instance.GetBonus(Var.playerPos[3-i, myIndex], birdScript), Var.playerPos[3 - i, myIndex]);
                        break;
                    }

                }
                break;
            case Bird.dir.bottom:
                for (int i = 0; i < 4; i++)
                {
                    if (Var.playerPos[myIndex, 3-i] != null)
                    {
                        ShowFeedback(GameLogic.Instance.GetBonus(Var.playerPos[myIndex, 3-i], birdScript),Var.playerPos[myIndex, 3 - i]);
                        break;
                    }

                }
                break;
        }
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
            case Bird.dir.bottom:
                for (int i = 0; i < 4; i++)
                {
                    if (Var.playerPos[myIndex, 3 - i] == bird)
                    {
                        if (Var.playerPos[myIndex, 3 - i] == bird)
                            return false;
                        else
                            break;
                    }

                }
                break;
        }
        return true;
    }



	
	public void ShowFeedback(float value,Bird bird)
    {
        bird.fighting = true;
        feedBackText.gameObject.SetActive(true);
        //float colorIndex = (value + 4.0f) / 8;
        value = Mathf.Clamp01(value);
        float colorIndex = value;
        Color textCol = Color.Lerp(Color.red, Color.green, colorIndex);
        LeanTween.color(feedBackText.gameObject, textCol, 0.2f);
        LeanTween.scale(feedBackText.gameObject, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutBack);
        //feedBackText.text = ((int)(value*100)).ToString("+#;-#;0") +" %";
        feedBackText.text =(Mathf.Ceil(value * 100)).ToString("0") + " %";
    }
    public void HideFeedBack()
    {
        LeanTween.scale(feedBackText.gameObject, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutBack);

    }
}