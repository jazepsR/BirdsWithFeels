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
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[0], "Hi! I am  Terry! Palce me on the grid in fornt of the enemy to make me FIGHT");
                break;
            case 1:
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[1], "Hey I’m Rebecca! Let's hang out!");
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[0], "Hi! That sounds fun!");
                shouldShowFriendlyPopup = true;
                break;
            case 2:
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[1], "These birds look tough! How will we beat them?");
                shouldShowEmotionPopup = true;
                break;
            case 3:
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[1], "I've lost some health in these battles");
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[0], "If you don't fight for one round you will regain one health");
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[0], "But you will lose some confidence");
                break;
            case 4:
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[2], "Hey guys! Can I join your team?");
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[0], "Sure!");
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[1], "Birds usually fight in teams of three, so this is perfect!");
                break;
            case 5:
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[1], "This looks dangerous!");
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[2], "I've seen this before! These enemies will attack from multiple directions!");
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[0], "That means I can fight two of them at once!");
                break;
        }
    }
    public void ShowTutorialBeforeBattleText(int stage)
    {
        switch (stage)
        {
            case 0:
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[0], "I am the best! I am so lonely! Any emotion beats neutral birds in this emo eats emo world! LEMME FIGHT!");
                break;
            case 1:
                GuiContoler.Instance.ShowSpeechBubble(birdSpeechPos[1], "Since me and the enemy have the same emotion, I have a 50% chance of winning the fight!");
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
                GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "We birds are affected emotionally by what happens to us!" );
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
                    GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "Yeey, resting works!");
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
                GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "Ohhh I became scared! That's because I lost the last fight! Losing isn't always bad- now I am stronger");
                break;
            case 2:               
                break;
            case 3:
                if (Var.activeBirds[1].prevRoundHealth < Var.activeBirds[1].health)
                    GuiContoler.Instance.ShowSpeechBubble(portraitPoint, "Yeey, resting works!");
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
