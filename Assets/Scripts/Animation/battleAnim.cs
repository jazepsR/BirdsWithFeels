using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleAnim : MonoBehaviour
{
    public static battleAnim Instance { get; private set; }
    public AnimationCurve vultureRunCurve;
    public AnimationCurve vultureEnterCloudCurve;
    List<battleData> battles = new List<battleData>();
    float enemySpeed = 5f;
    float enemySwitchToTalkOffset = 0.1f;
    float enemyTalkBeforeSound = 1f;
    float enemyTalkAfterSound = 1f;
    float windUpDelay = 0.34f;
    GameObject fightCloud;

    [Header("Camera animation")]
    public AnimationCurve cameraZoomInCurve;
    public AnimationCurve cameraYMovementCurve;
    private float cameraZoomInTime = 0.3f;
    private float cameraZoomOutTime = 0.8f;
    private float cameraStartingSize;
    private float cameraStartingY;
    void Awake()
    {
        Instance = this;
        fightCloud = Resources.Load<GameObject>("prefabs/fightcloud");
        cameraStartingSize = Camera.main.orthographicSize;
        cameraStartingY = Camera.main.transform.position.y;
    }
    private void Update()
    {
        if (Var.cheatsEnabled && Input.GetKeyDown(KeyCode.P))
        {
            DoQuickTransition();
        }
    }
    public void AddData(Bird player, Bird enemy, int result)
    {
        battles.Add(new battleData(player, enemy, result));
    }

    public void Battle(bool zoom)
    {
        StartCoroutine(DoBattles(Helpers.Instance.winWaitTime, Helpers.Instance.loseWaitTime,zoom));
    }

    IEnumerator StartBattle(Bird player, Bird enemy)
    {
        enemy.GetComponentInChildren<Animator>().SetTrigger("walkprepare");
        enemy.GetComponentInChildren<Animator>().SetBool("dead", false);
        enemy.GetComponentInChildren<Animator>().SetBool("win", false);
        yield return new WaitForSeconds(windUpDelay);
        enemy.GetComponentInChildren<Animator>().SetBool("walk", true);
        AudioControler.Instance.PlaySound(AudioControler.Instance.enemyRun);
        float enemyMoveTime = Vector2.Distance(enemy.transform.position, player.transform.position + Helpers.Instance.dirToVector(enemy.position) * 2) / enemySpeed;
        LeanTween.move(enemy.transform.gameObject, player.transform.position + Helpers.Instance.dirToVector(enemy.position) * 2, enemyMoveTime).setEase(vultureRunCurve).setOnComplete(() =>
              enemy.GetComponentInChildren<Animator>().SetBool("walk", false)
        );
        LeanTween.move(enemy.transform.gameObject, player.transform.position + Helpers.Instance.dirToVector(enemy.position)*2, enemyMoveTime).setEase(vultureRunCurve).setOnComplete(()=>
			enemy.GetComponentInChildren<Animator>().SetBool("walk", false)
		);


    }
    private IEnumerator CameraZoomInAnimation(float time)
    {
        float t = 0;
        while (t <= 1)
        {
            Camera.main.orthographicSize = cameraZoomInCurve.Evaluate(t) * cameraStartingSize;
            Camera.main.transform.Translate(new Vector3(0, cameraYMovementCurve.Evaluate(t) * Time.deltaTime, 0));
            t += Time.deltaTime / time;
            yield return null;
        }
    }

    public void CameraZoomOutAnimation(float time)
    {
        LeanTween.value(gameObject, (float val) => Camera.main.orthographicSize = val, Camera.main.orthographicSize, cameraStartingSize, time).setEaseInOutCubic();
        LeanTween.moveY(Camera.main.gameObject, cameraStartingY, time).setEaseInOutCubic();

    }
    IEnumerator DoBattles(float waitTime, float loseWaitTime, bool Zoom)
    {

        GuiContoler.Instance.canPause(false);
        if (Zoom)
        {
            StartCoroutine(CameraZoomInAnimation(cameraZoomInTime));

        }
        yield return new WaitForSeconds(waitTime / 3f);
        //float extraWait = 0.8f;
        foreach (battleData battle in battles)
        {

            StartCoroutine(StartBattle(battle.player, battle.enemy));
            yield return StartCoroutine(ShowResult(battle, waitTime + 1f));

            /*
			if (battle.result != 1)
				yield return new WaitForSeconds(loseWaitTime + 1);
			else
				yield return new WaitForSeconds(waitTime + 1f);*/
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
                    if (GuiContoler.Instance.canplayBossTransition && !Var.isEnding)
                    {
                        if (GuiContoler.Instance.isActiveBirdInjured() && (GuiContoler.Instance.nextNextMapArea != Var.Em.finish))
                        {
                            GuiContoler.Instance.InitiateGraph();
                            ProgressGUI.Instance.ActivateDeathSummaryScreen();
                        }
                        else
                        {
                            DoQuickTransition();
                            if (GuiContoler.Instance.FastForwardScript)
                            {
                                GuiContoler.Instance.FastForwardScript.SetIsInFight(false);
                            }
                        }
                    }

                    yield return new WaitForSeconds(0.5f);

                }
                else
                {
                    GuiContoler.Instance.InitiateGraph(Var.activeBirds[0]);
                    GuiContoler.Instance.CreateBattleReport();
                }
            }
            if (Zoom)
            {
                CameraZoomOutAnimation(cameraZoomOutTime);
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
        if (GuiContoler.Instance.nextNextMapArea != Var.Em.finish)
        {
            GuiContoler.Instance.bossTransition.GetComponent<Animator>().SetTrigger("TriggerTransition");
        }
        LeanTween.delayedCall(0.5f, () =>
         GuiContoler.Instance.CloseGraph());
    }

    IEnumerator ShowResult(battleData battle, float waitTime)
    {
        float enemyMoveTime = Vector2.Distance(battle.enemy.transform.position, battle.player.transform.position) / enemySpeed;
        if (battle.player.data.injured)
        {
            battle.enemy.GetComponentInChildren<Animator>().SetTrigger("walkprepare");
            yield return new WaitForSeconds(windUpDelay);
            battle.enemy.GetComponentInChildren<Animator>().SetBool("walk", true);
            AudioControler.Instance.PlaySound(AudioControler.Instance.enemyRun);
            LeanTween.move(battle.enemy.transform.gameObject, battle.player.transform.position, enemyMoveTime).setEase(vultureRunCurve);
            Debug.Log("I moved");
        }
        else
        {
            AudioControler.Instance.PlaySound(AudioControler.Instance.enemyRun);
            battle.player.GetComponentInChildren<Animator>().SetBool("lose", false);
            yield return new WaitForSeconds(enemyMoveTime + enemySwitchToTalkOffset);
            if (battle.enemy.position == Bird.dir.top)
                battle.player.GetComponentInChildren<Animator>().SetTrigger("startTalking_up");
            else
                battle.player.GetComponentInChildren<Animator>().SetTrigger("startTalking_right");
            battle.enemy.GetComponentInChildren<Animator>().SetTrigger("startListening");
            battle.player.GetComponentInChildren<Animator>().SetTrigger("startTalking");

            if (battle.player.birdSounds.birdBattleConversations.clips.Length == 0)
                battle.player.birdSounds.birdBattleConversations = battle.player.birdSounds.birdDialogueTalk;

            AudioControler.Instance.PlaySound(battle.player.birdSounds.birdBattleConversations);

            yield return new WaitForSeconds(enemyTalkBeforeSound);
            AudioControler.Instance.PlaySound(AudioControler.Instance.considerSound);
            yield return new WaitForSeconds(enemyTalkAfterSound);
            //lose
            if (battle.result != 1)
            {
                AudioControler.Instance.PlaySound(AudioControler.Instance.fightCloudSound);
                LeanTween.move(battle.enemy.transform.gameObject, battle.player.transform.position, 0.5f).setEase(vultureEnterCloudCurve);

                Vector3 cloudpos = battle.player.transform.position / 2 + battle.player.transform.position / 2;
                GameObject fightCloudObj = Instantiate(fightCloud, cloudpos, Quaternion.identity);

                if (Var.freezeEmotions)
                {
                    //bird was injured in a trial
                    //Stats.BirdInjuredInTrial(true);
                    Var.birdInjuredInTrial = true;
                }

                if (!battle.player.hasShieldBonus)
                {
                    battle.player.GetComponentInChildren<Animator>().SetBool("lose", true);
                }
                else
                {
                    Helpers.Instance.EmitEmotionParticles(battle.player.transform, Var.Em.Shield);
                    if (battle.player.data.powerUpShieldsUsed <= 999)
                    {
                        battle.player.data.powerUpShieldsUsed++;
                        Debug.Log("shield used");
                    }
                    battle.player.GetComponentInChildren<Animator>().SetTrigger("stopTalking");
                }

               
                battle.enemy.GetComponentInChildren<Animator>().SetBool("win", true);
                Destroy(fightCloudObj, waitTime - enemyMoveTime);
                yield return new WaitForSeconds(waitTime - enemyMoveTime - 2.3f);
                battle.enemy.GetComponentInChildren<Animator>().SetTrigger("walkprepare");
                yield return new WaitForSeconds(windUpDelay);
                battle.enemy.GetComponentInChildren<Animator>().SetBool("walk", true);
                LeanTween.move(battle.enemy.transform.gameObject, battle.player.transform.position, enemyMoveTime).setEase(LeanTweenType.easeOutQuad);
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
            if (Var.confrontFail < 999)
            {
                Var.confrontFail++;
            }

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
                if (Var.confrontSuccess < 999)
                {
                    Var.confrontSuccess++;
                }
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
                if (Var.confrontFail < 999)
                {
                    Var.confrontFail++;
                }
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