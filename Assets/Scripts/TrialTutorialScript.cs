using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrialTutorialScript : MonoBehaviour
{

    public Button[] levelsToDisable;
    public Button trialLevel;
    Animator anime;

    public Button continueButton;


    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setupClickTrialToContinueMoment()
    {
  
        setLevelsEnabled(false);
        trialLevel.interactable = true;
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

    public void playerClickedTrialLevel()
    {
        if (anime)
        {
            anime.SetTrigger("trialLevelClicked");
        }
        continueButton.gameObject.SetActive(true);
    }

    public void hidePopup()
    {
        this.gameObject.SetActive(false);
    }
}
