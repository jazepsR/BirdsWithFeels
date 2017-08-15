using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventConsequence :MonoBehaviour{
    public ConsequenceType type;
    public int magnitude;
    public bool useAutoExplanation;
    public Sprite icon;
    [TextArea(3, 10)]
    public string selectionText;
    [TextArea(3, 10)]
    public string selectionTooltip;    
    
    [TextArea(3, 10)]
    public string conclusionHeading;
    [TextArea(3, 20)]
    public string conclusionText;
   
    
	public enum ConsequenceType { Health,Friendliness,Courage,Nothing};
	

}
