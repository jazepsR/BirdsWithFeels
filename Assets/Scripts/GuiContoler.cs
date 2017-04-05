using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiContoler : MonoBehaviour {
    public Text infoText;
    public GameObject[] players;
    public GameObject[] enemies;
    public Image[] tiles;
    public Text reportText;
    public GameObject report;
    void Awake()
    {
        Var.birdInfo = infoText;
    }
	// Use this for initialization
	void Start () {
		
	}
	public void CloseReport()
    {
        report.SetActive(false);
    }
	// Update is called once per frame
	void Update () {
		
	}
    public void CreateReport(bool won)
    {
        string reportString = "";
        if (won)
        {
            reportString = "You won the fight!\n";
        }else
        {
            reportString = "You lost the fight :'(\n";
        }
        foreach(GameObject player in players)
        {
            Bird script = player.GetComponent<Bird>();
            reportString += script.charName + ": ";
            if ((script.confidence - script.prevConf) > 0)
                reportString += "Won\n";
            else
                reportString += "Lost\n";
            reportString += "friendliness: " + script.prevFriend + " -> " + script.friendliness +" (" +(script.friendliness - script.prevFriend) + ")";
            reportString += "\nconfidence: " + script.prevConf + " -> " + script.confidence + " (" + (script.confidence - script.prevConf) + ")\n"; 
        }
        reportText.text = reportString;
        report.SetActive(true);
        Debug.Log(reportString);
    }
    public void Fight()
    {

        Debug.Log("Fight selected");
        int result = 0;
        Bird playerBird = null;

     
       

       for (int i = 0; i <Var.enemies.Length; i++)
       {
            if (enemies[i].GetComponent<Bird>().inUse)
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
                bird.GetComponent<Bird>().confidence+= Var.confWinAll;
            }
            CreateReport(true);
        }
        else
        {
            Debug.Log("Enemy won");
            foreach (GameObject bird in players)
            {
                bird.GetComponent<Bird>().confidence+= Var.confLoseAll;
            }
            CreateReport(false);
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
        }
        GetComponent<fillEnemy>().createEnemies();
        foreach(Image img in tiles)
        {
            img.sprite = null;
        }

		// Main battle field button quick restart?
		GameLogic.Instance.OnRestartGame ();
    }

}
