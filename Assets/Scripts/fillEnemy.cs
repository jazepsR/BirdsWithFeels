using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class fillEnemy : MonoBehaviour {
    public GameObject[] Enemies;
    public enum enemyType {normal,wizard, drill,super };
    [Header("Debug")]
    public bool isDebug = false;
    public bool hasDrillsDebug = false;
    public bool hasWizardsDebug = false;
	List<Bird> newBirds;
    // Use this for initialization
    void Start ()
    {
		newBirds = new List<Bird>();
        if (Var.isTutorial)
            CreateTutorialEnemies(Tutorial.Instance.TutorialMap[0]);
        else
        {
            BattleData Area = Var.map[0];
            createEnemies(Area.battleData, Area.birdLVL, Area.dirs, Area.minEnemies, Area.maxEnemies, Area.hasWizards, Area.hasDrills,Area.hasSuper);
        }
    } 

    public void CreateTutorialEnemies(List<TutorialEnemy> enemies)
    {
        int index = 0;
        foreach (GameObject enemy in Enemies)
        {
            Var.enemies[index] = enemy.GetComponent<Bird>();
            foreach(feedBack fb in enemy.GetComponents<feedBack>())
            {
                fb.myIndex = index % 4;
            }
            enemy.GetComponent<Bird>().levelRollBonus = 0;
            enemy.GetComponent<Bird>().inUse = false;
            enemy.SetActive(false);
            index++;
        }


        index = 0;
        foreach (TutorialEnemy en in enemies)
        {
            if (en != null)
            {
                Bird enemy = Enemies[index].GetComponent<Bird>();
                enemy.confidence = en.confidence;
                enemy.friendliness = en.firendliness;
                CreateEnemy(enemy);             
                Enemies[index].SetActive(true);
                ApplyArt(enemy);
            }
            index++;
        }
    }



    public void createEnemies(MapBattleData battleData, int birdLVL = 1, List<Bird.dir> dirList = null, int minEnemies = 3, int maxEnemies = 4, bool hasWizards = false, bool hasDrills = false, bool hasSuper= false)
    {
        int index = 0;
        float wizardChance = 0.2f;
        float drillChance = 0.3f;
		float superChance = 0.5f;
        //isDebug = true;
        if (isDebug)
        {
            hasWizards = hasWizardsDebug;
            wizardChance = 0.1f;
            drillChance = 0.9f;
            hasDrills = hasDrillsDebug;
			hasSuper = true;
			superChance = 1f;
        }
        List<int> frontPos = new List<int>();
        List<int> topPos = new List<int>();
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
            foreach (feedBack fb in enemy.GetComponents<feedBack>())
            {
                fb.myIndex = index % 4;
            }        
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
            {
                frontBirds++;
                frontPos.Add(enemyPos);
            }
            if (enemy.position == Bird.dir.top)
                topPos.Add(enemyPos);        
            switch (GetEmotion(battleData))
            {
                case Var.Em.Confident:
                    enemy.confidence = 7;
                    enemy.friendliness = 0;
                    break;
                case Var.Em.Scared:
                    enemy.confidence = -7;
                    enemy.friendliness = 0;
                    break;
                case Var.Em.Friendly:
                    enemy.confidence = 0;
                    enemy.friendliness = 7;
                    break;
                case Var.Em.Lonely:
                    enemy.confidence = 0;
                    enemy.friendliness = -7;
                    break;
                case Var.Em.Neutral:
                    enemy.confidence = 0;
                    enemy.friendliness = 0;
                    break;
                default:
                    enemy.confidence = 0;
                    enemy.friendliness = 0;
                    break;                    
            }
            float rand = Random.Range(0, 1f);
			if (hasSuper && rand < superChance && rand > wizardChance)
			{
				CreateEnemy(enemy, enemyType.super);
			}
			else if (rand < wizardChance && hasWizards)
                CreateEnemy(enemy, enemyType.wizard);
            else
                CreateEnemy(enemy);            
            Enemies[enemyPos].SetActive(true);
			newBirds.Add(enemy);
		}
        if (hasDrills) {            
            if (frontPos.Count < 3 && frontPos.Count > 0 && Random.Range(0, 1f) < drillChance) {
                int id = frontPos[Random.Range(0, frontPos.Count)];
                Enemies[id].GetComponent<Bird>().enemyType = enemyType.drill;
            }
            if (topPos.Count < 3 && topPos.Count > 0 && Random.Range(0, 1f) < drillChance)
            {
                int id = topPos[Random.Range(0, topPos.Count)];
                Enemies[id].GetComponent<Bird>().enemyType= enemyType.drill;
            }
		}
		foreach (Bird newBird in newBirds)
			ApplyArt(newBird);
    } 

    void ApplyArt(Bird enemy)
    {
        enemy.EnemyArt = Instantiate(Helpers.Instance.GetEnemyVisual(enemy.position, enemy.emotion, enemy.enemyType), enemy.transform);
        enemy.EnemyArt.transform.localPosition = new Vector3(0, 0, 0);
        foreach (SpriteRenderer child in enemy.EnemyArt.transform.GetComponentsInChildren<SpriteRenderer>(true))
        {
            if (child.gameObject.name.Contains("flat"))
            {
                child.color = Helpers.Instance.GetEmotionColor(enemy.emotion);
            }
        }
        foreach (TextMesh text in enemy.transform.GetComponentsInChildren<TextMesh>())
        {
            if (text.gameObject.tag == "number")
                text.text = (enemy.levelRollBonus + 1).ToString();

        }
        enemy.GetComponent<feedBack>().SetEnemyHoverText();
        enemy.GetComponentInChildren<Animator>().SetBool("dead", false);
    }


    Var.Em GetEmotion(MapBattleData data)
    {
        Var.Em emotion = Var.Em.finish;
        float rand = Random.Range(0f, 1f);
        float currentVal = 0;
        for (int i = 0; i < data.emotionType.Count; i++)
        {
            if (rand < data.emotionPercentage[i])
            {
                emotion = data.emotionType[i];
                break;
            }
            else
            {
                currentVal += data.emotionPercentage[i];
            }
        }     
        if(emotion == Var.Em.Random)
        {
            int rnd = Random.Range(0, 5);
            switch (rnd)
            {
                case 0:
                    return Var.Em.Neutral;
                case 1:
                    return Var.Em.Confident;
                case 2:
                    return Var.Em.Lonely;
                case 3:
                    return Var.Em.Scared;
                case 4:
                    return Var.Em.Friendly;
            }
        }
        return emotion;
    }


  


    void CreateEnemy(Bird enemy, enemyType type = enemyType.normal)
    {
        enemy.SetEmotion();        
        if (enemy.EnemyArt != null)
            Destroy(enemy.EnemyArt);       
        enemy.enemyType = type;
        if (enemy.emotion == Var.Em.Neutral && (enemy.enemyType == enemyType.wizard || enemy.enemyType == enemyType.super))
            enemy.enemyType = enemyType.normal;
		enemy.inUse = true;
        enemy.transform.localPosition = enemy.home;       
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
                if (enemy.EnemyArt != null)
                    Destroy(enemy.EnemyArt);

            }

        }
    }
}
