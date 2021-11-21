using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public static Ending Instance { get; private set; }
    public Animator kingAnimator;

    public List<EndingEventStructureScript> EndingBattles;

    public bossBattleVultureVisuals visuals;

    public List<List<TutorialEnemy>> TutorialMap = new List<List<TutorialEnemy>>();
    public List<BattleData> map = new List<BattleData>();
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

        for(int i = 0; i < EndingBattles.Count; i++)
        {
            AddEnemiesToList(EndingBattles[i].EnemiesToSpawn);
        }


        visuals.gameObject.SetActive(true);
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
        foreach (GuiMap map in FindObjectsOfType<GuiMap>())
            map.CreateMap(this.map);
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad > 0.5f && !settingStuffUp)
        {
            if (showedFirstKingColor&& !showedSecondKingColor && !GuiContoler.Instance.speechBubbleObj.activeSelf) //This is a REALLY jank way to change the king's colors holy cow
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
              //  showedFinalEvent = true;
            }

            if(showedFinalEvent && !EventController.Instance.eventObject.activeSelf)
            {
                ActivateEnding();
            }
        }
    }

    private void ActivateEnding()
    {
        Achievements.vultureKingFightStatus(true);
        Achievements.BeatGameInTime();
        SceneManager.LoadScene("Credits");
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

    void AddEnemiesToList(Var.Em[] array, bool addToMap = true)
    {
        List<TutorialEnemy> list = new List<TutorialEnemy>();
        Var.Em lastEmotion = Var.Em.Neutral;
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == Var.Em.finish)
                list.Add(null);
            else
            {
                list.Add(new TutorialEnemy(array[i]));
                lastEmotion = array[i];
            }
        }
        TutorialMap.Add(list);
        if (addToMap)
        {
            map.Add(new BattleData(lastEmotion, false, new List<Var.Em>(), new MapBattleData()));
        }
    }
    public void ShowEndingStartingText(int stage)
    {

        if (EndingBattles[stage].EventToPlayBeforeBattle!= null){
            EventController.Instance.CreateEvent(EndingBattles[stage].EventToPlayBeforeBattle);

        }

        if (EndingBattles[stage].DialogueToPlayBeforeBattle != null)
        {
            LeanTween.delayedCall(0.2f, () => DialogueControl.Instance.CreateParticularDialog(EndingBattles[stage].DialogueToPlayBeforeBattle));
        }

        if (EndingBattles[stage].VulturesShouldGoToLeftSideThisTurn)
        {
            visuals.addVulturesToLeftSide();
        }

        if (EndingBattles[stage].isFinalBattle)
        {
            showedFinalEvent = true;
        }


    }
    public void ShowEndingBeforeBattleText(int stage)
    {

        if (shownMapInfos[stage] == true)
            return;
        shownMapInfos[stage] = true;

        if (EndingBattles[stage].EventToPlayOnceBirdsArePlaced != null)
        {
            LeanTween.delayedCall(0.2f, () => EventController.Instance.CreateEvent(EndingBattles[stage].EventToPlayOnceBirdsArePlaced));

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

