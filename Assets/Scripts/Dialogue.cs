using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour {
    public enum Location { map, graph, battle }; 
    public ConditionCheck.Condition condition = ConditionCheck.Condition.none;
    public Location location = Location.battle;
    [HideInInspector]
    public List<DialoguePart> dialogueParts;    
	// Use this for initialization
	void Awake () {
       dialogueParts =  new List<DialoguePart>(transform.GetComponentsInChildren<DialoguePart>());
	}
	
}
