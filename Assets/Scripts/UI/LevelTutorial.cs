﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTutorial : MonoBehaviour {
	[Header("first level")]
	public Dialogue planningDialogue;
	[Header("second level")]
	public Dialogue firstBattleDailog;
	public Dialogue graphDialog;
	public Dialogue secondBattleDialog;
	public GameObject GraphHighlight;
	public static bool shouldShowFirstBattleDialog = false;
	bool shouldShowGraphDialog = false;
	bool shouldShowGraphDiag2 = false;
	bool shouldShowSecondBattleDialog = false;
	float timeSinceStart = 1f;

	[Header("map feature tutorials")]
	public Dialogue swordTutorialDialogue;
	public Dialogue shieldTutorialDialogue;
	public Dialogue emoSquareTutorialDialogue;
	public Dialogue wizardTutorialDialogue;
	public Dialogue oneHealthDialogue;
	public Dialogue heartDialogue;
	public Dialogue firstTrialDialogue;
	// Use this for initialization
	public static LevelTutorial Instance;
    private void Awake()
    {
		Instance = this;
    }
    void Start ()
	{
		//Var.currentStageID = 1;
		if((Var.currentStageID == Var.battlePlanningTutorialID) && !Var.gameSettings.shownBattlePlanningTutorial) //In the map previous to to lvl 3 (ID =2) players learn abt the trials
		{
            LeanTween.delayedCall(0.14f, () =>
				Var.gameSettings.shownBattlePlanningTutorial = true);
            GuiContoler.Instance.minimap.SetActive(true);
            DialogueControl.Instance.CreateParticularDialog(planningDialogue);
		}

		if (Var.currentStageID == Var.levelTutorialID && !Var.gameSettings.shownLevelTutorial)
		{
			Var.gameSettings.shownLevelTutorial = true;
            DialogueControl.Instance.CreateParticularDialog(firstBattleDailog);
			shouldShowFirstBattleDialog = true;
			shouldShowGraphDialog = true;
			Var.CanShowHover = false;
		}

		if (Var.freezeEmotions)
		{
			ShowFirstTrialTutorial();
		}
	}
   public void ShowSwordTutorial()
    {
		if (!Var.gameSettings.shownSwordTutorial && Var.gameSettings.shownLevelTutorial)
		{
			DialogueControl.Instance.CreateParticularDialog(swordTutorialDialogue);
			Var.gameSettings.shownSwordTutorial = true;
			SaveLoad.Save(false);
		}
    }
	public void ShowFirstTrialTutorial()
	{
		if (!Var.gameSettings.shownTrialTutorial)
		{
			DialogueControl.Instance.CreateParticularDialog(firstTrialDialogue);
			Var.gameSettings.shownTrialTutorial = true;
			SaveLoad.Save(false);
		}
	}

	public void ShowShieldTutorial()
    {
		if (!Var.gameSettings.shownShieldTutorial && Var.gameSettings.shownLevelTutorial)
		{
			DialogueControl.Instance.CreateParticularDialog(shieldTutorialDialogue);
			Var.gameSettings.shownShieldTutorial = true;
			SaveLoad.Save(false);
		}
	}

	public void ShowEmoTileTutorial()
    {
		if (!Var.gameSettings.shownEmoSquareTutorial && Var.gameSettings.shownLevelTutorial)
		{
			DialogueControl.Instance.CreateParticularDialog(emoSquareTutorialDialogue);
			Var.gameSettings.shownEmoSquareTutorial = true;
			SaveLoad.Save(false);
		}
	}

	public void ShowWizardTutorial()
    {
		if (!Var.gameSettings.shownWizardTutorial && Var.gameSettings.shownLevelTutorial)
		{
			DialogueControl.Instance.CreateParticularDialog(wizardTutorialDialogue);
			Var.gameSettings.shownWizardTutorial = true;
			SaveLoad.Save(false);
		}
	}

	public void ShowOneHealthTutorial()
	{
		if (!Var.gameSettings.shownOneHealthTutorial && Var.gameSettings.shownLevelTutorial)
		{
			DialogueControl.Instance.CreateParticularDialog(oneHealthDialogue);
			Var.gameSettings.shownOneHealthTutorial = true;
			SaveLoad.Save(false);
		}
	}

	public void ShowHeartTutorial()
	{
		if (!Var.gameSettings.shownHeartTutorial && Var.gameSettings.shownLevelTutorial)
		{
			DialogueControl.Instance.CreateParticularDialog(heartDialogue);
			Var.gameSettings.shownHeartTutorial = true;
			SaveLoad.Save(false);
		}
	}
	// Update is called once per frame
	void Update()
	{		
		//if (Var.currentStageID != 5)
		//	return;
		if (shouldShowFirstBattleDialog && !GuiContoler.Instance.speechBubbleObj.activeSelf && Time.timeSinceLevelLoad >0.5f)
		{
			GraphHighlight.SetActive(true);
			Var.activeBirds[0].Speak("Leader, open up the emotional grid and I'll explain!");
			try
			{
				Var.activeBirds[0].showText();
				GuiContoler.Instance.speechBubbleObj.transform.Find("BG").gameObject.SetActive(false);
			}
			catch
			{
				Debug.LogError("couldnt disable BG");
			}
		}
		if(shouldShowGraphDialog && GuiContoler.Instance.GraphBlocker.activeSelf)
		{
			Var.CanShowHover = true;
			shouldShowFirstBattleDialog = false;
			shouldShowGraphDialog = false;
			GuiContoler.Instance.CloseBattleReport.interactable = false;
			GraphHighlight.SetActive(false);
			DialogueControl.Instance.CreateParticularDialog(graphDialog);
			shouldShowSecondBattleDialog = true;
			shouldShowGraphDiag2 = true;
			timeSinceStart = Time.timeSinceLevelLoad;
			try
			{
				GuiContoler.Instance.speechBubbleObj.transform.Find("BG").gameObject.SetActive(true);

			}
			catch
			{
				Debug.LogError("couldnt enable BG");
			}
		}
		if (shouldShowGraphDiag2 && !GuiContoler.Instance.speechBubbleObj.activeSelf && Time.timeSinceLevelLoad> 0.5f + timeSinceStart) 
		{
			GuiContoler.Instance.CloseBattleReport.interactable = true;
			//GuiContoler.Instance.ShowSpeechBubble(DialogueControl.Instance.portraitPoint, "Let's get back out there and get those seeds, leader!", AudioControler.Instance.RebeccaSounds.birdDialogueTalk);
			shouldShowGraphDiag2 = false;			
		}
		if (shouldShowSecondBattleDialog && !GuiContoler.Instance.GraphBlocker.activeSelf)
		{
			shouldShowSecondBattleDialog = false;
			DialogueControl.Instance.CreateParticularDialog(secondBattleDialog);
			//shouldShowSecondBattleDialog = true;
		}
	}
}
