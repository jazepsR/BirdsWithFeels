﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimedEventControl : MonoBehaviour {
	[Header("Start")]
	public string eventName;
	public MapIcon startArea;
	//public EventScript startEvent;
	public int timeToComplete;
	[Header("Resolution")]
	public MapIcon endArea;
	public EventScript activationEvent;
	public EventScript activationEventFail;
	public EventScript completionEvent;
	[Header("failure")]
	public EventScript initialFailEvent;
	public EventScript completionAfterFailEvent;
	public Text EventNotification;
	[HideInInspector]
	public TimedEventData data = null;
	Vector3 offset = new Vector3(-95, 30f, 0);
	bool shouldTriggerBattle = false;
    // Use this for initialization
    
    void Start () {
		if (Helpers.Instance.VarContainsTimedEvent(eventName))
		{
			data = Helpers.Instance.GetTimedEvent(eventName);
		}
		if (startArea)
		{
			startArea.timedEvent = this;
		}
		if (MapControler.Instance)
		{
			CheckStatus();
		}		
	}
	
	public void CheckIfTimedEvent()
    {
		if (startArea)
		{
			data = new TimedEventData(eventName, Var.currentWeek + timeToComplete, endArea.ID);
			data.currentState = TimedEventData.state.active;
			Var.timedEvents.Add(data);
			CheckStatus();
			//EventController.Instance.CreateEvent(startEvent);
		}
	}


	public void CheckStatus()
	{
		if (data.currentState == TimedEventData.state.active || data.currentState == TimedEventData.state.failed 
			|| data.currentState == TimedEventData.state.completedFail)
		{
			if (EventNotification != null)
			{
				EventNotification.transform.parent.gameObject.SetActive(true);
				EventNotification.text = (Mathf.Max(0, data.completeBy - Var.currentWeek)).ToString();
				EventNotification.transform.parent.parent = endArea.transform;
				EventNotification.transform.parent.localPosition = offset;
			}
			if (endArea != null && endArea.completed)
			{
				if (Var.currentWeek<= data.completeBy)
				{
					data.currentState = TimedEventData.state.completedSuccess;
					
					//complete time trial in time achievements


					EventController.Instance.CreateEvent(completionEvent);
					HealAllBirds();
                    data.currentState = TimedEventData.state.notStarted;
				}else
				{
					data.currentState = TimedEventData.state.completedFail;
					EventController.Instance.CreateEvent(completionAfterFailEvent);
                    data.currentState = TimedEventData.state.notStarted;
					//HealAllBirds();
				}
				
			if(EventNotification != null)
				EventNotification.transform.parent.gameObject.SetActive(false);
			}
			if (Var.currentWeek == data.completeBy)
			{
				data.currentState = TimedEventData.state.completedFail;
				EventController.Instance.CreateEvent(initialFailEvent);
			}
			if(Var.currentWeek >= data.completeBy)
            {
				if (EventNotification != null)
					EventNotification.transform.parent.gameObject.SetActive(false);
			}

			if(endArea && endArea != null)
			{
				endArea.timedEvent = this;
			}
            foreach (MapIcon icon in FindObjectsOfType<MapIcon>())
                icon.SetState();

			
				SetupTrialUI();
		}	
	}
	private void HealAllBirds()
    {
		foreach(Bird bird in FillPlayer.Instance.playerBirds)
        {
			bird.HealFully(true);
        }

    }
	void SetupTrialUI()
	{
		if (endArea.completed)
			return;

		MapControler.Instance.trialUiObject.SetActive(true);
		MapControler.Instance.trialNameText.text = endArea.levelName;
		MapControler.Instance.trialNameText.color = Helpers.Instance.GetEmotionColor(endArea.type);
		MapControler.Instance.trialUiIcon.color = Helpers.Instance.GetEmotionColor(endArea.type);

		if(data.currentState == TimedEventData.state.failed || data.currentState == TimedEventData.state.completedFail)
        {
			MapControler.Instance.trialTooLateText.gameObject.SetActive(true);
			MapControler.Instance.trialWeeksLeftText.gameObject.SetActive(false);
        }
        else
		{
			MapControler.Instance.trialTooLateText.gameObject.SetActive(false);
			MapControler.Instance.trialWeeksLeftText.gameObject.SetActive(true);
			if (data.completeBy - Var.currentWeek == 1)
			{
				MapControler.Instance.trialWeeksLeftText.text = (Mathf.Max(0, data.completeBy - Var.currentWeek)).ToString() + " WEEK TO GO!";
				MapControler.Instance.trialWeeksLeftText.color = Helpers.Instance.GetEmotionColor(Var.Em.Confident);
			}
            else
			{
				MapControler.Instance.trialWeeksLeftText.text = (Mathf.Max(0, data.completeBy - Var.currentWeek)).ToString() + " weeks to go";
				MapControler.Instance.trialWeeksLeftText.color = Color.black;
			}
		}
}
	// Update is called once per frame
	void Update () {
		if (shouldTriggerBattle)
		{
			if (!EventController.Instance.eventObject.activeSelf)
				endArea.LoadBattleScene();
		}
	}
	public void TriggerActivationEvent()
	{
		if(!data.activationEventShown)
		{
			if (Var.currentWeek >= data.completeBy)
			{
				EventController.Instance.CreateEvent(activationEventFail);
			}
			else
			{
				EventController.Instance.CreateEvent(activationEvent);
			}
			data.activationEventShown = true;
		}
		//shouldTriggerBattle = true;

	}
}

[Serializable]
public class TimedEventData
{
	public string eventName;
	public int completeBy;
	public enum state { notStarted,active,failed,completedSuccess,completedFail};
	public state currentState = state.notStarted;
	public bool activationEventShown = false;
	public int activationEventID = 0;
	public TimedEventData(string eventName, int completeBy, int activationEventID)
	{
		this.eventName = eventName;
		this.completeBy = completeBy;
		this.activationEventID = activationEventID;
	}
}
