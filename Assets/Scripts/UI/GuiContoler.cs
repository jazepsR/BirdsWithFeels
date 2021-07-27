﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using System.Linq;
using UnityEngine.SceneManagement;
#if ENABLE_CLOUD_SERVICES_ANALYTICS
using UnityEngine.Analytics;
#endif

public class GuiContoler : MonoBehaviour {
    public static GuiContoler Instance { get; private set; }
    public Image dangerFollowHighlight;
    public Image[] vingette;
    public GameObject kingMouth;
    public GameObject playerMouth;
    // public Text EmotionChangeFeedback;
    public GameObject feelReport;
    public emoReportBit emoReportBit;
    public Transform emoReportBitParent;
    public Transform socialTotalReportParent;
    public Transform confTotalReportParent;
    public Image socialTotalReportIcon;
    public Image confTotalReportIcon;
    public Text socialTotalReportCount;
    public Text confTotalReportCount;
    public Text EmotionChangeHeading;
    public Text ToggleRelationPanelText;
    public GameObject relationshipPortrait;
    public Text relationshipText;
    public GameObject reportRelationshipPortrait;
    public Text reportRelationshipText;
    public GameObject statPanel;
    public GameObject relationshipPanel;
    public RectTransform canvasRect;
    public Text BirdLVLUpText;
    public Text BirdCombatStr;
    public Text infoText;
    public Text infoHeading;
    public Text infoFeeling;
    public Image powerBarTemp;
    public Text powerTextTemp;
    public LVLIconScript[] lvlIcons;
    public Text reportText;
    public GameObject report;
    public Image[] hearts;
    public GameObject loseBanner;
    public GameObject winBanner;
    public GameObject[] portraits;
    public Transform[] battleTrag;
    public GameObject rerollBox;
    public FastForwardScript FastForwardScript;
    Var.Em currentMapArea; 
    public Var.Em nextMapArea; 
    public Var.Em nextNextMapArea; //next spot on map
    public static int mapPos = 0;
    private int finalResult = 0;
    public GameObject graph;
    public SetEmoGraphColor emoGraphColor;
    public Animator graphPageAnimator;
    public GameObject battlePanel;
    public GameObject closeReportBtn;
    public Button CloseBattleReport;
    public GuiMap mapBirdScript;
    List<Bird> players = new List<Bird>();
    public bool inMap = false;
    List<string> messages;
    public GameObject messageBox;
    public Text messageText;
    [HideInInspector]
    public int activePortrait = 0;
    public Text tooltipText;
    public GameObject speechBubble;
    public GameObject speechBubble2;
    List<string> speechTexts = new List<string>();
    List<Transform> speechPos = new List<Transform>();
    List<bool> speechPivotIsFirst = new List<bool>();
    List<AudioGroup> speechAudioGroup = new List<AudioGroup>();
    public Text SpeechBubbleText;
    public Text SpeechBubbleText2;
    public Text SpeechBubbleReminderText;
    public GameObject speechBubbleObj;
    public SliderSwitcher confSlider;
    public SliderSwitcher firendSlider;
    public Image[] BirdInfoHearts;
    public Image[] BirdMentalHearts;
    public Text levelNumberText;
    public GameObject pause;
    public GameObject toMapBtn;
    public GameObject mainMenuBtn;
    public GameObject pauseBtn;
    public GameObject deathMenu;
    public Text deathTitle;
    public Toggle[] deadBirdToggles;
    bool CanToggleDeathTriggers = true;
    Bird SelectedDeathBird = null;
    Bird DeadBird = null;
    [HideInInspector]
    public Bird selectedBird;
    public Button SelectDeathBirdBtn;
    [HideInInspector]
    public int currentGraph = -1;
    public Button nextGraph;
    public Button prevGraph;
    public Button HideSmallGraph;
    public GameObject relationshipSliders;
    public GameObject reportRelationshipSliders;
    public float levelinfoHideXValue;
    public float levelinfoshowXValue;
    public Graph smallGraph;
    int maxGraph = 3;
    public GameObject boss;
    public GameObject bossTransition; 
    public bool canplayBossTransition = false; // make sure you are in a trial for this to work
    Transform lastSpeechPos = null;
    public Animator graphAnime;
    public GameObject minimap;
    public GameObject dangerZoneBorder;
    [HideInInspector]
    public bool canChangeGraph = true;
    [HideInInspector]
    public bool GraphActive = false;
    public Slider soundSlider;
    public Slider musicSlider;
    public GameObject GraphBlocker;
    public Text controlButtonText;
    private List<emoReportBit> bits = new List<emoReportBit>();
    private Text activeSpeechText;
    [HideInInspector]
    public GameObject activeSpeechBubble;
    [HideInInspector]
    public TimedEventControl control;
    [SerializeField] public TimedEventControl[] timedEventControllers;
    [HideInInspector] public int graphInteractTweenID = -1;
    void Awake()
    {
        //if (!Var.StartedNormally)
        //Var.gameSettings.shownLevelTutorial = true;
        LeanTween.init(1000);
        Instance = this;
        maxGraph = 3;
        Var.birdInfo = infoText;
        Var.birdInfoHeading = infoHeading;
        Var.birdInfoFeeling = infoFeeling;
        Var.powerBar = powerBarTemp;
        Var.powerText = powerTextTemp;
        activeSpeechText = SpeechBubbleText;
        activeSpeechBubble = speechBubble;
               
    }
    void Start()
    {
        Var.Infight = false;
        if (Var.emotionParticles == null)
            Var.emotionParticles = Resources.Load("EmotionParticle") as GameObject;
        if (Var.isTutorial)
        {
            
            try
            {
                Tutorial.Instance.enabled = true;
                canPause(false);
            }
            catch { }
        }
        messages = new List<string>();
        if (!inMap)
        {
            if (!Var.isEnding)
            {
                foreach (GuiMap map in FindObjectsOfType<GuiMap>())
                    map.CreateMap(Var.map);
                minimap.SetActive(Var.gameSettings.shownBattlePlanningTutorial);
            }
            setMapLocation(0);
            canplayBossTransition = true;
            LeanTween.delayedCall(0.05f, tryDialog);
            boss.SetActive(Var.isBoss);
            GraphBlocker.SetActive(false);

            if (Var.freezeEmotions && Var.selectedTimeEvent != null && !Var.selectedTimeEvent.activationEventShown  && !Var.isEnding && !inMap)
            {
                foreach (TimedEventControl anEvent in timedEventControllers)
                {
                    if (anEvent.eventName == Var.selectedTimeEvent.eventName)
                    {
                        control = anEvent;
                        control.data = Var.selectedTimeEvent;
                    }
                }
                Debug.Log("control is" + control.eventName);
                control.TriggerActivationEvent();

            }
        }

      /*  if (showEmoGraphArrow != null)
        {
            if ((Var.isTutorial || Var.currentStageID == Var.battlePlanningTutorialID) || Var.freezeEmotions || inMap || Var.isEnding)
                showEmoGraphArrow.SetActive(false);
            else
            {
                showEmoGraphArrow.SetActive(true);
            }
        }*/
            
    }

