using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
	public static Tutorial Instance { get; private set; }
	public GameObject outlines;
	public GameObject FirendlinessPopUp;
	public GameObject EmotionalWheelPopUp;
	public Animator graphAnim;
	public Var.Em[] firstStageEnemies;
	public Var.Em[] secondStageEnemies;
	public Var.Em[] thirdStageEnemies;
	public Var.Em[] forthStageEnemies;
	public Var.Em[] fifthStageEnemies;
	public Var.Em[] sixthStageEnemies;
	public EventScript AddAlexEvent;
	public List<List<TutorialEnemy>> TutorialMap = new List<List<TutorialEnemy>>();
	public List<int> BirdCount;
	[HideInInspector]
	public int CurrentPos = 0;
	public Transform[] birdSpeechPos;
	public Transform portraitPoint;
	bool shouldShowFriendlyPopup = false;
	bool shouldShowEmotionPopup = false;
	bool shouldShowOutlines = true;
	bool showedSecondBirdReportText = false;
	bool showedThirdBirdReportText = false;
	[HideInInspector]
	public bool[] shownMapInfos = new bool[6];
	// Use this for initialization
	void Awake () {
		Instance = this;
		if (!Var.isTutorial)
			return;
		CurrentPos = 0;
		AddEnemiesToList(firstStageEnemies);
		AddEnemiesToList(secondStageEnemies);
		AddEnemiesToList(thirdStageEnemies);
		AddEnemiesToList(forthStageEnemies);
		AddEnemiesToList(fifthStageEnemies);
		AddEnemiesToList(sixthStageEnemies);
	}
	void Start()
	{
		if (!Var.isTutorial)
			return;

		GuiContoler.Instance.smallGraph.graphArea.transform.parent.gameObject.SetActive(false);
		ShowtutorialStartingText(0);
		if (Var.isTutorial)
		{
			for (int i = 0; i < FillPlayer.Instance.playerBirds.Length; i++)
			{
				if (i < Tutorial.Instance.BirdCount[0])
				{
					FillPlayer.Instance.playerBirds[i].gameObject.SetActive(true);
				}
				else
				{
					FillPlayer.Instance.playerBirds[i].gameObject.SetActive(false);
				}
			}
		}
	}
	public void jiggleGraph()
	{
		graphAnim.SetBool("shake", true);
		LeanTween.delayedCall(0.7f, stopJiggle);
	}
	void stopJiggle()
	{
		graphAnim.SetBool("shake", false);
	}
	public void ShowSmallGraph(float waitTime)
	{
		LeanTween.delayedCall(waitTime, () => GuiContoler.Instance.smallGraph.graphArea.transform.parent.gameObject.SetActive(true));
	}
	void Update()
	{
		if (!Var.isTutorial)
			return;
		if (shouldShowOutlines && !GuiContoler.Instance.speechBubbleObj.activeSelf)
		{
			outlines.SetActive(true);
			shouldShowOutlines = false;
		}
		if (shouldShowFriendlyPopup && !GuiContoler.Instance.speechBubbleObj.activeSelf)
		{
			FirendlinessPopUp.SetActive(true);
			shouldShowFriendlyPopup = false;
		}
		if (shouldShowEmotionPopup && !GuiContoler.Instance.speechBubbleObj.activeSelf)
		{
			EmotionalWheelPopUp.SetActive(true);
			shouldShowEmotionPopup = false;
		}
	   
	}

	public void SetCurrenPos(int pos)
	{
		showedSecondBirdReportText = false;
		showedThirdBirdReportText = false;
		CurrentPos = pos;
	}


	public void ShowGraphSpeech(int currentGraph)
	{
		if (currentGraph == 1)
			ShowTutorialSecondGridText(CurrentPos);
		if (currentGraph == 2)
			ShowTutorialThirdGridText(CurrentPos);
	}

	void AddEnemiesToList(Var.Em[] array)
	{
		List<TutorialEnemy> list = new List<TutorialEnemy>(); 
		for(int i = 0; i < array.Length; i++)
		{
			if (array[i] == Var.Em.finish)
				list.Add(null);
			else
				list.Add(new TutorialEnemy(array[i]));
		}
		TutorialMap.Add(list);
	}
	public void ShowtutorialStartingText(int stage)
	{
	 
		switch (stage)
		{
			case 0:
				
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Hiya, I'm Terry! ");
                GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Looks like that vulture is itching for a fight!!  ");
                GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Honestly, I don’t know anything bout’ fighting. I’m a peaceful bird!   ");
                GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "But..maybe I can talk to them?   ");
                GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "<b>Drag</b> me <b>anywhere</b> in front of them and I’ll try to convince them to not beat us up! ");
				break;
			case 1:
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[1].GetMouthTransform(), "Hey, I’m Rebecca! Let's hang out!");
                GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Alright sure whatever!");
                shouldShowFriendlyPopup = true;
				break;
			case 2:
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[1].GetMouthTransform(), " These birds are not just neutral, they're confident and cautious! Will they listen to us?");
				shouldShowEmotionPopup = true;
				break;
			case 3:
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[1].GetMouthTransform(), "These battles are taking a toll on me..");
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Hey no worries - I'll fight so that you can <b>rest</b> and regain some <b>health</b>");
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "You'll also gain some <b>caution!</b>");
				break;
			case 4:
				EventController.Instance.CreateEvent(AddAlexEvent);
			 //   GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[2].GetMouthTransform(), "A Rock!");
			 //   GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Birds can't stand there, but vultures move right over the rocks!");
				break;
			case 5:
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[1].GetMouthTransform(), "This looks dangerous!");
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[2].GetMouthTransform(), "I've seen this before! These enemies will attack from multiple directions!");
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "You guys can you let me fight <b>two at once?</b> I've always wanted to do that! ");
				break;
		}
	}
	public void ShowTutorialBeforeBattleText(int stage)
	{
		
		if (shownMapInfos[stage] == true)
			return;
		shownMapInfos[stage] = true;
		switch (stage)
		{
			case 0:
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Vultures will walk forward forever until they reach a confrontation! ");
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Here goes nothing!!"); 
				outlines.SetActive(false);
				break;
			case 1:
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[1].GetMouthTransform(), "Hey you! Let's talk about life!!");
				break;
			case 2:
				int total = 0;
				foreach(feedBack fb in FindObjectsOfType<feedBack>())
				{
					total += fb.feedbackVal;
				}
				if (total > 100)
				{
				
					graphAnim.SetBool("shake", false);
				}
				else
				{
					GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Hmm.. This seems risky");
					GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[1].GetMouthTransform(), "We should use our <b>emotions</b> to our advantage!");
					graphAnim.SetBool("shake", true);
				}
				shownMapInfos[stage] = false;
				break;
			case 3:               
				break;
			case 4:                
				break;
			case 5:                
				break;
		}
	}
	public void ShowTutorialFirstGridText(int stage)
	{
		switch (stage)
		{
			case 0:
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "We birds are <b>affected emotionally</b> by what happens to us! Unlike you, you monster" );
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "I befriended them, my <b>confidence</b> is surging! ");
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "But there’s also no other birds to hang out with - making me more <b>solitary</b>.");
				break;
			case 1:
				if(Var.activeBirds[0].FriendGainedInRound>0)
					GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "Huh, hanging out with other birds aint that bad! I feel more <b>social!</b>");
				else
				{
					if (Var.activeBirds[0].FriendGainedInRound < 0)
					{
						GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "Being alone this battle made me more lonely..");
						GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "I mean independent! ");
						GuiContoler.Instance.ShowSpeechBubble(portraitPoint, " and strong"); 
						GuiContoler.Instance.ShowSpeechBubble(portraitPoint, " Strong and independent.");
					}
					else
					{
						GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "We sorta hung out, but not really? So we’re unchanged emotionally!");
					}
				}
				break;
			case 2:
				graphAnim.SetBool("shake", false);
				break;
			case 3:
				if(Var.activeBirds[0].prevRoundHealth<Var.activeBirds[0].data.health)
					GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "Ahh, I needed that rest - back to full health!");
				break;
			case 4:                
				break;
			case 5:                
				break;
		}
	}
	public void ShowTutorialSecondGridText(int stage)
	{
		if (showedSecondBirdReportText)
			return;
		showedSecondBirdReportText = true;
		switch (stage)
		{
			case 0:
				 break;
			case 1:
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "I became more <b>cautious</b> - anyone would after getting beat up like that ");
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "But that strong emotion is making me <b>stronger</b>! Losing is <b>not always bad</b>.");
				break;
			case 2:               
				break;
			case 3:
				if (Var.activeBirds[1].prevRoundHealth < Var.activeBirds[1].data.health)
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "Ahhh, back to full health!");
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "I love resting. I could do it all day");
				break;
			case 4:                
				break;
			case 5:               
				break;
		}
	}
	public void ShowTutorialThirdGridText(int stage)
	{
		if (showedThirdBirdReportText)
			return;
		showedThirdBirdReportText = true;
		switch (stage)
		{
			case 0:
				 break;
			case 1:
				  break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "Onwards! To victory!");
				break;
			case 5:
				break;
		}
	}

}




public class TutorialEnemy
{

	public int confidence;
	public int firendliness;

	public TutorialEnemy(Var.Em emotion)
	{
		switch (emotion){
			case Var.Em.Neutral:
				confidence = 0;
				firendliness = 0;
				break;
			case Var.Em.Cautious:
				confidence = -7;
				firendliness = 0;
				break;
			case Var.Em.Confident:
				confidence = 7;
				firendliness = 0;
				break;
			case Var.Em.Social:
				confidence = 0;
				firendliness = 7;
				break;
			case Var.Em.Solitary:
				confidence = 0;
				firendliness = -7;
				break;

		}
	}

}
