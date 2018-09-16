using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TimedEventControl : MonoBehaviour {
	[Header("Start")]
	public string eventName;
	public MapIcon startArea;
	public EventScript startEvent;
	public int timeToComplete;
	[Header("Resolution")]
	public MapIcon endArea;
	public EventScript activationEvent;
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
		else
		{
			if (startArea.completed)
			{
				data = new TimedEventData(eventName, Var.currentWeek + timeToComplete);
				data.currentState = TimedEventData.state.active;
				Var.timedEvents.Add(data);
				EventController.Instance.CreateEvent(startEvent);
				endArea.timedEventTrigger = this;				
			}
		}
		CheckStatus();
		
	}
	

	public void CheckStatus()
	{
		if (data.currentState == TimedEventData.state.active || data.currentState == TimedEventData.state.failed)
		{
			EventNotification.transform.parent.gameObject.SetActive(true);
			EventNotification.text = (Mathf.Max(0,data.completeBy - Var.currentWeek)).ToString();
			EventNotification.transform.parent.parent = endArea.transform;
			EventNotification.transform.parent.localPosition = offset;
			if (endArea.completed)
			{
				if (Var.currentWeek< data.completeBy)
				{
					data.currentState = TimedEventData.state.completedSuccess;
					EventController.Instance.CreateEvent(completionEvent);
				}else
				{
					data.currentState = TimedEventData.state.completedFail;
					EventController.Instance.CreateEvent(completionAfterFailEvent);
				}
				EventNotification.transform.parent.gameObject.SetActive(false);
			}
			if (Var.currentWeek >= data.completeBy)
			{
				data.currentState = TimedEventData.state.failed;
				EventController.Instance.CreateEvent(initialFailEvent);
				EventNotification.transform.parent.gameObject.SetActive(false);
			}else if(endArea.available)
			{
				TriggerActivationEvent();
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
		EventController.Instance.CreateEvent(activationEvent);
		data.activationEventShown = true;
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
	public TimedEventData(string eventName, int completeBy)
	{
		this.eventName = eventName;
		this.completeBy = completeBy;
	}
}
