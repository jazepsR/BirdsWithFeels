using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    public static Tutorial Instance { get; private set; }
    public Var.Em[] firstStageEnemies;
    public Var.Em[] secondStageEnemies;
    public Var.Em[] thirdStageEnemies;
    public Var.Em[] forthStageEnemies;
    public Var.Em[] fifthStageEnemies;
    public Var.Em[] sixthStageEnemies;

    public List<List<TutorialEnemy>> TutorialMap = new List<List<TutorialEnemy>>();
    // Use this for initialization
    void Awake () {
        Instance = this;
        AddEnemiesToList(firstStageEnemies);
        AddEnemiesToList(secondStageEnemies);
        AddEnemiesToList(thirdStageEnemies);
        AddEnemiesToList(forthStageEnemies);
        AddEnemiesToList(fifthStageEnemies);
        AddEnemiesToList(sixthStageEnemies);
    }
	void AddEnemiesToList(Var.Em[] array)
    {
        List<TutorialEnemy> list = new List<TutorialEnemy>(); 
        for(int i = 0; i < array.Length; i++)
        {
            if (array[i] == Var.Em.finish)
                list.Add(null);
            else
                list.Add(new TutorialEnemy(array[i]));
        }
        TutorialMap.Add(list);
    }
	
}




public class TutorialEnemy
{

    public int confidence;
    public int firendliness;

    public TutorialEnemy(Var.Em emotion)
    {
        switch (emotion){
            case Var.Em.Neutral:
                confidence = 0;
                firendliness = 0;
                break;
            case Var.Em.Scared:
                confidence = -7;
                firendliness = 0;
                break;
            case Var.Em.Confident:
                confidence = 7;
                firendliness = 0;
                break;
            case Var.Em.Friendly:
                confidence = 0;
                firendliness = 7;
                break;
            case Var.Em.Lonely:
                confidence = 0;
                firendliness = -7;
                break;

        }
    }

}
