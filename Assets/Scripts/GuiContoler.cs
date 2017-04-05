using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiContoler : MonoBehaviour {
    public Text infoText;
    public GameObject[] players;
    public GameObject[] enemies;

    void Awake()
    {
        Var.birdInfo = infoText;
    }
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Fight()
    {

        Debug.Log("Fight selected");
        int result = 0;
        Bird playerBird = null;

        /*for (int i = 0; i < Var.enemies.Length; i++)
        {//
            for (int j = 0; j < 3; j++)
            {
                Debug.Log(i + " " + j + " " +(Var.playerPos[j,i] == null));

            }
        }*/
        for (int i = 0; i < Var.enemies.Length; i++)
       {
            Debug.Log(i + " " + " " + (Var.enemies[i] == null));
        }

               for (int i = 0; i <3; i++)
       {//try
           {
               
                   for (int j = 0; j < 3; j++)
                   {
                       if (Var.playerPos[j, i] != null)
                       {
                           playerBird = Var.playerPos[j, i];
                       }

                   }


                   result += GameLogic.Instance.Fight(playerBird, Var.enemies[i]);
               
               
           }
           /*catch
           {
               Debug.Log("Catch");
           }*/

    }
        if (result > 0)
        {
            Debug.Log("Player won!");
        }
        else
        {
            Debug.Log("Enemy won");
        }

    }

    public void Reset()
    {
        Var.playerPos = new Bird[3, 5];
        Var.enemies = new Bird[5];
        foreach(GameObject bird in players)
        {
            bird.GetComponent<Bird>().SetEmotion();
        }
        foreach(GameObject evilBird in enemies)
        {
            
            
        }
    }

}
