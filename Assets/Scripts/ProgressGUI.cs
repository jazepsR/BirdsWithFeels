using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressGUI : MonoBehaviour {
	public Image portrait;
	public Image portraitFill;
	public Text NameText;
	public LevelArea[] levelAreas;
	public Image[] Hearts;	
    public LVLIconScript[] LvlIcons;  
	public static ProgressGUI Instance { get; private set; }
	public GameObject skillArea;
    public RectTransform lvlListParent;
    public levelElementFill[] lvlListElements;
    
	// Use this for initialization
	void Start () {
		Instance = this;        
        Setup();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void Setup()
    {
        for (int i = 0; i < Var.activeBirds.Count; i++)
        {
            /*portraits[i].sprite = Var.activeBirds[i].portrait.transform.Find("bird").GetComponent<Image>().sprite;
            portraitFill[i].sprite = Var.activeBirds[i].portrait.transform.Find("bird_color").GetComponent<Image>().sprite;
            portraitFill[i].color = Helpers.Instance.GetEmotionColor(Var.activeBirds[i].emotion);
            names[i].text = Var.activeBirds[i].charName;*/
        }
    }
	void UpdateLevelAreas(Bird bird)
	{
		bool HasSuper = (bird.level > 1);
		foreach(LevelArea lvl in levelAreas)
		{
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
                if(!HasSuper)
				    lvl.Lock();
                if (!Helpers.Instance.ListContainsEmotion(lvl.emotion, bird.levelList))
                    lvl.Lock();

			}
		}
	}
	public void updateHearts(Image[] hearts, int index)
	{
        Helpers.Instance.setHearts(hearts, Var.activeBirds[index].health, Var.activeBirds[index].maxHealth);
			
		
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


	
	public void PortraitClick(Bird bird)
	{
        
        NameText.text = bird.charName;
        Helpers.Instance.setHearts(Hearts, bird.health, bird.maxHealth);
        bird.portrait.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
        bird.portrait.transform.Find("bird").GetComponent<Image>().color = Color.white;				
		portraitFill.color = Helpers.Instance.GetEmotionColor(bird.emotion);
        portraitFill.sprite = bird.portrait.transform.Find("bird_color").GetComponent<Image>().sprite;
        portrait.sprite = bird.portrait.transform.Find("bird").GetComponent<Image>().sprite;
        updateLevels(bird);
        if (!bird.inMap)
        {            
            UpdateLevelAreas(bird);
            skillArea.SetActive(false);
        }
    }


	public void ShowAllPortraits()
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
	}
 
	


	public void SmallPortraitClick()
	{

	}
}

