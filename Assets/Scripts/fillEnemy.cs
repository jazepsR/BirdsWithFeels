using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class fillEnemy : MonoBehaviour {
    public GameObject[] Enemies;
    public enum enemyType {normal,wizard, drill };
    [HideInInspector]
    public bool hasDrill = false;
        // Use this for initialization
    void Start ()
    {

        if (Var.isTutorial)
            CreateTutorialEnemies(Tutorial.Instance.TutorialMap[0]);
        else
        {
            BattleData Area = Var.map[0];
            createEnemies(Area.battleData, Area.birdLVL, Area.dirs, Area.minEnemies, Area.maxEnemies, Area.hasWizards, Area.hasDrills);
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
                
            }
            index++;
        }
    }



    public void createEnemies(MapBattleData battleData, int birdLVL = 1, List<Bird.dir> dirList = null, int minEnemies = 3, int maxEnemies = 4, bool hasWizards = false, bool hasDrills = false)
    {
        int index = 0;
        //hasWizards = true;
        //hasDrills = true;
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
                frontPos.Add(enemyPos);
            if (enemy.position == Bird.dir.top)
                topPos.Add(enemyPos);                       
            if (enemy.position == Bird.dir.front)
                frontBirds++;
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
            if (rand < 0.2f && hasWizards)
                CreateEnemy(enemy, enemyType.wizard);
            else
                CreateEnemy(enemy);            
            Enemies[enemyPos].SetActive(true);
        }
        if (hasDrills) {
            float drillrange = 0.3f;

            if (frontPos.Count < 3 && frontPos.Count > 0 && Random.Range(0, 1f) < drillrange) {
                int id = frontPos[Random.Range(0, frontPos.Count)];
                setAsDrill(Enemies[id].GetComponent<Bird>());
            }
            if (topPos.Count < 3 && topPos.Count > 0 && Random.Range(0, 1f) < drillrange)
            {
                int id = topPos[Random.Range(0, topPos.Count)];
                setAsDrill(Enemies[id].GetComponent<Bird>());
            }

        }
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


    void setAsDrill(Bird bird)
    {
        bird.enemyType = enemyType.drill;
        foreach (TextMesh text in bird.transform.GetComponentsInChildren<TextMesh>())
        {
            if (text.gameObject.tag == "number")
            {                
                text.text = "D";
                text.color = Color.red;
            }
        }

    }


    void CreateEnemy(Bird enemy, enemyType type = enemyType.normal)
    {
        enemy.SetEmotion();
        
        if (enemy.EnemyArt != null)
            Destroy(enemy.EnemyArt);
        enemy.EnemyArt = Instantiate(Helpers.Instance.GetEnemyVisual(enemy.position, enemy.emotion), enemy.transform);
        enemy.EnemyArt.transform.localPosition = new Vector3(0, 0, 0);
        enemy.enemyType = type;
        if (enemy.emotion == Var.Em.Neutral && enemy.enemyType == enemyType.wizard)
            enemy.enemyType = enemyType.normal;
        foreach (SpriteRenderer child in enemy.EnemyArt.transform.GetComponentsInChildren<SpriteRenderer>(true))
        {
            if (child.gameObject.name.Contains("flat"))
            {
                child.color = Helpers.Instance.GetEmotionColor(enemy.emotion);
            }

        }       
        enemy.GetComponent<feedBack>().SetEnemyHoverText();        
        enemy.GetComponentInChildren<Animator>().SetBool("dead", false);
        enemy.inUse = true;
        enemy.transform.localPosition = enemy.home;
        foreach (TextMesh text in enemy.transform.GetComponentsInChildren<TextMesh>())
        {
            if (text.gameObject.tag == "number")
            {
                if (enemy.enemyType == enemyType.wizard)
                    text.text = "W";
                
                else
                    text.text = (enemy.levelRollBonus + 1).ToString();                
            }
        }
    }

    public void Reset()
    {
        hasDrill = false;
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
