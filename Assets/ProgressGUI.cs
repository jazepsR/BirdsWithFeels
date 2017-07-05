using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressGUI : MonoBehaviour {
    public Image[] portraits;
    public Image[] portraitFill;
    public Text[] names;
    public LevelArea[] levelAreas;
    public Image[] firstHearts;
    public Image[] secondHearts;
    public Image[] thirdHearts;
    public static ProgressGUI Instance { get; private set; }
    public GameObject skillArea;
    // Use this for initialization
    void Start () {
        Instance = this;
		for(int i=0;i<Var.activeBirds.Count;i++)
        {
            portraits[i].sprite = Var.activeBirds[i].portrait.transform.Find("bird").GetComponent<Image>().sprite;
            portraitFill[i].sprite = Var.activeBirds[i].portrait.transform.Find("bird_color").GetComponent<Image>().sprite;
            portraitFill[i].color = Helpers.Instance.GetEmotionColor(Var.activeBirds[i].emotion);
            names[i].text = Var.activeBirds[i].charName;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void UpdateLevelAreas(Bird bird)
    {
        bool HasSuper = (bird.level > 1);
        foreach(LevelArea lvl in levelAreas)
        {
            lvl.gameObject.SetActive(true);
            lvl.Unlock();
            if (Helpers.Instance.ListContainsLevel(lvl.level, bird.levelList))
            {
                lvl.gameObject.SetActive(false);                
            }
            if(bird.lastLevel.emotion == lvl.emotion)
            {
                lvl.gameObject.SetActive(false);
            }
            if(lvl.level.ToString().Contains("2") && !HasSuper)
            {
                lvl.Lock();
            }
        }
    }
    public void updateHearts(Image[] hearts, int index)
    {
        for(int i = 0; i < hearts.Length; i++)
        {
            if (i < Var.activeBirds[index].health)
            {
                hearts[i].gameObject.SetActive(true);
            }else
            {
                hearts[i].gameObject.SetActive(false);
            }
        }
    }
    public void hideHearts()
    {
        foreach (Image heart in firstHearts)
            heart.gameObject.SetActive(false);
        foreach (Image heart in secondHearts)
            heart.gameObject.SetActive(false);
        foreach (Image heart in thirdHearts)
            heart.gameObject.SetActive(false);
    }

    public void Portrait1Click()
    {
        PortraitClick(0);
        
    }
    public void Portrait2Click()
    {
        PortraitClick(1);
        
    }
    public void Portrait3Click()
    {
        PortraitClick(2);
        
    }
    public void PortraitClick(int index)
    {
        hideHearts();
        for(int i = 0; i < 3; i++)
        {
            if (i == index)
            {
                
                Graph.Instance.portraits[i].transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(Var.activeBirds[i].emotion);
                Graph.Instance.portraits[i].transform.Find("bird").GetComponent<Image>().color = Color.white;
                Graph.Instance.portraits[i].GetComponent<Image>().color = new Color(0.686f, 0.584f, 0.78f);
                Graph.Instance.portraits[i].transform.Find("BirdName").gameObject.SetActive(true);
                portraits[i].color = Color.white;
                portraitFill[i].color = Helpers.Instance.GetEmotionColor(Var.activeBirds[i].emotion);
                UpdateLevelAreas(Var.activeBirds[i]);
            }
            else
            {
                Graph.Instance.portraits[i].transform.Find("bird_color").GetComponent<Image>().color = Color.gray;
                Graph.Instance.portraits[i].transform.Find("bird").GetComponent<Image>().color = new Color(1, 1, 1, 0.35f);
                Graph.Instance.portraits[i].GetComponent<Image>().color = Color.gray;
                Graph.Instance.portraits[i].transform.Find("BirdName").gameObject.SetActive(false);
                portraits[i].color = new Color(1, 1, 1, 0.35f);
                portraitFill[i].color = Color.gray;

            }
        }
        switch (index)
        {
            case 0:
                updateHearts(firstHearts, 0);
                break;
            case 1:
                updateHearts(secondHearts, 1);
                break;
            case 2:
                updateHearts(thirdHearts, 2);
                break;
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
            portraits[i].color = Color.white;
            portraitFill[i].color = Helpers.Instance.GetEmotionColor(Var.activeBirds[i].emotion);
            foreach(LevelArea level in levelAreas)
            {
                level.gameObject.SetActive(false);
            }
        }
        updateHearts(firstHearts, 0);
        updateHearts(secondHearts, 1);
        updateHearts(thirdHearts, 2);
    }

    


    public void SmallPortraitClick()
    {

    }
}

