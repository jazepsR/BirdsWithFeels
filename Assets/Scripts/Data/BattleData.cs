using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class BattleData {
	public float minConf, maxConf, minFriend, maxFriend;
	public Var.Em type;
	public bool hasRocks;
	public List<Var.Em> powerUps;
	public int birdLVL;
	public int minEnemies=3;
	public int maxEnemies=4;
	public List<Bird.dir> dirs;
	public List<Var.PowerUps> powers;
    [NonSerialized]
    public MapBattleData battleData;
	public bool hasWizards;
	public bool hasDrills;
	public bool hasSuper;
	
	void SetBattleData(float minConf, float maxConf, float minFriend, float maxFriend)
	{
		this.minConf = minConf;
		this.maxConf = maxConf;
		this.minFriend = minFriend;
		this.maxFriend = maxFriend;        
	}
	public BattleData(Var.Em type,bool hasRocks,List<Var.Em> powerUps, MapBattleData battleData,int birdLVL=1, List<Bird.dir> dirs= null, 
		List<Var.PowerUps> powers = null, bool hasWizards = false, bool hasDrills = false, bool hasSuper = false)
	{
        this.battleData = battleData;
		this.hasWizards = hasWizards;
		this.hasDrills = hasDrills;
		this.powers = powers;
		this.hasSuper = hasSuper;
		this.type = type;
		this.powerUps = powerUps;
		this.hasRocks = hasRocks;
		this.birdLVL = birdLVL;
		this.dirs = dirs;
		switch (type)
		{
			case Var.Em.Neutral:
				SetBattleData(-5, 5, -5, 5);
				break;
			case Var.Em.Social:
				SetBattleData(-5, 5, 2, 8);
				break;
			case Var.Em.Solitary:
				SetBattleData(-5, 5, -8, -2);
				break;
			case Var.Em.Confident:
				SetBattleData(2, 8, -5, 5);
				break;
			case Var.Em.Cautious:
				SetBattleData(-8, 2, -5, 5);
				break;
			default:
				SetBattleData(-5, 5, -5, 5);
				break;
					

		}
	}	
}
