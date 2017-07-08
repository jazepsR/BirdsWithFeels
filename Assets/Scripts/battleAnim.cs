using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleAnim :MonoBehaviour {
    public static battleAnim Instance { get; private set; }
    List<battleData> battles = new List<battleData>();




    void Awake()
    {
        Instance = this;
    }

    public void AddData(Bird player, Bird enemy, int result)
    {
        battles.Add(new battleData(player, enemy, result));    
    }

    public void Battle()
    {

        StartCoroutine(DoBattles(0.5f));
    }


	void StartBattle(Bird player,Bird enemy)
    {
        
        LeanTween.move(enemy.transform.gameObject, player.transform.position + Helpers.Instance.dirToVector(enemy.position), 0.7f).setEase(LeanTweenType.easeInBack); 
        

    }

    IEnumerator DoBattles(float waitTime)
    {
        foreach (battleData battle in battles)
        {

            StartBattle(battle.player,battle.enemy);
            StartCoroutine(ShowResult(battle, 0.85f));
            yield return new WaitForSeconds(waitTime);
        }
        battles = new List<battleData>();
        bool canReroll = false;
        foreach (Bird bird in Var.activeBirds)
        {
            if (Helpers.Instance.ListContainsLevel(Levels.type.Lonely2, bird.levelList) && bird.CoolDownLeft == 0 && !bird.foughtInRound)
                canReroll = true;
            
        }
        if (canReroll)
        {
            GuiContoler.Instance.rerollBox.SetActive(true);
        }
        else
        {
            foreach (Bird bird in Var.activeBirds)
            {
                bird.UpdateBattleCount();
                bird.AddRoundBonuses();
            }
            yield return new WaitForSeconds(1.5f);
            GuiContoler.Instance.CreateGraph();
            GuiContoler.Instance.CreateBattleReport();
        }
       
    }

    IEnumerator ShowResult(battleData battle,float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        ShowBattleResult(battle);
    }

    void ShowBattleResult(battleData battle)
    {
        battle.player.GetComponent<Animator>().SetBool("iswalking", false);
        //Player won
        if (battle.result == 1)
        {
            battle.player.GetComponent<Animator>().SetBool("victory", true);
            battle.enemy.gameObject.SetActive(false);
          //  enemy.GetComponent<Animator>().SetBool("lose", true);
        }
        else
        {
            battle.player.GetComponent<Animator>().SetBool("lose", true);
            battle.player.ChageHealth(-1);
            // enemy.GetComponent<Animator>().SetBool("victory", true);

        }
    }
}



class battleData
{
    public Bird player;
    public Bird enemy;
    public int result;    

    public battleData(Bird player, Bird enemy, int result)
    {
        this.player = player;
        this.enemy = enemy;
        this.result = result;      
    }
}