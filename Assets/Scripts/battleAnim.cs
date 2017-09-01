using System.Collections;
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

	public void AddData(Bird player, Bird enemy, int result)
	{
		battles.Add(new battleData(player, enemy, result));    
	}

	public void Battle()
	{

		StartCoroutine(DoBattles(2.7f));
	}


	void StartBattle(Bird player,Bird enemy)
	{
        enemy.GetComponentInChildren<Animator>().SetBool("walk", true);
		LeanTween.move(enemy.transform.gameObject, player.transform.position + Helpers.Instance.dirToVector(enemy.position), enemySpeed).setEase(LeanTweenType.easeInBack); 
		

	}

	IEnumerator DoBattles(float waitTime)
	{
		yield return new WaitForSeconds(1.8f);
		foreach (battleData battle in battles)
		{

			StartBattle(battle.player,battle.enemy);
			StartCoroutine(ShowResult(battle, 5.0f));
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
                if (bird.gameObject.activeSelf)
                {
                    bird.UpdateBattleCount();
                    bird.AddRoundBonuses();
                    GuiContoler.Instance.UpdateBirdSave(bird);
                    bird.SetEmotion();
                }
			}
			yield return new WaitForSeconds(2.4f);
			AudioControler.Instance.setBattleVolume(0f);            
			GuiContoler.Instance.InitiateGraph(Var.activeBirds[0]);
			GuiContoler.Instance.CreateBattleReport();

            if(Var.isTutorial)
            {
                yield return new WaitForSeconds(1.0f);
                Tutorial.Instance.ShowTutorialFirstGridText(Tutorial.Instance.CurrentPos);
            }
		}
	   
	}

	IEnumerator ShowResult(battleData battle,float waitTime)
	{
		AudioControler.Instance.PlaySoundWithPitch(AudioControler.Instance.enemyMove);
		yield return new WaitForSeconds(enemySpeed+0.25f);
        Vector3 cloudpos = battle.player.transform.position / 2 + battle.player.transform.position / 2;
        GameObject fightCloudObj = Instantiate(fightCloud, cloudpos, Quaternion.identity);
        Destroy(fightCloudObj, 1.5f);
        yield return new WaitForSeconds(1.5f);
        ShowBattleResult(battle);
		yield return new WaitForSeconds(waitTime-(enemySpeed + 1.75f));
	}

	void ShowBattleResult(battleData battle)
	{
		   
        //Player won
        if (battle.result == 1)
		{
			AudioControler.Instance.PlaySoundWithPitch(AudioControler.Instance.playerWin);            
            battle.player.GetComponentInChildren<Animator>().SetTrigger("victory 0");            
            battle.enemy.GetComponentInChildren<Animator>().SetBool("dead", true);
            battle.enemy.GetComponentInChildren<Animator>().SetBool("walk", false);
            // battle.enemy.gameObject.SetActive(false);
            battle.player.battleConfBoos += Var.confWinFight;
            Helpers.Instance.EmitEmotionParticles(battle.player.transform, Var.Em.Confident);
		    battle.enemy.GetComponentInChildren<Animator>().SetBool("lose", true);
            if (battle.player == GuiContoler.Instance.selectedBird)
                battle.player.showText();
		}
		else
		{
            battle.player.battleConfBoos += Var.confLoseFight;
            battle.player.GetComponentInChildren<Animator>().SetTrigger("lose 0");
            battle.enemy.GetComponentInChildren<Animator>().SetBool("walk", false);
			AudioControler.Instance.EnemySound();
			battle.player.ChageHealth(-1);
			Helpers.Instance.EmitEmotionParticles(battle.player.transform, Var.Em.Scared);
            battle.enemy.GetComponentInChildren<Animator>().SetBool("victory", true);
            if (battle.player == GuiContoler.Instance.selectedBird)
                battle.player.showText();
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