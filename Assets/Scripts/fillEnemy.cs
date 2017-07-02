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
    public void createEnemies(float minConf=-5, float maxConf=5, float minFriend=-5, float maxFriend=5,int birdLVL = 1)
    {
        int index = 0;
        int frontBirds = 0;
        foreach(GameObject enemy in Enemies)
        {
            Var.enemies[index] = enemy.GetComponent<Bird>();
            enemy.GetComponent<feedBack>().myIndex = index % 4;
            enemy.GetComponent<Bird>().level = (int)Mathf.Max(1,Helpers.Instance.RandGaussian(1, birdLVL));
            enemy.GetComponent<Bird>().inUse = false;
            enemy.SetActive(false);
            index++;
        }


        List<int> usedPos = new List<int>();
        int enemyCount = Random.Range(3, 5);
            for (int i = 0; i < enemyCount; i++)
            {
                int enemyPos = 0;
            if (i == 0)
            {
                enemyPos = Random.Range(4, 8);
            }
            else
            {
                while (true)
                {
                    enemyPos = Random.Range(0, Var.enemies.Length);
                    if (!usedPos.Contains(enemyPos) && !(frontBirds >= 3 && Enemies[enemyPos].GetComponent<Bird>().position == Bird.dir.front))
                    {
                        break;
                    }
                }
            }
                usedPos.Add(enemyPos);
                Bird enemy = Enemies[enemyPos].GetComponent<Bird>();
               
                if (enemy.position == Bird.dir.front)
                    frontBirds++;
                enemy.confidence = (int)Random.Range(minConf, maxConf);
                enemy.friendliness = (int)Random.Range(minFriend, maxFriend);
                enemy.SetEmotion();
                Enemies[enemyPos].SetActive(true);
                enemy.inUse = true;
                enemy.transform.localPosition = enemy.home;
        }
        
        
    } 
}
