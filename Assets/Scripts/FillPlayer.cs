﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillPlayer : MonoBehaviour {
    public Bird[] playerBirds;
    public bool inMap = false;
	// Use this for initialization
	void Awake ()
    {
       
        if (inMap && Var.availableBirds.Count>0)
        {
            for (int i = 0; i < Var.availableBirds.Count; i++)
            {
                SetupBird(playerBirds[i], Var.availableBirds[i]);
            }
        }
        if (!inMap && Var.activeBirds.Count > 0)
        {
            for (int i = 0; i < Var.activeBirds.Count; i++)
            {
                SetupBird(playerBirds[i], Var.activeBirds[i]);
            }
        }
        if (Var.availableBirds.Count < 1 && inMap)
        {
            Var.availableBirds.AddRange(playerBirds);
        }
           
    }
		
	
	void SetupBird(Bird target, Bird template)
    {
        target.charName = template.charName;
        target.friendliness = template.friendliness;
        target.confidence = template.confidence;
        target.portraitOrder = template.portraitOrder;
        target.health = template.health;
        if (template.health < 1)
            target.gameObject.SetActive(false);
        target.portrait = template.portrait;
        target.transform.Find("BIRB_sprite/hat").GetComponent<SpriteRenderer>().sprite = template.hatSprite;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
