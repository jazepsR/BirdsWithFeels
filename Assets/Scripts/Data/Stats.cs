using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stats : MonoBehaviour
{
    public bool isShowingStatsMenu = false;
    public GameObject statsMenu;
    public GameObject closeButton;
    public int currentPage = 0;
    //public int currentSection;
    public List<GameObject> listOfPages;
    public List<GameObject> generalPageStatDisplayers;
    public List<GameObject> terryPageStats;
    public List<GameObject> rebeccaPageStats;
    public List<GameObject> alexanderPageStats;
    public List<GameObject> kimPageStats;
    public List<GameObject> sophiePageStats;

    [SerializeField] private List<Bird> birds;



    //private GameObject sectionContainers;

    [SerializeField]
    private Animator myStatPageAnimator;

    [SerializeField]
    private Animator myPageTurner;


    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetSceneByName("Stats") == SceneManager.GetActiveScene())
        {
            isShowingStatsMenu = true;

            Var.runPlayTimeTimer = false;

            Var.activeBirds = new List<Bird>(birds);
            ShowStatsMenu();
            
            
        }


        if (statsMenu != null && isShowingStatsMenu)
        {

           // goToPage(listOfPages, currentPage); //first section, first page
           // loadDataGeneralPage();
        }  


           /* //if(SceneManager.GetSceneByName("Stats") == SceneManager.GetActiveScene())
            // {
            foreach (Bird bird in Var.activeBirds)
            {

                

                if (bird != null)
                {
                    switch (bird.name)
                    {
                        case "Terry":
                            loadDataTerry(bird);
                            TerryEvent(Var.timedEvents[0]);
                            break;
                        case "Rebecca":
                            loadDataRebecca(bird);
                            RebeccaEvent(Var.timedEvents[1]);
                            break;
                        case "Alexander":
                            loadDataAlexander(bird);
                            AlexanderEvent(Var.timedEvents[2]);
                            break;
                        case "Kim":
                            loadDataKim(bird);
                            KimEvent(Var.timedEvents[3]);
                            break;
                        case "Sophie":
                            loadDataSophie(bird);
                            SophieEvent(Var.timedEvents[4]);
                            break;
                        default:
                            Debug.Log("cant find birb");
                            break;
                    }
                }
            }

            if (Var.SophieUnlocked)
            {
                //all birds are unlocked do nothing
            }
            else if (Var.KimUnlocked)
            {
                //cut sophie page

                listOfPages.RemoveAt(listOfPages.Count - 1);
                // listOfPages.Sort();

            }
            else if (listOfPages.Count != 4)
            {
                listOfPages.RemoveAt(listOfPages.Count - 2);
                //listOfPages.Sort();
                listOfPages.RemoveAt(listOfPages.Count - 1);
                // listOfPages.Sort();
            }

        } */
    }

    void loadDataGeneralPage()
    {
        generalPageStatDisplayers[0].GetComponent<Text>().text = (Var.totalTimeDays < 10 ? "0" + Var.totalTimeDays.ToString() : Var.totalTimeDays.ToString()) + " : " + (Var.totalTimeHours < 10 ? "0" + Var.totalTimeHours.ToString() : Var.totalTimeHours.ToString()) + " : " + (Var.totalTimeMinutes < 10 ? "0" + Var.totalTimeMinutes.ToString() : Var.totalTimeMinutes.ToString()) + " : " + (Var.totalTimeSeconds < 10 ? "0" + Var.totalTimeSeconds.ToString() : Var.totalTimeSeconds.ToString());
        generalPageStatDisplayers[1].GetComponent<Text>().text = (Var.currentWeek < 100 ? "00" + Var.currentWeek.ToString() : Var.currentWeek < 10 ? "0" + Var.currentWeek.ToString() : Var.currentWeek.ToString());
        generalPageStatDisplayers[2].GetComponent<Text>().text = (Var.confrontSuccess < 100 ? "00" + Var.confrontSuccess.ToString() : Var.confrontSuccess < 10 ? "0" + Var.confrontSuccess.ToString() : Var.confrontSuccess.ToString());
        generalPageStatDisplayers[3].GetComponent<Text>().text = (Var.confrontFail < 100 ? "00" + Var.confrontFail.ToString() : Var.confrontFail < 10 ? "0" + Var.confrontFail.ToString() : Var.confrontFail.ToString());
        generalPageStatDisplayers[4].GetComponent<Text>().text = Var.narrativeEventsCompleted < 10 ? "0" + Var.narrativeEventsCompleted.ToString() : Var.narrativeEventsCompleted.ToString();
        generalPageStatDisplayers[5].GetComponent<Text>().text = Achievements.total_Narrative_Events < 10 ? "0" + Achievements.total_Narrative_Events.ToString() : Achievements.total_Narrative_Events.ToString();
        generalPageStatDisplayers[6].GetComponent<Text>().text = Var.levelsCompleted < 10 ? "0" + Var.levelsCompleted.ToString() : Var.levelsCompleted.ToString();
        generalPageStatDisplayers[7].GetComponent<Text>().text = Achievements.total_Levels < 10 ? "0" + Achievements.total_Levels.ToString() : Achievements.total_Levels.ToString();
    }

    void loadDataTerry(Bird bird, TimedEventData data = null)
    {
        //terryPageStats[0].GetComponent<Image>().color;
       // Debug.Log("hi");

        terryPageStats[0].GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
        try
        {
            terryPageStats[1].GetComponent<Text>().text = Helpers.Instance.ApplyTitle(bird, bird.data.lastLevel.birdTitle);
        }

        catch
        {
            terryPageStats[1].GetComponent<Text>().text = Helpers.Instance.ApplyTitle(bird, "Level " + bird.name + " up to get a title!");
        }

       // terryPageStats[2].GetComponent<Text>().text = (bird.data.level < 10 ? "0" + bird.data.level.ToString() : bird.data.level.ToString());

        //terryPageStats[3].GetComponent<Text>().text = "bird description can be changed here if you want to instead of in gui";

       // terryPageStats[4].GetComponent<Text>().text = "epilogue hidden text can go here if you want to change instead of in gui";

        if (SceneManager.GetSceneByName("Stats") == SceneManager.GetActiveScene())
        {
            
            terryPageStats[3].GetComponent<Text>().enabled = false; //hides to make room for epilogue 
            terryPageStats[3].GetComponent<Text>().text = "";
            terryPageStats[4].GetComponent<Text>().enabled = false; //hides hidden epilogue message
            terryPageStats[5].SetActive(true);
        }

        terryPageStats[6].GetComponent<Text>().text = (bird.data.emotionsChanged < 100 ? "0" + bird.data.emotionsChanged.ToString() : bird.data.emotionsChanged < 10 ? "0" + bird.data.emotionsChanged.ToString() : bird.data.emotionsChanged.ToString());

        terryPageStats[7].GetComponent<Text>().text = (bird.data.emotionSeedsCollected < 10 ? "0" + bird.data.emotionSeedsCollected.ToString() : bird.data.emotionSeedsCollected.ToString());
        terryPageStats[8].GetComponent<Text>().text = (bird.data.turnsInDangerZone < 10 ? "00" + bird.data.turnsInDangerZone.ToString() : bird.data.turnsInDangerZone < 10 ? "0" + bird.data.turnsInDangerZone.ToString() : bird.data.turnsInDangerZone.ToString());
        terryPageStats[9].GetComponent<Text>().text = (bird.data.powerUpHeartsUsed < 100 ? "00" + bird.data.powerUpHeartsUsed.ToString() : bird.data.powerUpHeartsUsed < 10 ? "0" + bird.data.powerUpHeartsUsed.ToString() : bird.data.powerUpHeartsUsed.ToString());
        terryPageStats[10].GetComponent<Text>().text = (bird.data.powerUpSwordsUsed < 100 ? "00" + bird.data.powerUpSwordsUsed.ToString() : bird.data.powerUpSwordsUsed < 10 ? "0" + bird.data.powerUpSwordsUsed.ToString() : bird.data.powerUpSwordsUsed.ToString());
        terryPageStats[11].GetComponent<Text>().text = (bird.data.powerUpShieldsUsed < 100 ? "00" + bird.data.powerUpShieldsUsed.ToString() : bird.data.powerUpShieldsUsed < 10 ? "0" + bird.data.powerUpShieldsUsed.ToString() : bird.data.powerUpShieldsUsed.ToString());


    }

    void loadDataRebecca(Bird bird)
    {

    }

    void loadDataAlexander(Bird bird)
    {

    }

    void loadDataKim(Bird bird)
    {

    }

    void loadDataSophie(Bird bird)
    {

    }

    public void TerryEvent(TimedEventData data)
    {
        try
        {

            switch (data.currentState)
            {
                case TimedEventData.state.completedSuccess:
                    terryPageStats[5].GetComponent<Text>().text = "terry did it!";
                    break;
                case TimedEventData.state.completedFail:
                    terryPageStats[5].GetComponent<Text>().text = "terry made it in time but failed!";
                    break;
                case TimedEventData.state.failed:
                    terryPageStats[5].GetComponent<Text>().text = "terry failed to make it in time!";
                    break;
                default:
                    Debug.Log("none can be found");
                    break;
            }
        }

        catch
        {
            Debug.Log("Terry: Timed Event List size is: " + Var.timedEvents.Count);
        }
    }

    void RebeccaEvent(TimedEventData data)
    {
        try
        {
            switch (data.currentState)
            {
                case TimedEventData.state.completedSuccess:
                    break;
                case TimedEventData.state.completedFail:
                    break;
                case TimedEventData.state.failed:
                    break;
                default:
                    Debug.Log("none can be found");
                    break;
            }
            if (Var.trialsSuccessfullCount == Achievements.stats_total_trial_count)
            {
                Achievements.SetAchievement("all_trials_complete_in_time", "");
            }
        }
        catch
        {
            Debug.Log("Rebecca: Timed Event List size is: " + Var.timedEvents.Count);
        }
    }

    void AlexanderEvent(TimedEventData data)
    {
        try
        {
            switch (data.currentState)
            {
                case TimedEventData.state.completedSuccess:
                    break;
                case TimedEventData.state.completedFail:
                    break;
                case TimedEventData.state.failed:
                    break;
                default:
                    Debug.Log("none can be found");
                    break;
            }
        }

        catch
        {
            Debug.Log("Alexander: Timed Event List size is: " + Var.timedEvents.Count);
        }
    }

    void KimEvent(TimedEventData data)
    {
        try
        {
            switch (data.currentState)
            {
                case TimedEventData.state.completedSuccess:
                    break;
                case TimedEventData.state.completedFail:
                    break;
                case TimedEventData.state.failed:
                    break;
                default:
                    Debug.Log("none can be found");
                    break;
            }
        }

        catch
        {
            Debug.Log("Kim: Timed Event List size is: " + Var.timedEvents.Count);
        }
    }

    void SophieEvent(TimedEventData data)
    {
        try
        {
            switch (data.currentState)
            {
                case TimedEventData.state.completedSuccess:
                    break;
                case TimedEventData.state.completedFail:
                    break;
                case TimedEventData.state.failed:
                    break;
                default:
                    Debug.Log("none can be found");
                    break;
            }
        }

        catch
        {
            Debug.Log("Sophie: Timed Event List size is: " + Var.timedEvents.Count);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowStatsMenu()
    {
        isShowingStatsMenu = true;
        if(SceneManager.GetSceneByName("Stats") != SceneManager.GetActiveScene())
        { 
            statsMenu.SetActive(true);
            mapPan.Instance.scrollingEnabled = false;
            goToPage(listOfPages, currentPage); //first section, first page
            StartCoroutine(generalPageTimer());
        }
        
  
            loadDataGeneralPage();

        if (Var.SophieUnlocked)
        {
            //all birds are unlocked do nothing
        }
        else if (Var.KimUnlocked)
        {
            //cut sophie page

            listOfPages.RemoveAt(listOfPages.Count - 1);
            // listOfPages.Sort();

        }
        else if (listOfPages.Count != 4)
        {
            listOfPages.RemoveAt(listOfPages.Count - 2);
            //listOfPages.Sort();
            listOfPages.RemoveAt(listOfPages.Count - 1);
            // listOfPages.Sort();
        }

        foreach (Bird bird in Var.activeBirds)
        {
            Debug.Log("hi" + bird.name);

            if (bird != null)
            {
                switch (bird.name)
                {
                    case "Terry":
                        Debug.Log("hi terrypoo");
                        loadDataTerry(bird);
                        break;
                    case "Rebecca":
                        loadDataRebecca(bird);
                        break;
                    case "Alexander":
                        loadDataAlexander(bird);
                        break;
                    case "Kim":
                        loadDataKim(bird);
                        break;
                    case "Sophie":
                        loadDataSophie(bird);
                        break;
                    default:
                        Debug.Log("cant find bird");
                        break;

                }
            }
            else
            {
                Debug.Log("i am null");
            }
        }

    }

    public void HideStatsMenu()
    {

        
        isShowingStatsMenu = false;
        if (SceneManager.GetSceneByName("Stats") != SceneManager.GetActiveScene())
        {
            StopCoroutine(generalPageTimer());
        }
        statsMenu.SetActive(false);
        mapPan.Instance.scrollingEnabled = true;
    }

    IEnumerator generalPageTimer()
    {
        
        while (isShowingStatsMenu)
        {
            generalPageStatDisplayers[0].GetComponent<Text>().text = (Var.totalTimeDays < 10 ? "0" + Var.totalTimeDays.ToString() : Var.totalTimeDays.ToString()) + " : " + (Var.totalTimeHours < 10 ? "0" + Var.totalTimeHours.ToString() : Var.totalTimeHours.ToString()) + " : " + (Var.totalTimeMinutes < 10 ? "0" + Var.totalTimeMinutes.ToString() : Var.totalTimeMinutes.ToString()) + " : " + (Var.totalTimeSeconds < 10 ? "0" + Var.totalTimeSeconds.ToString() : Var.totalTimeSeconds.ToString());
            yield return new WaitForSeconds(.5f);
        }
    }

    public void NextPage()
    {

        if (myPageTurner != null)
        {
            myPageTurner.SetTrigger("turnright");
        }

       // StartCoroutine("CloseButtonAfterDelay");

        Debug.Log("next page");


        goToPage(listOfPages, currentPage + 1);
    }

    IEnumerator PreviousPageAfterDelay()
    {
        yield return new WaitForSeconds(0.32f);

        if (currentPage != 0)
        {
            goToPage(listOfPages, currentPage - 1);
        }
        else
        {
            goToPage(listOfPages, listOfPages.Count - 1);
        }

    }

   /* IEnumerator CloseButtonAfterDelay()
    {
        closeButton.SetActive(false);
        yield return new WaitForSeconds(0.32f);

        closeButton.SetActive(true);
    }*/

    public void PreviousPage()
    {
        Debug.Log("try previous");

        if (myPageTurner != null)
        {
            myPageTurner.SetTrigger("turnleft");
        }

        StartCoroutine("PreviousPageAfterDelay");
        //StartCoroutine("CloseButtonAfterDelay");

    }

    public void exitStats()
    {
        if (SceneManager.GetSceneByName("Stats") == SceneManager.GetActiveScene())
        {
            SceneManager.LoadScene("mainMenu");
        }
        else {
            if (myStatPageAnimator != null) //animator will call the "hide stats menu" functionality after a delay
            {
                myStatPageAnimator.SetTrigger("close");
            }
        }
    }

    public void goToPage(List<GameObject> pages, int number)
    {
        try
        {
            pages[currentPage].gameObject.SetActive(false);
            if (number > pages.Count - 1 || number < 0)
            {
                pages[0].gameObject.SetActive(true);
                currentPage = 0;
            }

            else
            {
                pages[number].SetActive(true);
                currentPage = number;
            }
        }

        catch
        {

        }



    }

}
