using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class fillEnemy : MonoBehaviour {
    public GameObject[] Enemies;      

        // Use this for initialization
    void Start ()
    {
        createEnemies();
        
    } 
    public void createEnemies(float minConf=-5, float maxConf=5, float minFriend=-5, float maxFriend=5)
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
                enemy.confidence = (int)Random.Range(minConf, maxConf);
                enemy.friendliness = (int)Random.Range(minFriend, maxFriend);
                enemy.SetEmotion();
                Enemies[enemyPos].SetActive(true);
                enemy.inUse = true;
                
            }
        
        
    }
    // Update is called once per frame
    void Update () {
        
    }
}
