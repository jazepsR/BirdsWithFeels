using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoIndicator : MonoBehaviour
{
    public GameObject Background;
    public GameObject plusIcon;

    public GameObject socialIcon;
    public GameObject solitaryIcon;
    public GameObject confidentIcon;
    public GameObject cautiousIcon;
    public Sprite confidenceSprite;
    public Sprite cautiousSprite;
    public Sprite solitarySprite;
    public Sprite socialSprite;
    public Animator anim;
    //public SpriteRenderer firstIcon;
    //public SpriteRenderer secondIcon;
    public Bird bird;
    bool hidden = false;
    int hideCount = 0;
    // Start is called before the first frame update
    void Start()
    {
      confidenceSprite = confidentIcon.GetComponent<SpriteRenderer>().sprite;
      cautiousSprite= cautiousIcon.GetComponent<SpriteRenderer>().sprite;
        solitarySprite = solitaryIcon.GetComponent<SpriteRenderer>().sprite; 
      socialSprite = socialIcon.GetComponent<SpriteRenderer>().sprite; 
    }

    public void Hide()
    {
        
        anim.ResetTrigger("show");
        anim.SetTrigger("hide");
        hidden = true;
    }

    public void Show(Var.Em emo1, Var.Em emo2, bool setEmos = true)
    {
        anim.ResetTrigger("hide");
        anim.SetTrigger("show");
        if (setEmos)
        {
            SetEmotions(emo1, emo2);
        }
        hidden = false;
        hideCount++;
    }

    public void ResetLayer()
    {
        foreach (SpriteRenderer child in transform.GetComponentsInChildren<SpriteRenderer>(true))
        {

            child.sortingLayerName = "Default";
        }
    }

    public void SetEmotions(Var.Em emo1, Var.Em emo2)
    {
        socialIcon.SetActive(emo1 == Var.Em.Social);
        solitaryIcon.SetActive(emo1 == Var.Em.Solitary);
        confidentIcon.SetActive(emo2 == Var.Em.Confident);
        cautiousIcon.SetActive(emo2 == Var.Em.Cautious);
       //Debug.LogError(bird.charName + " emo1: " + emo1 + " emo2: " + emo2);
         if((emo1 == Var.Em.Neutral && emo2 == Var.Em.Neutral) || bird.target == bird.home)
         {
           //  Debug.Log("hiding");
             Hide();
         }
        else
         {
            //Debug.Log("showing");

            if (emo1 == Var.Em.Neutral && emo2 != Var.Em.Neutral)
            {
                if (emo2 == Var.Em.Confident)
                {
                    emo2 = Var.Em.Neutral;
                    confidentIcon.SetActive(false);
                    solitaryIcon.GetComponent<SpriteRenderer>().sprite = confidenceSprite;
                    solitaryIcon.SetActive(true);
                }

                if (emo2 == Var.Em.Cautious)
                {
                    emo2 = Var.Em.Neutral;
                    cautiousIcon.SetActive(false);
                    socialIcon.GetComponent<SpriteRenderer>().sprite = cautiousSprite;
                    socialIcon.SetActive(true);
                }

            }
            else
            {
                solitaryIcon.GetComponent<SpriteRenderer>().sprite = solitarySprite;
                socialIcon.GetComponent<SpriteRenderer>().sprite = socialSprite;
            }

                
                Show(emo1, emo2,false);
         }
    }

    public void Update()
    {
      //  if(bird.totalConfidence)
    }
    // Update is called once per frame
    public void RefreshEmotions()
    {
       
    }
}
