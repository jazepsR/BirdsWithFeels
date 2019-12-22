using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class birdEnding : MonoBehaviour {

	public EventScript.Character Bird;
    [Header("Name must match the one in the map scene")]
    public string timedEventName;
    [Header("Testing - activated by main script")]
	public bool completedTimedEvent = false;
	[Header("0 = default 1= first emotion 2= second emotion")]
	public int developementType = 0;
	[Header("Timed event texts")]
	[TextArea(3, 10)]
	public string completedTimedEventText;
	public Sprite completedTimedEventImage;
	[TextArea(3, 10)]
	public string failedTimedEvent;
    public Sprite failedTimedEventImage;
    /*
	[Header("Character developement")]
	public Var.Em EmotionOne;
	public Var.Em EmotionTwo;
	[TextArea(3, 10)]
	public string EmotionOneDominantText;
	public Sprite EmotionOneDominantImage;
	[TextArea(3, 10)]
	public string EmotionTwoDominantText;
	public Sprite EmotionTwoDominantImage;
	[TextArea(3, 10)]
	public string NoEmotionDominantText;
	public Sprite NoEmotionDominantImage;
	*/
}
