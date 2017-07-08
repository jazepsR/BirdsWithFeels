using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapControler : MonoBehaviour {
	public static MapControler Instance { get; private set; }
	[HideInInspector]
	public bool canFight= false;
    [HideInInspector]
    public bool canHeal = false;
    GameObject healTrail;
    public Text title;
	void Awake()
	{
		
	}
	// Use this for initialization
	void Start () {
		Instance = this;
        canHeal = false;
        foreach(Bird bird in Var.activeBirds)
        {
            if(Helpers.Instance.ListContainsLevel(Levels.type.Friend2, bird.levelList))
            {
                healTrail = Instantiate(Resources.Load("MouseHealParticle"), Input.mousePosition, Quaternion.identity) as GameObject;
                canHeal = true;
                title.text = bird.charName + " can heal one of your birds!";
            }
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (canHeal)
        {
            healTrail.transform.position = Camera.main.ScreenToWorldPoint( Input.mousePosition);
        }else
        {
            Destroy(healTrail);
        }
	}
	public void CanLoadBattle()
	{
		canFight = true;
		for(int i = 0; i < 3; i++)
		{
			if(Var.playerPos[i,0]== null)
			{
				canFight = false;
				break;
			}
		}
		if (canFight)
		{
			MapIcon[] icons = FindObjectsOfType<MapIcon>();
			foreach(MapIcon icon in icons)
			{
				icon.LockedIcon.SetActive(false);
			}
		}
		
	}
}
