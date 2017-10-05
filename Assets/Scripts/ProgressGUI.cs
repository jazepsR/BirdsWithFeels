using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressGUI : MonoBehaviour {
	public Image[] portraits;
	public Image skillBG;
	public Image[] portraitFill;
	public Text[] NameText;
	public LevelArea[] levelAreas;
	public Image[] Hearts;
    public Image[] Hearts2;
    public Image[] Hearts3;
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
		
	}
   
	void UpdateLevelAreas(Bird bird, bool isFinal =false)
	{
		bool HasSuper = (bird.level > 1);
		foreach(LevelArea lvl in levelAreas)
		{
			lvl.myImage.color = lvl.defaultColor;
			lvl.gameObject.SetActive(true);
			lvl.myImage.sprite = lvl.Default;
			lvl.Unlock();
			if (Helpers.Instance.ListContainsLevel(lvl.level, bird.levelList))
			{
                lvl.gameObject.SetActive(false);
				/*lvl.myImage.sprite = lvl.Completed;
				if(lvl.gameObject.GetComponent<LevelArea>().isSmall)
					lvl.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(75, 525);
				break;*/
			}else
			{
				if (lvl.gameObject.GetComponent<LevelArea>().isSmall)
					lvl.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(140, 650);
			}
            /*if (bird.lastLevel.emotion == lvl.emotion || bird.bannedLevels == lvl.emotion)
			{
				lvl.gameObject.SetActive(false);
			}*/
            if ( bird.bannedLevels == lvl.emotion)           
               lvl.gameObject.SetActive(false);
           
            if (lvl.level.ToString().Contains("2"))
			{
				/*//print("two");
				if(!HasSuper)
					lvl.Lock();
				if (!Helpers.Instance.ListContainsEmotion(lvl.emotion, bird.levelList))
					lvl.gameObject.SetActive(false);         */     

			}
            if (Var.isTutorial || isFinal || Var.currentStageID == 0)
                lvl.gameObject.SetActive(false);
        }
		skillBG.color = skillDefaultCol;
	}
	
	public void updateLevels(Bird bird)
	{
		int index = 0;
		foreach (levelElementFill element in lvlListElements)
		{
			if (bird.levelList.Count > index)
			{
				element.FillLevel(bird.levelList[index]);
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
	
    public void AllPortraitClick()
    {
        for(int i = 0; i < 3; i++)
        {
            PortraitClick(Var.activeBirds[i], i);
        }
        UpdateLevelAreas(Var.activeBirds[0], true);
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
	public void PortraitClick(Bird bird, int portraitNum=0)
	{
        portraits[portraitNum].transform.parent.gameObject.SetActive(true);
		if (!bird.inMap)
		{
			bird.SetRelationshipText(GuiContoler.Instance.reportRelationshipPortrait, GuiContoler.Instance.reportRelationshipText);
			//bird.SetRelationshipSliders(GuiContoler.Instance.reportRelationshipSliders);
		}
		NameText[portraitNum].text = bird.charName;
		if (bird.inMap)
			birdBio.text = bird.birdBio;
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
		//print("helat: "+ bird.health+ " prev: "+ bird.prevRoundHealth);
		if (bird.dead)
		{
			if(!bird.inMap)
				deathScreen.ShowDeathMenu(bird);
			portraits[portraitNum].sprite = skull;
            Helpers.Instance.setHearts(activeHearts, 0, bird.maxHealth, bird.prevRoundHealth);
            portraitFill[portraitNum].color = new Color(0, 0, 0, 0);
			NameText[portraitNum].text += "- Dead";
        }
		else
		{
            Helpers.Instance.setHearts(activeHearts, bird.health, bird.maxHealth, bird.prevRoundHealth);
            portraitFill[portraitNum].sprite = bird.portrait.transform.Find("bird_color").GetComponent<Image>().sprite;
			if (bird.prevEmotion != bird.emotion && !bird.inMap)
			{
				AudioControler.Instance.PlaySound(AudioControler.Instance.newEmotion);
				LeanTween.scale(portraits[portraitNum].transform.parent.GetComponent<RectTransform>(), Vector3.one * 1.7f, 0.2f).setEase(LeanTweenType.linear).setOnComplete(scaleDownPortrait).setOnCompleteParam(portraitNum as object);
				portraitFill[portraitNum].color = Helpers.Instance.GetEmotionColor(bird.prevEmotion);
				bird.portrait.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.prevEmotion);
				LeanTween.color(portraitFill[portraitNum].rectTransform, Helpers.Instance.GetEmotionColor(bird.emotion), 2.25f);
				Debug.Log("had emotional change! From: " + bird.prevEmotion.ToString() + " to: " + bird.emotion.ToString());
				newEmotion[portraitNum].color = Helpers.Instance.GetEmotionColor(bird.emotion);
				newEmotion[portraitNum].rectTransform.localScale = Vector3.one;

			}
			else
			{
				bird.portrait.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
				portraitFill[portraitNum].color = Helpers.Instance.GetEmotionColor(bird.emotion);
			}
			bird.portrait.transform.Find("bird").GetComponent<Image>().color = Color.white;
			portraits[portraitNum].sprite = bird.portrait.transform.Find("bird").GetComponent<Image>().sprite;
			updateLevels(bird);
			if (!bird.inMap)
			{
				UpdateLevelAreas(bird);
				skillArea.SetActive(false);
			}
		}
	}
}

