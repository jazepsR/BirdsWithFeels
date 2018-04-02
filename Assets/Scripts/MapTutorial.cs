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
            print("Did map tutorial dialog");
        }
		if (Var.gameSettings.shownMapTutorial && !Var.gameSettings.shownMapPlanningTutorial) //this needs to react to a specific level, not to whether it has shown the initial tutorial before. 
		{
			DialogueControl.Instance.CreateParticularDialog(mapPlanningTutorial);
			Var.gameSettings.shownMapPlanningTutorial = true;
		}
	}
	
public void ShowDialog2()
    {
        DialogueControl.Instance.CreateParticularDialog(mapTutorialDialog2);
        Var.gameSettings.shownMapTutorial = true;
    }
}
