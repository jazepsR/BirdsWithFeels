using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {
    public static Tutorial Instance { get; private set; }
    public GameObject FirendlinessPopUp;
    public GameObject EmotionalWheelPopUp;
    public Var.Em[] firstStageEnemies;
    public Var.Em[] secondStageEnemies;
    public Var.Em[] thirdStageEnemies;
    public Var.Em[] forthStageEnemies;
    public Var.Em[] fifthStageEnemies;
    public Var.Em[] sixthStageEnemies;
    public List<List<TutorialEnemy>> TutorialMap = new List<List<TutorialEnemy>>();
    public List<int> BirdCount;
    [HideInInspector]
    public int CurrentPos = 0;
    public Transform[] birdSpeechPos;
    public Transform portraitPoint;
    bool shouldShowFriendlyPopup = false;
    bool shouldShowEmotionPopup = false;
    bool showedSecondBirdReportText = false;
    bool showedThirdBirdReportText = false;
    bool[] shownMapInfos = new bool[6];
    // Use this for initialization
    void Awake () {
        
        if (!Var.isTutorial)
            return;
        CurrentPos = 0;
        Instance = this;
        AddEnemiesToList(firstStageEnemies);
        AddEnemiesToList(secondStageEnemies);
        AddEnemiesToList(thirdStageEnemies);
        AddEnemiesToList(forthStageEnemies);
        AddEnemiesToList(fifthStageEnemies);
        AddEnemiesToList(sixthStageEnemies);
        ShowtutorialStartingText(0);
    }
    
    void Update()
    {
        if (!Var.isTutorial)
            return;        
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
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[0], "Hiya, I'm Terry! Drag and place me in front of the enemy to make me FIGHT");
                break;
            case 1:
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[1], "Hey, I’m Rebecca! Let's hang out!");
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[0], "Alright sure whatever!");
                shouldShowFriendlyPopup = true;
                break;
            case 2:
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[1], "These birds look tough! How will we beat them?");
                shouldShowEmotionPopup = true;
                break;
            case 3:
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[1], "These battles are costing me some health!");
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[0], "Try resting - If you don't fight for one round you will regain one health");
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[0], "You'll also gain some scaredness!");
                break;
            case 4:
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[2], "Hey guys! Can I join your team?");
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[0], "Eh I guess sure!");
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[1], "Birds usually fight in teams of three, so this is perfect!");
                break;
            case 5:
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[1], "This looks dangerous!");
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[2], "I've seen this before! These enemies will attack from multiple directions!");
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[0], "You guys can you let me take two at once? I've always wanted to do that! ");
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
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[0], "I may be lonely, but my independence makes me stronger! Any emotion beats a lack of emotion in this emo eats emo world! LEMME FIGHT!");
                break;
            case 1:
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[1], "Since me and that mean bird have the same emotion, I have a 50% chance of winning the fight!");
                break;
            case 2:                
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
                GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "We birds are affected emotionally by what happens to us! Unlike you, you monster" );
                GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "I won the battle, my confidence is surging! ");
                GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "But there’s also no other birds to hang out with - making me more lonely.");
                break;
            case 1:
                if(Var.activeBirds[0].FriendGainedInRound>0)
                    GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "By hanging out with Rebecca I became more friendly!");
                else
                {
                    if (Var.activeBirds[0].FriendGainedInRound < 0)
                    {
                        GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "Being alone this battle made me more lonely");
                    }
                    else
                    {
                        GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "We sorta hung out, but not really? So we’re unchanged emotionally!");
                    }
                }
                break;
            case 2:                
                break;
            case 3:
                if(Var.activeBirds[0].prevRoundHealth<Var.activeBirds[0].health)
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
                GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "I became scared - due to me losing that last fight! I may be scared, but that strong emotion is making me stronger! Losing is not always bad. ");
                break;
            case 2:               
                break;
            case 3:
                if (Var.activeBirds[1].prevRoundHealth < Var.activeBirds[1].health)
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
            case Var.Em.Scared:
                confidence = -7;
                firendliness = 0;
                break;
            case Var.Em.Confident:
                confidence = 7;
                firendliness = 0;
                break;
            case Var.Em.Friendly:
                confidence = 0;
                firendliness = 7;
                break;
            case Var.Em.Lonely:
                confidence = 0;
                firendliness = -7;
                break;

        }
    }

}
