using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class fillEnemy : MonoBehaviour {
    public GameObject[] Enemies;      

    void Awake()
    {
        Var.spriteDict.Add("Neutral",Resources.Load<Sprite>("Sprites/neutral"));
        Var.spriteDict.Add("Confident",Resources.Load<Sprite>("Sprites/confident"));
        Var.spriteDict.Add("Scared",Resources.Load<Sprite>("Sprites/scared"));
        Var.spriteDict.Add("Lonely",Resources.Load<Sprite>("Sprites/lonely"));
        Var.spriteDict.Add("Friendly", Resources.Load<Sprite>("Sprites/friendly"));
    }

        // Use this for initialization
    void Start ()
    {
        createEnemies();
		
	} 
    public void createEnemies()
    {
        int index = 0;
        foreach(GameObject enemy in Enemies)
        {
            Var.enemies[index] = enemy.GetComponent<Bird>();
            enemy.GetComponent<Bird>().inUse = false;
            enemy.SetActive(false);
            index++;
        }


        List<int> usedPos = new List<int>();
        
            for (int i = 0; i < 3; i++)
            {
                int enemyPos = 0;
                while (true)
                {
                    enemyPos = Random.Range(0, Var.enemies.Length);
                    if (!usedPos.Contains(enemyPos))
                    {
                        break;
                    }
                }
                usedPos.Add(enemyPos);
                Bird enemy = Enemies[enemyPos].GetComponent<Bird>();
                enemy.confidence = (int)Random.Range(-6, 6);
                enemy.friendliness = (int)Random.Range(-6, 6);
                enemy.SetEmotion();
                Enemies[enemyPos].SetActive(true);
                enemy.inUse = true;
                
            }
        
        
    }
	// Update is called once per frame
	void Update () {
		
	}
}
