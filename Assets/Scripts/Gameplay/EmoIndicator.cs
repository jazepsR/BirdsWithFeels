using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmoIndicator : MonoBehaviour
{
    public GameObject Background;
    public GameObject plusIcon;
    public GameObject heartIcon;
    public Transform heartDefaultPosition;

    public SpriteRenderer icon1;
    public SpriteRenderer icon2;


   /* public GameObject socialIcon;
    public GameObject solitaryIcon;
    public GameObject confidentIcon;
    public GameObject cautiousIcon;
    public Sprite confidenceSprite;
    public Sprite cautiousSprite;
    public Sprite solitarySprite;
    public Sprite socialSprite;*/
    public Animator anim;
    //public SpriteRenderer firstIcon;
    //public SpriteRenderer secondIcon;
    public Bird bird;
    bool hidden = false;
    int hideCount = 0;
    // Start is called before the first frame update
    void Start()
    {
    /*  confidenceSprite = confidentIcon.GetComponent<SpriteRenderer>().sprite;
      cautiousSprite= cautiousIcon.GetComponent<SpriteRenderer>().sprite;
        solitarySprite = solitaryIcon.GetComponent<SpriteRenderer>().sprite; 
      socialSprite = socialIcon.GetComponent<SpriteRenderer>().sprite; */
    }

    public void Hide()
    {
        //Debug.LogError("HIDING! bird: "+ bird.charName);
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
        if (Var.freezeEmotions)
        {
            emo1 = Var.Em.Neutral;
            emo2 = Var.Em.Neutral;
        }
        bool isResting = bird.GetComponentInChildren<Animator>().GetBool("rest");
        heartIcon.SetActive(isResting);
        //Debug.LogError("setting emotions! bird: "+ bird.charName);
        if ((emo1 == Var.Em.Neutral && emo2 == Var.Em.Neutral) || bird.target == bird.home)
        {
            SetIconEmotion(Var.Em.Neutral, icon1);
            SetIconEmotion(Var.Em.Neutral, icon2);
            heartIcon.transform.localPosition = icon1.transform.localPosition;
            if (isResting)
            {
                Show(emo1, emo2, false);
            }
            // Hide();
        }
        else
         {
            if (emo1 == Var.Em.Neutral && emo2 != Var.Em.Neutral)
            {
                SetIconEmotion(emo2, icon1);
                SetIconEmotion(Var.Em.Neutral, icon2);
                heartIcon.transform.localPosition = icon2.transform.localPosition;
            }
            else
            {
                SetIconEmotion(emo1, icon1);
                SetIconEmotion(emo2, icon2);
                heartIcon.transform.localPosition = heartDefaultPosition.localPosition;
            }
            Show(emo1, emo2,false);
         }
    }

    private void SetIconEmotion(Var.Em emotion, SpriteRenderer sr)
    {
        
       // sr.gameObject.SetActive(false);
       sr.gameObject.SetActive(emotion != Var.Em.Neutral);
       // Debug.Log("name: " + sr.name + " emo: " + emotion);
        if (emotion == Var.Em.Neutral)
        {
            return;
        }

        sr.sprite = Helpers.Instance.GetEmotionIcon(emotion, false);
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
