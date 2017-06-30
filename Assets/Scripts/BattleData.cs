using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData {
    public float minConf, maxConf, minFriend, maxFriend;
    public Var.Em type;
    public bool hasRocks;
	// Use this for initialization
	void Start () {
		
	}
	void SetBattleData(float minConf, float maxConf, float minFriend, float maxFriend,bool hasRocks)
    {
        this.minConf = minConf;
        this.maxConf = maxConf;
        this.minFriend = minFriend;
        this.maxFriend = maxFriend;
        this.hasRocks = hasRocks;
    }
    public BattleData(Var.Em type,bool hasRocks)
    {
        this.type = type;
        switch (type)
        {
            case Var.Em.Neutral:
                SetBattleData(-5, 5, -5, 5,hasRocks);
                break;
            case Var.Em.Friendly:
                SetBattleData(-5, 5, 2, 8, hasRocks);
                break;
            case Var.Em.Lonely:
                SetBattleData(-5, 5, -8, -2, hasRocks);
                break;
            case Var.Em.Confident:
                SetBattleData(2, 8, -5, 5, hasRocks);
                break;
            case Var.Em.Scared:
                SetBattleData(-8, 2, -5, 5, hasRocks);
                break;
            default:
                SetBattleData(-5, 5, -5, 5, hasRocks);
                break;
                    

        }
    }
    // Update is called once per frame
    void Update () {
		
	}
}
