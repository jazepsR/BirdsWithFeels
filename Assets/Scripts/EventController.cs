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
	EventScript currentEvent;
	GameObject currentPortrait;
	Bird currentBird;
	public List<Transform> areaDialogues;
	List<GameObject> portraits;
	List<string> texts;    
	int currentText = 0;
	// Use this for initialization
	void Awake () {
		Instance = this;
	}
	void Start()
	{
		
	}
	public void ContinueBtn()
	{
		try
		{
			events.AddRange(areaDialogues[Var.currentBG].GetComponentsInChildren<EventScript>());
		}
		catch
		{

		}
		currentText++;
		AudioControler.Instance.ClickSound();
		if (currentText < texts.Count)
		{
			text.text = Helpers.Instance.ApplyTitle(currentBird, texts[currentText]);
			SetPortrait(currentText);

		}
		if (currentText == texts.Count)
		{
			continueBtn.SetActive(false);
			CreateChoices();
            SetPortrait(0);
            return;         
		}
		if (currentText > texts.Count)
		{
			eventObject.SetActive(false);
			if (inMap)
			{
				DialogueControl.Instance.TryDialogue(Dialogue.Location.map);
			}
			else
			{
				if (GuiContoler.Instance.graph.transform.localPosition.x<-1000)
				{
					GuiContoler.Instance.battlePanel.SetActive(true);
					DialogueControl.Instance.TryDialogue(Dialogue.Location.battle);
				}
			}
		}
		
	   
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
				if (Var.shownEvents.Contains(ev.text1))
					canCreateEvent = false;
				if (ev.speakers.Contains(EventScript.Character.Toby) && Var.availableBirds.Count < 4)
					canCreateEvent = false;
				if (ev.speakers.Contains(EventScript.Character.Tova) && Var.availableBirds.Count < 5)
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
			Var.shownEvents.Add(eventData.text1);
		}
		choiceList.gameObject.SetActive(false);
		currentText = 0;
		if (!inMap)
		{
			GuiContoler.Instance.battlePanel.SetActive(false);
		}
		currentEvent = eventData;
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
		foreach(EventScript.Character Char in eventData.speakers)
		{
			portraits.Add(Helpers.Instance.GetPortrait(Char));
		}


		eventObject.SetActive(true);
		texts = new List<string>();
		texts.Add(eventData.text1);
		if (eventData.text2 != "")
			texts.Add(eventData.text2);
		if (eventData.text3 != "")
			texts.Add(eventData.text3);
		if (eventData.text4 != "")
			texts.Add(eventData.text4);
		if (eventData.text5 != "")
			texts.Add(eventData.text5);       
		heading.text = Helpers.Instance.ApplyTitle(currentBird, eventData.heading);
		text.text = Helpers.Instance.ApplyTitle(currentBird, eventData.text1);             
		continueBtn.SetActive(false);
		SetPortrait(0);
	   
		if (eventData.options.Length > 0 && eventData.text2 == "" )
		{
			CreateChoices();
		}else
		{
			continueBtn.SetActive(true);
		}
	}


	void SetPortrait(int id)
	{
		
		if (currentEvent.useCustomPic && currentEvent.customPic != null)
		{
			portrait.transform.parent.gameObject.SetActive(true);
			customImage.gameObject.SetActive(true);
			portraitFill.gameObject.SetActive(false);
			portrait.gameObject.SetActive(false);
			customImage.sprite = currentEvent.customPic;
		}
		else
		{
			customImage.gameObject.SetActive(false);
			try
			{
				portrait.transform.parent.gameObject.SetActive(true);
				portraitFill.gameObject.SetActive(true);
				portrait.gameObject.SetActive(true);
				portraitFill.sprite = portraits[id].transform.Find("bird_color").GetComponent<Image>().sprite;
				portraitFill.color = Helpers.Instance.GetEmotionColor(Helpers.Instance.RandomEmotion());
				portrait.sprite = portraits[id].transform.Find("bird").GetComponent<Image>().sprite;
			}
			catch
			{
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
		}
		else
		{
			continueBtn.SetActive(true);
			ContinueBtn();
		}
		currentText++;       
		 
	}
	void DisplayChoiceResult(int ID)
	{
		AudioControler.Instance.PlayPaperSound();
		choiceList.gameObject.SetActive(false);
		Helpers.Instance.HideTooltip();
		string consequences = ApplyConsequences(ID);
		if (currentBird != null)
		{
			currentBird.AddRoundBonuses(false);
			currentBird.showText();
		}
		foreach (Transform child in choiceList)
		{
			Destroy(child.gameObject);
		}
		continueBtn.SetActive(true);      
		heading.text = Helpers.Instance.ApplyTitle(currentBird, currentEvent.options[ID].conclusionHeading);
		text.text = Helpers.Instance.ApplyTitle(currentBird, currentEvent.options[ID].conclusionText);
		if(currentEvent.options[ID].AfterImage!= null)
		{
			portrait.transform.parent.gameObject.SetActive(true);
			customImage.gameObject.SetActive(true);
			portraitFill.gameObject.SetActive(false);
			portrait.gameObject.SetActive(false);
			customImage.sprite = currentEvent.options[ID].AfterImage;
		}
		if (currentEvent.options[ID].useAutoExplanation)
			text.text += "\n" + consequences;

	}

	string ApplyConsequences(int ID)
	{
		string ConsequenceText = ApplyConsequence(currentEvent.options[ID].consequenceType1, currentEvent.options[ID].magnitude1);
		ConsequenceText += "\n" + ApplyConsequence(currentEvent.options[ID].consequenceType2, currentEvent.options[ID].magnitude2);
		ConsequenceText += "\n" + ApplyConsequence(currentEvent.options[ID].consequenceType3, currentEvent.options[ID].magnitude3);
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
				currentBird.confidence += magnitude;
				if (magnitude > 0)
				{
					return currentBird.charName + " gained " + magnitude + " confidence.";
				}
				else
				{
					return currentBird.charName + "'s fear increased by " + Mathf.Abs(magnitude) + ".";
				}
			case EventConsequence.ConsequenceType.Friendliness:
				currentBird.friendliness += magnitude;
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
