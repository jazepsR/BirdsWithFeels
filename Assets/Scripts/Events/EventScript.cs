using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventScript:MonoBehaviour{
	public enum Character  {
        Terry,
        Rebecca,
        Sophie,
        Kim,
        Alexander,
        Random,
        None,
        The_Queen,
        The_Vulture_King,
        A_vulture,
        A_bird,
        player,
        A_vulture2,
        Vulture_King0_1neutral,
        Vulture_King0_2angry,
        Vulture_King0_3coy,
        Vulture_King0_4lookAtPlayer,
        Vulture_King0_5Solitary,
        Vulture_King0_6Social,
        Vulture_King0_7Confident,
        Vulture_King0_8Cautious,
        Terrys_Dad,
        Rebeccas_Mom,
        Percy,
        Auntie_Judy,
        Jerry,
        Anna_Penelope,
        Emma,
        Jennifer,
        Vulture_Stranger,
        Librarian,
        Military_leader,
        Military_Bird,
        Another_Military_bird,
        Bird_adventurer,
        Bird_traveler,
        Another_Bird_traveler,
        SadBird,
        Another_vulture,
        Merchant,
        Aggressive_bird,
        Injured_Vulture,
        Messenger,
        You,
        The_Queen0_sneaky,
        The_Queen0_angry,
        Percy0_Love,
        Cousin_Adelaide,
        A_vulture0_3,
    };
	public List<Character> speakers;
	public ConditionCheck.Condition condition;
	public int magnitude = 0;
	public Var.Em targetEmotion;
	[TextArea(1, 3)]
	public string heading;
	[HideInInspector]
	[SerializeField]
	public List<EventPart> parts;
	public EventConsequence[] options;
	public bool canShowMultipleTimes = false;
	public Dialogue afterEventDialog;
    public bool quitAfterLevel = false;
    public Sprite eventBackground;
    public bool useBgFog = true;
    public bool useEventAudio;
    public AudioGroup eventAudio;
    public EventScript()
	{

	}
   
  
    public void Awake()
	{
		if(Application.isEditor)
		{
			EventPart[] parts = transform.GetComponentsInChildren<EventPart>();
			foreach(EventPart part in parts)
			{
				if(part.text.Length> Var.eventTextCharLimit)
				{
					Debug.LogWarning("Event \"" + name +"\" part " + part.name+ " text might be too long to display correctly ("+ part.text.Length+" characters)");
				}
			}
		}
	}
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
