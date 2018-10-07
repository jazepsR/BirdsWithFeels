using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTutorial : MonoBehaviour {

	public Animator tutorialHighlight;
	public Dialogue mapTutorialDialog;
	public Dialogue mapTutorialDialog2;
	public Dialogue mapPlanningTutorial;
	// Use this for initialization
	void Start () {
		if (!Var.gameSettings.shownMapTutorial)
		{
			tutorialHighlight.gameObject.SetActive(true);
			DialogueControl.Instance.CreateParticularDialog(mapTutorialDialog);
		}else
		{
			tutorialHighlight.gameObject.SetActive(false);
		}
	}
	
public void ShowDialog2()
	{
		DialogueControl.Instance.CreateParticularDialog(mapTutorialDialog2);
		Var.gameSettings.shownMapTutorial = true;
	}
}
