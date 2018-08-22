using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialSetup : MonoBehaviour {
	[Header("Terry")]
	public int confidence1 = 0;
	public int friendliness1 = 0;
	public Levels.type startingLVL1;
	[Header("Rebecca")]
	public int confidence2 = 0;
	public int friendliness2 = 0;
	public Levels.type startingLVL2;
	[Header("Alex")]
	public int confidence3 = 0;
	public int friendlines3 = 0;
	public Levels.type startingLVL3;
	// Use this for initialization
	void Start () {
		if (!Var.isTutorial)
			return;
		//Setup Terry
		Bird terry =FillPlayer.Instance.playerBirds[0];
		terry.data.confidence = confidence1;
		terry.data.friendliness = friendliness1;
		//terry.startingLVL = startingLVL1;
		terry.SetEmotion();
		//AddLevel(terry, startingLVL1);

		//Setup Rebecca
		Bird rebecca = FillPlayer.Instance.playerBirds[1];
		rebecca.data.confidence = confidence2;
		rebecca.data.friendliness = friendliness2;
        //rebecca.startingLVL = startingLVL2;
        rebecca.SetEmotion();
		//AddLevel(rebecca, startingLVL2);

		//Setup Alex
		Bird alex = FillPlayer.Instance.playerBirds[2];
		alex.data.confidence = confidence3;
		alex.data.friendliness = friendlines3;
        alex.SetEmotion();
        //AddLevel(alex, startingLVL3);
        LeanTween.delayedCall(0.1f, terry.showText);
	}
	void AddLevel(Bird bird, Levels.type lvl)
	{
		Sprite icon = Helpers.Instance.GetLVLSprite(lvl);
		bird.AddLevel(new LevelData(lvl, Var.Em.Neutral, icon));
	}
	// Update is called once per frame
	void Update () {
		
	}
}
