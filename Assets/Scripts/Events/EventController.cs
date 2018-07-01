using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour {	
	[Range(0.0f, 1.0f)]
	public float eventFreq;
	public static EventController Instance { get; private set; }
	public EventScript testEvent;
	public List<EventScript> events;
	public bool inMap;
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
	public List<Transform> areaEvents;
	public List<int> areaStartPoints;
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
				int area = -1;
				foreach (int id in areaStartPoints)
				{
					if (Var.currentStageID < id)
						break;
					else
						area++;
				}


				events.AddRange(areaEvents[area].GetComponentsInChildren<EventScript>());
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
		if (activeChoices)
			return;
		currentText++;
		print("currentText: " + currentText);
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
			CreateChoices();
			return;         
		}
		if (currentText > currentEvent.parts.Count-1)
		{
			eventObject.SetActive(false);
			if(nextEvent!= null)
			{
				CreateEvent(nextEvent);
				return;
			}

			if (eventsToShow.Count>0)
			{
				EventScript nextEvent = eventsToShow[0];
				eventsToShow.RemoveAt(0);
				CreateEvent(nextEvent);

			}
			else if (currentEvent.afterEventDialog != null)
				DialogueControl.Instance.CreateParticularDialog(currentEvent.afterEventDialog);
			else
			{
				if (inMap)
				{
					DialogueControl.Instance.TryDialogue(Dialogue.Location.map);
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
		foreach (char ch in printText)
		{
			text.text += ch;
			yield return null;
		}
		continueBtn.GetComponent<Animator>().SetBool("active", shouldShowContinue);
	}



	public bool tryEvent()
	{
		if (GuiContoler.Instance.winBanner != null && GuiContoler.Instance.winBanner.activeSelf)
			return false;
		if (eventObject.activeSelf)
		   return false;
		if (Var.isTutorial)
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
		mouseOver.tooltipText = "";
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
				if (currentBird != null)
					mouseOver.tooltipText = Helpers.Instance.GetStatInfo(currentBird.data.confidence, currentBird.data.friendliness);
				portrait.transform.parent.gameObject.SetActive(true);
				portraitFill.gameObject.SetActive(true);
				portrait.gameObject.SetActive(true);
				portraitFill.sprite = portraits[currentEvent.parts[currentText].speakerId].transform.Find("bird_color").GetComponent<Image>().sprite;
				portraitFill.color = colors[currentEvent.parts[currentText].speakerId];
				portrait.sprite = portraits[currentEvent.parts[currentText].speakerId].transform.Find("bird").GetComponent<Image>().sprite;
				nameText.text = currentEvent.speakers[currentEvent.parts[currentText].speakerId].ToString().Replace('_', ' ');


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
		string ConsequenceText = ApplyConsequence(currentEvent.options[ID].consequenceType1, currentEvent.options[ID].magnitude1);
		ConsequenceText += "\n" + ApplyConsequence(currentEvent.options[ID].consequenceType2, currentEvent.options[ID].magnitude2);
		ConsequenceText += "\n" + ApplyConsequence(currentEvent.options[ID].consequenceType3, currentEvent.options[ID].magnitude3);
		if(currentBird!= null)
			currentBird.SetEmotion();        
		return ConsequenceText;

	}
	string ApplyConsequence(EventConsequence.ConsequenceType type, int magnitude)
	{        
		if (currentBird == null)
			return "";
		switch (type)
		{
			case EventConsequence.ConsequenceType.Courage:
				currentBird.data.confidence += magnitude;
				if (magnitude > 0)
				{
					return currentBird.charName + " gained " + magnitude + " confidence.";
				}
				else
				{
					return currentBird.charName + "'s fear increased by " + Mathf.Abs(magnitude) + ".";
				}
			case EventConsequence.ConsequenceType.Friendliness:
				currentBird.data.friendliness += magnitude;
				if (magnitude > 0)
				{
					return currentBird.charName + "'s firendliness has increased by " + magnitude + ".";
				}
				else
				{
					return currentBird.charName + " gained " + Mathf.Abs(magnitude) + " loneliness.";
				}               
			case EventConsequence.ConsequenceType.Health:
				currentBird.ChageHealth(magnitude);
				if (magnitude > 0)
				{
					return currentBird.charName+" gained "+ magnitude + " health.";
				}else
				{
					return currentBird.charName + " lost " + Mathf.Abs(magnitude) + " health.";
				}
				
			default:
				return "";
		}
		
	}
	void SetupChoice(GameObject choiceObj,int ID)
	{
		EventConsequence choiceData = currentEvent.options[ID];
		choiceObj.GetComponent<Button>().onClick.AddListener(delegate { DisplayChoiceResult(ID); });        
		choiceObj.GetComponent<ShowTooltip>().tooltipText = Helpers.Instance.ApplyTitle(currentBird, choiceData.selectionTooltip);
		choiceObj.transform.Find("Description").GetComponent<Text>().text = Helpers.Instance.ApplyTitle(currentBird, choiceData.selectionText);
		
		if (choiceData.icon != null)
			choiceObj.transform.Find("Icon").GetComponent<Image>().sprite = choiceData.icon;
		else
			choiceObj.transform.Find("Icon").gameObject.SetActive(false);

	}
}
