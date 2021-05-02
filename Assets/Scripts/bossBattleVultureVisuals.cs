using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossBattleVultureVisuals : MonoBehaviour
{
    public Animator myAnimator;

    public GameObject vultureKing;
    public Animator vultureKingAnimator;

    public bool debugFinalBattleActive;

    public Animator[] Vultures;

    // Start is called before the first frame update
    void Start()
    {
       
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

    public void addVulturesToLeftSide()
    {
        if (myAnimator != null)
        {
            myAnimator.SetTrigger("progress");
        }


    }
}





