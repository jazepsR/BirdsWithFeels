﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleAnim :MonoBehaviour {
	public static battleAnim Instance { get; private set; }
	List<battleData> battles = new List<battleData>();
	float enemySpeed = 0.85f;
	GameObject fightCloud;


	void Awake()
	{
		Instance = this;
		fightCloud = Resources.Load<GameObject>("prefabs/fightcloud");
	}
    private void Update()
    {
        if(Var.cheatsEnabled && Input.GetKeyDown(KeyCode.P))
        {
            DoQuickTransition();
        }
    }
    public void AddData(Bird player, Bird enemy, int result)
	{
		battles.Add(new battleData(player, enemy, result));    
	}

	public void Battle()
	{
		StartCoroutine(DoBattles(Helpers.Instance.winWaitTime, Helpers.Instance.loseWaitTime));
	}


	void StartBattle(Bird player,Bird enemy)
	{
		enemy.GetComponentInChildren<Animator>().SetBool("walk", true);
		enemy.GetComponentInChildren<Animator>().SetBool("dead", false);
		enemy.GetComponentInChildren<Animator>().SetBool("win", false);
		AudioControler.Instance.PlaySound(AudioControler.Instance.enemyRun);
        LeanTween.move(enemy.transform.gameObject, player.transform.position + Helpers.Instance.dirToVector(enemy.position)*2, enemySpeed).setEase(LeanTweenType.easeInBack).setOnComplete(()=>
			enemy.GetComponentInChildren<Animator>().SetBool("walk", false)
		); 
	}

	IEnumerator DoBattles(float waitTime, float loseWaitTime)
	{
        GuiContoler.Instance.canPause(false);
		yield return new WaitForSeconds(waitTime/3f);

        //float extraWait = 0.8f;
        foreach (battleData battle in battles)
		{

			StartBattle(battle.player,battle.enemy);
			StartCoroutine(ShowResult(battle, waitTime+1f));
			if (battle.result != 1)
				yield return new WaitForSeconds(loseWaitTime + 1);
			else
				yield return new WaitForSeconds(waitTime + 1f);
		}
		battles = new List<battleData>();
		bool canReroll = false;
		foreach (Bird bird in Var.activeBirds)
		{
			//if (Helpers.Instance.ListContainsLevel(Levels.type.Lonely2, bird.data.levelList) && bird.data.CoolDownLeft == 0 && !bird.foughtInRound)
			//	canReroll = true;
			
		}
        if (canReroll)
        {
            GuiContoler.Instance.rerollBox.SetActive(true);
        }
        else
        {
            yield return new WaitForSeconds((battles.Count + 1) * 0.2f);
            foreach (Bird bird in FillPlayer.Instance.playerBirds)
            {
                if (bird.gameObject.activeSelf)
                {
                    bird.gameObject.GetComponentInChildren<Animator>().SetBool("rest", false);
                    bird.TryLevelUp();
                    bird.AddRoundBonuses();
                    bird.SetEmotion();
                }
            }

            AudioControler.Instance.battleOver.Play();
            if (!Var.freezeEmotions)
            {
                yield return new WaitForSeconds(1.0f);
                AudioControler.Instance.ActivateMusicSource(audioSourceType.graphMusic);
            }
            else
            {
                AudioControler.Instance.ActivateMusicSource(audioSourceType.musicSource);
            }
            yield return new WaitForSeconds(1.0f);
            foreach (Bird bird in Var.activeBirds)
                bird.GetComponentInChildren<Animator>().SetBool("lose", false);
            if (Var.isBoss)
            {
                GuiContoler.Instance.Reset();
            }
            else
            {
                if (Var.freezeEmotions)
                {
                    //SHOW TARNSITION AND CREATE NEXT BATTLE

                    //Debug.Log("canplayBossTransition: " + GuiContoler.Instance.canplayBossTransition);
                    if (GuiContoler.Instance.canplayBossTransition && (GuiContoler.Instance.nextNextMapArea != Var.Em.finish) && !Var.isEnding)
                    {
                        DoQuickTransition();
                    }
                    yield return new WaitForSeconds(0.5f);

                }
				else
				{
				GuiContoler.Instance.InitiateGraph(Var.activeBirds[0]);				
				GuiContoler.Instance.CreateBattleReport();
                }
            }
            foreach (Bird enemy in Var.enemies)
            {
                if (enemy != null)
                    LeanTween.cancel(enemy.gameObject);
            }
            if (Var.isTutorial)
            {
                //Debug.Log("hello its me: " + Tutorial.Instance.showedSecondBirdReportText);
                if (GuiContoler.Instance.GraphActive)
                 {

                     //GuiContoler.Instance.nextGraph.interactable = false;
                 }

                yield return new WaitForSeconds(1.0f);

     
                Tutorial.Instance.ShowTutorialFirstGridText(Tutorial.Instance.CurrentPos);
            }
            if (Var.isEnding)
            {

                //if (GuiContoler.Instance.GraphActive)
                //{
                  //  GuiContoler.Instance.nextGraph.interactable = false; //not sure if ending needs this but im going to put it here 
                //}

                yield return new WaitForSeconds(1.0f);
                Ending.Instance.ShowEndingFirstGridText(Tutorial.Instance.CurrentPos);
            }
        }
	}

    private void DoQuickTransition()
    {      
        GuiContoler.Instance.bossTransition.GetComponent<Animator>().SetTrigger("TriggerTransition");
        //GuiContoler.Instance.canplayBossTransition = false;
        LeanTween.delayedCall(0.5f,()=>
        GuiContoler.Instance.CloseGraph());        
    }

	IEnumerator ShowResult(battleData battle,float waitTime)
	{
        if (battle.player.data.injured)
        {
            battle.enemy.GetComponentInChildren<Animator>().SetBool("walk", true);
            AudioControler.Instance.PlaySound(AudioControler.Instance.enemyRun);
            LeanTween.move(battle.enemy.transform.gameObject, battle.player.transform.position, enemySpeed).setEase(LeanTweenType.easeOutQuad);
            Debug.Log("I moved");
        }
        else
        {
            AudioControler.Instance.PlaySound(AudioControler.Instance.enemyRun);
            battle.player.GetComponentInChildren<Animator>().SetBool("lose", false);
            yield return new WaitForSeconds(enemySpeed);
            if (battle.enemy.position == Bird.dir.top)
                battle.player.GetComponentInChildren<Animator>().SetTrigger("startTalking_up");
            else
                battle.player.GetComponentInChildren<Animator>().SetTrigger("startTalking_right");
            battle.enemy.GetComponentInChildren<Animator>().SetTrigger("startListening");
            battle.player.GetComponentInChildren<Animator>().SetTrigger("startTalking");

            if (battle.player.birdSounds.birdBattleConversations.clips.Length == 0)
                battle.player.birdSounds.birdBattleConversations = battle.player.birdSounds.birdDialogueTalk;

            AudioControler.Instance.PlaySound(battle.player.birdSounds.birdBattleConversations);

            yield return new WaitForSeconds(0.8f);
            AudioControler.Instance.PlaySound(AudioControler.Instance.considerSound);
            yield return new WaitForSeconds(1.0f);
            //lose
            if (battle.result != 1)
            {
                AudioControler.Instance.PlaySound(AudioControler.Instance.fightCloudSound);
                Vector3 cloudpos = battle.player.transform.position / 2 + battle.player.transform.position / 2;
                GameObject fightCloudObj = Instantiate(fightCloud, cloudpos, Quaternion.identity);
                if (!battle.player.hasShieldBonus)
                {
                    battle.player.GetComponentInChildren<Animator>().SetBool("lose", true);
                }
                else
                {
                    Helpers.Instance.EmitEmotionParticles(battle.player.transform, Var.Em.Shield);
                    battle.player.GetComponentInChildren<Animator>().SetTrigger("stopTalking");
                }
                battle.enemy.GetComponentInChildren<Animator>().SetBool("win", true);
                Destroy(fightCloudObj, waitTime - enemySpeed);
                yield return new WaitForSeconds(waitTime - enemySpeed - 2.3f);




                /*/if (!battle.player.hasShieldBonus)
                {
                    AudioControler.Instance.PlaySound(AudioControler.Instance.fightCloudSound);
                    Vector3 cloudpos = battle.player.transform.position / 2 + battle.player.transform.position / 2;
                    GameObject fightCloudObj = Instantiate(fightCloud, cloudpos, Quaternion.identity);
                    battle.player.GetComponentInChildren<Animator>().SetBool("lose", true);
                    battle.enemy.GetComponentInChildren<Animator>().SetBool("win", true);
                    Destroy(fightCloudObj, waitTime - enemySpeed);
                    yield return new WaitForSeconds(waitTime - enemySpeed - 2.3f);
                }
                else
                {
                    battle.player.GetComponentInChildren<Animator>().SetBool("rest", true);
                    LeanTween.delayedCall(1.5f,()=>battle.player.GetComponentInChildren<Animator>().SetBool("rest", false));
                }*/
                battle.enemy.GetComponentInChildren<Animator>().SetBool("walk", true);
                LeanTween.move(battle.enemy.transform.gameObject, battle.player.transform.position, enemySpeed).setEase(LeanTweenType.easeOutQuad);
            }
            else
            {
                AudioControler.Instance.PlaySound(battle.player.birdSounds.birdWinSound);
                battle.enemy.GetComponentInChildren<Animator>().SetBool("win", false);
            }
        }
		ShowBattleResult(battle);
	}

	void ShowBattleResult(battleData battle)
	{

        if (battle.player.data.injured)
        {
            battle.enemy.GetComponentInChildren<Animator>().SetBool("walk", true);
            LeanTween.delayedCall(0.1f, () => LeanTween.move(battle.enemy.transform.gameObject, battle.enemy.transform.position - 20 * Helpers.Instance.dirToVector(battle.enemy.position)
                     , 2.75f).setEaseOutQuad());            
        }
        else
        {
            //Player won
            if (battle.result == 1)
            {
                AudioControler.Instance.PlaySound(AudioControler.Instance.conflictWin);
                battle.player.GetComponentInChildren<Animator>().SetTrigger("victory 0");
                battle.player.battleConfBoos += Var.confWinFight;
                Helpers.Instance.EmitEmotionParticles(battle.player.transform, Var.Em.Confident);
                AudioControler.Instance.PlaySound(AudioControler.Instance.confidentParticlePostBattle);
                //battle.enemy.GetComponentInChildren<Animator>().SetBool("lose", true);
                if (battle.player == GuiContoler.Instance.selectedBird)
                    battle.player.showText();
                print(battle.player.charName + " won fight");
            }
            else
            {
                battle.player.battleConfBoos += Var.confLoseFight;
                if (!(battle.enemy.enemyType == fillEnemy.enemyType.drill && !battle.enemy.foughtInRound))
                    LeanTween.delayedCall(0.1f, () => LeanTween.move(battle.enemy.transform.gameObject, battle.enemy.transform.position - 20 * Helpers.Instance.dirToVector(battle.enemy.position)
                     , 2.75f).setEaseOutQuad());
                AudioControler.Instance.PlaySound(AudioControler.Instance.combatLose);
                battle.player.ChageHealth(-1);
                Helpers.Instance.EmitEmotionParticles(battle.player.transform, Var.Em.Cautious);
                //battle.enemy.GetComponentInChildren<Animator>().SetBool("victory", true);
                if (battle.player == GuiContoler.Instance.selectedBird)
                    battle.player.showText();
                print(battle.player.charName + " lost fight");
            }
        }
            battle.enemy.foughtInRound = true;
        
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