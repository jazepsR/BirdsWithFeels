using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour {

	// Use this for initialization
	void Start () {
		for(int i =0; i<4; i++)
        {
            Bird a = new Bird("Alice",(int)Random.Range(-15, 15), (int)Random.Range(-15, 15));
            Bird b = new Bird("Bob", (int)Random.Range(-15, 15), (int)Random.Range(-15, 15));
            Debug.Log(Fight(a, b).charName + " won!");
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}





    Bird Fight(Bird playerBird, Bird enemyBird)
    {
        if (Bird1Win(playerBird, enemyBird))
        {
            return playerBird;
        }
        if (Bird1Win(enemyBird, playerBird))
        {
            return enemyBird;
        }
        if (Random.Range(0, 1) > 0.5f)
        {
            return playerBird;
        }else
        {
            return enemyBird;
        }

    }

    bool Bird1Win(Bird Bird1, Bird Bird2)
    {
        Var.Em em1 = Bird1.emotion;
        Var.Em em2 = Bird2.emotion;


        if (em1 == Var.Em.Friendly && em2 == Var.Em.Confident)
            return true;
        if (em1 == Var.Em.Confident && em2 == Var.Em.Lonely)
            return true;
        if (em1 == Var.Em.Lonely && em2 == Var.Em.Scared)
            return true;
        if (em1 == Var.Em.Scared && em2 == Var.Em.Friendly)
            return true;

        if (em1 == Var.Em.SuperFriendly && em2 == Var.Em.Confident)
            return true;
        if (em1 == Var.Em.SuperConfident && em2 == Var.Em.Lonely)
            return true;
        if (em1 == Var.Em.SuperLonely && em2 == Var.Em.Scared)
            return true;
        if (em1 == Var.Em.SuperScared && em2 == Var.Em.Friendly)
            return true;

        if (em1 == Var.Em.SuperFriendly && em2 == Var.Em.SuperConfident)
            return true;
        if (em1 == Var.Em.SuperConfident && em2 == Var.Em.SuperLonely)
            return true;
        if (em1 == Var.Em.SuperLonely && em2 == Var.Em.SuperScared)
            return true;
        if (em1 == Var.Em.SuperScared && em2 == Var.Em.SuperFriendly)
            return true;

        if (em1 == Var.Em.SuperConfident && em2 == Var.Em.Scared)
            return true;
        if (em1 == Var.Em.SuperLonely && em2 == Var.Em.Friendly)
            return true;
        if (em1 == Var.Em.SuperScared && em2 == Var.Em.Confident)
            return true;
        if (em1 == Var.Em.SuperFriendly && em2 == Var.Em.Lonely)
            return true;





        return false;
    }
}