    void tryDialog()
    {
        if (Var.currentStageID != 1 && Var.currentStageID != 0)
            DialogueControl.Instance.TryDialogue(Dialogue.Location.battle);
    }



    public void StatToggle()
    {
        

        if (relationshipPanel.activeSelf)
        {
            relationshipPanel.SetActive(false);
            statPanel.SetActive(true);
            ToggleRelationPanelText.text = "Relationships";
            Debug.Log("i am a relationship");
        } else
        {
            relationshipPanel.SetActive(true);
            statPanel.SetActive(false);
            ToggleRelationPanelText.text = "Stats";
            Debug.Log("i am a stat");
        }


    }
    public void ChangeControls()
    {
        Var.isDragControls = !Var.isDragControls;
        SetControlButtonText();
        SaveLoad.Save(false);
    }
    public void UnlockAllMap()
    {
        if(!Var.cheatsEnabled)
        {
            return;
        }

        MapIcon[] icons = FindObjectsOfType<MapIcon>();
        foreach (MapIcon icon in icons)
        {
            icon.available = true;
            icon.SetState();
            if (icon.fogObject)
            {
                icon.fogObject.SetActive(false);
            }
        }
        mapPan.Instance.activeFog = null;
        mapPan.Instance.scrollingEnabled = true;
    }
    public void SetControlButtonText()
    {
        if (controlButtonText)
        {
            if (Var.isDragControls)
                controlButtonText.text = "Drag controls";
            else
                controlButtonText.text = "Click controls";
        }
    }

    public void setPause()
    {
        AudioControler.Instance.ClickSound();
        if (soundSlider)
            soundSlider.value = AudioControler.Instance.defaultSoundVol;
        if (musicSlider)
            musicSlider.value = AudioControler.Instance.defaultMusicVol;
        if (pause.activeSelf)
        {
            pause.SetActive(false);
            Time.timeScale = 1.0f;
            AudioControler.Instance.SaveVolumeSettings();
            if(inMap)
            {
                MapControler.Instance.canMove = true;
                MapControler.Instance.restBtnRaycaster.enabled = true;
            }
            GraphBlocker.SetActive(false);
        }
        else
        {
            pause.SetActive(true);
            SetControlButtonText();
            Time.timeScale = 0.0f;
            GraphBlocker.SetActive(true);
            if ((Var.isTutorial || Var.currentStageID == Var.battlePlanningTutorialID) && !Var.cheatsEnabled)
            {
                mainMenuBtn.GetComponent<Button>().interactable = false;
                toMapBtn.GetComponent<Button>().interactable = false;    
            }
            if (inMap)
            {
                MapControler.Instance.canMove = false;
                MapControler.Instance.restBtnRaycaster.enabled = false;
            }
        }
    }

    public bool canPause(bool canPause)
    {
        if (canPause)
        {
            pauseBtn.SetActive(true);
            return true;
        }
        else
        {
            pauseBtn.SetActive(false);
            return false;
        }
    }

    public void QuitGame()
    {
        AudioControler.Instance.ClickSound();
        if(Var.tutorialCompleted)
        {
           SaveLoad.Save();
        }
        Application.Quit();
    }

    public void QuitToMap()
    {
        if (!Var.tutorialCompleted)
        {
            Time.timeScale = 1.0f;
            //SaveLoad.Save();
            SceneManager.LoadScene("MainMenu");
            return;
        }
        int livingCount = 0;
        Var.currentWeek++;
        foreach (Bird bird in Var.availableBirds)
        {
            bird.DecreaseTurnsInjured();
            if (bird.data.health <= 0)
            {
                livingCount++;
                bird.AddRoundBonuses();
            }
        }
        if (livingCount < 3)
        {
            AudioControler.Instance.ClickSound();
            Time.timeScale = 1.0f;
            Var.fled = true;
            LeanTween.cancelAll();
            SaveLoad.Save();
            SceneManager.LoadScene("Map");
        } else
        {
            Time.timeScale = 1.0f;
            loseBanner.SetActive(true);
        }

    }


    public void ShowDeathMenu(Bird deadBird)
    {
        DeadBird = deadBird;
        SelectDeathBirdBtn.interactable = false;
        deathTitle.text = deadBird.charName + " has died! Select a bird to replace him!";
        for (int i = 0; i < Var.availableBirds.Count; i++)
        {
            if (Var.availableBirds[i].charName == deadBird.charName)
            {
                Debug.Log("Set doude as dead");
                Var.availableBirds[i] = deadBird;
            }
        }
        deathMenu.SetActive(true);
    }

    public void DeadBirdToggle(int num)
    {
        if (!CanToggleDeathTriggers)
            return;
        CanToggleDeathTriggers = false;

        LeanTween.delayedCall(0.05f, allowCall);
        /*for(int i = 0; i < deadBirdToggles.Length; i++)
		{
			if (num != i)
				deadBirdToggles[i].isOn = false;
			if(num == i)
			{
				SelectDeathBirdBtn.interactable = deadBirdToggles[i].isOn;
				if (deadBirdToggles[i].isOn)                
					//SelectedDeathBird = FillPlayer.Instance.deadBirds[num];                
				else                
					SelectedDeathBird = null;                 
			}
		}*/
    }
    void allowCall()
    {
        CanToggleDeathTriggers = true;
    }
    public void SelectDeathBird()
    {
        Var.availableBirds.Remove(SelectedDeathBird);
        //FillPlayer.SetupBird(DeadBird, SelectedDeathBird);        
        DeadBird.gameObject.SetActive(true);
        deathMenu.SetActive(false);
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && pauseBtn.activeSelf)
        {
            if (EventController.Instance.eventObject.gameObject.activeSelf || GraphBlocker.gameObject.activeSelf || winBanner.activeSelf)
            {     
                
            }
            else
            {
                setPause();
            }
           
        }
        if (Input.GetKeyDown(KeyCode.O) && Var.cheatsEnabled)
            ReturnToMap();

