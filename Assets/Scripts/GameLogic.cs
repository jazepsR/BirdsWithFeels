﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

	// What is the class of birds?
	public class BirdExampleClass
		for(int i =0; i<4; i++)
        {
            Bird a = new Bird("Alice",(int)Random.Range(-15, 15), (int)Random.Range(-15, 15));
            Bird b = new Bird("Bob", (int)Random.Range(-15, 15), (int)Random.Range(-15, 15));
            Debug.Log(Fight(a, b).charName + " won!");
        }
	[HideInInspector]
	public Vector2 dropVector;
	[HideInInspector]
	public Vector3 touchStartPosition;

	private Vector3 dragPosition;
	private Vector3 mouseOffset;

	public Image dragImage = null;
	private bool dragingBird = false;

	[HideInInspector]
	public int[,] board = new int[5, 3];

	void Awake()
	{
		Instance = this;

		// Just some random stuff
		Application.targetFrameRate = 60;
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton (0)) {
			if (dragingBird) {
				// We drag the action point!
				Vector3 currentScreenSpace = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10f);
				//convert screen position to world position with offset changes.
				dragPosition = Camera.main.ScreenToWorldPoint (currentScreenSpace) + mouseOffset;

				dragImage.transform.position = dragPosition;
			}
		} else if (Input.GetMouseButtonUp (0)) {
			if (dropVector.x != -1) {
				// Can we drop the bird here?
				board[(int)dropVector.y,(int)dropVector.x] = 1;	// Put bird id here?
			} else {
				
				// Cancel action
				dragImage.sprite = null;
			}

			dragingBird = false;
		}
	}

	// For now!
	public void OnDragBird(BirdButton info)
	{
		// Dont start this if finger not moved a bit up !
		dragingBird = true;

		dragImage.sprite = info.src.sprite;

		mouseOffset = new Vector3(0,0,10f);
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
