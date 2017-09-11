using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript:MonoBehaviour{
    public enum Character  { Terry,Rebecca,Sophie,Kim,Alexander,Random,None, the_Queen, the_Vulture_King, a_vulture, a_bird, player };
    public List<Character> speakers;
    public ConditionCheck.Condition condition;
    public int magnitude = 0;
    public Var.Em targetEmotion;
    [TextArea(1, 3)]
    public string heading;
    [HideInInspector]
    public List<EventPart> parts;
    public EventConsequence[] options;
    public bool canShowMultipleTimes = false;
    public EventScript(Character speaker, string heading, string text1, string text2="",string text3 = "",string text4 ="",string text5= "")
    {
        this.speakers = new List<Character>() { speaker };
        this.heading = heading;
        /*this.text1 = text1;
        this.text2 = text2;
        this.text3 = text3;
        this.text4 = text4;
        this.text5 = text5;*/
        options = new EventConsequence[0];
        condition = ConditionCheck.Condition.none;
        canShowMultipleTimes = true;
    }
}
