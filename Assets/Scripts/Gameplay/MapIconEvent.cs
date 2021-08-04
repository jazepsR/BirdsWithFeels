using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapIconEvent : MapIcon
{
    public List<EventScript> possibleEvents;
    private bool eventActive = false;
    public Sprite NarrativeIcon;
    public Image iconToChange;
    public bool showGraph = true;
    public string headingName = "Narrative Event";
    public string description = "";

    internal override void Start()
    {
        base.Start();

        levelName = headingName;
        levelDescription = description;

        if (iconToChange != null && NarrativeIcon != null)
        {
            iconToChange.sprite = NarrativeIcon;
        }
    }

    internal override void Update()
    {
        if (eventActive && !EventController.Instance.eventObject.activeSelf)
        {
            completed = true;
            firstCompletion = true;
            eventActive = false;
            tooltipInfo.tooltipText = GetTooltipText();
            SetState();
        }
    }
    public override void SetState()
    {
        if (available && !stateSet)
        {
            ExcelExport.CreateExportTable();
            ExcelExport.AddMapNode(this);
            //Debug.LogError("SETTING STATE! ID: "+ ID);
            if (completed)
            {
                if (firstCompletion) //Stuff that happens the FIRST time user completes level
                {
                    stateSet = true;
                    firstCompletion = false;
                    anim.SetInteger("state", 1);
                    float time = 0.8f;
                    LeanTween.delayedCall(time, () => anim.SetTrigger("playCompleteAnim")); //Delay b4 playing unlock anim
                    LeanTween.delayedCall(time, () => anim.SetInteger("state", 2));
                    AudioControler.Instance.PlaySound(AudioControler.Instance.mapNodeClick);
                    if (unlockedRoad != null)
                    {
                        unlockedRoad.gameObject.SetActive(false);
                        LeanTween.delayedCall(1.7f, () => unlockedRoad.gameObject.GetComponent<Animator>().SetBool("new", true));
                        LeanTween.delayedCall(1.7f, () => unlockedRoad.gameObject.SetActive(true));
                    }
                    LeanTween.delayedCall(1.3f, () =>
                    {
                    foreach (MapIcon icon in targets)
                    {
                        
                            icon.stateSet = false;
                            icon.available = true;
                            icon.SetState();
                            icon.TryCreateNewSave();
                    }
                    TryCreateNewSave();
                    SaveLoad.Save();
                    });
                    tooltipInfo.tooltipText = GetTooltipText();
                }
                else
                {
                    anim.SetInteger("state", 2); //set map icon to "completed" state instantly
                    tooltipInfo.tooltipText = GetTooltipText();
                }
            }
            else
            {
                float time = 2f;
                anim.SetInteger("state", 0);
                LeanTween.delayedCall(time, () => anim.SetTrigger("playUnlockAnim"));  //Set map icon to "available" state after a delay
                LeanTween.delayedCall(time, () => anim.SetInteger("state", 1));
                tooltipInfo.tooltipText = GetTooltipText();
            }
        }
        else
        {
            anim.SetInteger("state", 0); //Set map icon to "locked" state
        }
    }
    internal override string GetTooltipText()
    {
        string tooltipText = "<b>" + levelName + "</b>";
        if (!available)
        {
            tooltipText += " -locked";
        }
        if (completed)
        {
            tooltipText += "<color=#E7CA21ff> -completed</color>";
        }
        tooltipText += "\n" + levelDescription +"\nThis node will not take a week";
        return tooltipText;
    }

    public override void mapBtnClick()
    {
        if (available && !completed)
        {
            Var.currentStageID = ID;
            EventController.Instance.CreateEvent(GetEventToDisplay());
            MapControler.Instance.showGraphAfterEvent = showGraph;
            eventActive = true;
            if(timedEvent)
            {
                timedEvent.CheckIfTimedEvent();
            }
        }
    }

    private EventScript GetEventToDisplay()
    {
        bool eventFound = false;
        EventScript ev= null;
        while (possibleEvents.Count>0 && !eventFound)
        {
            int foundEvent = Random.Range(0, possibleEvents.Count);
            ev = possibleEvents[foundEvent];
            if (ev.gameObject.name != "" && !Var.shownEvents.Contains(ev.gameObject.name))
            {
                eventFound = true;
            }
            possibleEvents.RemoveAt(foundEvent);
        }
        if(eventFound)
        {
            return ev;
        }
        else
        {
            Debug.LogError("Had to use event node fallback event!");
            return EventController.Instance.defaultMapEvent;
        }
    }

    public override string ToString()
    {
        return "Im an event node string!";
    }

    internal override void Validate(int id, MapBattleData data)
    {

    }

}
