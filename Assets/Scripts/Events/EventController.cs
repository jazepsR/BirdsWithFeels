using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class EventSegment
{
    public int minID;
    public int maxID;
    public Transform eventParent;
}

public class EventController : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float eventFreq;
    public EventScript defaultMapEvent;
    public static EventController Instance { get; private set; }
    [SerializeField] private Image eventBg;
    [SerializeField] private Image bgFog;
    public EventScript testEvent;
    public List<EventScript> events;
    public bool inMap;
    [Header("Event segments")]
    [SerializeField]
    public List<EventSegment> eventSegments;
    [Header("References")]
    public GameObject eventObject;
    public GameObject hideEventButton;
    public Text hideEventButtonText;
    public Text heading;
    public Text text;
    public GameObject choice;
    public RectTransform choiceList;
    public GameObject portraitParent;
    public GameObject nameFieldParent;
    public Image portrait;
    public Image portraitFill;
    public Image customImage;
    public GameObject continueBtn;
    public Text nameText;
    EventScript currentEvent;
    GameObject currentPortrait;
    public Bird currentBird;
    List<GameObject> portraits;
    List<Color> colors;
    EventScript nextEvent = null;
    public ShowTooltip mouseOver;
    IEnumerator coroutine;
    [HideInInspector]
    public List<EventScript> eventsToShow;
    //List<string> texts;    
    private Animator myEventGUIAnimator;
    int currentText = 0;
    bool activeChoices = false;
    bool printing = false;


    public AudioSource eventAudioSource;
    // Use this for initialization
    void Awake()
    {
        eventsToShow = new List<EventScript>();
        Instance = this;
    }
    void Start()
    {
        myEventGUIAnimator = eventObject.GetComponent<Animator>();

        if (eventAudioSource == null)
        {
            eventAudioSource = gameObject.AddComponent<AudioSource>();
        }
        if (testEvent == null)
        {
            if (!inMap)
            {

                foreach (EventSegment segment in eventSegments)
                {
                    if (Var.currentStageID == -1)
                    {
                        events.AddRange(segment.eventParent.GetComponentsInChildren<EventScript>());
                    }
                    else
                    {

                        int id = Var.currentStageID % 1000;
                        if (id >= segment.minID && id <= segment.maxID)
                        {
                            events.AddRange(segment.eventParent.GetComponentsInChildren<EventScript>());
                            break;
                        }
                    }
                }
                LeanTween.delayedCall(0.25f, () => tryEvent()); //Event plays as soon as player enters scene
            }
        }
        else
        {
            CreateEvent(testEvent);
        }
        nextEvent = null;
    }


    public void ContinueBtn()
    {
        if (printing)
        {
            printing = false;
            if (currentText == currentEvent.parts.Count - 1) //If text is printing - display all text at once when user clicks 
            {
                CreateChoices();
                AudioControler.Instance.FadeOutBirdTalk();

            }

            return;
        }
        if (activeChoices) //Continue button does not work if there's choices active 
            return;

        currentText++;
        // Debug.Log("currentText: " + currentText + "currentEvent.parts.Count" + (currentEvent.parts.Count - 1));
        AudioControler.Instance.ClickSound();
        myEventGUIAnimator.SetTrigger("click");



        if (currentEvent != null && (currentText < currentEvent.parts.Count - 1)) //If there's more to show, show it
        {
            print("if there is more to show, show it");
            string text = Helpers.Instance.ApplyTitle(currentBird, currentEvent.parts[currentText].text);
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = WaitAndPrint(text, true);
            //nameText.text = currentBird.charName;
            StartCoroutine(coroutine);
            SetPortrait(currentText); //Seb. Maybe only play new portrait anims if bird has changed? Easy to set up
        }

        if (currentEvent != null && (currentText == currentEvent.parts.Count - 1))
        {
            print("i am last in event");
            string text = Helpers.Instance.ApplyTitle(currentBird, currentEvent.parts[currentText].text);
            if (coroutine != null)
                StopCoroutine(coroutine);
            coroutine = WaitAndPrint(text, false);
            //nameText.text = currentBird.charName;
            StartCoroutine(coroutine);
            SetPortrait(currentText);
            return;
        }
        if (currentEvent == null)
        {
            myEventGUIAnimator.SetTrigger("close"); //Hide GUI once it has finished animating closed
            LeanTween.delayedCall(0.7f, () =>
             eventObject.SetActive(false));

            if (inMap) //Update map icons once event has finished playing. 
            {
                DialogueControl.Instance.TryDialogue(Dialogue.Location.map);
                foreach (MapIcon icon in FindObjectsOfType<MapIcon>())
                    icon.SetState();
                
            }
        }
            else if (currentEvent != null && (currentText > currentEvent.parts.Count - 1)) //Has finished playing all parts in current event? 
        {
            if (currentEvent.quitAfterLevel) //Go to main menu after current event? 
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("mainMenu");
                return;
            }


            //Turns off event object
            LeanTween.value(gameObject, (float vol) => eventAudioSource.volume = vol, eventAudioSource.volume, 0, 0.5f);


            if (nextEvent != null) //Chaining events together - play next event if applicable 
            {
                // currentEvent = null;
                nextEvent.canShowMultipleTimes = true;
                currentEvent = null;
                CreateEvent(nextEvent);
                nextEvent = null;
                Debug.Log("playing next event");
                return;
            }

            if (eventsToShow.Count > 0) //Play queued up events to show 
            {
                EventScript nextEvent = eventsToShow[0];
                Debug.Log("play queue up events to show");
                eventsToShow.RemoveAt(0);
                CreateEvent(nextEvent);

            }
            else if (currentEvent.afterEventDialog != null) //Play specific dialogue after event finishes 
            {
                DialogueControl.Instance.CreateParticularDialog(currentEvent.afterEventDialog);
                myEventGUIAnimator.SetTrigger("close"); //Hide GUI once it has finished animating closed
                LeanTween.delayedCall(0.7f, () =>
                 eventObject.SetActive(false));
                Debug.Log("play specific dialogue");
            }
            else
            {
                Debug.Log("i am closing now");
                myEventGUIAnimator.SetTrigger("close"); //Hide GUI once it has finished animating closed
                LeanTween.delayedCall(0.7f, () =>
                 eventObject.SetActive(false));

                if (inMap) 
                {
                    DialogueControl.Instance.TryDialogue(Dialogue.Location.map); 
                    if (currentBird && MapControler.Instance.showGraphAfterEvent)
                    {
                        LeanTween.delayedCall(0.7f, () =>GuiContoler.Instance.OpenMapBigGraph(currentBird));
                    }
                    //foreach (MapIcon icon in FindObjectsOfType<MapIcon>())
                    // icon.SetState();
                }
                else
                {
                    if (GuiContoler.Instance.graph.transform.localPosition.y < -500) //??? no idea what kind of madness this is /seb 
                    {
                        GuiContoler.Instance.GraphBlocker.SetActive(false);
                        DialogueControl.Instance.TryDialogue(Dialogue.Location.battle);
                    }
                }
            }

            currentEvent = null;
            nextEvent = null;
        }
       

    }

   

    private IEnumerator WaitAndPrint(string printText, bool shouldShowContinue)
    {

        continueBtn.GetComponent<Animator>().SetBool("active", false);
        text.text = "";
        printing = true;
        foreach (char ch in printText)
        {
            text.text += ch;
            yield return null;
            if (!printing)
            {
                text.text = printText;
                break;
            }
        }
        printing = false;
        continueBtn.GetComponent<Animator>().SetBool("active", shouldShowContinue);
        if (currentText == currentEvent.parts.Count - 1)
            CreateChoices();
        AudioControler.Instance.FadeOutBirdTalk();
    }



    public bool tryEvent()
    {
        if (GuiContoler.Instance.winBanner != null && GuiContoler.Instance.winBanner.activeSelf)
            return false;
        if (eventObject.activeSelf)
            return false;
        if (Var.isTutorial || Var.isEnding || Var.freezeEmotions || (Var.currentStageID == Var.battlePlanningTutorialID && !Var.gameSettings.shownBattlePlanningTutorial) || (Var.currentStageID == Var.levelTutorialID && !Var.gameSettings.shownLevelTutorial))
            return false;
        currentBird = null;
        currentPortrait = null;
        currentEvent = null;
        if (Random.Range(0, 1.0f) > eventFreq)
            return false;
        List<Bird> birdsToCheck;
        if (inMap)
        {
            birdsToCheck = Var.availableBirds;
        }
        else
        {
            birdsToCheck = Var.activeBirds;
        }
        bool canCreateEvent = false;
        if (events.Count > 0)
        {
            EventScript ev = events[Random.Range(0, events.Count)];

            for (int i = 0; i < 100; i++)
            {



                if (testEvent != null)
                    ev = testEvent;
                else
                    ev = events[Random.Range(0, events.Count)];
                canCreateEvent = true;
                if (ev == null)
                    canCreateEvent = false;
                else
                {
                    if (ev.gameObject.name != "" && Var.shownEvents.Contains(ev.gameObject.name))
                        canCreateEvent = false;
                    if (ev.speakers.Contains(EventScript.Character.Alexander) && Var.availableBirds.Count < 4)
                        canCreateEvent = false;
                    if (ev.speakers.Contains(EventScript.Character.Sophie) && Var.availableBirds.Count < 5)
                        canCreateEvent = false;
                    if (ev.speakers[0] != EventScript.Character.None)
                    {
                        if (!ConditionCheck.CheckCondition(ev.condition, Helpers.Instance.GetBirdFromEnum(ev.speakers[0]), ev.targetEmotion, ev.magnitude))
                            canCreateEvent = false;
                    }
                }
                if (canCreateEvent)
                    break;
            }

            if (canCreateEvent)
                CreateEvent(ev);
        }
        return true;
    }
    public void CreateEvent(EventScript eventData)
    {
        Debug.Log("creating Event!");
        if (eventData.isCampFireScene && inMap)
        {
            try
            {
                Transform campFire = eventObject.transform.Find("Campfire");
                if (campFire != null)
                {
                    foreach (Bird bird in FillPlayer.Instance.playerBirds)
                    {

                        foreach (Transform child in campFire.GetChild(1))
                        {

                            if (bird.charName == child.transform.name && bird.data.unlocked)
                            {
                                try
                                {

                                    if (child.transform.GetChild(0).GetComponent<Image>() != null)
                                    {
                                        child.transform.GetChild(0).GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
                                    }
                                }
                                catch
                                {
                                    Debug.Log("campfire bird image component not found");
                                }

                            }
                            else if (bird.charName == child.transform.name && !bird.data.unlocked)
                            {
                                child.transform.gameObject.SetActive(false);
                            }

                        }
                    }

                    campFire.gameObject.SetActive(true);
                }

            }
            catch
            {

                Debug.Log("campfire can not be shown");
            }

        }
        else
        {
            Transform campFire = eventObject.transform.Find("Campfire");
            if (campFire != null)
            {
                campFire.gameObject.SetActive(false);
            }
        }
        if (eventData.eventBackground != null)
        {
            eventBg.sprite = eventData.eventBackground;
        }
        if (hideEventButton)
        {
            //hideEventButton.SetActive(false);
        }
        eventBg.gameObject.SetActive(eventData.eventBackground != null);
        bgFog.gameObject.SetActive(eventData.useBgFog);

        if (eventData.useEventAudio && eventData.eventAudio.clips.Length > 0)
        {
            eventAudioSource.volume = 1f;
            eventData.eventAudio.Play();
        }


        if (!eventData.canShowMultipleTimes)
        {
            Var.shownEvents.Add(eventData.gameObject.name);
        }
        if (currentEvent != null)
        {
            eventsToShow.Add(eventData);
            return;
        }
        choiceList.gameObject.SetActive(false);
        currentText = 0;
        if (!inMap)
        {
            GuiContoler.Instance.GraphBlocker.SetActive(true);
        }


        currentEvent = eventData;
        currentEvent.parts = new List<EventPart>();
        currentEvent.parts.AddRange(eventData.transform.GetComponentsInChildren<EventPart>());
        if (eventData.speakers[0] != EventScript.Character.None)
        {
            if (currentEvent.affectedBird != EventScript.Character.None)
            {
                try
                {
                    currentBird = Helpers.Instance.GetBirdFromEnum(currentEvent.affectedBird);
                }
                catch { }
            }
            else
            {
                try
                {
                    currentBird = Helpers.Instance.GetBirdFromEnum(eventData.speakers[0]);
                }
                catch { }
            }
            //currentPortrait = currentBird.portrait;
        }
        portraits = new List<GameObject>();
        colors = new List<Color>();
        foreach (EventScript.Character Char in eventData.speakers)
        {
            portraits.Add(Helpers.Instance.GetPortrait(Char));
            try
            {
                Color col = Helpers.Instance.GetEmotionColor(Helpers.Instance.GetBirdFromEnum(Char).emotion);
                colors.Add(col);

            }
            catch
            {
                colors.Add(Helpers.Instance.GetEmotionColor(Helpers.Instance.RandomEmotion()));
            }

        }


        eventObject.SetActive(true);
        heading.text = Helpers.Instance.ApplyTitle(currentBird, eventData.heading);
        string text = Helpers.Instance.ApplyTitle(currentBird, eventData.parts[0].text);
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = WaitAndPrint(text, true);
        StartCoroutine(coroutine);
        SetPortrait(0);
        if (eventData.options.Length > 0 && eventData.parts.Count <= 1)
        {
            CreateChoices();
        }
        else
        {
            continueBtn.GetComponent<Animator>().SetBool("active", true);
        }
    }



    void SetPortrait(int id)
    {
        portraitParent.SetActive(true);
        nameFieldParent.SetActive(true);
        if (mouseOver)
        {
            mouseOver.tooltipText = "";
        }
        if (currentEvent.parts[id].useCustomPic && currentEvent.parts[id].customPic != null)
        {
            portrait.transform.parent.gameObject.SetActive(true);
            customImage.gameObject.SetActive(true);
            portraitFill.gameObject.SetActive(false);
            portrait.gameObject.SetActive(false);
            customImage.sprite = currentEvent.parts[id].customPic;
        }
        else
        {
            customImage.gameObject.SetActive(false);
            try
            {
                portrait.transform.parent.gameObject.SetActive(true);
                if (portraits[currentEvent.parts[currentText].speakerId].transform.Find("bg/bird_color").GetComponent<Image>().sprite == null)
                {
                    portraitFill.gameObject.SetActive(false);
                }
                else
                {
                    portraitFill.gameObject.SetActive(true);
                }


                portrait.gameObject.SetActive(true);
                portraitFill.sprite = portraits[currentEvent.parts[currentText].speakerId].transform.Find("bg/bird_color").GetComponent<Image>().sprite;
                portraitFill.color = colors[currentEvent.parts[currentText].speakerId];
                portrait.sprite = portraits[currentEvent.parts[currentText].speakerId].transform.Find("bg/bird").GetComponent<Image>().sprite;
                //Debug.LogError("portriat name: " + portrait.sprite.name);

                //SEB ADD ON - take the local scale of relevant portrait 
                portrait.rectTransform.localScale = portraits[currentEvent.parts[currentText].speakerId].transform.Find("bg/bird").localScale;
                portraitFill.rectTransform.localScale = portraits[currentEvent.parts[currentText].speakerId].transform.Find("bg/bird_color").localScale;

                //SEB ADD ON - take the position of the relevant portrait and apply it 
                portrait.rectTransform.localPosition = portraits[currentEvent.parts[currentText].speakerId].transform.Find("bg/bird").localPosition;
                portraitFill.rectTransform.localPosition = portraits[currentEvent.parts[currentText].speakerId].transform.Find("bg/bird_color").localPosition;

                nameText.text = currentEvent.speakers[currentEvent.parts[currentText].speakerId].ToString().Replace('_', ' ');

                string characterNameText = nameText.text;

                int index = characterNameText.IndexOf("0");
                if (index > 0)
                {
                    characterNameText = characterNameText.Substring(0, index);
                }

                nameText.text = characterNameText;
                StartBirdTalk(currentEvent.speakers[currentEvent.parts[currentText].speakerId]);


            }
            catch
            {
                print("failed to show portrait");
                portraitParent.SetActive(false);
                nameFieldParent.SetActive(false);
            }
        }


    }
    private void StartBirdTalk(EventScript.Character birdToTalk)
    {
        try
        {
            try
            {
                AudioControler.Instance.PlaySound(AudioControler.Instance.GetEventAudio(birdToTalk));
            }
            catch
            {
                AudioControler.Instance.PlaySound(AudioControler.Instance.DefaultBirdSound.eventAudio);

            }
            //AudioControler.Instance.PlaySound(Helpers.Instance.GetBirdFromEnum(currentEvent.speakers[currentEvent.parts[currentText].speakerId]).birdSounds.eventAudio);
        }
        catch
        {
            Debug.LogError("audio not found!");
        }
    }
    public void HideEventGUI()
    {
        if (hideEventButtonText)
        {
            hideEventButtonText.text = "Show Field";
        }
        myEventGUIAnimator.SetBool("showBattlefield", true);
    }
    public void ShowEventGUI()
    {
        if (hideEventButtonText)
        {
            hideEventButtonText.text = "Show Field";
        }
        myEventGUIAnimator.SetBool("showBattlefield", false);
    }
    void CreateChoices()
    {

        if (currentEvent.options.Length > 0)
        {
            int i = 0;
            choiceList.gameObject.SetActive(true);

            LeanTween.delayedCall(2f, () =>
            {
                if (!inMap && hideEventButton && !Var.isTutorial)
                {
                   // hideEventButton.SetActive(true);
                }
            }
            );

            bool ChoicesInfluenceEmotions = false;
            int myAmountOfChoices = 0;

            foreach (EventConsequence choiceData in currentEvent.options)
            {
                myAmountOfChoices += 1;
                GameObject choiceObj = Instantiate(choice, choiceList);
                SetupChoice(choiceObj, i);

                ChoicesInfluenceEmotions = DoesOptionIfluenceEmotions(currentEvent.options[i]);
                i++;
            }

            if (ChoicesInfluenceEmotions && myAmountOfChoices > 1) //Seb, only show the emo graph if the choices influences the relevant bird, and there's more then one choice. 
            {
                myEventGUIAnimator.SetBool("showingEmoGraph", true);
            }
            activeChoices = true;
            if (currentBird)
            {
                currentBird.showText();
                if(inMap)
                {
                    MapControler.Instance.charInfoAnim.SetBool("show", true);
                    MapControler.Instance.charInfoAnim.SetBool("hide", false);
                }
            }
        }
        else
        {
            continueBtn.GetComponent<Animator>().SetBool("active", true);
            ContinueBtn();
        }
        currentText++;

    }

    private bool DoesOptionIfluenceEmotions(EventConsequence A_Option)
    {
        if (A_Option.magnitude1 == 0 && A_Option.magnitude2 == 0 && A_Option.magnitude3 == 0)
        {
            return false;
        }

        return true;
    }

    void DisplayChoiceResult(int ID)
    {
        if (hideEventButton && currentEvent.isCharacterJoinEvent)
        {
            hideEventButton.SetActive(false);
        }
        myEventGUIAnimator.SetBool("showingEmoGraph", false);
        activeChoices = false;
        AudioControler.Instance.PlayPaperSound();
        choiceList.gameObject.SetActive(false); 
        Helpers.Instance.HideTooltip();
        ApplyConsequences(ID);
        string consequences = GetEventConsequenceText(ID);
        if (currentBird != null)
        {
            try
            {
                currentBird.AddRoundBonuses(false);
                currentBird.showText();
                // StartBirdTalk(Helpers.Instance.GetCharEnum(currentBird));

            }
            catch { }
        }
        foreach (Transform child in choiceList)
        {
            Destroy(child.gameObject);
        }
        nextEvent = currentEvent.options[ID].onComplete;
        heading.text = Helpers.Instance.ApplyTitle(currentBird, currentEvent.options[ID].conclusionHeading);
        string text = Helpers.Instance.ApplyTitle(currentBird, currentEvent.options[ID].conclusionText);
        nameText.text = "";
        //nameText.text = currentBird.charName;
        if (currentEvent.options[ID].useAutoExplanation)
            text += "\n" + consequences;
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = WaitAndPrint(text, true);
        StartCoroutine(coroutine);
        if (currentEvent.options[ID].AfterImage != null)
        {
            portrait.transform.parent.gameObject.SetActive(true);
            customImage.gameObject.SetActive(true);
            portraitFill.gameObject.SetActive(false);
            portrait.gameObject.SetActive(false);
            customImage.sprite = currentEvent.options[ID].AfterImage;
        }
        
        if(currentEvent.options[ID].useNarration)
        {
            portraitParent.SetActive(false);
            nameFieldParent.SetActive(false);
        }

    }

    void ApplyConsequences(int ID)
    {
        ApplyConsequence(currentEvent.options[ID].consequenceType1, currentEvent.options[ID].magnitude1, currentEvent.options[ID].applyToAll);
        ApplyConsequence(currentEvent.options[ID].consequenceType2, currentEvent.options[ID].magnitude2, currentEvent.options[ID].applyToAll);
        ApplyConsequence(currentEvent.options[ID].consequenceType3, currentEvent.options[ID].magnitude3, currentEvent.options[ID].applyToAll);
        if (currentBird != null)
            currentBird.SetEmotion();
        if (GameLogic.Instance)
        {
            GameLogic.Instance.UpdateFeedback();
        }
        if (GuiContoler.Instance.GraphBlocker)
        {
            GuiContoler.Instance.GraphBlocker.SetActive(false);
        }

    }
    public string GetEventConsequenceText(int ID)
    {
        string[] consequences = new string[] {
        GetConsequenceText(currentEvent.options[ID].consequenceType1, currentEvent.options[ID].magnitude1, currentEvent.options[ID].applyToAll),
            GetConsequenceText(currentEvent.options[ID].consequenceType2, currentEvent.options[ID].magnitude2, currentEvent.options[ID].applyToAll),
            GetConsequenceText(currentEvent.options[ID].consequenceType3, currentEvent.options[ID].magnitude3, currentEvent.options[ID].applyToAll) };
        return string.Join("\n",consequences.Where(s => !System.String.IsNullOrEmpty(s)));
    }

    public string GetConsequenceText(ConsequenceType type, int magnitude, bool applyToAll)
    {
        if (currentBird == null)
            return "";
        List<Bird> birdsToApply = new List<Bird>();
        if (applyToAll)
        {
            foreach (Bird bird in Var.activeBirds)
            {
                birdsToApply.Add(bird);
            }
        }
        else
        {
            birdsToApply.Add(currentBird);
        }

        string infoString = "";
        foreach (Bird bird in birdsToApply)
        {
            switch (type)
            {
                case ConsequenceType.Courage:
                    if (magnitude > 0)
                    {
                        infoString += bird.charName + " gains " + magnitude + " confidence\n";
                    }
                    else
                    {
                        infoString += bird.charName + "'s caution increases by " + Mathf.Abs(magnitude) + "\n";
                    }
                    break;
                case ConsequenceType.Friendliness:
                    if (magnitude > 0)
                    {
                        infoString += bird.charName + " gains " + magnitude + " social\n";
                    }
                    else
                    {
                        infoString += bird.charName + " gains " + Mathf.Abs(magnitude) + " solitude\n";
                    }

                    break;
                case ConsequenceType.Health:
                    if (magnitude > 0)
                    {
                        infoString += bird.charName + " gained " + magnitude + " health\n";
                    }
                    else
                    {
                        infoString += bird.charName + " lost " + Mathf.Abs(magnitude) + " health\n";
                    }
                    break;
                default:
                    break;
            }
        }
        return infoString;
    }
    void ApplyConsequence(ConsequenceType type, int magnitude, bool applyToAll)
    {
        if (currentBird == null)
            return;
        List<Bird> birdsToApply = new List<Bird>();
        if (applyToAll)
        {
            foreach (Bird bird in Var.activeBirds)
            {
                birdsToApply.Add(bird);
            }
        }
        else
        {
            birdsToApply.Add(currentBird);
        }

        string infoString = "";
        foreach (Bird bird in birdsToApply)
        {
            if (bird != null)
                bird.SetEmotion();

            int clamp = Var.isEnding ? 13 : 15;
            switch (type)
            {
                case ConsequenceType.Courage:
                    bird.prevConf = bird.data.confidence;
                    bird.data.confidence = Mathf.Clamp(bird.data.confidence + magnitude, -clamp, clamp);
                    break;
                case ConsequenceType.Friendliness:
                    bird.prevFriend = bird.data.friendliness;
                    bird.data.friendliness = Mathf.Clamp(bird.data.friendliness + magnitude, -clamp, clamp);

                    break;
                case ConsequenceType.Health:
                    currentBird.ChageHealth(magnitude);
                    break;
                default:
                    break;
            }
        }

    }
    void SetupChoice(GameObject choiceObj, int ID)
    {
        Transform myTransformToGetComponentsFrom = choiceObj.transform.Find("BG");

        EventConsequence choiceData = currentEvent.options[ID];
        choiceObj.GetComponent<Button>().onClick.AddListener(delegate { DisplayChoiceResult(ID); });
        try
        {
            var tooltip = choiceObj.GetComponentInChildren<ShowTooltip>();
            choiceObj.GetComponentInChildren<EventOptionAudio>().Setup(ID);
            if (choiceData.selectionTooltip.Trim() != "")
            {
                tooltip.tooltipText = Helpers.Instance.ApplyTitle(currentBird, choiceData.selectionTooltip);
                if (choiceData.useAutoExplanation)
                {
                    tooltip.tooltipText += "\n" + GetEventConsequenceText(ID);
                }
            }
            else
            {
                tooltip.tooltipText = GetEventConsequenceText(ID);
            }
            
            Text myChoiceTest = myTransformToGetComponentsFrom.transform.Find("Description").GetComponent<Text>();

            myChoiceTest.text = Helpers.Instance.ApplyTitle(currentBird, choiceData.selectionText);

        }
        catch
        {

        }
        if (choiceData.icon != null)
            myTransformToGetComponentsFrom.transform.Find("Icon").GetComponent<Image>().sprite = choiceData.icon;
        else
            myTransformToGetComponentsFrom.transform.Find("Icon").gameObject.SetActive(false);

    }

    public void SetIsHovering(bool hovering)
    {

        if (hovering && !activeChoices) //Only set it to hover if there's no choices active - since only the choices will make progress happen! 
        {
            myEventGUIAnimator.SetBool("hover", true);
        }
        else
        {

            myEventGUIAnimator.SetBool("hover", false);
        }

    }

}
