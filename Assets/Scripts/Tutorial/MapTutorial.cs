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
    public GameObject mapTutPopup2;
    public static MapTutorial instance;
    private int secondMapTutID = 11;
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
        Debug.LogError(Var.currentStageID);
        if (!Var.gameSettings.shownMapTutorial2 && Var.currentStageID == secondMapTutID)
        {
            mapTutPopup2.SetActive(true);
            Var.gameSettings.shownMapTutorial2 = true;
            SaveLoad.Save(false);
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
		//DialogueControl.Instance.CreateParticularDialog(mapTutorialDialog2);
		Var.gameSettings.shownMapTutorial = true;
	}
}
