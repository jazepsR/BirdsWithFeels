using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressGUI : MonoBehaviour {
	public Image portrait;
    public Image skillBG;
	public Image portraitFill;
	public Text NameText;
	public LevelArea[] levelAreas;
	public Image[] Hearts;	
    public LVLIconScript[] LvlIcons;  
	public static ProgressGUI Instance { get; private set; }
	public GameObject skillArea;
    public RectTransform lvlListParent;
    public levelElementFill[] lvlListElements;
    public Text newEmotion;
    public Sprite skull;
    public deathScreenManager deathScreen;
    public Text birdBio;
    Color skillDefaultCol;
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
   
	void UpdateLevelAreas(Bird bird)
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
                lvl.myImage.sprite = lvl.Completed;
                if(lvl.gameObject.GetComponent<LevelArea>().isSmall)
                    lvl.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(75, 525);
                break;
            }else
            {
                if (lvl.gameObject.GetComponent<LevelArea>().isSmall)
                    lvl.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(140, 650);
            }
			if(bird.lastLevel.emotion == lvl.emotion)
			{
				lvl.gameObject.SetActive(false);
			}
			if(lvl.level.ToString().Contains("2"))
			{
                print("two");
                if(!HasSuper)
				    lvl.Lock();
                if (!Helpers.Instance.ListContainsEmotion(lvl.emotion, bird.levelList))
                    lvl.gameObject.SetActive(false);              

			}
		}
        skillBG.color = skillDefaultCol;
	}
	
    public void updateLevels(Bird bird)
    {
        int index = 0;
       // lvlListParent.sizeDelta = new Vector2(110 * Var.activeBirds[id].levelList.Count, lvlListParent.rect.width);
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

    public void scaleDownPortrait()
    {
        LeanTween.scale(portrait.transform.parent.GetComponent<RectTransform>(), Vector3.one*1.3f, 1f).setEase(LeanTweenType.easeInBack);
        LeanTween.textAlpha(newEmotion.rectTransform, 0.0f, 2.5f);
        LeanTween.scale(newEmotion.rectTransform, Vector3.one * 2.3f, 2.5f).setEase(LeanTweenType.easeInOutQuad);
    }
	
	public void PortraitClick(Bird bird)
	{
        
        NameText.text = bird.charName;
        if (bird.inMap)
            birdBio.text = bird.birdBio;
        Helpers.Instance.setHearts(Hearts, bird.health, bird.maxHealth,bird.prevRoundHealth);
        //print("helat: "+ bird.health+ " prev: "+ bird.prevRoundHealth);
        if (bird.health <= 0)
        {
            if(!bird.inMap)
                deathScreen.ShowDeathMenu(bird);
            portrait.sprite = skull;
            portraitFill.color = new Color(0, 0, 0, 0);
            NameText.text += "- Dead";
        }
        else
        { 
            portraitFill.sprite = bird.portrait.transform.Find("bird_color").GetComponent<Image>().sprite;
            if (bird.prevEmotion != bird.emotion && !bird.inMap)
            {
                AudioControler.Instance.PlaySound(AudioControler.Instance.newEmotion);
                LeanTween.scale(portrait.transform.parent.GetComponent<RectTransform>(), Vector3.one * 1.7f, 0.2f).setEase(LeanTweenType.linear).setOnComplete(scaleDownPortrait);
                portraitFill.color = Helpers.Instance.GetEmotionColor(bird.prevEmotion);
                bird.portrait.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.prevEmotion);
                LeanTween.color(portraitFill.rectTransform, Helpers.Instance.GetEmotionColor(bird.emotion), 2.25f);
                Debug.Log("had emotional change! From: " + bird.prevEmotion.ToString() + " to: " + bird.emotion.ToString());
                newEmotion.color = Helpers.Instance.GetEmotionColor(bird.emotion);
                newEmotion.rectTransform.localScale = Vector3.one;

            }
            else
            {
                bird.portrait.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
                portraitFill.color = Helpers.Instance.GetEmotionColor(bird.emotion);
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


	/*public void ShowAllPortraits()
	{
		skillArea.SetActive(false);
		for (int i = 0; i < 3; i++)
		{
				
			Graph.Instance.portraits[i].transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(Var.activeBirds[i].emotion);
			Graph.Instance.portraits[i].transform.Find("bird").GetComponent<Image>().color = Color.white;
			Graph.Instance.portraits[i].GetComponent<Image>().color = new Color(0.686f, 0.584f, 0.78f);
			Graph.Instance.portraits[i].transform.Find("BirdName").gameObject.SetActive(true);			
			foreach(LevelArea level in levelAreas)
			{
				level.gameObject.SetActive(false);
			}
		}      
	}*/
 
	


	public void SmallPortraitClick()
	{

	}
}

