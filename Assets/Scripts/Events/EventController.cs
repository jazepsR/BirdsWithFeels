using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[System.Serializable]
public class EventSegment
{
    public int minID;
    public int maxID;
    public Transform eventParent;
}

public class EventController : MonoBehaviour {	
	[Range(0.0f, 1.0f)]
	public float eventFreq;
	public static EventController Instance { get; private set; }
	public EventScript testEvent;
	public List<EventScript> events;
	public bool inMap;
    [Header("Event segments")]
    [SerializeField]
    public List<EventSegment> eventSegments;
	[Header("References")]
	public GameObject eventObject;
	public Text heading;
	public Text text;
	public GameObject choice;
	public RectTransform choiceList;
	public Image portrait;
	public Image portraitFill;
	public Image customImage;
	public GameObject continueBtn;
	public Text nameText; 
	EventScript currentEvent;
	GameObject currentPortrait;
	Bird currentBird;
	List<GameObject> portraits;
	List<Color> colors;
	EventScript nextEvent = null;
	public ShowTooltip mouseOver;
	IEnumerator coroutine;
	 [HideInInspector]
	public List<EventScript> eventsToShow;
	//List<string> texts;    
	int currentText = 0;
	bool activeChoices = false;
	bool printing = false;
	// Use this for initialization
	void Awake () {
		eventsToShow = new List<EventScript>();
		Instance = this;
	}
	void Start()
	{
		try
		{
            if (!inMap)
            {

                foreach (EventSegment segment in eventSegments)
                {
                    int id = Var.currentStageID % 1000;
                    if (id >= segment.minID && id <= segment.maxID)
                    {
                        events.AddRange(segment.eventParent.GetComponentsInChildren<EventScript>());
                        break;
                    }
                }
            }
        }
		catch
		{

		}
		if (testEvent != null)
			CreateEvent(testEvent);
		nextEvent = null;
	}


