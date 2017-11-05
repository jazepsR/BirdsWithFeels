﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTutorial : MonoBehaviour {
	[Header("first level")]
	public Dialogue abilityDialogue;
	[Header("second level")]
	public Dialogue firstBattleDailog;
	public Dialogue graphDialog;
	public Dialogue secondBattleDialog;
	public GameObject GraphHighlight;
	public static bool shouldShowFirstBattleDialog = false;
	bool shouldShowGraphDialog = false;
	bool shouldShowGraphDiag2 = false;
	bool shouldShowSecondBattleDialog = false;
	// Use this for initialization
	void Start ()
	{
		//Var.currentStageID = 1;
		if(Var.currentStageID == 0 && !Var.gameSettings.shownAbilityTutorial)
		{
			Var.gameSettings.shownAbilityTutorial = true;
			DialogueControl.Instance.CreateParticularDialog(abilityDialogue);
		}


		if (Var.currentStageID == 1 && !Var.gameSettings.shownLevelTutorial)
		{
			DialogueControl.Instance.CreateParticularDialog(firstBattleDailog);
			shouldShowFirstBattleDialog = true;
			shouldShowGraphDialog = true;
			Var.CanShowHover = false;
			try
			{               
				Helpers.Instance.GetBirdFromEnum(EventScript.Character.Rebecca).showText();
			}
			catch { }
		}
	}
   
	// Update is called once per frame
	void Update()
	{
		if (Var.currentStageID != 1)
			return;
		if (shouldShowFirstBattleDialog && !GuiContoler.Instance.speechBubbleObj.activeSelf)
		{			
			GraphHighlight.SetActive(true);
			Helpers.Instance.GetBirdFromEnum(EventScript.Character.Rebecca).Speak("Leader, open up the emotional grid and I'll explain!");
			try
			{
				GuiContoler.Instance.speechBubbleObj.transform.Find("BG").gameObject.SetActive(false);

			}
			catch
			{
				Debug.LogError("couldnt disable BG");
			}
		}
		if(shouldShowGraphDialog && !GuiContoler.Instance.battlePanel.activeSelf)
		{
			Var.CanShowHover = true;
			shouldShowFirstBattleDialog = false;
			shouldShowGraphDialog = false;
			GuiContoler.Instance.CloseBattleReport.interactable = false;
			GraphHighlight.SetActive(false);
			DialogueControl.Instance.CreateParticularDialog(graphDialog);
			shouldShowSecondBattleDialog = true;
			shouldShowGraphDiag2 = true; try
			{
				GuiContoler.Instance.speechBubbleObj.transform.Find("BG").gameObject.SetActive(true);

			}
			catch
			{
				Debug.LogError("couldnt enable BG");
			}
		}
		if (shouldShowGraphDiag2 && !GuiContoler.Instance.speechBubbleObj.activeSelf)
		{
			GuiContoler.Instance.CloseBattleReport.interactable = true;
			GuiContoler.Instance.ShowSpeechBubble(DialogueControl.Instance.portraitPoint, "Now let's get back out there and help me level up!");
			shouldShowGraphDiag2 = false;
			
		}
		if (shouldShowSecondBattleDialog && GuiContoler.Instance.battlePanel.activeSelf)
		{
			shouldShowSecondBattleDialog = false;
			DialogueControl.Instance.CreateParticularDialog(secondBattleDialog);
			Var.gameSettings.shownLevelTutorial = true;
			//shouldShowSecondBattleDialog = true;
		}
	}
}
