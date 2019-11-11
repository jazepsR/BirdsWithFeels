﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public static Ending Instance { get; private set; }
    public Animator kingAnimator;
    public Var.Em[] firstStageEnemies;
    public Var.Em[] secondStageEnemies;
    public Var.Em[] thirdStageEnemies;
    public Var.Em[] forthStageEnemies;
    public Var.Em[] fifthStageEnemies;
    public Var.Em[] sixthStageEnemies;
    public Var.Em[] seventhStageEnemies;
    public Var.Em[] eightStageEnemies;
    public Var.Em[] ninethStageEnemies;
    public Var.Em[] tenthStageEnemies;
    [Header("ENDING EVENTS")]
    public EventScript EndingEvent0_Introduction;
    public EventScript EndingEvent_PlayerManipulateEmotions1;
    public EventScript EndingEvent_PlayerManipulateEmotions2;
    public EventScript EndingEvent_VultureKingCallsPlayerOut;
    public EventScript EndingEvent_PercySaysNo;
    public EventScript EndingEvent4_FinalTalk;
    [Header("ENDING DIALOGUES")]
    public Dialogue EndingDialogue1;
    public Dialogue EndingDialogue2;
    public Dialogue EndingDialogue3;

    public bossBattleVultureVisuals visuals;

    public List<List<TutorialEnemy>> TutorialMap = new List<List<TutorialEnemy>>();
    [HideInInspector]
    public int CurrentPos = 0;
    public Transform[] birdSpeechPos;
    public Transform portraitPoint;
    bool showedSecondBirdReportText = false;
    bool showedThirdBirdReportText = false;
    bool showedFirstKingColor = false;
    bool showedSecondKingColor = false;
    bool showedThirdKingColor = false;
    bool showedForthKingColor = false;
    bool showedFinalEvent = false;
    [HideInInspector]
    public bool[] shownMapInfos = new bool[6];
    bool settingStuffUp = false;
    private AudioGroup TerrySounds;
    private AudioGroup RebeccaSounds;
    private AudioGroup AlexSounds;
    // Use this for initialization
    void Awake()
    {
        Instance = this;
        if (!Var.isEnding)
            return;
        CurrentPos = 0;
        AddEnemiesToList(firstStageEnemies);
        AddEnemiesToList(secondStageEnemies);
        AddEnemiesToList(thirdStageEnemies);
        AddEnemiesToList(forthStageEnemies);
        AddEnemiesToList(fifthStageEnemies);
        AddEnemiesToList(sixthStageEnemies);
        AddEnemiesToList(seventhStageEnemies);
        AddEnemiesToList(eightStageEnemies);
        AddEnemiesToList(ninethStageEnemies);
        AddEnemiesToList(tenthStageEnemies);
        visuals.debugFinalBattleActive = true;
        visuals.setupLastBattle();
        Var.currentBG = 13;

    }
    void Start()
    {
        if (!Var.isEnding)
            return;
        kingAnimator.SetInteger("emotion", 0);
        tutorialSetup.TutorialSetup.SetupBirds();
        LeanTween.delayedCall(0.2f, () => ShowEndingStartingText(0));
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad > 0.5f && !settingStuffUp)
        {
            if (showedFirstKingColor&& !showedSecondKingColor && !GuiContoler.Instance.speechBubbleObj.activeSelf)
            {
                LeanTween.delayedCall(0.3f, () =>
                {
                    showedSecondKingColor = true;
                    settingStuffUp = false;
                });
                settingStuffUp = true;
                //DialogueControl.Instance.CreateParticularDialog(EndingDialogue4b);
                kingAnimator.SetInteger("emotion", 2);
                Debug.LogError("second");
            }
            else if (showedSecondKingColor && !showedThirdKingColor&& !GuiContoler.Instance.speechBubbleObj.activeSelf)
            {
                LeanTween.delayedCall(0.3f, () =>
                {
                    showedThirdKingColor = true;
                    settingStuffUp = false;
                });
                settingStuffUp = true;
               // DialogueControl.Instance.CreateParticularDialog(EndingDialogue4c);
                kingAnimator.SetInteger("emotion", 3);
                Debug.LogError("third");
            }else if (showedThirdKingColor && !showedForthKingColor && !GuiContoler.Instance.speechBubbleObj.activeSelf)
            {
                LeanTween.delayedCall(0.3f, () =>
                {
                    showedForthKingColor = true;
                    settingStuffUp = false;
                });
                settingStuffUp = true;
               // DialogueControl.Instance.CreateParticularDialog(EndingDialogue4d);
                kingAnimator.SetInteger("emotion", 4);
                Debug.LogError("forth");
            }
            if(showedForthKingColor && !showedFinalEvent&& !GuiContoler.Instance.speechBubbleObj.activeSelf)
            {
               // EventController.Instance.CreateEvent(EndingEvent4);
                showedFinalEvent = true;
            }
            if(showedFinalEvent && !EventController.Instance.eventObject.activeSelf)
            {
                SceneManager.LoadScene("endCutscene");
            }
        }
    }

    public void ShowSmallGraph(float waitTime)
    {
        GuiContoler.Instance.GraphBlocker.SetActive(false);
    }
 

    public void SetCurrenPos(int pos)
    {
        showedSecondBirdReportText = false;
        showedThirdBirdReportText = false;
        CurrentPos = pos;
    }


    public void ShowGraphSpeech(int currentGraph)
    {
        if (currentGraph == 1)
            ShowEndingSecondGridText(CurrentPos);
        if (currentGraph == 2)
            ShowEndingThirdGridText(CurrentPos);
    }

    void AddEnemiesToList(Var.Em[] array)
    {
        List<TutorialEnemy> list = new List<TutorialEnemy>();
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == Var.Em.finish)
                list.Add(null);
            else
                list.Add(new TutorialEnemy(array[i]));
        }
        TutorialMap.Add(list);
    }
    public void ShowEndingStartingText(int stage)
    {

        switch (stage)
        {
            case 0:
                EventController.Instance.CreateEvent(EndingEvent0_Introduction);
                break;

            case 1:
                LeanTween.delayedCall(0.2f,()=> DialogueControl.Instance.CreateParticularDialog(EndingDialogue1));
                

                break;

            case 2:

                visuals.addVulturesToLeftSide();
                LeanTween.delayedCall(0.2f, () => DialogueControl.Instance.CreateParticularDialog(EndingDialogue2));

                break;
            case 3:
                visuals.addVulturesToLeftSide();
                //percy says no once birds have been placed

                break;
            case 4:
                visuals.addVulturesToLeftSide();
              //  LeanTween.delayedCall(0.2f, () => DialogueControl.Instance.CreateParticularDialog(EndingDialogue3));
                
                break;
            case 5:
                EventController.Instance.CreateEvent(EndingEvent4_FinalTalk);
                visuals.addVulturesToLeftSide();
                break;
            case 6:

                if (!showedFirstKingColor)
                {
                  //  DialogueControl.Instance.CreateParticularDialog(EndingDialogue4a);
                    showedFirstKingColor = true;
                    kingAnimator.SetInteger("emotion", 1);
                }

                break;
        }
    }
    public void ShowEndingBeforeBattleText(int stage)
    {

        if (shownMapInfos[stage] == true)
            return;
        shownMapInfos[stage] = true;
        switch (stage)
        {
            case 0:

               
                LeanTween.delayedCall(0.2f, () => EventController.Instance.CreateEvent(EndingEvent_PlayerManipulateEmotions1));

                break;
            case 1:
                LeanTween.delayedCall(0.2f, () => EventController.Instance.CreateEvent(EndingEvent_PlayerManipulateEmotions2));

                break;
            case 2:
                LeanTween.delayedCall(0.2f, () => EventController.Instance.CreateEvent(EndingEvent_VultureKingCallsPlayerOut));

              
                

                break;
            case 3:

                LeanTween.delayedCall(0.2f, () => EventController.Instance.CreateEvent(EndingEvent_PercySaysNo));

              
                

                break;
            case 4:
                
                break;
            case 5:
                break;
        }
    }
    public void ShowEndingFirstGridText(int stage)
    {
        switch (stage)
        {
            case 0:
                  break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }
    public void ShowEndingSecondGridText(int stage)
    {
        if (showedSecondBirdReportText)
            return;
        showedSecondBirdReportText = true;
        switch (stage)
        {
            case 0:
                break;
            case 1: break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }
    public void ShowEndingThirdGridText(int stage)
    {
        if (showedThirdBirdReportText)
            return;
        showedThirdBirdReportText = true;
        switch (stage)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
        }
    }
}
