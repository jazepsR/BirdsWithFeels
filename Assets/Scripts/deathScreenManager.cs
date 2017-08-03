using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class deathScreenManager : MonoBehaviour {
    public Text heading;
    public Text description;
    public GameObject DeathMenu;
    public Button returnToMapBtn;


    public void ShowDeathMenu(Bird bird)
    {
        returnToMapBtn.gameObject.SetActive(true);
        if (GuiContoler.Instance.nextMapArea == Var.Em.finish && (GuiContoler.Instance.roundLength == GuiContoler.Instance.posInMapRound+1))
            returnToMapBtn.gameObject.SetActive(false);
        DeathMenu.SetActive(true);
        heading.text = bird.charName + " has died!";
        description.text = Helpers.Instance.GetDeathText(bird.lastLevel.type, bird.charName);
        print("showed death menu!");
        foreach (Transform child in Graph.Instance.graphArea.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    public void HideDeathMenu()
    {
        DeathMenu.SetActive(false);
        returnToMapBtn.gameObject.SetActive(false);
    }
	// Use this for initialization
	
}
