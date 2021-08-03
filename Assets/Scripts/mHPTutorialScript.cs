using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mHPTutorialScript : MonoBehaviour
{
    public Animator mHeart1;
    public Animator mHeart2;
    public Animator mHeart3; 

    public void OnClose()
    {
        FillPlayer.Instance.playerBirds[0].showText();
    }

    public void blinkHeart()
    {
        mHeart1.SetBool("indanger", true);
    }

    public void loseHeart()
    {
        mHeart1.SetBool("lose", true);
        mHeart1.SetBool("active", false);
    }

    public void gainHeart()
    {
        mHeart1.SetBool("gain", true);
        mHeart1.SetBool("indanger", false);
        mHeart1.SetBool("active", true);
    }

void    setHeartsStatus(bool active)
    {
        mHeart1.SetBool("active", active);
        mHeart2.SetBool("active", active);
        mHeart3.SetBool("active", active);
    }
    // Start is called before the first frame update
    void Start()
    {
        setHeartsStatus(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
