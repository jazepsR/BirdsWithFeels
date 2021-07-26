﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressGUI : MonoBehaviour {
    [Header("emo header")]
    public GameObject emoHeader;
    public Image[] emoHeaderHearts;
    public Text emoHeaderName;
    public Image emoHeaderPortrait;
    public Image emoHeaderPortraitFill;
    public Text lvlNumber;
    public Text newEmotionHeader;
    public Text levelStuffText;
    [Header("summary")]
    public Image skillBG;
    public Image[] portraits;
    public Image[] portraitFill;
    public Text[] NameText;
    public LevelArea[] levelAreas;
    public Image[] Hearts;
    public Image[] Hearts2;
    public Image[] Hearts3;
    public Image[] mentalHearts;
    public LVLIconScript[] LvlIcons;  
    public static ProgressGUI Instance { get; private set; }
    public GameObject skillArea;
    public RectTransform lvlListParent;
    public levelElementFill[] lvlListElements;
    public Text[] newEmotion;
    public Sprite skull;
    public deathScreenManager deathScreen;
    public Text birdBio;
    Color skillDefaultCol;
    public GameObject levelArea;
    public Text emotionValuesText;
    public Image ConditionsBG;
    public GameObject mentalPainPopup;
    public GameObject injuryEndScreen;
    public Text injuryEndText;
    public string injuryEndStringSingular = " was injured and cannot continue";
    public string injuryEndStringPlural = " were injured and cannot continue";
    internal bool CanLevel;
    public Button nextGraphButton;
    public Button prevGraphButton;
    // Use this for initialization
    void Start () {
        Instance = this;
        try
        {
            deathScreen.HideDeathMenu();
            skillDefaultCol = skillBG.color;
        }
        catch { }
    }
    
    // Update is called once per frame
    void Update () {
        if (nextGraphButton)
        {
            nextGraphButton.interactable = GuiContoler.Instance.canChangeGraph;
        }
        if (prevGraphButton)
        {
            prevGraphButton.interactable = GuiContoler.Instance.canChangeGraph;
        }
    }
   
    void UpdateLevelAreas(Bird bird, bool isFinal =false)
    {
        //bool HasSuper = (bird.level > 1);
        CanLevel = false;
        foreach (LevelArea lvl in levelAreas)
        {
            lvl.SetAnimator(bird, isFinal);
        }
        ConditionsBG.transform.parent.gameObject.SetActive(CanLevel);
        skillBG.color = skillDefaultCol;
    }
    
    public void updateLevels(Bird bird)
    {
        return;
        int index = 0;
        foreach (levelElementFill element in lvlListElements)
        {
            if (bird.levelList.Count > index)
            {
                //element.FillLevel(bird.data.levelList[index]);
                element.gameObject.SetActive(true);
            }
            else
            {
                element.gameObject.SetActive(false);               
            }
            index++;
            
        }
       
    }

    public void scaleDownPortrait(object o)
    {
        int portraitNum = (int)o;
        LeanTween.scale(portraits[portraitNum].transform.parent.GetComponent<RectTransform>(), Vector3.one*1.3f, 1f).setEase(LeanTweenType.easeInBack);
        LeanTween.textAlpha(newEmotion[portraitNum].rectTransform, 0.0f, 2.5f);
        LeanTween.scale(newEmotion[portraitNum].rectTransform, Vector3.one * 2.3f, 2.5f).setEase(LeanTweenType.easeInOutQuad);
    }
    public void scaleDownEmoHeaderPortrait()
    {
        LeanTween.scale(emoHeaderPortrait.transform.parent.GetComponent<RectTransform>(), Vector3.one * 1.3f, 1f).setEase(LeanTweenType.easeInBack);
        LeanTween.textAlpha(newEmotionHeader.rectTransform, 0.0f, 2.5f);
        LeanTween.scale(newEmotionHeader.rectTransform, Vector3.one * 2.3f, 2.5f).setEase(LeanTweenType.easeInOutQuad);
    }

    public void ActivateDeathSummaryScreen()
    {
        injuryEndScreen.SetActive(true);
        LeanTween.delayedCall(0.5f, () => GuiContoler.Instance.canChangeGraph = true);
        emoHeader.SetActive(false);
        emotionValuesText.text = "";
        List<string> injuredBirdNames = GetInjuredBirdNameList();
        string injuryNames = string.Join(" and ", injuredBirdNames);
        string injuryString = injuryNames + (injuredBirdNames.Count>1?injuryEndStringPlural: injuryEndStringSingular);
        injuryEndText.text = injuryString;
    }

    private List<string> GetInjuredBirdNameList()
    {
        List<string> nameList = new List<string>();
        List<Bird> injuredBirds = GetInjuredBirdList();
        foreach (Bird bird in Var.activeBirds)
        {
            if (bird.data.injured)
            {
                nameList.Add(bird.charName);
            }
        }
        return nameList;
    }
    private List<string> GetInjuredBirdNameList(List<Bird> injuredBirds)
    {
        List<string> nameList = new List<string>();
        foreach (Bird bird in Var.activeBirds)
        {
            nameList.Add(bird.charName);
        }
        return nameList;
    }
    private List<Bird> GetInjuredBirdList()
    {
        List<Bird> injuredBirds = new List<Bird>();
        foreach(Bird bird in Var.activeBirds)
        {
            if(bird.data.injured)
            {
                injuredBirds.Add(bird);
            }
        }
        return injuredBirds;
    }

    public void AllPortraitClick()
    {
        for(int i = 0; i < 3; i++)
        {
            PortraitClick(Var.activeBirds[i], i,false,false);
        }
        //UpdateLevelAreas(Var.activeBirds[0], true);
        levelArea.SetActive(false);
    }

    public void SetOnePortrait()
    {
        foreach (Image portrait in portraits)
        {
            portrait.transform.parent.gameObject.SetActive(false);
        }
        levelArea.SetActive(true);
    }
    public void PortraitClick(Bird bird, int portraitNum=0,bool useEmoHeader = true, bool showDeath = true)
    {
        Text nameText;
        Image portrait;
        Image portraitFillObj;
        if (useEmoHeader)
        {
            nameText = emoHeaderName;
            portrait = emoHeaderPortrait;
            portraitFillObj = emoHeaderPortraitFill;
            lvlNumber.text = bird.data.level.ToString();
        }
        else
        {
            nameText = NameText[portraitNum];
            portrait = portraits[portraitNum];
            portraitFillObj = portraitFill[portraitNum];
        }
        injuryEndScreen.SetActive(false);
        emoHeader.SetActive(useEmoHeader);
        portraits[portraitNum].transform.parent.gameObject.SetActive(!useEmoHeader);
        nameText.text = bird.charName;
        emotionValuesText.text = Helpers.Instance.GetStatInfo(bird.data.confidence, bird.data.friendliness).Replace('\n', ' ');
        Image[] activeHearts;
        switch (portraitNum)
        {
            case 0:
                activeHearts = Hearts;
                break;
            case 1:
                activeHearts = Hearts2;
                break;
            case 2:
                activeHearts = Hearts3;
                break;
            default:
                activeHearts = Hearts;
                break;
        }
        if(useEmoHeader)
            activeHearts = emoHeaderHearts;


        portraitFillObj.sprite = bird.portrait.transform.Find("bg").Find("bird_color").GetComponent<Image>().sprite;
        portrait.sprite = bird.portrait.transform.Find("bg").Find("bird").GetComponent<Image>().sprite;

        if (bird.data.injured && showDeath)
        {
            if(!bird.inMap)
                deathScreen.ShowDeathMenu(bird);
            Helpers.Instance.setHearts(activeHearts, 0, bird.data.maxHealth, bird.prevRoundHealth);
           // portraitFillObj.color = new Color(0, 0, 0, 0);
            nameText.text += "- Injured";
            portraitFillObj.color = Helpers.Instance.GetEmotionColor(bird.emotion);

            AudioControler.Instance.PlaySound(AudioControler.Instance.birdInjuredPopup);
        }
        else
        {
            Helpers.Instance.setHearts(activeHearts, bird.data.health, bird.data.maxHealth, bird.prevRoundHealth);
            LeanTween.delayedCall(1f, () => SetMentalHearts(bird.data.mentalHealth, Var.maxMentalHealth, bird.prevRoundMentalHealth, bird));
            if(bird.hadMentalPain)
            {
                mentalPainPopup.SetActive(true);
                bird.hadMentalPain = false;

            }
            if (bird.prevEmotion != bird.emotion && !bird.inMap)
            {
                AudioControler.Instance.PlaySound(AudioControler.Instance.newEmotion);
                if(useEmoHeader)
                    LeanTween.scale(portrait.transform.parent.GetComponent<RectTransform>(), Vector3.one * 1.7f, 0.2f).setEase(LeanTweenType.linear).setOnComplete(scaleDownEmoHeaderPortrait);
                else
                    LeanTween.scale(portrait.transform.parent.GetComponent<RectTransform>(), Vector3.one * 1.7f, 0.2f).setEase(LeanTweenType.linear).setOnComplete(scaleDownPortrait).setOnCompleteParam(portraitNum as object);
                portraitFillObj.color = Helpers.Instance.GetEmotionColor(bird.prevEmotion);
                bird.portrait.transform.Find("bg").Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.prevEmotion);
                LeanTween.color(portraitFillObj.rectTransform, Helpers.Instance.GetEmotionColor(bird.emotion), 2.25f);
                Debug.Log("had emotional change! From: " + bird.prevEmotion.ToString() + " to: " + bird.emotion.ToString());
                if (useEmoHeader)
                {
                    newEmotionHeader.color = Helpers.Instance.GetEmotionColor(bird.emotion);
                    newEmotionHeader.rectTransform.localScale = Vector3.one;
                }
                else
                {
                    newEmotion[portraitNum].color = Helpers.Instance.GetEmotionColor(bird.emotion);
                    newEmotion[portraitNum].rectTransform.localScale = Vector3.one;
                }

            }
            else
            {
                bird.portrait.transform.Find("bg").Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
                portraitFillObj.color = Helpers.Instance.GetEmotionColor(bird.emotion);
            }
            bird.portrait.transform.Find("bg").Find("bird").GetComponent<Image>().color = Color.white;
            updateLevels(bird);
            if (!bird.inMap)
            {
                UpdateLevelAreas(bird);
                skillArea.SetActive(false);
            }
        }
    }
    void SetMentalHearts(int MHP, int maxMHP, int prevMHP, Bird bird)
    {
        for (int i = 0; i < mentalHearts.Length; i++)
        {

            Animator anim = mentalHearts[i].GetComponent<Animator>();
            anim.SetBool("indanger", false);
            if (bird.hadMentalPain)
            {
                anim.SetBool("active", true);
            }
            else
            {
                if (i < MHP)
                {
                    if (i >= prevMHP && prevMHP != -1)
                    {
                        anim.SetTrigger("gain");
                        LeanTween.delayedCall(0.3f, () => anim.SetBool("active", true)).setUseEstimatedTime(true);
                    }
                    else
                    {
                        anim.SetBool("active", true);
                    }
                }
                else
                {
                    if (i < prevMHP && prevMHP != -1)
                    {
                        anim.SetTrigger("lose");
                        LeanTween.delayedCall(0.3f, () =>
                        {
                            anim.SetBool("active", false);
                            AudioControler.Instance.loseMHP.Play();
                        }).setUseEstimatedTime(true);
                    }

                    if (i < maxMHP)
                    {
                        anim.SetBool("active", false);
                    }
                }
                if (i == MHP - 1 && (Mathf.Abs(bird.data.confidence) >= 12 || Mathf.Abs(bird.data.friendliness) >= 12))
                    anim.SetBool("indanger", true);

            }
        }
    }
}

