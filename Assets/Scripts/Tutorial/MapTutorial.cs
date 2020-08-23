using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTutorial : MonoBehaviour {

	public Animator tutorialHighlight;
	public Dialogue mapTutorialDialog;
	public Dialogue mapTutorialDialog2;
	public Dialogue mapPlanningTutorial;

    private bool shouldShowMapTutPopup = false;
    public GameObject mapTutPopup;
    public static MapTutorial instance;
    // Use this for initialization

    private void Awake()
    {
        instance = this;
    }


    void Start () {
		if (!Var.gameSettings.shownMapTutorial)
		{
			tutorialHighlight.gameObject.SetActive(true);
			DialogueControl.Instance.CreateParticularDialog(mapTutorialDialog);
            shouldShowMapTutPopup = true;
		}else
		{
			tutorialHighlight.gameObject.SetActive(false);

		}
	}

    public void Update()
    {
        if(shouldShowMapTutPopup && !GuiContoler.Instance.speechBubbleObj.activeSelf)
        {
            shouldShowMapTutPopup = false;
            mapTutPopup.SetActive(true);
        }
    }
    public void ShowDialog2()
	{
		DialogueControl.Instance.CreateParticularDialog(mapTutorialDialog2);
		Var.gameSettings.shownMapTutorial = true;
	}
}
