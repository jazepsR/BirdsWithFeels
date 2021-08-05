using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrialTutorialScript_part1 : MonoBehaviour
{

    public Button[] levelsToDisable;
    public Button LevelToOpenInTutorial;
    Animator anime;
    public MapControler mapctrl; //used for hiding the selection menu 
    public Animator HighlightAnimator;

    public Button continueButton;


    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame


    public void setupClickLevelToContinueMoment()
    {
  
        setLevelsEnabled(false);
        LevelToOpenInTutorial.interactable = true;
        continueButton.gameObject.SetActive(false);
        if (HighlightAnimator)
        {
            HighlightAnimator.gameObject.SetActive(true);
        }

    }

    public void disableLevels()
    {
        setLevelsEnabled(false);
    }
    public void enableLevels()
    {
        setLevelsEnabled(true);
    }

    public void setLevelsEnabled(bool enabled)
    {
        
        for(int i=0;i<levelsToDisable.Length; i++)
        {
            levelsToDisable[i].interactable = enabled;
        }
    }

    public void playerOpenedNode()
    {
        if (anime)
        {
            anime.SetTrigger("menuOpen");
        }
        continueButton.gameObject.SetActive(true);
        if (HighlightAnimator)
        {
            HighlightAnimator.gameObject.SetActive(false);
        }
    }

    public void hideTutorialPopup()
    {
        enableLevels();
        this.gameObject.SetActive(false);
    }

    public void CloseMapSelectionMenu()
    {
        if (mapctrl)
        {
            mapctrl.HideSelectionMenu();
        }
    }


}
