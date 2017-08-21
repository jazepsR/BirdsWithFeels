using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventController : MonoBehaviour {	
	[Range(0.0f, 1.0f)]
	public float eventFreq;
	public static EventController Instance { get; private set; }
	public EventScript testEvent;
	public EventScript[] events;
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
		currentText++;
		if (currentText < texts.Count)
			text.text = Helpers.Instance.ApplyTitle(currentBird,texts[currentText]);
		if (currentText == texts.Count)
		{
			continueBtn.SetActive(false);
			CreateChoices();
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
		EventScript ev = events[Random.Range(0, events.Length)];
		for (int i = 0; i < 10; i++)
		{


			if (canCreateEvent)
				break;
			if (testEvent != null)
				ev = testEvent;
			else
				ev = events[Random.Range(0, events.Length)];
			if (ev != null)
			{
				if (ev.speaker == EventScript.Character.None)
				{
					canCreateEvent = true;
					break;
				}
				if (ev.speaker == EventScript.Character.Random)
				{					
					Bird tempBird = birdsToCheck[Random.Range(0, birdsToCheck.Count)];
					if (ConditionCheck.CheckCondition(ev.condition, tempBird, ev.targetEmotion, ev.magnitude))
					{
						canCreateEvent = true;
						currentBird = tempBird;
						currentPortrait = currentBird.portrait;
						break;
					}
				}
				foreach (Bird bird in birdsToCheck)
				{
					if (bird.charName == ev.speaker.ToString())
					{

						if (ConditionCheck.CheckCondition(ev.condition, bird, ev.targetEmotion, ev.magnitude))
						{
							currentBird = bird;
							currentPortrait = bird.portrait;
							canCreateEvent = true;
							break;
						}
					}
				}

			}
		}
		if(canCreateEvent)
			CreateEvent(ev);
		return true;
	}
	public void CreateEvent(EventScript eventData)
	{
		choiceList.gameObject.SetActive(false);
		currentText = 0;
		if (!inMap)
		{
			GuiContoler.Instance.battlePanel.SetActive(false);
		}
		currentEvent = eventData;
		if (currentBird == null && eventData.speaker!= EventScript.Character.None)
		{
			currentBird = Helpers.Instance.GetBirdFromEnum(eventData.speaker);
			currentPortrait = currentBird.portrait;

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
		if(eventData.useCustomPic && eventData.customPic != null)
		{
			portrait.transform.parent.gameObject.SetActive(true);
			customImage.gameObject.SetActive(true);
			portraitFill.gameObject.SetActive(false);
			portrait.gameObject.SetActive(false);
			customImage.sprite = eventData.customPic;
		}
		else
		{
			customImage.gameObject.SetActive(false);
			if (currentPortrait != null)
			{
				portrait.transform.parent.gameObject.SetActive(true);
				portraitFill.gameObject.SetActive(true);
				portrait.gameObject.SetActive(true);
				portraitFill.sprite = currentPortrait.transform.Find("bird_color").GetComponent<Image>().sprite;
				portraitFill.color = Helpers.Instance.GetEmotionColor(currentBird.emotion);
				portrait.sprite = currentPortrait.transform.Find("bird").GetComponent<Image>().sprite;
			}
			else
			{
				portrait.transform.parent.gameObject.SetActive(false);
			}
		}
	   
		if (eventData.options.Length > 0 && eventData.text2 == "" )
		{
			CreateChoices();
		}else
		{
			continueBtn.SetActive(true);
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
		
		if (currentEvent.options[ID].useAutoExplanation)
			text.text += "\n" + consequences;

	}

	string ApplyConsequences(int ID)
	{
		string ConsequenceText = ApplyConsequence(currentEvent.options[ID].consequenceType1, currentEvent.options[ID].magnitude1);
		ConsequenceText += "\n" + ApplyConsequence(currentEvent.options[ID].consequenceType2, currentEvent.options[ID].magnitude2);
		ConsequenceText += "\n" + ApplyConsequence(currentEvent.options[ID].consequenceType3, currentEvent.options[ID].magnitude3);
		return ConsequenceText;

	}
	string ApplyConsequence(EventConsequence.ConsequenceType type, int magnitude)
	{        
		if (currentBird == null)
			return "";
		switch (type)
		{
			case EventConsequence.ConsequenceType.Courage:
				currentBird.confBoos += magnitude;
				if (magnitude > 0)
				{
					return currentBird.charName + " gained " + magnitude + " confidence.";
				}
				else
				{
					return currentBird.charName + "'s fear increased by " + Mathf.Abs(magnitude) + ".";
				}
			case EventConsequence.ConsequenceType.Friendliness:
				currentBird.friendBoost += magnitude;
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
