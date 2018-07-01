using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ending : MonoBehaviour {
	[Header("Testing")]
	public bool isDebug = false;
	public bool finalChoice = true;
	[Header("0 = bad, 1= normal 2=good")]
	public int endingGoodnes = 0;
	[Header("Data")]
	public int goodEndingTime;
	public int normalEndingTime;
	[Header("Texts")]
	[TextArea(3, 10)]
	public string goodEndingText;
	public Sprite goodEndingImage;
	[TextArea(3, 10)]
	public string normalEndingText;
	public Sprite normalEndingImage;
	[TextArea(3, 10)]
	public string badEndingText;
	public Sprite badEndingImage;
	[TextArea(3, 10)]
	public string finalChoiceYesText;
	public Sprite finalChoiceYesImage;
	[TextArea(3, 10)]
	public string finalChoiceNoText;
	public Sprite finalChoiceNoImage;
	[Header("bird specific texts")]
	public birdEnding[] birdEndings;
	// Use this for initialization
	void Start () {
		
	}
	public void BuildCutscene(cutScene endCutscene)
	{
		if (isDebug)
			BuildCutsceneDebug(endCutscene);
		else
			BuildCutsceneReal(endCutscene);

		

	}
	public void BuildCutsceneDebug(cutScene endCutscene)
	{
		//Timed completion
		if (endingGoodnes== 2)
		{
			cutscenePart part = new cutscenePart(goodEndingImage, goodEndingText);
			endCutscene.parts.Add(part);
		}
		else if (endingGoodnes == 1)
		{
			cutscenePart part = new cutscenePart(normalEndingImage, normalEndingText);
			endCutscene.parts.Add(part);
		}
		else
		{
			cutscenePart part = new cutscenePart(normalEndingImage, normalEndingText);
			endCutscene.parts.Add(part);
		}
		//final choice
		if (finalChoice)
		{
			cutscenePart part = new cutscenePart(finalChoiceYesImage, finalChoiceYesText);
			endCutscene.parts.Add(part);
		}
		else
		{
			cutscenePart part = new cutscenePart(finalChoiceNoImage, finalChoiceNoText);
			endCutscene.parts.Add(part);
		}
		foreach (birdEnding data in birdEndings)
		{   //Result of timed event
			if (data.completedTimedEvent)
			{
				cutscenePart part = new cutscenePart(data.completedTimedEventImage, data.completedTimedEventText);
				endCutscene.parts.Add(part);
			}
			else
			{
				cutscenePart part = new cutscenePart(data.failedTimedEventImage, data.failedTimedEvent);
				endCutscene.parts.Add(part);
			}
			//Emotional developement
			if (data.developementType == 0)
			{
				cutscenePart part = new cutscenePart(data.NoEmotionDominantImage, data.NoEmotionDominantText);
				endCutscene.parts.Add(part);

			}
			else if (data.developementType == 1)
			{
				cutscenePart part = new cutscenePart(data.EmotionOneDominantImage, data.EmotionOneDominantText);
				endCutscene.parts.Add(part);
			}
			else
			{
				cutscenePart part = new cutscenePart(data.EmotionTwoDominantImage, data.EmotionTwoDominantText);
				endCutscene.parts.Add(part);
			}
		}
	}
	public void BuildCutsceneReal(cutScene endCutscene)
	{
		//Timed completion
		if(Var.currentWeek<= goodEndingTime)
		{
			cutscenePart part = new cutscenePart(goodEndingImage, goodEndingText);
			endCutscene.parts.Add(part);
		}
		else if(Var.currentWeek <= normalEndingTime)
		{
			cutscenePart part = new cutscenePart(normalEndingImage, normalEndingText);
			endCutscene.parts.Add(part);
		}
		else
		{
			cutscenePart part = new cutscenePart(normalEndingImage, normalEndingText);
			endCutscene.parts.Add(part);
		}
		//final choice
		if (Var.yesFinalChoice)
		{
			cutscenePart part = new cutscenePart(finalChoiceYesImage, finalChoiceYesText);
			endCutscene.parts.Add(part);
		}
		else
		{
			cutscenePart part = new cutscenePart(finalChoiceNoImage, finalChoiceNoText);
			endCutscene.parts.Add(part);
		}

		foreach(birdEnding data in birdEndings)
		{
			TimedEventData timedEventData = Helpers.Instance.GetTimedEvent(data.Bird.ToString());
			//Result of timed event
			if(timedEventData.currentState == TimedEventData.state.completed)
			{
				cutscenePart part = new cutscenePart(data.completedTimedEventImage, data.completedTimedEventText);
				endCutscene.parts.Add(part);
			}else
			{
				cutscenePart part = new cutscenePart(data.failedTimedEventImage, data.failedTimedEvent);
				endCutscene.parts.Add(part);
			}
			//Emotional developement
			Bird bird = Helpers.Instance.GetBirdFromEnum(data.Bird);
			int firstEmotionLvlCount = 0;
			int secondEmotionLvlCount = 0;
			foreach(LevelData lvl in bird.data.levelList)
			{
				if (lvl.emotion == data.EmotionOne)
					firstEmotionLvlCount++;
				if (lvl.emotion == data.EmotionTwo)
					secondEmotionLvlCount++;
			}
			if(firstEmotionLvlCount> 0 && secondEmotionLvlCount > 0 && firstEmotionLvlCount== secondEmotionLvlCount)
			{
				if(GetTotalEmotionVal(bird, data.EmotionOne) >= GetTotalEmotionVal(bird, data.EmotionTwo))
				{
					cutscenePart part = new cutscenePart(data.EmotionOneDominantImage, data.EmotionOneDominantText);
					endCutscene.parts.Add(part);
				}
				else
				{
					cutscenePart part = new cutscenePart(data.EmotionTwoDominantImage, data.EmotionTwoDominantText);
					endCutscene.parts.Add(part);
				}
				
			}
			else if (firstEmotionLvlCount > 0)
			{
				cutscenePart part = new cutscenePart(data.EmotionOneDominantImage, data.EmotionOneDominantText);
				endCutscene.parts.Add(part);
			}
			else if(secondEmotionLvlCount > 0)
			{
				cutscenePart part = new cutscenePart(data.EmotionTwoDominantImage, data.EmotionTwoDominantText);
				endCutscene.parts.Add(part);
			}
			else
			{
				cutscenePart part = new cutscenePart(data.NoEmotionDominantImage, data.NoEmotionDominantText);
				endCutscene.parts.Add(part);
			}



		}

	}
	int GetTotalEmotionVal(Bird bird, Var.Em emotion)
	{
		switch (emotion)
		{
			case Var.Em.Confident:
				return bird.totalConfidence;
			case Var.Em.Social:
				return bird.totalFriendliness;
			case Var.Em.Solitary:
				return -bird.totalFriendliness;
			case Var.Em.Cautious:
				return -bird.totalConfidence;
			default:
				return 0;
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
