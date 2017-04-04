using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour {
    public int confidence=0;
    public int friendliness = 0;
    public Var.Em emotion;
    public string charName;


    public Bird(string name,int confidence =0,int friendliness = 0)
    {
        this.confidence = confidence;
        this.friendliness = friendliness;
        this.charName = name;
        SetEmotion();
        Debug.Log(ToString());
    }
	public string ToString()
    {
        return "Name: " + charName + " friendly: " + friendliness + " confidence: " + confidence + " type: " + emotion.ToString();

    }
	void SetEmotion()
    {

        
        if(Mathf.Abs((float)confidence)<Var.lvl1 && Mathf.Abs((float)friendliness) < Var.lvl1)
        {
            //No type
            emotion = Var.Em.Neutral;
            return;
        }
        
        if (Mathf.Abs((float)confidence) > Mathf.Abs((float)friendliness))
        {
            // Confident or sad
            if (confidence > 0)
            {
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
                if (friendliness >= Var.lvl1)
                    emotion = Var.Em.Friendly;
                //SuperFriendly
                if (friendliness >= Var.lvl2)
                    emotion = Var.Em.SuperFriendly;
            }
            else
            {
                //Lonely
                if (friendliness <= -Var.lvl1)
                    emotion = Var.Em.Lonely;
                //SuperLonely
                if (friendliness <= -Var.lvl2)
                    emotion = Var.Em.SuperLonely;
            }

        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
