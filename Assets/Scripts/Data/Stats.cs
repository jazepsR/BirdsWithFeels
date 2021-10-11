using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stats : MonoBehaviour
{
    public bool isShowingStatsMenu = false;
    public GameObject statsMenu;
    public int currentPage;
    public int currentSection;
    public List<GameObject> listOfSections;
    public List<GameObject> listOfPagesInASection;
    private bool isLastPage;
    private bool isFirstPage;

    private GameObject sectionContainers;
    


    // Start is called before the first frame update
    void Start()
    {
        isShowingStatsMenu = true; //debug purposes

        if (statsMenu != null && isShowingStatsMenu)
        {
            foreach (Transform child in statsMenu.transform)
            {
                if (child.name == "Sections")
                {
                    sectionContainers = child.gameObject;
                    
                    break;
                }

            }

            foreach (Transform child in sectionContainers.transform)
            {
                listOfSections.Add(child.gameObject); // sections
            }

            refreshPageList(currentSection);

            goToPage(currentSection, currentPage); //first section, first page

            

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowStatsMenu()
    {
        isShowingStatsMenu = true;
        statsMenu.SetActive(true);
    }

    public void HideStatsMenu()
    {
        isShowingStatsMenu = false;
        statsMenu.SetActive(false);
        
    }

    public void refreshPageList(int aSection)
    {
        listOfPagesInASection.Clear();

        foreach (Transform child in listOfSections[aSection].transform)
        {
            listOfPagesInASection.Add(child.gameObject); // pages in a section
        }
    }
    public void NextPage()
    {
        //check if last page of section, if so - go to next one, if not loop back to the start. 
        if (currentPage == listOfPagesInASection.Count - 1)
        {
            try
            {
                listOfPagesInASection[currentPage].transform.gameObject.SetActive(false);
                goToPage(currentSection + 1, 0);
            }
            catch {
                listOfPagesInASection[currentPage].transform.gameObject.SetActive(false);
                goToPage(0, 0); //reset to first section and page
            }
        }

        else
        {
            //go to next page of section
            listOfPagesInASection[currentPage].transform.gameObject.SetActive(false);
            goToPage(currentSection, currentPage + 1);
        }
    }

    public void PreviousPage()
    {
        Debug.Log("try previous");
        if (currentPage == 0)
        {

            if (currentSection == listOfSections.Count - 1)
            {
                Debug.Log("LOL2");
                refreshPageList(listOfSections.Count - 2);
                goToPage(listOfSections.Count - 2, listOfPagesInASection.Count - 1);

            }
            else if (currentSection == 0) {
                refreshPageList(listOfSections.Count - 1);
                goToPage(listOfSections.Count - 1, listOfPagesInASection.Count - 1);
            }
            else {
                    refreshPageList(currentSection - 1);
                    goToPage(currentSection - 1, listOfPagesInASection.Count - 1);
                }
               
         
         
               // refreshPageList(currentSection - 1);
               // goToPage(listOfSections.Count - 1, listOfPagesInASection.Count - 1); 
            
        }

        else
        {
            //go to previous page of section
            Debug.Log("hello");
            goToPage(currentSection, currentPage - 1);
        }
    }

    public void exitStats()
    {
        if (SceneManager.GetSceneByName("Stats") == SceneManager.GetActiveScene())
        {
            SceneManager.LoadScene("MainMenu");
        }
        else {
            HideStatsMenu();
        }
    }

    public void goToPage(int section, int page)
    {
        //disables other sections and their pages and enables selected section
        
        try {
            
            if (!listOfSections[section].activeSelf)
            {
                foreach (GameObject aSection in listOfSections)
                {
                    if (aSection != listOfSections[section])
                    {
                        aSection.SetActive(false);

                        foreach (Transform aPage in aSection.transform)
                        {
                            aPage.gameObject.SetActive(false);
                        }
                    }
                }

                listOfSections[section].SetActive(true);
            }
        }

        catch { }

        //goes to select page
        
        listOfSections[section].transform.Find(listOfPagesInASection[page].name).gameObject.SetActive(true);

        currentPage = page;
        currentSection = section;

        Debug.Log("current section:" + currentSection + " currentPage: " + currentPage);

       


    }
}
