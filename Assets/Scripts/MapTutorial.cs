using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTutorial : MonoBehaviour {

    public Animator tutorialHighlight;
    public Dialogue mapTutorialDialog;
    public Dialogue mapTutorialDialog2;
    // Use this for initialization
    void Start () {
        if (!Var.gameSettings.shownMapTutorial)
        {
            tutorialHighlight.gameObject.SetActive(true);
            DialogueControl.Instance.CreateParticularDialog(mapTutorialDialog);
        }
	}
	
public void ShowDialog2()
    {
        DialogueControl.Instance.CreateParticularDialog(mapTutorialDialog2);
        Var.gameSettings.shownMapTutorial = true;
    }
}
