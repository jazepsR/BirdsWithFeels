using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossBattleVultureVisuals : MonoBehaviour
{
    public GameObject[] rightVultures;
    public GameObject[] leftVultures;

    public GameObject vultureKing;
    Animator vultureKingAnimator;

    public bool debugFinalBattleActive;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < leftVultures.Length; i++)
        {
            leftVultures[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < rightVultures.Length; i++)
        {
            rightVultures[i].gameObject.SetActive(false);
        }
        vultureKingAnimator = vultureKing.GetComponent<Animator>();
        vultureKing.SetActive(false);

        if(debugFinalBattleActive)
        {
            setupLastBattle();
        }
    }

    public void setupLastBattle()
    {

        vultureKing.SetActive(true);
        for (int i = 0; i < leftVultures.Length; i++)
        {
            leftVultures[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < rightVultures.Length; i++)
        {
            rightVultures[i].gameObject.SetActive(true);
        }
        vultureKing.SetActive(true);
    }

    public void vultureKingFistPumps()
    {
        vultureKingAnimator.SetTrigger("charge");
    }

    // Update is called once per frame
    void Update()
    {

        if (debugFinalBattleActive)
        { 
    if( Input.GetKeyDown(KeyCode.A))
        {
            addVulturesToLeftSide();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            vultureKingFistPumps();
        }
        }
    }

    
    void addVulturesToLeftSide()
    {


        for (int i = 0; i < leftVultures.Length; i++)
        {
            if(leftVultures[i].gameObject.activeInHierarchy==false)
            {
                leftVultures[i].gameObject.SetActive(true);
                break;
            }
            
        }

        for (int i = 0; i < rightVultures.Length; i++)
        {
            if (rightVultures[i].gameObject.activeInHierarchy == true)
            {
                rightVultures[i].gameObject.SetActive(false);
                break;
            }

        }


    }

}
