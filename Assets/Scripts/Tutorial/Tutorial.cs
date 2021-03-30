using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
	public static Tutorial Instance { get; private set; }
	public GameObject outlines;
	public GameObject FirendlinessPopUp;
	public GameObject EmotionalWheelPopUp;
    public GameObject mHPTutorialPopup;
    public Animator graphAnim;
    public Dialogue tutorialFirstDialogue;
    public Dialogue tutorialSecondDialogue;

	public Var.Em[] firstStageEnemies;
	public Var.Em[] secondStageEnemies;
	public Var.Em[] thirdStageEnemies;
	public Var.Em[] forthStageEnemies;
	public Var.Em[] fifthStageEnemies;
	public Var.Em[] sixthStageEnemies;

    public EventScript AddAlexEvent;
    public EventScript AddRebeccaEvent;
    public EventScript introduction;

	public List<List<TutorialEnemy>> TutorialMap = new List<List<TutorialEnemy>>();
	public List<int> BirdCount;
	[HideInInspector]
	public int CurrentPos = 0;
	public Transform[] birdSpeechPos;
	public Transform portraitPoint;
	bool shouldShowFriendlyPopup = false;
	bool shouldShowEmotionPopup = false;
    bool shouldShowMHPTutorial = false;
	bool shouldShowOutlines = true;
	bool showedSecondBirdReportText = false;
	bool showedThirdBirdReportText = false;
	bool shouldActivateBattleButton = false;
	[HideInInspector]
	public bool[] shownMapInfos = new bool[6];
	private AudioGroup TerrySounds;
	private AudioGroup RebeccaSounds;
	private AudioGroup AlexSounds;
	// Use this for initialization
	void Awake () {
		Instance = this;
		if (!Var.isTutorial)
			return;
        Var.currentBG = 0;
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
		TerrySounds = AudioControler.Instance.TerrySounds.birdDialogueTalk;
		RebeccaSounds = AudioControler.Instance.RebeccaSounds.birdDialogueTalk;
		AlexSounds = AudioControler.Instance.AlexSound.birdDialogueTalk;
		GuiContoler.Instance.smallGraph.graphArea.transform.parent.gameObject.SetActive(false);
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
		tutorialSetup.TutorialSetup.SetupBirds();
		LeanTween.delayedCall(0.5f,()=> ShowtutorialStartingText(0));
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
		GuiContoler.Instance.GraphBlocker.SetActive(false);
	}
	void LateUpdate()
	{
		if (!Var.isTutorial)
			return;


		if(shouldActivateBattleButton && !GuiContoler.Instance.speechBubbleObj.activeSelf)
        {
			AudioControler.Instance.PlaySound(AudioControler.Instance.fightButtonAppear);
			shouldActivateBattleButton = false;
        }

		if (shouldShowOutlines && !GuiContoler.Instance.speechBubbleObj.activeSelf)
		{
			outlines.SetActive(true);
			shouldShowOutlines = false;
		}
		if (shouldShowFriendlyPopup && !GuiContoler.Instance.speechBubbleObj.activeSelf && !EventController.Instance.eventObject.activeSelf)
		{
			FirendlinessPopUp.SetActive(true);
			GuiContoler.Instance.GraphBlocker.SetActive(true);
			shouldShowFriendlyPopup = false;
		}
		if (shouldShowEmotionPopup && !GuiContoler.Instance.speechBubbleObj.activeSelf)
		{
			EmotionalWheelPopUp.SetActive(true);
			GuiContoler.Instance.GraphBlocker.SetActive(true);
			shouldShowEmotionPopup = false;
		}
        if(shouldShowMHPTutorial&& !GuiContoler.Instance.speechBubbleObj.activeSelf)
        {
            mHPTutorialPopup.SetActive(true);
            GuiContoler.Instance.GraphBlocker.SetActive(true);
            shouldShowMHPTutorial = false;
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

                EventController.Instance.CreateEvent(introduction);
              //  DialogueControl.Instance.CreateDialogue(tutorialFirstDialogue);
                DialogueControl.Instance.CreateParticularDialog(tutorialFirstDialogue);
                /*GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), " Oh feathers, there are vultures here! In the city!", TerrySounds);
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Looks like that vulture is itching for a fight!!  ",TerrySounds);
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Honestly, I don’t know anything bout’ fighting. I’m a peaceful bird!   ",TerrySounds);
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "But..maybe I can talk to them? That vulture is <b>neutral</b>, so they should be easy to convince!  ",TerrySounds);
                GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "<b>Drag</b> me <b>anywhere</b> in front of them and I’ll try to convince them to not beat us up! ", TerrySounds);
        */
                break;
			case 1:

                EventController.Instance.CreateEvent(AddRebeccaEvent);
                DialogueControl.Instance.CreateParticularDialog(tutorialSecondDialogue);
                /*
                GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[1].GetMouthTransform(), "Hey, I’m Rebecca! Let's hang out!",RebeccaSounds);
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Alright sure whatever!",TerrySounds);*/
                shouldShowFriendlyPopup = true;
				break;
			case 2:
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[1].GetMouthTransform(), "These birds are not just neutral, they're solitary and cautious! Will they listen to us?",RebeccaSounds);
                shouldShowEmotionPopup = true;

                break;
			case 3:
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[1].GetMouthTransform(), "These encounters are taking a toll on me..",RebeccaSounds);
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Hey no worries - I'll fight so that you can <b>rest</b> and regain some <b>health</b>.",TerrySounds);
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "You'll also gain some <b>caution!</b>",TerrySounds);
				break;
			case 4:
				EventController.Instance.CreateEvent(AddAlexEvent);
                

                //   GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[2].GetMouthTransform(), "A Rock!");
                //   GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Birds can't stand there, but vultures move right over the rocks!");
                break;
			case 5:
                shouldShowMHPTutorial = true;

                GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[1].GetMouthTransform(), "This looks dangerous!",RebeccaSounds);
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[2].GetMouthTransform(), "I've seen this before! These enemies will attack from multiple directions!",AlexSounds);
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "You guys can you let me fight <b>two at once?</b> I feel SUPER PUMPED UP!! ",TerrySounds);
                
                GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[1].GetMouthTransform(), "Just don’t let it get to your head", RebeccaSounds);
                GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Pffft, what’s the worst that could happen?", TerrySounds);
                break;
            case 6:
            

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
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Vultures will walk forward forever until they reach a confrontation!",TerrySounds);
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Here goes nothing!!",TerrySounds); 
				outlines.SetActive(false);
				shouldActivateBattleButton = true;
				break;
			case 1:
				GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[1].GetMouthTransform(), "Hey you! Let's talk about life!!",RebeccaSounds);
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
					GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[0].GetMouthTransform(), "Hmm.. This seems risky",TerrySounds);
					GuiContoler.Instance.ShowSpeechBubble(FillPlayer.Instance.playerBirds[1].GetMouthTransform(), "We should use our <b>emotions</b> to our advantage!",RebeccaSounds);
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
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "We birds are <b>affected emotionally</b> by what happens to us!",TerrySounds);
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "I befriended them, my <b>confidence</b> is surging! ",TerrySounds);
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "But there’s also no other birds to hang out with - making me more <b>solitary</b>.",TerrySounds);
				break;
			case 1:
				if(Var.activeBirds[0].FriendGainedInRound>0)
					GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "Huh, hanging out with other birds aint that bad! I feel more <b>social!</b>",TerrySounds);
				else
				{
					if (Var.activeBirds[0].FriendGainedInRound < 0)
					{
						GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "Being alone this battle made me more lonely..",TerrySounds);
						GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "I mean independent!",TerrySounds);
						GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "And strong.",TerrySounds); 
						GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "Strong and independent.",TerrySounds);
					}
					else
					{
						GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "We sorta hung out, but not really? So we’re unchanged emotionally!",TerrySounds);
					}
				}
				break;
			case 2:
				GuiContoler.Instance.nextGraph.interactable = true;
				graphAnim.SetBool("shake", false);
				break;
			case 3:
				GuiContoler.Instance.nextGraph.interactable = true;
				if (Var.activeBirds[0].prevRoundHealth<Var.activeBirds[0].data.health)
					GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "Ahh, I needed that rest - back to full health!",TerrySounds);
				break;
			case 4:
				GuiContoler.Instance.nextGraph.interactable = true;
				break;
			case 5:
				GuiContoler.Instance.nextGraph.interactable = true;
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
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "I became more <b>cautious</b> - anyone would after getting beat up like that.",RebeccaSounds);
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "But that strong emotion is making me <b>stronger</b>! Losing is <b>not always bad</b>.",RebeccaSounds);
				break;
			case 2:               
				break;
			case 3:
				if (Var.activeBirds[1].prevRoundHealth < Var.activeBirds[1].data.health)
				{
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "Ahhh, back to full health!",RebeccaSounds);
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "I love resting. I could do it all day",RebeccaSounds);
				}
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
				GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "Onwards! To victory!",AlexSounds);
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
