using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credits_special : MonoBehaviour
{

    public List<Credits_person> teamMembers;
    public List<Credits_person> IntroductorySlides;
    int activePerson = -1;
    private bool readyToContinue = false;
    public GameObject NextGameObject;



    public void PrepareTeamMembers()
    {
        if (teamMembers.Count > 1)
        {
            teamMembers = shuffleList(teamMembers);

        }

        foreach (Credits_person person in teamMembers)
        {
            person.gameObject.transform.position = this.gameObject.transform.position; //Make sure the credit pieces are centered! :) 

        }

        readyToContinue = true;

        teamMembers.Insert(0, IntroductorySlides[0]); //fuck it let's put the first slide in the start, to make it count as a team member.
        activePerson = 0;
        teamMembers[activePerson].ShowPerson(this);

    }

    private void Finished()
    {
        print("we did it!");
        NextGameObject.SetActive(true);
    }

    public void ContinueButtonClicked(Credits_person personWhoIsDone)
    {

        if (personWhoIsDone == teamMembers[activePerson])
        {
            teamMembers[activePerson].HidePerson();
            activePerson++;

           

            if (activePerson < teamMembers.Count)
            {
                teamMembers[activePerson].ShowPerson(this);

                if (teamMembers[activePerson].GetProgressText() != null)
                {
                    teamMembers[activePerson].GetProgressText().gameObject.SetActive(true);
                    teamMembers[activePerson].GetProgressText().text = activePerson.ToString() + "/6";
                }
            }
            else
            {
                StartCoroutine("WaitAndContinue", 1.5f);
            }
        }
    }

     IEnumerator WaitAndContinue(float seconds)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Finished();
    }

    private List<Credits_person> shuffleList(List<Credits_person> team)
    {
        List<Credits_person> tempList = new List<Credits_person>(team);
        List<Credits_person> shuffledList = new List<Credits_person>();
        Credits_person tempPerson;

        while (shuffledList.Count < team.Count)
        {
            int randWithinTeamRange = Random.Range(0, tempList.Count); //get number between 0 and the amount of remaining team members 
            tempPerson = tempList[randWithinTeamRange]; //set temp person to be selected remaining team member 
            shuffledList.Add(tempPerson); //Add newly found tem member to shuffled version of list 
            tempList.RemoveAt(randWithinTeamRange);//Remove selected team member from future rand 

        }
        return shuffledList;


    }

    // Start is called before the first frame update
    void Start()
    {
        NextGameObject.SetActive(false);
        PrepareTeamMembers();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
