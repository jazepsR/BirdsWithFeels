using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript:MonoBehaviour{
    public enum Character  { Terry,Rebecca,Tova,Kim,Toby,Random,None, the_Queen, the_Vulture_King, a_vulture, a_bird };
    public List<Character> speakers;
    public ConditionCheck.Condition condition;
    public int magnitude = 0;
    public Var.Em targetEmotion;
    public bool useCustomPic = false;
    public Sprite customPic;
    [TextArea(1, 3)]
    public string heading;
    public int speaker1Id = 0;
    [TextArea(3, 20)]
    public string text1;
    public int speaker2Id = 0;
    [TextArea(3, 20)]
    public string text2;
    public int speaker3Id = 0;
    [TextArea(3, 20)]
    public string text3;
    public int speaker4Id = 0;
    [TextArea(3, 20)]
    public string text4;
    public int speaker5Id = 0;
    [TextArea(3, 20)]
    public string text5;
    public EventConsequence[] options;
    public bool canShowMultipleTimes = false;
    public EventScript(Character speaker, string heading, string text1, string text2="",string text3 = "",string text4 ="",string text5= "")
    {
        this.speakers = new List<Character>() { speaker };
        this.heading = heading;
        this.text1 = text1;
        this.text2 = text2;
        this.text3 = text3;
        this.text4 = text4;
        this.text5 = text5;
        options = new EventConsequence[0];
        condition = ConditionCheck.Condition.none;
        canShowMultipleTimes = true;
    }
}
