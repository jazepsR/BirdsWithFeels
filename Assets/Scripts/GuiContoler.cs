using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiContoler : MonoBehaviour {
    public Text infoText;
    public GameObject[] players;
    public GameObject[] enemies;
    public Image[] tiles;

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

     
       

       for (int i = 0; i <Var.enemies.Length; i++)
       {
            if (enemies[i].GetComponent<Bird>().enabled)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (Var.playerPos[j, i] != null)
                    {
                        playerBird = Var.playerPos[j, i];
                        playerBird.friendliness += Helpers.Instance.Findfirendlieness(j, i);
                        break;
                    }

                }
                result += GameLogic.Instance.Fight(playerBird, Var.enemies[i]);
                
            }

    }
        if (result > 0)
        {
            Debug.Log("Player won!");
            foreach(GameObject bird in players)
            {
                bird.GetComponent<Bird>().confidence+= Var.confLoseAll;
            }
        }
        else
        {
            Debug.Log("Enemy won");
            foreach (GameObject bird in players)
            {
                bird.GetComponent<Bird>().confidence+= Var.confLoseAll;
            }
        }
        Reset();
    }

    public void Reset()
    {
        Var.playerPos = new Bird[3, 5];
        Var.enemies = new Bird[5];
        foreach(GameObject bird in players)
        {
            bird.GetComponent<Bird>().SetEmotion();
            bird.GetComponent<Bird>().src.enabled = true;
        }
        GetComponent<fillEnemy>().createEnemies();
        foreach(Image img in tiles)
        {
            img.sprite = null;
        }
    }

}
