using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ending : MonoBehaviour
{
    public static Ending Instance { get; private set; }
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
    public EventScript EndingEvent0;
    public EventScript EndingEvent1;
    public EventScript EndingEvent2;
    public EventScript EndingEvent3;
    public EventScript EndingEvent4;
    [Header("ENDING DIALOGUES")]
    public Dialogue EndingDialogue1;
    public Dialogue EndingDialogue2;
    public Dialogue EndingDialogue3;
    public Dialogue EndingDialogue4a;
    public Dialogue EndingDialogue4b;
    public Dialogue EndingDialogue4c;
    public Dialogue EndingDialogue4d;

    public bossBattleVultureVisuals visuals;

    public List<List<TutorialEnemy>> TutorialMap = new List<List<TutorialEnemy>>();
    [HideInInspector]
    public int CurrentPos = 0;
    public Transform[] birdSpeechPos;
    public Transform portraitPoint;
    bool showedSecondBirdReportText = false;
    bool showedThirdBirdReportText = false;
    [HideInInspector]
    public bool[] shownMapInfos = new bool[6];
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
        tutorialSetup.TutorialSetup.SetupBirds();
        LeanTween.delayedCall(0.2f, () => ShowEndingStartingText(0));
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
                DialogueControl.Instance.CreateParticularDialog(EndingDialogue1);
               // EventController.Instance.CreateEvent(EndingEvent0);
                break;
            case 1:
                DialogueControl.Instance.CreateDialogue(EndingDialogue1);
                visuals.addVulturesToLeftSide();
                break;
            case 2:

                visuals.addVulturesToLeftSide();
                break;
            case 3:
                visuals.addVulturesToLeftSide();
                break;
            case 4:
                visuals.addVulturesToLeftSide();
                EventController.Instance.CreateEvent(EndingEvent1);


                //   GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[2].GetMouthTransform(), "A Rock!");
                //   GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Birds can't stand there, but vultures move right over the rocks!");
                break;
            case 5:
                  break;
            case 6:


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
                LeanTween.delayedCall(0.3f, () => EventController.Instance.CreateEvent(EndingEvent1));
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

