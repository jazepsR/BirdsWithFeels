using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class fillEnemy : MonoBehaviour {
    public Image[] Enemies;      

    void Awake()
    {
        Var.spriteDict.Add("Neutral",Resources.Load<Sprite>("Sprites/neutral"));
        Var.spriteDict.Add("Confident",Resources.Load<Sprite>("Sprites/confident"));
        Var.spriteDict.Add("Scared",Resources.Load<Sprite>("Sprites/scared"));
        Var.spriteDict.Add("Lonely",Resources.Load<Sprite>("Sprites/lonely"));
        Var.spriteDict.Add("Friendly", Resources.Load<Sprite>("Sprites/friendly"));
    }

        // Use this for initialization
        void Start () {
        try
        {
            for (int i = 0; i < 3; i++)
            {
                int enemyPos = 0;
                /*while (true)
                {
                    enemyPos = Random.Range(0, 4);
                    if (Var.enemies[enemyPos] == null)
                    {
                        break;
                    }
                }*/
                Bird enemy = new Bird("Enemy", (int)Random.Range(-6, 6), (int)Random.Range(-6, 6));
                Var.enemies[i] = enemy;
                Enemies[i].sprite = Var.spriteDict[enemy.emotion.ToString()];
                Enemies[i].color = Color.white;


            }
        }catch
        {
            int j = 0;
        }
        int k = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
