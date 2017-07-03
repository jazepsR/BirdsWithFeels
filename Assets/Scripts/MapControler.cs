using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControler : MonoBehaviour {
	public static MapControler Instance { get; private set; }
	[HideInInspector]
	public bool canFight= false;
	void Awake()
	{
		
	}
	// Use this for initialization
	void Start () {
		Instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
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
