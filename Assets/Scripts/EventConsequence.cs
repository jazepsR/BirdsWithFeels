using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventConsequence :MonoBehaviour{
	public ConsequenceType consequenceType1 = ConsequenceType.Nothing;
	public int magnitude1;
	public ConsequenceType consequenceType2 = ConsequenceType.Nothing;
	public int magnitude2;
	public ConsequenceType consequenceType3 = ConsequenceType.Nothing;
	public int magnitude3;
	public bool useAutoExplanation;
    public Sprite AfterImage = null;
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
