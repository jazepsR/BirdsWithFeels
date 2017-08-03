using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class fillEnemy : MonoBehaviour {
    public GameObject[] Enemies;      

        // Use this for initialization
    void Start ()
    {
        BattleData Area = Var.map[0];
        createEnemies(Area.minConf, Area.maxConf, Area.minFriend, Area.maxFriend, Area.birdLVL, Area.dirs,Area.minEnemies,Area.maxEnemies);

    } 
    public void createEnemies(float minConf=-5, float maxConf=5, float minFriend=-5, float maxFriend=5,int birdLVL = 1,List<Bird.dir> dirList= null,int minEnemies= 3,int maxEnemies =4)
    {
        int index = 0;
        int frontBirds = 0;
        if (dirList == null)
            dirList = new List<Bird.dir>() { Bird.dir.front, Bird.dir.top};
        
        int max = maxEnemies;
        print("min: "+ minEnemies + " max: " + maxEnemies);
        if (dirList.Count == 1 && dirList.Contains(Bird.dir.front))
            max = (int)Mathf.Min( 3f,maxEnemies);
        foreach(GameObject enemy in Enemies)
        {
            Var.enemies[index] = enemy.GetComponent<Bird>();
            enemy.GetComponent<feedBack>().myIndex = index % 4;            
            enemy.GetComponent<Bird>().levelRollBonus = (int)Mathf.Max(1,Helpers.Instance.RandGaussian(1, birdLVL))-1;
            enemy.GetComponent<Bird>().inUse = false;
            enemy.SetActive(false);
            index++;
        }


        List<int> usedPos = new List<int>();
        int enemyCount = Random.Range(minEnemies, max+1);
        for (int i = 0; i < enemyCount; i++)
        {
            int enemyPos = 0;
            if (i == 0 && dirList.Contains(Bird.dir.front))
            {
                enemyPos = Random.Range(4, 8);
            }
            else
            {
                while (true)
                {
                    enemyPos = Random.Range(0, Var.enemies.Length);
                    Bird enemyScript = Enemies[enemyPos].GetComponent<Bird>();
                    if (!usedPos.Contains(enemyPos) && !(frontBirds >= 3 && enemyScript.position == Bird.dir.front) && dirList.Contains(enemyScript.position)) 
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
            enemy.GetComponent<feedBack>().SetEnemyHoverText();
            Enemies[enemyPos].SetActive(true);
            enemy.inUse = true;
            enemy.transform.localPosition = enemy.home;
        }
        
        
    } 
    public void Reset()
    {
       foreach(Bird enemy in Var.enemies)
        {
            if (enemy.inUse)
            {
                enemy.gameObject.SetActive(true);
                enemy.transform.localPosition = enemy.home;
                enemy.GroundRollBonus = 0;
                enemy.colorRenderer.color = Helpers.Instance.GetEmotionColor(enemy.emotion);
            }

        }
    }
}