	public void ContinueBtn()
	{
		if (printing)
		{
			printing = false;
            if (currentText == currentEvent.parts.Count - 1)
            {
                CreateChoices();
                AudioControler.Instance.FadeOutBirdTalk();
            }
			return;
		}
		if (activeChoices)
			return;
	   
		currentText++;
		AudioControler.Instance.ClickSound();
		if (currentText < currentEvent.parts.Count-1)
		{
			string text = Helpers.Instance.ApplyTitle(currentBird, currentEvent.parts[currentText].text);
			if (coroutine != null)
				StopCoroutine(coroutine);
			coroutine = WaitAndPrint(text,true);
			//nameText.text = currentBird.charName;
			StartCoroutine(coroutine);
			SetPortrait(currentText);

		}
		if (currentText == currentEvent.parts.Count-1)
		{
			string text = Helpers.Instance.ApplyTitle(currentBird, currentEvent.parts[currentText].text);
			if (coroutine != null)
				StopCoroutine(coroutine);
			coroutine = WaitAndPrint(text, false);
			//nameText.text = currentBird.charName;
			StartCoroutine(coroutine);
			SetPortrait(currentText);
			return;         
		}
		
		if (currentText > currentEvent.parts.Count-1)
		{
            if (currentEvent.quitAfterLevel)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("mainMenu");
                return;
            }
            eventObject.SetActive(false);
			if(nextEvent!= null)
            {
                // currentEvent = null;
                nextEvent.canShowMultipleTimes = true;
                currentEvent = null;
                CreateEvent(nextEvent);
				return;
			}

            if (eventsToShow.Count > 0)
            {
                EventScript nextEvent = eventsToShow[0];
                eventsToShow.RemoveAt(0);
                CreateEvent(nextEvent);

            }
            else if (currentEvent.afterEventDialog != null)
            {
                DialogueControl.Instance.CreateParticularDialog(currentEvent.afterEventDialog);
            }
            else
            {
                if (inMap)
                {
                    DialogueControl.Instance.TryDialogue(Dialogue.Location.map);
                    foreach (MapIcon icon in FindObjectsOfType<MapIcon>())
                        icon.SetState();
                }
                else
                {
                    if (GuiContoler.Instance.graph.transform.localPosition.y < -500)
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

	private IEnumerator WaitAndPrint(string printText,bool shouldShowContinue)
	{

		continueBtn.GetComponent<Animator>().SetBool("active",false);
		text.text = "";
		printing = true;
		foreach (char ch in printText)
		{
			text.text += ch;
			yield return null;
			if(!printing)
			{
				text.text = printText;
				break;
			}
		}
		printing = false;
		continueBtn.GetComponent<Animator>().SetBool("active", shouldShowContinue);
		if(currentText == currentEvent.parts.Count - 1)
			CreateChoices();			
		AudioControler.Instance.FadeOutBirdTalk();
	}



	public bool tryEvent()
	{
		if (GuiContoler.Instance.winBanner != null && GuiContoler.Instance.winBanner.activeSelf)
			return false;
		if (eventObject.activeSelf)
		   return false;
		if (Var.isTutorial || Var.isEnding)
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
				if (Var.shownEvents.Contains(ev.heading))
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

		if(canCreateEvent)
			CreateEvent(ev);
		return true;
	}
	public void CreateEvent(EventScript eventData)
	{
		
		if (!eventData.canShowMultipleTimes)
		{
			Var.shownEvents.Add(eventData.heading);
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
			try
			{
				currentBird = Helpers.Instance.GetBirdFromEnum(eventData.speakers[0]);
			}
			catch { }
			//currentPortrait = currentBird.portrait;
		}
		portraits = new List<GameObject>();
		colors = new List<Color>();
		foreach(EventScript.Character Char in eventData.speakers)
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
		//nameText.text = currentBird.charName;
		if(coroutine!=null)
			StopCoroutine(coroutine);
		coroutine = WaitAndPrint(text, false);
		StartCoroutine(coroutine);
		SetPortrait(0);
	   
		if (eventData.options.Length > 0 && eventData.parts.Count <=1 )
		{
			CreateChoices();
		}else
		{
			continueBtn.GetComponent<Animator>().SetBool("active", true);
		}
	}



    void SetPortrait(int id)
	{
		if(mouseOver)
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
				//if (currentBird != null)
				//	mouseOver.tooltipText = Helpers.Instance.GetStatInfo(currentBird.data.confidence, currentBird.data.friendliness);
				portrait.transform.parent.gameObject.SetActive(true);
				portraitFill.gameObject.SetActive(true);
				portrait.gameObject.SetActive(true);
				portraitFill.sprite = portraits[currentEvent.parts[currentText].speakerId].transform.Find("bg/bird_color").GetComponent<Image>().sprite;
				portraitFill.color = colors[currentEvent.parts[currentText].speakerId];
				portrait.sprite = portraits[currentEvent.parts[currentText].speakerId].transform.Find("bg/bird").GetComponent<Image>().sprite;


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

                nameText.text=characterNameText;


                    try
                {
					Bird birdToTalk = Helpers.Instance.GetBirdFromEnum(currentEvent.speakers[currentEvent.parts[currentText].speakerId]);
					AudioControler.Instance.PlaySound(AudioControler.Instance.GetBirdSoundGroup(birdToTalk.data.charName).eventAudio);
				//AudioControler.Instance.PlaySound(Helpers.Instance.GetBirdFromEnum(currentEvent.speakers[currentEvent.parts[currentText].speakerId]).birdSounds.eventAudio);
				}catch{
					Debug.Log("audio not found!");
				}
			}
			catch
			{
				print("failed to show portrait");
				portrait.transform.parent.gameObject.SetActive(false);
			}
		}

	}

  
	void CreateChoices()
	{
		
		if (currentEvent.options.Length > 0)
		{
			int i = 0;
			choiceList.gameObject.SetActive(true);
			foreach (EventConsequence choiceData in currentEvent.options)
			{
				GameObject choiceObj = Instantiate(choice, choiceList);
				SetupChoice(choiceObj, i);
				i++;
			}
			activeChoices = true;
		}
		else
		{
			continueBtn.GetComponent<Animator>().SetBool("active", true);
			ContinueBtn();
		}
		currentText++;       
		 
	}
	void DisplayChoiceResult(int ID)
	{
		activeChoices = false;
		AudioControler.Instance.PlayPaperSound();
		choiceList.gameObject.SetActive(false);
		Helpers.Instance.HideTooltip();
		string consequences = ApplyConsequences(ID);
		if (currentBird != null)
		{
			try
			{
				currentBird.AddRoundBonuses(false);
				currentBird.showText();
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
		if(currentEvent.options[ID].AfterImage!= null)
		{
			portrait.transform.parent.gameObject.SetActive(true);
			customImage.gameObject.SetActive(true);
			portraitFill.gameObject.SetActive(false);
			portrait.gameObject.SetActive(false);
			customImage.sprite = currentEvent.options[ID].AfterImage;
		}

    }

	string ApplyConsequences(int ID)
	{
		string ConsequenceText = ApplyConsequence(currentEvent.options[ID].consequenceType1, currentEvent.options[ID].magnitude1, currentEvent.options[ID].applyToAll);
		ConsequenceText += "\n" + ApplyConsequence(currentEvent.options[ID].consequenceType2, currentEvent.options[ID].magnitude2, currentEvent.options[ID].applyToAll);
		ConsequenceText += "\n" + ApplyConsequence(currentEvent.options[ID].consequenceType3, currentEvent.options[ID].magnitude3, currentEvent.options[ID].applyToAll);
		if(currentBird!= null)
			currentBird.SetEmotion();
        GameLogic.Instance.UpdateFeedback();
        GuiContoler.Instance.GraphBlocker.SetActive(false);
		return ConsequenceText;

	}
	string ApplyConsequence(ConsequenceType type, int magnitude, bool applyToAll)
	{        
		if (currentBird == null)
			return "";
        List<Bird> birdsToApply = new List<Bird>();
        if(applyToAll)
        {
            foreach(Bird bird in Var.activeBirds)
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
            switch (type)
            {
                case ConsequenceType.Courage:
                    bird.data.confidence += magnitude;
                    bird.prevConf += magnitude;
                    if (magnitude > 0)
                    {
                        infoString += bird.charName + " gained " + magnitude + " confidence\n";
                    }
                    else
                    {
                        infoString += bird.charName + "'s caution increased by " + Mathf.Abs(magnitude) + "\n";
                    }
                    break;
                case ConsequenceType.Friendliness:
                    bird.data.friendliness += magnitude;
                    bird.prevFriend += magnitude;
                    if (magnitude > 0)
                    {
                        infoString += bird.charName + " gained " + magnitude + " social\n";
                    }
                    else
                    {
                        infoString += bird.charName + " gained " + Mathf.Abs(magnitude) + " solitude\n";
                    }
                    break;
                case ConsequenceType.Health:
                    currentBird.ChageHealth(magnitude);
                    if (magnitude > 0)
                    {
                        infoString+= bird.charName + " gained " + magnitude + " health\n";
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
	void SetupChoice(GameObject choiceObj,int ID)
	{
		EventConsequence choiceData = currentEvent.options[ID];
		choiceObj.GetComponent<Button>().onClick.AddListener(delegate { DisplayChoiceResult(ID); });
        try
        {
            choiceObj.GetComponent<ShowTooltip>().tooltipText = Helpers.Instance.ApplyTitle(currentBird, choiceData.selectionTooltip);
            choiceObj.transform.Find("Description").GetComponent<Text>().text = Helpers.Instance.ApplyTitle(currentBird, choiceData.selectionText);
        }
        catch
        {

        }
		if (choiceData.icon != null)
			choiceObj.transform.Find("Icon").GetComponent<Image>().sprite = choiceData.icon;
		else
			choiceObj.transform.Find("Icon").gameObject.SetActive(false);

	}
}
