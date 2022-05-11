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
            Var.availableBirds = new List<Bird>(birds);
            ShowStatsMenu();         
            
        }

       // Var.activeBirds = new List<Bird>(birds);
        


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

    void loadDataTerry(Bird bird)
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

        terryPageStats[2].GetComponent<Text>().text = (bird.data.level < 10 ? "0" + bird.data.level.ToString() : bird.data.level.ToString());

        //terryPageStats[3].GetComponent<Text>().text = "bird description can be changed here if you want to instead of in gui";

        //terryPageStats[4].GetComponent<Text>().text = "epilogue hidden text can go here if you want to change instead of in gui";

        if (SceneManager.GetSceneByName("Stats") == SceneManager.GetActiveScene())
        {
            
            terryPageStats[3].GetComponent<Text>().enabled = false; //hides to make room for epilogue 
            //terryPageStats[3].GetComponent<Text>().text = "";
           // terryPageStats[3].SetActive(false);
            terryPageStats[4].SetActive(false); //hides hidden epilogue message
            terryPageStats[5].SetActive(true);
            TerryEvent(bird);
            
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
        rebeccaPageStats[0].GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
        try
        {
            rebeccaPageStats[1].GetComponent<Text>().text = Helpers.Instance.ApplyTitle(bird, bird.data.lastLevel.birdTitle);
        }

        catch
        {
            rebeccaPageStats[1].GetComponent<Text>().text = Helpers.Instance.ApplyTitle(bird, "Level " + bird.name + " up to get a title!");
        }

        rebeccaPageStats[2].GetComponent<Text>().text = (bird.data.level < 10 ? "0" + bird.data.level.ToString() : bird.data.level.ToString());

        //rebeccaPageStats[3].GetComponent<Text>().text = "bird description can be changed here if you want to instead of in gui";

        //rebeccaPageStats[4].GetComponent<Text>().text = "epilogue hidden text can go here if you want to change instead of in gui";

        if (SceneManager.GetSceneByName("Stats") == SceneManager.GetActiveScene())
        {

            rebeccaPageStats[3].GetComponent<Text>().enabled = false; //hides to make room for epilogue 
                                                                      //terryPageStats[3].GetComponent<Text>().text = "";
                                                                      // terryPageStats[3].SetActive(false);
            rebeccaPageStats[4].SetActive(false); //hides hidden epilogue message
            rebeccaPageStats[5].SetActive(true);
            RebeccaEvent(bird);

        }

        rebeccaPageStats[6].GetComponent<Text>().text = (bird.data.emotionsChanged < 100 ? "0" + bird.data.emotionsChanged.ToString() : bird.data.emotionsChanged < 10 ? "0" + bird.data.emotionsChanged.ToString() : bird.data.emotionsChanged.ToString());

        rebeccaPageStats[7].GetComponent<Text>().text = (bird.data.emotionSeedsCollected < 10 ? "0" + bird.data.emotionSeedsCollected.ToString() : bird.data.emotionSeedsCollected.ToString());
        rebeccaPageStats[8].GetComponent<Text>().text = (bird.data.turnsInDangerZone < 10 ? "00" + bird.data.turnsInDangerZone.ToString() : bird.data.turnsInDangerZone < 10 ? "0" + bird.data.turnsInDangerZone.ToString() : bird.data.turnsInDangerZone.ToString());
        rebeccaPageStats[9].GetComponent<Text>().text = (bird.data.powerUpHeartsUsed < 100 ? "00" + bird.data.powerUpHeartsUsed.ToString() : bird.data.powerUpHeartsUsed < 10 ? "0" + bird.data.powerUpHeartsUsed.ToString() : bird.data.powerUpHeartsUsed.ToString());
        rebeccaPageStats[10].GetComponent<Text>().text = (bird.data.powerUpSwordsUsed < 100 ? "00" + bird.data.powerUpSwordsUsed.ToString() : bird.data.powerUpSwordsUsed < 10 ? "0" + bird.data.powerUpSwordsUsed.ToString() : bird.data.powerUpSwordsUsed.ToString());
        rebeccaPageStats[11].GetComponent<Text>().text = (bird.data.powerUpShieldsUsed < 100 ? "00" + bird.data.powerUpShieldsUsed.ToString() : bird.data.powerUpShieldsUsed < 10 ? "0" + bird.data.powerUpShieldsUsed.ToString() : bird.data.powerUpShieldsUsed.ToString());
    }

    void loadDataAlexander(Bird bird)
    {
        alexanderPageStats[0].GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
        try
        {
            alexanderPageStats[1].GetComponent<Text>().text = Helpers.Instance.ApplyTitle(bird, bird.data.lastLevel.birdTitle);
        }

        catch
        {
            alexanderPageStats[1].GetComponent<Text>().text = Helpers.Instance.ApplyTitle(bird, "Level " + bird.name + " up to get a title!");
        }

        alexanderPageStats[2].GetComponent<Text>().text = (bird.data.level < 10 ? "0" + bird.data.level.ToString() : bird.data.level.ToString());

        //alexanderPageStats[3].GetComponent<Text>().text = "bird description can be changed here if you want to instead of in gui";

        //alexanderPageStats[4].GetComponent<Text>().text = "epilogue hidden text can go here if you want to change instead of in gui";

        if (SceneManager.GetSceneByName("Stats") == SceneManager.GetActiveScene())
        {

            alexanderPageStats[3].GetComponent<Text>().enabled = false; //hides to make room for epilogue 
                                                                        //terryPageStats[3].GetComponent<Text>().text = "";
                                                                        // terryPageStats[3].SetActive(false);
            alexanderPageStats[4].SetActive(false); //hides hidden epilogue message
            alexanderPageStats[5].SetActive(true);
            AlexanderEvent(bird);

        }

        alexanderPageStats[6].GetComponent<Text>().text = (bird.data.emotionsChanged < 100 ? "0" + bird.data.emotionsChanged.ToString() : bird.data.emotionsChanged < 10 ? "0" + bird.data.emotionsChanged.ToString() : bird.data.emotionsChanged.ToString());

        alexanderPageStats[7].GetComponent<Text>().text = (bird.data.emotionSeedsCollected < 10 ? "0" + bird.data.emotionSeedsCollected.ToString() : bird.data.emotionSeedsCollected.ToString());
        alexanderPageStats[8].GetComponent<Text>().text = (bird.data.turnsInDangerZone < 10 ? "00" + bird.data.turnsInDangerZone.ToString() : bird.data.turnsInDangerZone < 10 ? "0" + bird.data.turnsInDangerZone.ToString() : bird.data.turnsInDangerZone.ToString());
        alexanderPageStats[9].GetComponent<Text>().text = (bird.data.powerUpHeartsUsed < 100 ? "00" + bird.data.powerUpHeartsUsed.ToString() : bird.data.powerUpHeartsUsed < 10 ? "0" + bird.data.powerUpHeartsUsed.ToString() : bird.data.powerUpHeartsUsed.ToString());
        alexanderPageStats[10].GetComponent<Text>().text = (bird.data.powerUpSwordsUsed < 100 ? "00" + bird.data.powerUpSwordsUsed.ToString() : bird.data.powerUpSwordsUsed < 10 ? "0" + bird.data.powerUpSwordsUsed.ToString() : bird.data.powerUpSwordsUsed.ToString());
        alexanderPageStats[11].GetComponent<Text>().text = (bird.data.powerUpShieldsUsed < 100 ? "00" + bird.data.powerUpShieldsUsed.ToString() : bird.data.powerUpShieldsUsed < 10 ? "0" + bird.data.powerUpShieldsUsed.ToString() : bird.data.powerUpShieldsUsed.ToString());
    }

    void loadDataKim(Bird bird)
    {
        kimPageStats[0].GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
        try
        {
            kimPageStats[1].GetComponent<Text>().text = Helpers.Instance.ApplyTitle(bird, bird.data.lastLevel.birdTitle);
        }

        catch
        {
            kimPageStats[1].GetComponent<Text>().text = Helpers.Instance.ApplyTitle(bird, "Level " + bird.name + " up to get a title!");
        }

        kimPageStats[2].GetComponent<Text>().text = (bird.data.level < 10 ? "0" + bird.data.level.ToString() : bird.data.level.ToString());

        //kimPageStats[3].GetComponent<Text>().text = "bird description can be changed here if you want to instead of in gui";

        //kimPageStats[4].GetComponent<Text>().text = "epilogue hidden text can go here if you want to change instead of in gui";

        if (SceneManager.GetSceneByName("Stats") == SceneManager.GetActiveScene())
        {

            kimPageStats[3].GetComponent<Text>().enabled = false; //hides to make room for epilogue 
                                                                  //terryPageStats[3].GetComponent<Text>().text = "";
                                                                  // terryPageStats[3].SetActive(false);
            kimPageStats[4].SetActive(false); //hides hidden epilogue message
            kimPageStats[5].SetActive(true);
            KimEvent(bird);

        }

        kimPageStats[6].GetComponent<Text>().text = (bird.data.emotionsChanged < 100 ? "0" + bird.data.emotionsChanged.ToString() : bird.data.emotionsChanged < 10 ? "0" + bird.data.emotionsChanged.ToString() : bird.data.emotionsChanged.ToString());

        kimPageStats[7].GetComponent<Text>().text = (bird.data.emotionSeedsCollected < 10 ? "0" + bird.data.emotionSeedsCollected.ToString() : bird.data.emotionSeedsCollected.ToString());
        kimPageStats[8].GetComponent<Text>().text = (bird.data.turnsInDangerZone < 10 ? "00" + bird.data.turnsInDangerZone.ToString() : bird.data.turnsInDangerZone < 10 ? "0" + bird.data.turnsInDangerZone.ToString() : bird.data.turnsInDangerZone.ToString());
        kimPageStats[9].GetComponent<Text>().text = (bird.data.powerUpHeartsUsed < 100 ? "00" + bird.data.powerUpHeartsUsed.ToString() : bird.data.powerUpHeartsUsed < 10 ? "0" + bird.data.powerUpHeartsUsed.ToString() : bird.data.powerUpHeartsUsed.ToString());
        kimPageStats[10].GetComponent<Text>().text = (bird.data.powerUpSwordsUsed < 100 ? "00" + bird.data.powerUpSwordsUsed.ToString() : bird.data.powerUpSwordsUsed < 10 ? "0" + bird.data.powerUpSwordsUsed.ToString() : bird.data.powerUpSwordsUsed.ToString());
        kimPageStats[11].GetComponent<Text>().text = (bird.data.powerUpShieldsUsed < 100 ? "00" + bird.data.powerUpShieldsUsed.ToString() : bird.data.powerUpShieldsUsed < 10 ? "0" + bird.data.powerUpShieldsUsed.ToString() : bird.data.powerUpShieldsUsed.ToString());
    }

    void loadDataSophie(Bird bird)
    {
        sophiePageStats[0].GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
        try
        {
            sophiePageStats[1].GetComponent<Text>().text = Helpers.Instance.ApplyTitle(bird, bird.data.lastLevel.birdTitle);
        }

        catch
        {
            sophiePageStats[1].GetComponent<Text>().text = Helpers.Instance.ApplyTitle(bird, "Level " + bird.name + " up to get a title!");
        }

        sophiePageStats[2].GetComponent<Text>().text = (bird.data.level < 10 ? "0" + bird.data.level.ToString() : bird.data.level.ToString());

        //sophiePageStats[3].GetComponent<Text>().text = "bird description can be changed here if you want to instead of in gui";

        //sophiePageStats[4].GetComponent<Text>().text = "epilogue hidden text can go here if you want to change instead of in gui";

        if (SceneManager.GetSceneByName("Stats") == SceneManager.GetActiveScene())
        {

            sophiePageStats[3].GetComponent<Text>().enabled = false; //hides to make room for epilogue 
                                                                     //terryPageStats[3].GetComponent<Text>().text = "";
                                                                     // terryPageStats[3].SetActive(false);
            sophiePageStats[4].SetActive(false); //hides hidden epilogue message
            sophiePageStats[5].SetActive(true);
            SophieEvent(bird);

        }

        sophiePageStats[6].GetComponent<Text>().text = (bird.data.emotionsChanged < 100 ? "0" + bird.data.emotionsChanged.ToString() : bird.data.emotionsChanged < 10 ? "0" + bird.data.emotionsChanged.ToString() : bird.data.emotionsChanged.ToString());

        sophiePageStats[7].GetComponent<Text>().text = (bird.data.emotionSeedsCollected < 10 ? "0" + bird.data.emotionSeedsCollected.ToString() : bird.data.emotionSeedsCollected.ToString());
        sophiePageStats[8].GetComponent<Text>().text = (bird.data.turnsInDangerZone < 10 ? "00" + bird.data.turnsInDangerZone.ToString() : bird.data.turnsInDangerZone < 10 ? "0" + bird.data.turnsInDangerZone.ToString() : bird.data.turnsInDangerZone.ToString());
        sophiePageStats[9].GetComponent<Text>().text = (bird.data.powerUpHeartsUsed < 100 ? "00" + bird.data.powerUpHeartsUsed.ToString() : bird.data.powerUpHeartsUsed < 10 ? "0" + bird.data.powerUpHeartsUsed.ToString() : bird.data.powerUpHeartsUsed.ToString());
        sophiePageStats[10].GetComponent<Text>().text = (bird.data.powerUpSwordsUsed < 100 ? "00" + bird.data.powerUpSwordsUsed.ToString() : bird.data.powerUpSwordsUsed < 10 ? "0" + bird.data.powerUpSwordsUsed.ToString() : bird.data.powerUpSwordsUsed.ToString());
        sophiePageStats[11].GetComponent<Text>().text = (bird.data.powerUpShieldsUsed < 100 ? "00" + bird.data.powerUpShieldsUsed.ToString() : bird.data.powerUpShieldsUsed < 10 ? "0" + bird.data.powerUpShieldsUsed.ToString() : bird.data.powerUpShieldsUsed.ToString());
    }

    public void TerryEvent(Bird bird)
    {
        try
        {

            switch (bird.data.birdEventState)
            {
                case TimedEventData.state.completedSuccess:
                    terryPageStats[5].transform.GetChild(2).GetComponent<Text>().text = "Terry returned to his family farm and helped repair the damage the vultures had done to it. He stayed there for some time before leaving the farm and heading out to travel the country.";
                    break;
                case TimedEventData.state.completedFail:
                    terryPageStats[5].transform.GetChild(2).GetComponent<Text>().text = "Terry made it in time but failed!";
                    break;
                case TimedEventData.state.failed:
                    terryPageStats[5].transform.GetChild(2).GetComponent<Text>().text = "Terry went to see his Uncle Keith where his parents had gone after the vultures destroyed their farm. Together they found a new patch of land where they built a new house, and Terry stayed there to farm the land like his father before him.";
                    break;
                default:
                    Debug.Log("none can be found");
                    terryPageStats[5].transform.GetChild(2).GetComponent<Text>().text = " terry: no event case can be found";
                    break;
            }
        }

        catch
        {
            Debug.Log("Terry: Timed Event List size is: " + Var.timedEvents.Count);
        }
    }

    void RebeccaEvent(Bird bird)
    {
        try
        {

            switch (bird.data.birdEventState)
            {
                case TimedEventData.state.completedSuccess:
                    rebeccaPageStats[5].transform.GetChild(2).GetComponent<Text>().text = "Rebecca left the country to find her parents, who had set up a new shop far away from the vultures. She learned about trading before becoming a travelling merchant, journeying across the country to sell her wares.";
                    break;
                case TimedEventData.state.completedFail:
                    rebeccaPageStats[5].transform.GetChild(2).GetComponent<Text>().text = "Rebecca made it in time but failed!";
                    break;
                case TimedEventData.state.failed:
                    rebeccaPageStats[5].transform.GetChild(2).GetComponent<Text>().text = "Rebecca left the country to find her parents, who had set up a new shop far away from the vultures. She learned the merchant trade and stayed with her family business.";
                    break;
                default:
                    Debug.Log("none can be found");
                    rebeccaPageStats[5].transform.GetChild(2).GetComponent<Text>().text = " rebecca: no event case can be found";
                    break;
            }
        }
        catch
        {
            Debug.Log("Rebecca: Timed Event List size is: " + Var.timedEvents.Count);
        }
    }

    void AlexanderEvent(Bird bird)
    {
        try
        {

            switch (bird.data.birdEventState)
            {
                case TimedEventData.state.completedSuccess:
                    alexanderPageStats[5].transform.GetChild(2).GetComponent<Text>().text = "Alex went back to Talonport, where he was greeted as a hero. He stayed there for some time to rebuild the army before traveling to the vulture kingdom to help organize the restoration of the vulture military.";
                    break;
                case TimedEventData.state.completedFail:
                    alexanderPageStats[5].transform.GetChild(2).GetComponent<Text>().text = "Alexander made it in time but failed!";
                    break;
                case TimedEventData.state.failed:
                    alexanderPageStats[5].transform.GetChild(2).GetComponent<Text>().text = "Alex never went back to Talonport, even after the defeat of the vulture king and the military birds managed to take back the city. Instead he traveled to the vulture kingdom to seek out and convert any vulture king sympathizers still hiding.";
                    break;
                default:
                    Debug.Log("none can be found");
                    alexanderPageStats[5].transform.GetChild(2).GetComponent<Text>().text = "alexander: no event case can be found";
                    break;
            }
        }

        catch
        {
            Debug.Log("Alexander: Timed Event List size is: " + Var.timedEvents.Count);
        }
    }

    void KimEvent(Bird bird)
    {
        try
        {

            switch (bird.data.birdEventState)
            {
                case TimedEventData.state.completedSuccess:
                    kimPageStats[5].transform.GetChild(2).GetComponent<Text>().text = "Kim traveled to her parents, where Percy was waiting for her. Together they moved back to their house at the border between the bird and the vulture kingdom, where they could run their hat shop together.";
                    break;
                case TimedEventData.state.completedFail:
                    kimPageStats[5].transform.GetChild(2).GetComponent<Text>().text = "Kim made it in time but failed!";
                    break;
                case TimedEventData.state.failed:
                    kimPageStats[5].transform.GetChild(2).GetComponent<Text>().text = "Kim left in search of her boyfriend, Percy. Once the vulture king was defeated, the birds and vultures he imprisoned were released, and among them were Percy. He and Kim left the country, traveling far away to a place where they would both be accepted.";
                    break;
                default:
                    Debug.Log("none can be found");
                    kimPageStats[5].transform.GetChild(2).GetComponent<Text>().text = " kim: no event case can be found";
                    break;
            }
        }

        catch
        {
            Debug.Log("Kim: Timed Event List size is: " + Var.timedEvents.Count);
        }
    }

    void SophieEvent(Bird bird)
    {
        try
        {

            switch (bird.data.birdEventState)
            {
                case TimedEventData.state.completedSuccess:
                    sophiePageStats[5].transform.GetChild(2).GetComponent<Text>().text = "Sophie returned to Lonely Peaks to ensure that the library was safe and protected. She then travelled to the vulture kingdom to help the vultures find their true emotions again and restore the balance within the kingdom.";
                    break;
                case TimedEventData.state.completedFail:
                    sophiePageStats[5].transform.GetChild(2).GetComponent<Text>().text = "Sophie made it in time but failed!";
                    break;
                case TimedEventData.state.failed:
                    sophiePageStats[5].transform.GetChild(2).GetComponent<Text>().text = "Sophie returned to Lonely Peaks to help rebuild the library and rewrite the books that had been destroyed. It was a long and arduous task, one which would likely take the rest of Sophie’s life to complete.";
                    break;
                default:
                    Debug.Log("none can be found");
                    sophiePageStats[5].transform.GetChild(2).GetComponent<Text>().text = "sophie: no event case can be found";
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

        var birdList = Var.activeBirds;
        int birdCount = 0;
        foreach(Bird bird in Var.activeBirds)
        {
            if(bird.gameObject.activeSelf)
            {
                birdCount++;
            }

        }
        if (birdCount == 5)
        {
            //all birds are unlocked do nothing
        }
        else if (birdCount == 4)
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
        //var birdList = Var.activeBirds.Count != 0 ? Var.activeBirds : Var.availableBirds;


        foreach (Bird bird in birdList)
        {

            try
            {
                switch (bird.charName)
                {
                    case "Terry":
                        Debug.Log("hello terry");
                        loadDataTerry(bird);
                        
                        break;
                    case "Rebecca":
                        loadDataRebecca(bird);
                        //RebeccaEvent(Var.timedEvents[1]);
                        break;
                    case "Alexander":
                        loadDataAlexander(bird);
                      //  AlexanderEvent(Var.timedEvents[2]);
                        break;
                    case "Kim":
                        if (Var.KimUnlocked)
                        {
                            loadDataKim(bird);
                            //KimEvent(Var.timedEvents[3]);
                        }
                        break;
                    case "Sophie":

                        if (Var.SophieUnlocked)
                        {
                            loadDataSophie(bird);
                           // SophieEvent(Var.timedEvents[4]);
                        }
                        break;
                    default:
                        Debug.Log("cant find bird");
                        break;

                }
            }
            catch(System.Exception ex)
            {
                Debug.Log("i am null " + Var.activeBirds.Count +" Exception: " + ex.Message);
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
