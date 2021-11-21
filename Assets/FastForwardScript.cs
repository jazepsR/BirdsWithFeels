using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FastForwardScript : MonoBehaviour
{

    public Animator myAnimator;
    public Animator ReadyButtonAnimator; 
    public Button mySpeedUpButton;
    private bool myIsInFight;
    public float TimeScaleWhenSpeedingUp = 4f;
    private bool SpeedButtonIsHeld;

    // Start is called before the first frame update
    void Start()
    {
        SetIsInFight(false);
    }

    // Update is called once per frame
    void Update()
    {
        //         Debug.Log(myIsInFight);
        if (Input.GetKey(KeyCode.LeftShift) && Var.cheatsEnabled) //Slow down time using cheats
        {
            Time.timeScale = 0.25f;

        }
        else if ((Input.GetKey(KeyCode.RightShift) || Input.GetMouseButton(1) || SpeedButtonIsHeld) && (myIsInFight == true && !GuiContoler.Instance.GraphActive))
        {
            SetGameSpeedUp(true);

        }
        else
        {
            SetGameSpeedUp(false);
        }

        /*if (myIsInFight) //Checks every frame if the fight is over - there's probably a better way to do this 
        {
            SetIsInFight(false);
        }*/
    }

    private void SetGameSpeedUp(bool fast)
    {
        if (fast)
        {
            Time.timeScale = TimeScaleWhenSpeedingUp;
            myAnimator.SetBool("active", true);
        }
        else
        {
            Time.timeScale = 1f;
            myAnimator.SetBool("active", false);
        }
    }

    public void SetIsInFight(bool aIsFighting)
    {
        myIsInFight = aIsFighting;
        ReadyButtonAnimator.SetBool("fight", aIsFighting);

        myAnimator.SetBool("fighting", myIsInFight);
        mySpeedUpButton.interactable = aIsFighting;

    }


    public void MouseHoversButton()
    {
        myAnimator.SetBool("hover", true);
    }

    public void MouseExitsButton()
    {
        myAnimator.SetBool("hover", false);
    }

    public void MouseButtonDown() //is picked up in "update" function
    {
        
        SpeedButtonIsHeld = true;
        //do speed up things here :)
    }

    public void MouseButtonUp()
    {
        SpeedButtonIsHeld = false;
    }


}
