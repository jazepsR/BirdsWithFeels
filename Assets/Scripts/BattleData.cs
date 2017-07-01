﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData {
    public float minConf, maxConf, minFriend, maxFriend;
    public Var.Em type;
    public bool hasRocks;
    public List<Var.Em> powerUps;
	// Use this for initialization
	void Start () {
		
	}
	void SetBattleData(float minConf, float maxConf, float minFriend, float maxFriend)
    {
        this.minConf = minConf;
        this.maxConf = maxConf;
        this.minFriend = minFriend;
        this.maxFriend = maxFriend;
        this.hasRocks = hasRocks;
    }
    public BattleData(Var.Em type,bool hasRocks,List<Var.Em> powerUps)
    {
        this.type = type;
        this.powerUps = powerUps;
        this.hasRocks = hasRocks;
        switch (type)
        {
            case Var.Em.Neutral:
                SetBattleData(-5, 5, -5, 5);
                break;
            case Var.Em.Friendly:
                SetBattleData(-5, 5, 2, 8);
                break;
            case Var.Em.Lonely:
                SetBattleData(-5, 5, -8, -2);
                break;
            case Var.Em.Confident:
                SetBattleData(2, 8, -5, 5);
                break;
            case Var.Em.Scared:
                SetBattleData(-8, 2, -5, 5);
                break;
            default:
                SetBattleData(-5, 5, -5, 5);
                break;
                    

        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
