using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjuryIndicator : MonoBehaviour
{
    public Bird bird;
    public TextMesh number;
    Animator anim;
    int prevInjury = 4;
    bool isInjured = false;
    bool isFinishingInjuryAnim;
    // Use this for initialization

    void Start()
    {
        if (bird == null)
            gameObject.SetActive(false);
        anim = GetComponent<Animator>();
        UpdateNumber();
    }

    // Update is called once per frame
    void Update()
    {

        if (bird.data.injured)
        {
            isInjured = true;
            gameObject.SetActive(true);
            if (prevInjury != bird.data.TurnsInjured)
            {
                anim.SetTrigger("Increment");

            }
        }
        else
        {
            if (isInjured && !isFinishingInjuryAnim)
            {
                isFinishingInjuryAnim = true;
                anim.SetTrigger("finish"); //Instead of turning off game object immedietly - we wait for closing anim to finish if it has been injured. 
                UpdateNumber();
            }
            else if(!isFinishingInjuryAnim)
            {
                HideInjuryIndicator();
            }
        }

        prevInjury = bird.data.TurnsInjured;

    }

    public void UpdateNumber() //Also called from animator 
    {
        number.text = bird.data.TurnsInjured.ToString();
    }

    public void HideInjuryIndicator()//called from animator 
    {
        isInjured = false;
        gameObject.SetActive(false);
        isFinishingInjuryAnim = false;
    }
}
