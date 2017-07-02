using System.Collections;
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
                        ShowFeedback(GameLogic.Instance.GetBonus(Var.playerPos[myIndex, i], birdScript));
                        break;
                    }

                }
                    break;
            case Bird.dir.front:
                for (int i = 0; i < 4; i++)
                {
                    if (Var.playerPos[i, myIndex] != null)
                    {
                        ShowFeedback(GameLogic.Instance.GetBonus(Var.playerPos[i, myIndex], birdScript));
                        break;
                    }

                }
                break;
            case Bird.dir.bottom:
                for (int i = 0; i < 4; i++)
                {
                    if (Var.playerPos[myIndex, 3-i] != null)
                    {
                        ShowFeedback(GameLogic.Instance.GetBonus(Var.playerPos[myIndex, 3-i], birdScript));
                        break;
                    }

                }
                break;
        }
    }




	
	public void ShowFeedback(float value)
    {
        feedBackText.gameObject.SetActive(true);
        float colorIndex = (value + 4.0f) / 8;

        Color textCol = Color.Lerp(Color.red, Color.green, colorIndex);
        LeanTween.color(feedBackText.gameObject, textCol, 0.2f);
        LeanTween.scale(feedBackText.gameObject, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutBack);
        feedBackText.text = ((int)value).ToString("+#;-#;0");
    }
    public void HideFeedBack()
    {
        LeanTween.scale(feedBackText.gameObject, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutBack);

    }
}
