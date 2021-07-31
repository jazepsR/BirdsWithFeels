using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapIconEvent : MapIcon
{
    public EventScript[] possibleEvents;
    private bool eventActive = false;
    public Sprite NarrativeIcon;
    public Image iconToChange;
    public bool showGraph = true;

    internal override void Start()
    {
        base.Start();

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
                    foreach (MapIcon icon in targets)
                    {
                        LeanTween.delayedCall(1.3f, () =>
                        {
                            icon.stateSet = false;
                            icon.available = true;
                            icon.SetState();
                        });
                    }
                    LeanTween.delayedCall(3f, () => SaveLoad.Save());
                }
                else
                {
                    anim.SetInteger("state", 2); //set map icon to "completed" state instantly
                }
            }
            else
            {
                float time = 2f;
                anim.SetInteger("state", 0);
                LeanTween.delayedCall(time, () => anim.SetTrigger("playUnlockAnim"));  //Set map icon to "available" state after a delay
                LeanTween.delayedCall(time, () => anim.SetInteger("state", 1));
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
        tooltipText += "\n" + levelDescription;
        return tooltipText;
    }

    public override void mapBtnClick()
    {
        if (available && !completed)
        {
            Var.currentStageID = ID;
            EventScript ev = possibleEvents[Random.Range(0, possibleEvents.Length)];
            EventController.Instance.CreateEvent(ev);
            MapControler.Instance.showGraphAfterEvent = showGraph;
            eventActive = true;
            if(timedEvent)
            {
                timedEvent.CheckIfTimedEvent();
            }
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
