using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour {
	public enum Location { map, graph, battle }; 
	[Header("Condition checked for first bird")]
	public ConditionCheck.Condition condition = ConditionCheck.Condition.none;
	public int magnitude;
	public Var.Em targetEmotion;
	public bool canUseRandomBirds = false;
	public Location location = Location.battle;
	public List<EventScript.Character> speakers;
	public bool canShowMultipleTimes = false;
	[HideInInspector]
	public List<DialoguePart> dialogueParts;    
	// Use this for initialization
	void Awake () {
	   dialogueParts =  new List<DialoguePart>(transform.GetComponentsInChildren<DialoguePart>());
	}
	
}