        if (DebugMenu.cameraControl && Var.cheatsEnabled)
        {
            if (Input.GetKeyDown(KeyCode.T))
                canvasRect.gameObject.SetActive(!canvasRect.gameObject.activeSelf);
            Camera.main.orthographicSize += Input.mouseScrollDelta.y * 0.25f;
            Vector2 moveBy = Vector2.zero;
            float moveSpeed = 7f;
            if (Input.GetKey(KeyCode.UpArrow))
                moveBy.y += moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.DownArrow))
                moveBy.y -= moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.LeftArrow))
                moveBy.x -= moveSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.RightArrow))
                moveBy.x += moveSpeed * Time.deltaTime;
            Camera.main.transform.position += (Vector3)moveBy;
        }
        if (GraphActive && Input.GetMouseButtonDown(1) && canChangeGraph)
        {
            if (nextGraph.gameObject.activeInHierarchy)
                nextGraph.onClick.Invoke();
            else if (CloseBattleReport.gameObject.activeSelf)
                CloseBattleReport.onClick.Invoke();


        }
       
        if (AudioControler.Instance.musicSource.clip == AudioControler.Instance.levelCompleteMusic)
            AudioControler.Instance.musicSource.loop = false;

        if (AudioControler.Instance.musicSource.clip == AudioControler.Instance.levelCompleteMusic && !AudioControler.Instance.musicSource.isPlaying)
        {
            AudioControler.Instance.musicSource.time = 41.44f;
            AudioControler.Instance.musicSource.Play();
        }
        
    }
    public void SpeechBubbleClicked()
    {

        AudioControler.Instance.speechBubbleContinue.Play();
        if (speechBubbleObj != null)
        {
            if (speechTexts.Count == 0)
            {
                speechBubbleObj.SetActive(false);

                lastSpeechPos = null;
                if (!Var.isBoss && !inMap)
                {
                    ///GameObject dustObj = Instantiate(Var.dustCloud, boss.transform.position, Quaternion.identity);
                    //dustObj.transform.localPosition = Vector3.zero;                       
                    //Destroy(dustObj, 1.0f);
                    boss.SetActive(false);

                }

                if (inMap)
                {

                    foreach (MapIcon icon in FindObjectsOfType<MapIcon>())
                        icon.SetState();
                }
                else
                {

                    foreach (Image img in vingette)
                    {
                        LeanTween.alpha(img.rectTransform, 0, 0.5f);
                    }
                    CheckGraphNavBtns();
                    GameLogic.Instance.CanWeFight();
                }

            }
            else
            {
                speechBubbleObj.gameObject.SetActive(true);
                speechAudioGroup[0].Play();
                SetupSpeechBubbles(speechPivotIsFirst[0]);
                activeSpeechText.text = speechTexts[0];
                activeSpeechBubble.GetComponent<UIFollow>().target = speechPos[0];
                if (lastSpeechPos != null && lastSpeechPos.Equals(speechPos[0]))
                {
                    speechBubbleObj.GetComponent<Animator>().SetTrigger("newline");
                }
                lastSpeechPos = speechPos[0];
                speechPos.RemoveAt(0);
                speechTexts.RemoveAt(0);
                speechPivotIsFirst.RemoveAt(0);
                speechAudioGroup.RemoveAt(0);
                if (!inMap)
                {
                    prevGraph.interactable = false;
                    nextGraph.interactable = false;
                }
            }
        }
    }
    void SetupSpeechBubbles(bool useFirst)
    {
        if (useFirst)
        {
            activeSpeechText = SpeechBubbleText;
            activeSpeechBubble = speechBubble;
            if (speechBubble2)
            {
                speechBubble2.SetActive(false);
            }
        }
        else
        {
            activeSpeechText = SpeechBubbleText2;
            activeSpeechBubble = speechBubble2;
            speechBubble.SetActive(false);
        }
        activeSpeechBubble.SetActive(true);
    }

    public void ShowSpeechBubble(Transform pos, string text, AudioGroup birdTalk, bool useFirst = true)
    {
       // Debug.LogError("pos: " + pos.position);
        if (speechBubbleObj.activeSelf) {
            speechTexts.Add(text);
            speechPos.Add(pos);
            speechPivotIsFirst.Add(useFirst);
            speechAudioGroup.Add(birdTalk);
          //  speechAudioGroup[0].Play();
            AudioControler.Instance.speechBubbleContinue.Play();

        }
        else
        {
            if (!inMap)
            {
                prevGraph.interactable = false;
                nextGraph.interactable = false;
            }
            foreach (Image img in vingette)
            {
                LeanTween.alpha(img.rectTransform, 0.5f, 0.5f);
            }
            SetupSpeechBubbles(useFirst);
            speechBubbleObj.gameObject.SetActive(true);
            activeSpeechText.text = text;
            activeSpeechBubble.GetComponent<UIFollow>().target = pos;

            if(speechAudioGroup.Count == 0)
            {
                speechAudioGroup.Add(birdTalk);
            }

            try
            {
                speechAudioGroup[0].Play();
                speechAudioGroup.RemoveAt(0);
            }
            catch { print("voice issue"); }
            // LeanTween.delayedCall(6f, ShowSpeechBubbleReminder);
        }
        //Debug.LogError("talk: "+ birdTalk.clips.Length);


    }
    void ShowSpeechBubbleReminder()
    {
        SpeechBubbleReminderText.gameObject.SetActive(true);
    }

    public void ShowNextGraph()
    {
        if (!canChangeGraph)
            return;
        canChangeGraph = false;
        if (currentGraph == maxGraph)
        {
            CloseGraph();
            return;
        }


        emoGraphColor.ToggleImageColor();
        graphPageAnimator.SetTrigger("turnright");
        AudioControler.Instance.PlaySound(AudioControler.Instance.notebookRight);
        AudioControler.Instance.ClickSound();
        currentGraph++;
        currentGraph = Mathf.Min(3, currentGraph);
        print("currentGraph : " + currentGraph);
        if (Var.isTutorial)
        {


            //GuiContoler.Instance.nextGraph.interactable = false;
           Tutorial.Instance.ShowGraphSpeech(currentGraph);
        }
        foreach (Transform child in graph.transform.Find("GraphParts").transform)
        {
            Destroy(child.gameObject);
        }
        if (currentGraph == 3)
        {
            if (isActiveBirdInjured())
            {
                ProgressGUI.Instance.ActivateDeathSummaryScreen();
            }
            else
            {

                CloseGraph();
                return;
                /* LeanTween.delayedCall(0.2f, () => CreateGraph(-1));
                 dangerZoneBorder.SetActive(false);
                 ProgressGUI.Instance.AllPortraitClick();*/
            }
        }
        else
        {
            LeanTween.delayedCall(0.2f, () =>
            {
                CreateGraph(currentGraph);
                ProgressGUI.Instance.PortraitClick(Var.activeBirds[currentGraph]);
            });
            ProgressGUI.Instance.SetOnePortrait();
        }



    }


    public bool isActiveBirdInjured()
    {
        foreach(Bird bird in Var.activeBirds)
        {
            if(bird.data.injured)
            {
                return true;
            }
        }
        return false;
    }
    public void ShowPrevGraph()
    {
        if (!canChangeGraph)
            return;
        canChangeGraph = false;
        currentGraph--;
        CreateGraph(Mathf.Max(0, currentGraph));
        ProgressGUI.Instance.SetOnePortrait();
        ProgressGUI.Instance.PortraitClick(Var.activeBirds[currentGraph]);
        AudioControler.Instance.PlaySound(AudioControler.Instance.notebookLeft);
        AudioControler.Instance.ClickSound();
        emoGraphColor.ToggleImageColor();
        graphPageAnimator.SetTrigger("turnleft");
    }

    public void ShowLvlText(string text)
    {
        BirdLVLUpText.text = text;
        //BirdLVLUpText.transform.parent.gameObject.SetActive(true);30
        MoveLvlText();
    }
    public void MoveLvlText()
    {
        if (selectedBird.levelUpText != null)
        {
            LeanTween.moveLocal(BirdLVLUpText.transform.parent.gameObject, new Vector3(levelinfoshowXValue, -200, 0), 0.3f).setEase(LeanTweenType.easeOutBack);
        }

    }
    public void HideLvlText()
    {
        //BirdLVLUpText.text = "";
        LeanTween.moveLocal(BirdLVLUpText.transform.parent.gameObject, new Vector3(levelinfoHideXValue, -200, 0), 0.3f).setEase(LeanTweenType.easeInBack);
        //BirdLVLUpText.transform.parent.gameObject.SetActive(false);-340

    }

    void UpdateHearts(int health)
    {
        Debug.Log("Health: " + health);
        for (int i = 0; i < hearts.Length; i++)
        {
            hearts[i].enabled = i < health;
        }
    }
    public void NoReroll()
    {
        foreach (Bird bird in FillPlayer.Instance.playerBirds)
        {
            bird.TryLevelUp();
            bird.AddRoundBonuses();
        }
        Instance.InitiateGraph(Var.activeBirds[0]);
        Instance.CreateBattleReport();
        rerollBox.SetActive(false);
    }
    public void YesReroll()
    {

        foreach (Bird bird in Var.activeBirds)
        {
            bird.SetEmotion();
            bird.gameObject.GetComponentInChildren<Animator>().SetBool("iswalking", false);
            // bird.gameObject.GetComponent<Animator>().SetBool("lose", false);
            // bird.gameObject.GetComponent<Animator>().SetBool("victory", false);
            bird.target = bird.home;
            bird.transform.position = bird.home;
            bird.data.health = bird.prevRoundHealth;
            if (bird.indicator)
            {
                bird.indicator.Hide();
                Debug.Log("i have rolled for you: " + bird.charName);
            }
            /*  if (Helpers.Instance.ListContainsLevel(Levels.type.Lonely2, bird.data.levelList) && bird.data.CoolDownLeft == 0 && !bird.foughtInRound)
              {
                  bird.data.CoolDownLeft = bird.data.CoolDownLength;
                  bird.CooldownRing.fillAmount = 0;
              }*/
        }
        for (int i = 0; i < Var.activeBirds.Count; i++)
        {
            //TODO: redo reroll
            //FillPlayer.SetupBird(FillPlayer.Instance.playerBirds[i], Var.activeBirds[i]);
        }
        foreach (LayoutButton tile in ObstacleGenerator.Instance.tiles)
        {
            tile.Reset();
        }
        Var.playerPos = new Bird[4, 4];
        GetComponent<fillEnemy>().Reset();
        GameLogic.Instance.CanWeFight();
        GameLogic.Instance.UpdateFeedback();
        rerollBox.SetActive(false);

    }




    public void CloseGraph()
    {
        if (AudioControler.Instance.emoGraphSource.isPlaying)
        {
            AudioControler.Instance.ActivateMusicSource(audioSourceType.musicSource);
        }

        LeanTween.delayedCall(0.7f, () => GraphActive = false);
        speechTexts = new List<string>();
        speechPos = new List<Transform>();
        speechBubbleObj.SetActive(false);
        AudioControler.Instance.PlaySound(AudioControler.Instance.notebookClose);
        canChangeGraph = true;
        GraphBlocker.SetActive(false);
        minimap.SetActive(Var.gameSettings.shownBattlePlanningTutorial);
        if (!Reset())
            return;
        graphAnime.SetBool("open", false);
        //LeanTween.moveLocal(graph, new Vector3(0, -Var.MoveGraphBy, graph.transform.position.z), 0.7f).setEase(LeanTweenType.easeOutBack);		
        foreach (Transform child in graph.transform.Find("GraphParts").transform)
        {
            Destroy(child.gameObject);
        }
        closeReportBtn.SetActive(false);
        HideSmallGraph.gameObject.SetActive(true);
        Var.activeBirds[0].showText();
        dangerFollowHighlight.gameObject.SetActive(false);
        foreach (Bird activeBird in Var.activeBirds)
        {
            activeBird.SetEmotion();
            activeBird.seedCollectedInRound = false;
        }        
    }
    public void CloseBirdStats()
    {
        minimap.SetActive(Var.gameSettings.shownBattlePlanningTutorial);
        graphAnime.SetBool("open", false);
        GraphBlocker.SetActive(false);
        foreach (Transform child in graph.transform.Find("GraphParts").transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Bird bird in FillPlayer.Instance.playerBirds)
            LeanTween.delayedCall(0.1f, () => bird.SetAnimation(bird.emotion));
        LeanTween.delayedCall(0.2f, () => selectedBird.showText());
        LeanTween.delayedCall(0.7f, () => GraphActive = false);
        AudioControler.Instance.PlaySound(AudioControler.Instance.notebookClose);
    }

    public void clearSmallGraph()
    {
        foreach (Transform child in smallGraph.graphParent.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public void CreateGraph(object o, bool afterBattle = true)
    {
        GraphActive = true;
        //canChangeGraph = true;
        GraphBlocker.SetActive(true);
        Helpers.Instance.HideTooltip();
        minimap.SetActive(false);
        //nextGraph.interactable = !Var.isTutorial;
        //prevGraph.interactable = !Var.isTutorial;
        foreach (Transform child in graph.transform.Find("GraphParts").transform)
        {
            Destroy(child.gameObject);
        }
        int birdNum = (int)o;

        Graph.Instance.portraits = new List<GameObject>();
        List<Bird> BirdsToGraph;
        if(graphInteractTweenID!= -1)
        {
            LeanTween.cancel(graphInteractTweenID);
        }
        //graphInteractTweenID= LeanTween.delayedCall(2f, () => canChangeGraph = true).id;
        if (birdNum == -1)
        {
            BirdsToGraph = Var.activeBirds;
            ProgressGUI.Instance.ConditionsBG.transform.parent.gameObject.SetActive(false);
            dangerFollowHighlight.gameObject.SetActive(false);
        }
        else
        {
            BirdsToGraph = new List<Bird>() { Var.activeBirds[birdNum] };
            dangerZoneBorder.SetActive(true);
            dangerFollowHighlight.gameObject.SetActive(true);
        }

        if (birdNum == -1)
            currentGraph = 3;
        else
            currentGraph = birdNum;


        for (int i = 0; i < BirdsToGraph.Count; i++)
        {
            if (!BirdsToGraph[i].data.injured)
            {
                Helpers.Instance.NormalizeStats(BirdsToGraph[i]);
                GameObject portrait = BirdsToGraph[i].portrait;
                Graph.Instance.PlotFull(BirdsToGraph[i], afterBattle,i==0);
            }
        }

        if (!Var.isTutorial || currentGraph > 0)
            CheckGraphNavBtns();

        if (bits.Count > 0)
        {
            foreach (emoReportBit bit in bits)
            {
                Destroy(bit.gameObject);
            }
            bits.Clear();
        }
        if (currentGraph != 3)
        {
            //Normal case
            DialogueControl.Instance.TryDialogue(Dialogue.Location.graph, Helpers.Instance.GetCharEnum(Var.activeBirds[birdNum]));
           // EmotionChangeFeedback.gameObject.SetActive(true);
            CreateEmotionChangeText(Var.activeBirds[birdNum], emoReportBitParent);
            //EmotionChangeFeedback.text = changeText;
            //EmotionChangeHeading.gameObject.SetActive(changeText != "");
            feelReport.SetActive(true);
        }
        else
        {
            feelReport.SetActive(false);
            //Summary
           // EmotionChangeFeedback.gameObject.SetActive(false);

        }
    }
    public void CreateEmoBit(Transform par, int gain, Var.Em emo, string info)
    {
        emoReportBit bit = Instantiate(emoReportBit, par);
        bit.SetEmoBit("+ " +Mathf.Abs((float)gain).ToString(), emo, info);
        bit.transform.SetAsFirstSibling();
        bits.Add(bit);
    }
    public void SetupTotal(Transform parent, Var.Em emo1, Var.Em emo2, int amount, Image sprite, Text amountText)
    {
       
        Var.Em emo = emo2;
        if(amount>0)
        {
            emo = emo1;
        }
        if(amount == 0)
        {
          //  emo = Var.Em.Neutral;
        }
        //Debug.LogError("Emo: " + emo + " amount: " + amount);
       
        
        sprite.gameObject.SetActive(true);
        sprite.sprite = Helpers.Instance.GetEmotionIcon(emo,false);
        
        amountText.text ="+"+ Mathf.Abs((float)amount).ToString();//.ToString("+#;-#;0");
        amountText.color = Helpers.Instance.GetEmotionColor(emo);
    }

	 public void CreateEmotionChangeText(Bird bird, Transform topParent)
	{
		Debug.Log("CHANGE TEXT " + bird.charName + " gorund conf: " + bird.groundConfBoos + " ground friend: " + bird.groundFriendBoos);

		string fbText = "";
		int ConfGainedInRound = bird.battleConfBoos + bird.groundConfBoos + bird.wizardConfBoos + bird.levelConfBoos;
		int FriendGainedInRound = bird.friendBoost + bird.wizardFrienBoos + bird.groundFriendBoos + bird.levelFriendBoos;
        SetupTotal(confTotalReportParent, Var.Em.Confident, Var.Em.Cautious, ConfGainedInRound, confTotalReportIcon, confTotalReportCount);
        SetupTotal(socialTotalReportParent, Var.Em.Social, Var.Em.Solitary, FriendGainedInRound, socialTotalReportIcon, socialTotalReportCount);
        //Confidence stuff
        //if (ConfGainedInRound > 0)
        //	fbText += "\n"+Helpers.Instance.BraveHexColor+"<b>Confidence gained: " + ConfGainedInRound +"</b></color>";
        //if (ConfGainedInRound < 0)
        //	fbText += "\n" + Helpers.Instance.ScaredHexColor + "<b>Caution gained: " + Mathf.Abs(ConfGainedInRound) + "</b></color>";

        if (bird.data.roundsRested != 0)
        {
            CreateEmoBit(topParent, 2, Var.Em.Cautious, "Resting");
        }
        else
        {
            if (bird.battleConfBoos > 0)
                CreateEmoBit(topParent, bird.battleConfBoos, Var.Em.Confident, "Confronting vultures");
            //fbText += Helpers.Instance.BraveHexColor + "\n\tFrom combat: " + bird.battleConfBoos.ToString("+#;-#;0") + " confidence</color>";
            if (bird.battleConfBoos < 0)
                CreateEmoBit(topParent, bird.battleConfBoos, Var.Em.Cautious, "Getting hurt by vultures");
        }
        //fbText += Helpers.Instance.ScaredHexColor + "\n\tFrom combat: " + Mathf.Abs(bird.battleConfBoos).ToString("+#;-#;0") + " caution</color>";
        if (bird.data.injured)
            CreateEmoBit(topParent, 5, Var.Em.Cautious, "Suffered injury");
       // fbText += Helpers.Instance.ScaredHexColor + "\n\t(5 caution from injury)</color>";
		if (bird.groundConfBoos > 0)
            CreateEmoBit(topParent, bird.groundConfBoos, Var.Em.Confident, "Ground tiles");
        //fbText += Helpers.Instance.BraveHexColor + "\n\tFrom tiles: " + bird.groundConfBoos.ToString("+#;-#;0") + " confidence</color>";
        if (bird.groundConfBoos < 0)
            CreateEmoBit(topParent, bird.groundConfBoos, Var.Em.Cautious, "Ground tiles");
        //fbText += Helpers.Instance.ScaredHexColor + "\n\tFrom tiles: " + Mathf.Abs(bird.groundConfBoos).ToString("+#;-#;0") + " caution</color>";
        if (bird.levelConfBoos > 0)
            CreateEmoBit(topParent, bird.levelConfBoos, Var.Em.Confident, "Friendly bird skills");

        //fbText += Helpers.Instance.BraveHexColor + "\n\tFrom level abilities: " + bird.levelConfBoos.ToString("+#;-#;0") + " confidence</color>";
        if (bird.levelConfBoos < 0)
            CreateEmoBit(topParent, bird.levelConfBoos, Var.Em.Cautious, "Friendly bird skills");
        //fbText += Helpers.Instance.ScaredHexColor + "\n\tFrom level abilities: " + Mathf.Abs(bird.levelConfBoos).ToString("+#;-#;0") + " caution</color>";
		if (bird.wizardConfBoos > 0)
            CreateEmoBit(topParent, bird.wizardConfBoos, Var.Em.Confident, "Enemy wizard influence");
       // fbText += Helpers.Instance.BraveHexColor + "\n\tFrom enemies: " + bird.wizardConfBoos.ToString("+#;-#;0") + " confidence</color>";
		if (bird.wizardConfBoos < 0)
            CreateEmoBit(topParent, bird.wizardConfBoos, Var.Em.Cautious, "Enemy wizard influence");
        //fbText += Helpers.Instance.ScaredHexColor + "\n\tFrom enemies: " + Mathf.Abs(bird.wizardConfBoos).ToString("+#;-#;0") + " caution</color>";






		//Friendship stuff
		if (FriendGainedInRound > 0)
			fbText += "\n" + Helpers.Instance.FriendlyHexColor + "<b>Social gained: " + FriendGainedInRound + "</b></color>";
		if (FriendGainedInRound < 0)
			fbText += "\n" + Helpers.Instance.LonelyHexColor + "<b>Solitude gained: " + Mathf.Abs(FriendGainedInRound) + "</b></color>";
		if (bird.friendBoost > 0)
            CreateEmoBit(topParent, bird.friendBoost, Var.Em.Social, "Spending time with friends");
       // fbText += Helpers.Instance.FriendlyHexColor + "\n\tFrom interactions: " + bird.friendBoost.ToString("+#;-#;0") + " social</color>";
		if (bird.friendBoost < 0)
            CreateEmoBit(topParent, bird.friendBoost, Var.Em.Solitary, "Spent time alone");
       // fbText += Helpers.Instance.LonelyHexColor + "\n\tFrom interactions: " + Mathf.Abs(bird.friendBoost).ToString("+#;-#;0") + " solitude</color>";
		if (bird.groundFriendBoos > 0)
            CreateEmoBit(topParent, bird.groundFriendBoos, Var.Em.Social, "Ground tiles");
       // fbText += Helpers.Instance.FriendlyHexColor + "\n\tFrom tiles: " + bird.groundFriendBoos.ToString("+#;-#;0") + " social</color>";
		if (bird.groundFriendBoos < 0)
            CreateEmoBit(topParent, bird.groundFriendBoos, Var.Em.Solitary, "Ground tiles");
       // fbText += Helpers.Instance.LonelyHexColor + "\n\tFrom tiles: " + Mathf.Abs(bird.groundFriendBoos).ToString("+#;-#;0") + " solitude</color>";
		if (bird.levelFriendBoos > 0)
            CreateEmoBit(topParent, bird.levelFriendBoos, Var.Em.Social, "Friendly bird skills");
        //fbText += Helpers.Instance.FriendlyHexColor + "\n\tFrom level abilities: " + bird.levelFriendBoos.ToString("+#;-#;0") + " social</color>";
		if (bird.levelFriendBoos < 0)
            CreateEmoBit(topParent, bird.levelFriendBoos, Var.Em.Solitary, "Friendly bird skills");
       // fbText += Helpers.Instance.LonelyHexColor + "\n\tFrom level abilities: " + Mathf.Abs(bird.levelFriendBoos).ToString("+#;-#;0") + " solitude</color>";
		if (bird.wizardFrienBoos > 0)
            CreateEmoBit(topParent, bird.wizardFrienBoos, Var.Em.Social, "Enemy wizard influence");
       // fbText += Helpers.Instance.FriendlyHexColor + "\n\tFrom enemies: " + bird.wizardFrienBoos.ToString("+#;-#;0") + " social</color>";
		if (bird.wizardFrienBoos < 0)
            CreateEmoBit(topParent, bird.wizardFrienBoos, Var.Em.Solitary, "Enemy wizard influence");
       // fbText += Helpers.Instance.LonelyHexColor + "\n\tFrom enemies: " + Mathf.Abs(bird.wizardFrienBoos).ToString("+#;-#;0") + " solitude</color>";
		//return fbText;
	}



	/*void CreateRelationshipEvent(int birdNum)
	{
		if (Var.activeBirds[birdNum].newRelationship)
		{
			Bird relationshipBird = Var.activeBirds[birdNum].relationshipBird;
			if (Var.activeBirds[birdNum].GetRelationshipBonus() > 0)
			{
				//Relationship
				string title = "<name> is in a relationship!";
				string text = "Things are getting serious for <name> and " + relationshipBird.charName + ". They seem very happy for now, but will it last?\nBoth <name> and " + relationshipBird.charName + "gain <b>+20% combat strength<\b> while in this relationship";
				EventScript relationshipEvent = new EventScript(Helpers.Instance.GetCharEnum(Var.activeBirds[birdNum]), title, text);
				EventController.Instance.CreateEvent(relationshipEvent);
			}
			if (Var.activeBirds[birdNum].GetRelationshipBonus() < 0)
			{
				//Crush
				string title = "<name> has a crush!";
				string text = "<name> has fallen hard for " + relationshipBird.charName + "! will you help <name> get together with his paramour or drive them apart?\n<name> will have <b>-20% combat strength</b> until he gets over the crush, or achives a realtionship with their beloved.";
				EventScript relationshipEvent = new EventScript(Helpers.Instance.GetCharEnum(Var.activeBirds[birdNum]), title, text);
				EventController.Instance.CreateEvent(relationshipEvent);
			}
			if (Var.activeBirds[birdNum].GetRelationshipBonus() == 0)
			{
				//Breakup
				string title = "A break up for <name>!";
				string text = "<name> have seen their new romantic dreams evaporate in front of them! Will <name> try to pursue the previous lover once again, find a new love or just be single for a while?\n<name>'s stats have <b>returned to normal.</b>";
				EventScript relationshipEvent = new EventScript(Helpers.Instance.GetCharEnum(Var.activeBirds[birdNum]), title, text);
				EventController.Instance.CreateEvent(relationshipEvent);
			}

			Var.activeBirds[birdNum].newRelationship = false;
		}
	}*/
	void CheckGraphNavBtns()
	{
		
		if (Var.isTutorial)
			maxGraph = Tutorial.Instance.BirdCount[Tutorial.Instance.CurrentPos]-1;		
		nextGraph.interactable = (currentGraph <= maxGraph);
		prevGraph.interactable = (currentGraph > 0);
		//CloseBattleReport.interactable = (currentGraph == maxGraph);
		
	}
	public void FightButtonMouseOver()
	{
		feedBack[] feedBack = FindObjectsOfType<feedBack>();
		foreach (feedBack fb in feedBack)
		{
			if (fb.isMain)
			{
				if (!fb.CheckOpponent())
				{
					fb.HighlightEnemy();
				}
			}
		}

	}
        

	public void GraphButton()
	{
        if (GraphActive || selectedBird.dragged || EventController.Instance.eventObject.activeSelf || Var.Infight)
            return;

        clearSmallGraph();
		if (LevelTutorial.shouldShowFirstBattleDialog)
		{
			LevelTutorial.shouldShowFirstBattleDialog = false;
			LeanTween.delayedCall(0.05f, CloseTutorialText);
		}               
		InitiateGraph(selectedBird,false);
		AudioControler.Instance.PlaySound(AudioControler.Instance.notebookOpen);
        
    }
	void CloseTutorialText()
	{
		speechBubbleObj.SetActive(false);       
	}
	public void InitiateGraph(Bird bird= null,bool afterBattle = true)
	{
		if (!canChangeGraph)
			return;
		canChangeGraph = false;
		if(inMap)
			minimap.SetActive(false);

        if (bird)
        {
            int index = -1;
            for (int i = 0; i < Var.activeBirds.Count; i++)
            {
                if (bird.charName == Var.activeBirds[i].charName)
                {
                    index = i;
                    break;
                }
            }
            //LeanTween.delayedCall(0.7f, () =>
            // {
            //Debug.Log("doing graph! " + bird.charName + " index: " + index);
            CreateGraph(index, afterBattle);
            ProgressGUI.Instance.PortraitClick(bird);
        }
        else
        {
            currentGraph = 3;
            CreateBattleReport();
        }
	   // });
		ProgressGUI.Instance.skillArea.SetActive(false);
		//LeanTween.moveLocal(graph, new Vector3(0, 0, graph.transform.position.z), 0.7f).setEase(LeanTweenType.easeOutBack).setOnComplete(CreateGraph).setOnCompleteParam(index as object);
		graphAnime.SetBool("open", true);
	}
	public void CreateBattleReport() //triggers upon finishing battles in bossless adventures
    {
        FastForwardScript.SetIsInFight(false);
        

        clearSmallGraph();
		closeReportBtn.SetActive(true);
		HideSmallGraph.gameObject.SetActive(false);
		if (finalResult < 0)
		{
			UpdateHearts(--Var.health);
		}
	}



	public void PortraitControl(int portNr,Var.Em color)
	{
		activePortrait = portNr;
		for (int i = 0; i < portraits.Length; i++)
		{
			if (i == portNr)
			{
				portraits[i].SetActive(true);
				GameObject colorObj= portraits[i].gameObject.transform.Find("bird_color").gameObject;
				colorObj.GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(color);
			}else
			{
				portraits[i].SetActive(false);
			}
		}
	}
	
	public void Fight()
	{
		GameLogic.Instance.FightButton.interactable = false;
        GameLogic.Instance.FightButton.GetComponent<Animator>().SetBool("fight", true);
        AudioControler.Instance.ClickSound();
        AudioControler.Instance.ActivateMusicSource(audioSourceType.battleSource);
		AudioControler.Instance.PlaySound(AudioControler.Instance.clicks);
		feedBack[] feedBackObj = FindObjectsOfType(typeof(feedBack)) as feedBack[];
        FastForwardScript.SetIsInFight(true);

		foreach (feedBack fb in feedBackObj)
		{
			fb.HideFeedBack(true);
		}
        if(Var.isEnding)
        {
            Ending.Instance.visuals.vultureKingFistPumps();
        }
        Debug.Log("Fight selected");
		int result = 0;
		Var.Infight = true;
		Bird playerBird = null;
		Bird secondPlayerBird = null;
	
	   

	   for (int i = 0; i <Var.enemies.Length; i++)
	   {
			if (Var.enemies[i].inUse)
			{
				for (int j = 0; j < 4; j++)
				{

					if (Var.enemies[i].position == Bird.dir.front)
					{
						if (Var.playerPos[3-j, i%4] != null)
						{
							if (!Var.playerPos[3 - j, i % 4].isHiding)
							{
								if (playerBird == null)
								{
									playerBird = Var.playerPos[3 - j, i % 4];
									if (Var.enemies[i].enemyType != fillEnemy.enemyType.drill)
										break;
								}
								else
								{
									secondPlayerBird = Var.playerPos[3 - j, i % 4];
									break;
								}
								
							}
						}
					}
					if (Var.enemies[i].position == Bird.dir.top)
					{
						if (Var.playerPos[i%4, j] != null)
						{
							if (!Var.playerPos[i % 4, j].isHiding)
							{
								if (playerBird == null)
								{
									playerBird = Var.playerPos[i % 4, j];
									if (Var.enemies[i].enemyType != fillEnemy.enemyType.drill)
										break;
								}
								else
								{
									secondPlayerBird = Var.playerPos[i % 4, j];
									break;
								}
							}
						}

					}
				}
				// Fight Logic
				playerBird.foughtInRound = true;
				int resultOfBattle = GameLogic.Instance.Fight(playerBird, Var.enemies[i]);
				battleAnim.Instance.AddData(playerBird, Var.enemies[i], resultOfBattle);
				result += resultOfBattle;
				if (secondPlayerBird != null)
				{
					secondPlayerBird.foughtInRound = true;
					int resultOfSecondBattle = GameLogic.Instance.Fight(secondPlayerBird, Var.enemies[i]);
					battleAnim.Instance.AddData(secondPlayerBird, Var.enemies[i], resultOfSecondBattle);
					result += resultOfSecondBattle;
				}
				playerBird = null;
				secondPlayerBird = null;
				


			}

		}
		Bird[] birds = FindObjectsOfType(typeof(Bird)) as Bird[];
		foreach(Bird bird in birds)
		{

			if (!bird.isEnemy && !bird.data.injured)
			{				
				players.Add(bird);
			}
		}
	   foreach(Bird bird in players)
		{
			int friendGain = Helpers.Instance.Findfirendlieness(bird);
			if (friendGain > 0)
			{
                if (friendGain > 2)
                {
                    Helpers.Instance.EmitEmotionParticles(bird.transform, Var.Em.Social, true, 2);
                }
                else
                {
                    Helpers.Instance.EmitEmotionParticles(bird.transform, Var.Em.Social);

                }
				AudioControler.Instance.PlaySound(AudioControler.Instance.SocialInfoAppear);
			}
			if (friendGain < 0)
			{
				Helpers.Instance.EmitEmotionParticles(bird.transform, Var.Em.Solitary);
                AudioControler.Instance.PlaySound(AudioControler.Instance.SocialInfoAppear);
			}

			bird.friendBoost += friendGain;			
			bird.gameObject.GetComponent<firendLine>().RemoveLines();

            bird.indicator.SetEmotions(Var.Em.Neutral, Var.Em.Neutral);
            bird.indicator.Hide();
        }



		battleAnim.Instance.Battle();        
		finalResult = result;
		
		

	}

	
	public void ReturnToMap()
	{
		foreach (Bird bird in Var.availableBirds)
		{
			bird.AddRoundBonuses();
			bird.DecreaseTurnsInjured();
		}
		if(Var.currentStageID != -1)
		{
			foreach(MapSaveData data in Var.mapSaveData)
			{
				if(data.ID == Var.currentStageID)
				{
					data.completed = true;
					foreach(int id in data.targets)
					{
						foreach(MapSaveData targ in Var.mapSaveData)
						{
							if(targ.ID == id)
							{
								targ.available = true;
								break;
							}
						}
						
					}
					break;
				}
			}
		}
		Var.currentWeek++;
		Var.shouldDoMapEvent = true;
		Var.CanShowHover = true;
		LeanTween.cancelAll();
        AudioControler.Instance.SaveVolumeSettings();
		SaveLoad.Save();
		SceneManager.LoadScene("Map");
	}

	public void LoadMainMenu()
	{
		Time.timeScale = 1.0f;
        AudioControler.Instance.SaveVolumeSettings();
        if (Var.tutorialCompleted)
        {
           SaveLoad.Save();
        }
		AudioControler.Instance.ClickSound();
		SceneManager.LoadScene("MainMenu");

	}
	public bool Reset()
	{
        //Debug.LogError("Resetting!");
        Var.enemies = new Bird[8];
		Var.Infight = false;		
		ProgressGUI.Instance.SetOnePortrait();       
		foreach (Bird bird in players)
		{
			bird.ResetBonuses(); 
            foreach (Bird activeBird in Var.activeBirds)
			{
				if (activeBird.data.injured)
				{
					Time.timeScale = 0.0f;
					QuitToMap();
					return false;
				}
			}
			//bird.gameObject.GetComponentInChildren<Animator>().SetBool("iswalking", false);

			//bird.gameObject.GetComponent<Animator>().SetBool("lose", false);
			//bird.gameObject.GetComponent<Animator>().SetBool("victory", false);
			bird.target = bird.home;
			bird.transform.position = bird.home;
			bird.prevConf = bird.data.confidence;
			bird.prevFriend = bird.data.friendliness;
			bird.totalConfidence += bird.data.confidence;
			bird.totalFriendliness += bird.totalFriendliness;
			bird.ResetBonuses();
			bird.GroundBonus.SetActive(false);
		}
		//After applying levels;
		/*GuiContoler.Instance.relationshipPanel.SetActive(false);
		GuiContoler.Instance.statPanel.SetActive(true);
		GuiContoler.Instance.ToggleRelationPanelText.text = "Relationships";*/
		foreach (Bird bird in players)
		{
			bird.ResetAfterLevel();
		}
		Var.playerPos = new Bird[4, 4];
		foreach (LayoutButton tile in ObstacleGenerator.Instance.tiles)
		{
			tile.gameObject.SetActive(true);
			tile.Reset();
		}

		finalResult = 0;
		players = new List<Bird>();
		moveInMap();
		mapPos++;
		foreach(GuiMap map in FindObjectsOfType<GuiMap>())
			map.MoveMapBird(mapPos);
		
		if (Var.isTutorial)
		{
			for (int i = 0; i < FillPlayer.Instance.playerBirds.Length; i++)
			{
				if (i < Tutorial.Instance.BirdCount[mapPos])
				{
					FillPlayer.Instance.playerBirds[i].gameObject.SetActive(true);
				}
				else
				{
					FillPlayer.Instance.playerBirds[i].gameObject.SetActive(false);
				}
			}
			Tutorial.Instance.ShowtutorialStartingText(mapPos);
		}
        if(Var.isEnding)
        {
            Ending.Instance.ShowEndingStartingText(mapPos);
        }
        BattleData Area = Var.map[mapPos];
		if (Area.type != Var.Em.finish)
		{
			if (Var.isTutorial)
			{				
				Tutorial.Instance.SetCurrenPos(mapPos);
				GetComponent<fillEnemy>().CreateTutorialEnemies(Tutorial.Instance.TutorialMap[mapPos]);
			}
            else
            {
                Ending.Instance.SetCurrenPos(mapPos);
                if (Var.isEnding)
                {
                    GetComponent<fillEnemy>().CreateTutorialEnemies(Ending.Instance.TutorialMap[mapPos]);
                }
                else
                {
                    GetComponent<fillEnemy>().CreateEnemies(Area.battleData, Area.birdLVL, Area.dirs, Area.minEnemies, Area.maxEnemies);
                }
				//if (!EventController.Instance.tryEvent())
					DialogueControl.Instance.TryDialogue(Dialogue.Location.battle);
			}
			ObstacleGenerator.Instance.clearObstacles();
			ObstacleGenerator.Instance.GenerateObstacles();
			GameLogic.Instance.CanWeFight();
			
		}
		return true;

	}

	public void UpdateBirdSave(Bird bird)
	{
		for (int i = 0; i < Var.activeBirds.Count; i++)
		{
			if (Var.activeBirds[i].charName == bird.charName)
			{
				Var.activeBirds[i] = bird;
				break;
			}
		}

		if (Var.availableBirds.Count > 0)
		{
			for (int i = 0; i < Var.availableBirds.Count; i++)
			{
				if (Var.availableBirds[i].charName == bird.charName)
				{
					Var.availableBirds[i] = bird;
					break;
				}
			}
		}else
		{
			Var.availableBirds.AddRange(FillPlayer.Instance.playerBirds);
		}
	}


	void setMapLocation(int index)
	{
		try
		{
			currentMapArea = Var.map[index].type;
		}
		catch { }
        try
        {
            nextNextMapArea = Var.map[index + 2].type;
        }

        catch {

        }
		try
		{
			nextMapArea = Var.map[index + 1].type;
		}
		catch
		{
			nextMapArea = Var.Em.finish;
		}
		
	}

    
	void moveInMap()
	{
        setMapLocation(mapPos);
        GuiContoler.Instance.canPause(true);
        if (nextMapArea == Var.Em.finish)
        {
            
            mapPos = 0;
            if (Var.isTutorial)
            {
                Var.KimUnlocked = false;
                Var.SophieUnlocked = false;
#if ENABLE_CLOUD_SERVICES_ANALYTICS
                Analytics.CustomEvent("tutorialCompleted");
#endif
            }
            Var.isTutorial = false;
            Var.tutorialCompleted = true;
            Var.isBoss = false;
            canplayBossTransition = false;
            showVictoryScreen();
            GraphBlocker.SetActive(true);
            AudioControler.Instance.PlayWinMusic();
            clearSmallGraph();
        }
  

    }
    
    public void showVictoryScreen()
    {
        winBanner.SetActive(true);
        WinScreen.Instance.SetupWinScreen(Var.availableBirds.Count > 0 ?Var.availableBirds: Var.activeBirds);//  new List<Bird>(FillPlayer.Instance.playerBirds));
        /* foreach (Transform child in winBanner.transform.GetChild(0).transform.GetChild(2))
         {
             child.transform.gameObject.SetActive(false);
         }

         foreach (Bird bird in Var.activeBirds)
         {

             foreach(Transform child in winBanner.transform.GetChild(0).transform.GetChild(2))
             {

                 child.transform.gameObject.SetActive(true);
                 if (bird.data.unlocked)
                 {
                     if (bird.charName.ToLower() == child.transform.GetChild(0).name)
                     {
                         child.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
                     }
                 }

             }
         }

         winBanner.SetActive(true);*/
    }
	
	public void ShowMessage(string message)
	{
		messages.Add(message);
		if (!messageBox.activeSelf)
		{
			messageText.text = message;
		}
		messageBox.SetActive(true);

	}	
	public void HideMessage()
	{
		messages.RemoveAt(0);
		if (messages.Count == 0)
		{
			messageText.text = "";
			messageBox.SetActive(false);
		}else
		{
			messageText.text = messages[0];
		}

	}
}

