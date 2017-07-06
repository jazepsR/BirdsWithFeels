using System.Collections;
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
            foreach (Bird bird in Var.availableBirds)
            {
                bool wasActive = false;
                foreach (Bird ActiveBird in Var.activeBirds)
                {
                    if (ActiveBird.charName == bird.charName)
                    {
                        wasActive = true;
                        break;
                    }
                }
                if (wasActive)
                    bird.AdventuresRested = 0;
                else
                    bird.AdventuresRested++;
            }
            foreach (Bird bird in playerBirds)
            {
                foreach (Bird loadBird in Var.availableBirds)
                {
                    if (bird.charName == loadBird.charName)
                    {
                        SetupBird(bird, loadBird);
                        break;
                    }
                }
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
        target.levelList = template.levelList;
        target.startingLVL = template.startingLVL;
        target.battleCount = template.battleCount;
        target.lastLevel = template.lastLevel;
        target.level = template.level;
        target.birdAbility = template.birdAbility;
        target.consecutiveFightsWon = template.consecutiveFightsWon;
        target.battlesToNextLVL = template.battlesToNextLVL;        
        target.roundsRested = template.roundsRested;
        target.AdventuresRested = template.AdventuresRested;
        target.transform.Find("BIRB_sprite/hat").GetComponent<SpriteRenderer>().sprite = template.hatSprite;
    }
	// Update is called once per frame
	void Update () {
		
	}
}
