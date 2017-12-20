using System.Collections;
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
	public GameObject noAbilities;
	public GameObject abilityHeading;
	public Image ConditionsBG;
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
		//bool HasSuper = (bird.level > 1);
		bool CanLevel = false;
		levelStuffText.text = "<b>" + bird.charName+"</b> must grow emotionally to gain new abilites";
		if(bird.levelList.Count ==0)
		{
			noAbilities.SetActive(true);
			abilityHeading.SetActive(false);
		}
		else
		{
			noAbilities.SetActive(false);
			abilityHeading.SetActive(true);
		}
		foreach (LevelArea lvl in levelAreas)
		{
			lvl.myImage.color = lvl.defaultColor;
			lvl.gameObject.SetActive(true);
			lvl.myImage.sprite = lvl.Default;
			lvl.Unlock();
			if (Helpers.Instance.ListContainsLevel(lvl.level, bird.levelList))
			{
				lvl.gameObject.SetActive(false);
			} else
			{
				if (lvl.gameObject.GetComponent<LevelArea>().isSmall)
					lvl.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(140, 650);
			}

			if (bird.bannedLevels == lvl.emotion)
				lvl.gameObject.SetActive(false);

			float emotionRequirement = 7;
			if (lvl.level.ToString().Contains("2"))
			{
				emotionRequirement = 10;
				if (!Helpers.Instance.ListContainsEmotion(lvl.emotion, bird.levelList))
					lvl.gameObject.SetActive(false);

			}
			if (Var.isTutorial || isFinal || !Var.gameSettings.shownLevelTutorial)
				lvl.gameObject.SetActive(false);
			if (lvl.gameObject.activeSelf && bird.emotion == lvl.emotion && Helpers.Instance.GetEmotionValue(bird, lvl.emotion) >= emotionRequirement)
			{
				CanLevel = true;
				lvl.gameObject.GetComponent<Animator>().SetBool("active", true);
				ConditionsBG.color = Helpers.Instance.GetSoftEmotionColor(lvl.emotion);
				levelStuffText.text = "<b>" + Helpers.Instance.GetLevelTitle(lvl.level) +
					"</b> available!<b>\nRequirements:</b>\n" + Helpers.Instance.GetLVLRequirements(lvl.level);
			}
			else
			{
				lvl.gameObject.GetComponent<Animator>().SetBool("active", false);
			}

			

		}
		ConditionsBG.transform.parent.gameObject.SetActive(CanLevel);
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
	public void scaleDownEmoHeaderPortrait()
	{
		LeanTween.scale(emoHeaderPortrait.transform.parent.GetComponent<RectTransform>(), Vector3.one * 1.3f, 1f).setEase(LeanTweenType.easeInBack);
		LeanTween.textAlpha(newEmotionHeader.rectTransform, 0.0f, 2.5f);
		LeanTween.scale(newEmotionHeader.rectTransform, Vector3.one * 2.3f, 2.5f).setEase(LeanTweenType.easeInOutQuad);
	}
	public void AllPortraitClick()
	{
		for(int i = 0; i < 3; i++)
		{
			PortraitClick(Var.activeBirds[i], i,false,false);
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
			lvlNumber.text = bird.level.ToString();
		}
		else
		{
			nameText = NameText[portraitNum];
			portrait = portraits[portraitNum];
			portraitFillObj = portraitFill[portraitNum];
		}
		emoHeader.SetActive(useEmoHeader);
		portraits[portraitNum].transform.parent.gameObject.SetActive(!useEmoHeader);
		nameText.text = bird.charName;
		emotionValuesText.text = Helpers.Instance.GetStatInfo(bird.confidence, bird.friendliness).Replace('\n', ' ');
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



		if (bird.injured && showDeath)
		{
			if(!bird.inMap)
				deathScreen.ShowDeathMenu(bird);
			Helpers.Instance.setHearts(activeHearts, 0, bird.maxHealth, bird.prevRoundHealth);
			portraitFillObj.color = new Color(0, 0, 0, 0);
			nameText.text += "- Injured";
		}
		else
		{
			Helpers.Instance.setHearts(activeHearts, bird.health, bird.maxHealth, bird.prevRoundHealth);
			portraitFillObj.sprite = bird.portrait.transform.Find("bird_color").GetComponent<Image>().sprite;
			if (bird.prevEmotion != bird.emotion && !bird.inMap)
			{
				AudioControler.Instance.PlaySound(AudioControler.Instance.newEmotion);
				if(useEmoHeader)
					LeanTween.scale(portrait.transform.parent.GetComponent<RectTransform>(), Vector3.one * 1.7f, 0.2f).setEase(LeanTweenType.linear).setOnComplete(scaleDownEmoHeaderPortrait);
				else
					LeanTween.scale(portrait.transform.parent.GetComponent<RectTransform>(), Vector3.one * 1.7f, 0.2f).setEase(LeanTweenType.linear).setOnComplete(scaleDownPortrait).setOnCompleteParam(portraitNum as object);
				portraitFillObj.color = Helpers.Instance.GetEmotionColor(bird.prevEmotion);
				bird.portrait.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.prevEmotion);
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
				bird.portrait.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
				portraitFillObj.color = Helpers.Instance.GetEmotionColor(bird.emotion);
			}
			bird.portrait.transform.Find("bird").GetComponent<Image>().color = Color.white;
			portrait.sprite = bird.portrait.transform.Find("bird").GetComponent<Image>().sprite;
			updateLevels(bird);
			if (!bird.inMap)
			{
				UpdateLevelAreas(bird);
				skillArea.SetActive(false);
			}
		}
	}
}

