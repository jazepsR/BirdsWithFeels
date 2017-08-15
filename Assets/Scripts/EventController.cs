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
	public GameObject continueBtn;
	EventScript currentEvent;
    GameObject currentPortrait;
    Bird currentBird;    
	// Use this for initialization
	void Awake () {
		Instance = this;
	}
	
	public void tryEvent()
	{
        currentBird = null;
        currentPortrait = null;
        currentEvent = null;        
        if (Random.Range(0, 1.0f) > eventFreq)
            return;
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
                    canCreateEvent = true;
                    currentBird = birdsToCheck[Random.Range(0, birdsToCheck.Count)];
                    currentPortrait = currentBird.portrait;
                    break;
                }
                foreach (Bird bird in birdsToCheck)
                {
                    if (bird.charName == ev.speaker.ToString())
                    {
                        currentPortrait = bird.portrait;
                        currentBird = bird;
                        canCreateEvent = true;
                        break;
                    }
                }

            }
        }
            if(canCreateEvent)
                CreateEvent(ev);
    }
	void CreateEvent(EventScript eventData)
	{
        if (!inMap)
        {
            GuiContoler.Instance.battlePanel.SetActive(false);
        }
		currentEvent = eventData;
        eventObject.SetActive(true);
        if (currentBird == null)
        {
            heading.text = eventData.heading;
            text.text = eventData.text;
        }
        else
        {
            heading.text = Helpers.Instance.ApplyTitle(currentBird.charName, eventData.heading);
            text.text = Helpers.Instance.ApplyTitle(currentBird.charName, eventData.text);
        }        
		continueBtn.SetActive(false);
        if (currentPortrait != null)
        {
            portrait.transform.parent.gameObject.SetActive(true);
            portraitFill.sprite = currentPortrait.transform.Find("bird_color").GetComponent<Image>().sprite;
            portrait.sprite = currentPortrait.transform.Find("bird").GetComponent<Image>().sprite;
        }else
        {
            portrait.transform.parent.gameObject.SetActive(false);
        }
		if (eventData.options.Length > 0)
		{
			int i = 0;
			foreach(EventConsequence choiceData in eventData.options)
			{
				GameObject choiceObj = Instantiate(choice, choiceList);
				SetupChoice(choiceObj,i);
				i++;
			}
		}else
		{
			continueBtn.SetActive(true);
		}
	}
	void DisplayChoiceResult(int ID)
	{
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
        if (currentBird == null)
        {
            heading.text =  currentEvent.options[ID].conclusionHeading;
            text.text =currentEvent.options[ID].conclusionText;
        }else
        {
            heading.text = Helpers.Instance.ApplyTitle(currentBird.charName, currentEvent.options[ID].conclusionHeading);
            text.text = Helpers.Instance.ApplyTitle(currentBird.charName, currentEvent.options[ID].conclusionText);
        }
        if (currentEvent.options[ID].useAutoExplanation)
            text.text += "\n" + consequences;

    }


    string ApplyConsequences(int ID)
    {
        EventConsequence choiceData = currentEvent.options[ID];
        if (currentBird == null)
            return "";
        switch (choiceData.type)
        {
            case EventConsequence.ConsequenceType.Courage:
                currentBird.confBoos += choiceData.magnitude;
                if (choiceData.magnitude > 0)
                {
                    return currentBird.charName + " gained " + choiceData.magnitude + " confidence.";
                }
                else
                {
                    return currentBird.charName + "'s fear increased by " + Mathf.Abs(choiceData.magnitude) + ".";
                }
            case EventConsequence.ConsequenceType.Friendliness:
                currentBird.friendBoost += choiceData.magnitude;
                if (choiceData.magnitude > 0)
                {
                    return currentBird.charName + "'s firendliness has increased by" + choiceData.magnitude + ".";
                }
                else
                {
                    return currentBird.charName + " gained " + Mathf.Abs(choiceData.magnitude) + " loneliness.";
                }               
            case EventConsequence.ConsequenceType.Health:
                currentBird.ChageHealth(choiceData.magnitude);
                if (choiceData.magnitude > 0)
                {
                    return currentBird.charName+" gained "+choiceData.magnitude + " health.";
                }else
                {
                    return currentBird.charName + " lost " + Mathf.Abs(choiceData.magnitude) + " health.";
                }
                
            default:
                return "";
        }
        
    }
	void SetupChoice(GameObject choiceObj,int ID)
	{
		EventConsequence choiceData = currentEvent.options[ID];
        choiceObj.GetComponent<Button>().onClick.AddListener(delegate { DisplayChoiceResult(ID); });
        if(currentBird== null)
        {
            choiceObj.GetComponent<ShowTooltip>().tooltipText = choiceData.selectionTooltip;
            choiceObj.transform.Find("Description").GetComponent<Text>().text = choiceData.selectionText;
        }
        else
        {
            choiceObj.GetComponent<ShowTooltip>().tooltipText = Helpers.Instance.ApplyTitle(currentBird.charName, choiceData.selectionTooltip);
            choiceObj.transform.Find("Description").GetComponent<Text>().text = Helpers.Instance.ApplyTitle(currentBird.charName, choiceData.selectionText);
        }
        if (choiceData.icon != null)
			choiceObj.transform.Find("Icon").GetComponent<Image>().sprite = choiceData.icon;
		else
			choiceObj.transform.Find("Icon").gameObject.SetActive(false);

	}
}
