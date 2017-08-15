using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventScript:MonoBehaviour{
    public enum Character  { Terry,Rebecca,Tova,Kim,Toby,Random,None};
    public Character speaker;
    [TextArea(3, 10)]
    public string heading;
    [TextArea(3, 20)]
    public string text;
    public EventConsequence[] options;
   
}
