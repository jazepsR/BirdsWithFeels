using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrialTutorialScript_part1 : MonoBehaviour
{

    public Button[] levelsToDisable;
    public Button LevelToOpenInTutorial;
    Animator anime;

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
    }

    public void hideTutorialPopup()
    {
        enableLevels();
        this.gameObject.SetActive(false);
    }
}
