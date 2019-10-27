using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoIndicator : MonoBehaviour
{
    public GameObject parent;
    public GameObject plusIcon;

    public GameObject socialIcon;
    public GameObject solitaryIcon;
    public GameObject confidentIcon;
    public GameObject cautiousIcon;
    public Animator anim;
    //public SpriteRenderer firstIcon;
    //public SpriteRenderer secondIcon;
    public Bird bird;
    bool hidden = false;
    int hideCount = 0;
    // Start is called before the first frame update
    void Start()
    {

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

    public void SetEmotions(Var.Em emo1, Var.Em emo2)
    {
        socialIcon.SetActive(emo1 == Var.Em.Social);
        solitaryIcon.SetActive(emo1 == Var.Em.Solitary);
        confidentIcon.SetActive(emo2 == Var.Em.Confident);
        cautiousIcon.SetActive(emo2 == Var.Em.Cautious);
       //Debug.LogError(bird.charName + " emo1: " + emo1 + " emo2: " + emo2);
         if(emo1 == Var.Em.Neutral && emo2 == Var.Em.Neutral)
         {
           //  Debug.Log("hiding");
             Hide();
         }
        else
         {
            // Debug.Log("showing");
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
       // Helpers.Instance.GetEmotionIcon();
    }
}
