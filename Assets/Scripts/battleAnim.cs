using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleAnim :MonoBehaviour {
    public static battleAnim Instance { get; private set; }
    List<battleData> battles = new List<battleData>();
    float enemySpeed = 0.85f;



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

        StartCoroutine(DoBattles(1.2f));
    }


	void StartBattle(Bird player,Bird enemy)
    {
        
        LeanTween.move(enemy.transform.gameObject, player.transform.position + Helpers.Instance.dirToVector(enemy.position), enemySpeed).setEase(LeanTweenType.easeInBack); 
        

    }

    IEnumerator DoBattles(float waitTime)
    {
        yield return new WaitForSeconds(1.8f);
        foreach (battleData battle in battles)
        {

            StartBattle(battle.player,battle.enemy);
            StartCoroutine(ShowResult(battle, 3.05f));
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
            foreach (Bird bird in FillPlayer.Instance.playerBirds)
            {               
                bird.UpdateBattleCount();
                bird.AddRoundBonuses();
                GuiContoler.Instance.UpdateBirdSave(bird);
                bird.SetEmotion();
            }
            yield return new WaitForSeconds(2.4f);
            GuiContoler.Instance.InitiateGraph(Var.activeBirds[0]);
            GuiContoler.Instance.CreateBattleReport();
        }
       
    }

    IEnumerator ShowResult(battleData battle,float waitTime)
    {
        AudioControler.Instance.mainAudioSource.PlayOneShot(AudioControler.Instance.enemyMove);
        yield return new WaitForSeconds(enemySpeed+0.25f);
        ShowBattleResult(battle);
        yield return new WaitForSeconds(waitTime-(enemySpeed + 0.25f));
    }

    void ShowBattleResult(battleData battle)
    {
        battle.player.GetComponent<Animator>().SetBool("iswalking", false);
        
        //Player won
        if (battle.result == 1)
        {
            AudioControler.Instance.mainAudioSource.PlayOneShot(AudioControler.Instance.playerWin);
            battle.player.GetComponent<Animator>().SetTrigger("victory 0");
            battle.enemy.gameObject.SetActive(false);
            Helpers.Instance.EmitEmotionParticles(battle.player.transform, Var.Em.Confident);
          //  enemy.GetComponent<Animator>().SetBool("lose", true);
        }
        else
        {
            battle.player.GetComponent<Animator>().SetTrigger("lose 0");
            AudioControler.Instance.EnemySound();
            battle.player.ChageHealth(-1);
            Helpers.Instance.EmitEmotionParticles(battle.player.transform, Var.Em.Scared);
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