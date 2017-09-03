using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EmotionBarScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	string available = "Once the birds emotions are this strong, a new level will be available";
	string completed = "This bird can level up!";
	string unavailable = "This bird must complete more battles before it can level up";
	public enum state {available, completed, unavailable };
	public state currentState = state.unavailable;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
       // Debug.Log("pointer enter");
		string tooltipText = "";
		switch (currentState)
		{
			case state.available:
				tooltipText = available;
				break;
			case state.completed:
				tooltipText = completed;
				break;
			case state.unavailable:
				tooltipText = unavailable;
				break;
		}


		Helpers.Instance.ShowTooltip(tooltipText);
	}


	public void OnPointerExit(PointerEventData eventData)
	{
       // Debug.Log("pointer exit");
        Helpers.Instance.HideTooltip();
	}
	}
