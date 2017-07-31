using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelPopupScript : MonoBehaviour {
	public static levelPopupScript Instance { get; private set; }
    public GameObject LevelPopup;
	public Text title;
	public Text firstText;
	public Image firstImage;
	public Text secondText;
	public Image secondImage;
	public Text thirdText;
    public Image thirdImage;
	public GameObject firstPart;
	public GameObject secondPart;
	public GameObject thirdPart;
    Sprite heart;
    Sprite sword;
	Bird activeBird;
    int birdNum;
	// Use this for initialization
	void Start () {
		Instance = this;
        heart = Resources.Load<Sprite>("Sprites/heart");
        sword = Resources.Load<Sprite>("Sprites/swords");
        //Setup(FillPlayer.Instance.playerBirds[1], new LevelData(Levels.type.Brave1, Var.Em.Confident, Var.lvlSprites[2]));
	}
	
	public void Setup(Bird bird, LevelData data,int birdNum)
	{
        this.birdNum = birdNum;
		activeBird = bird;
        firstPart.SetActive(true);
        secondPart.SetActive(false);
        thirdPart.SetActive(false);
        string name = activeBird.charName;
        title.text = name + " gained a level!";
        LevelPopup.SetActive(true);
        //First part
        if (data.emotion == Var.Em.Scared || data.emotion == Var.Em.Lonely)
        {
            firstImage.sprite = sword;
            firstText.text = name + " gained +10% combat strength!";
        }
        else
        {
            firstImage.sprite = heart;
            firstText.text = name + " gained +1 health!";
        }
        //second part
        secondText.text = Helpers.Instance.GetLevelUpText(name, data.type);
        secondImage.sprite = Helpers.Instance.GetSkillPicture(data.type);
        // Third part
        thirdText.text = Helpers.Instance.GetLVLInfoText(data.type);
        thirdImage.sprite = Helpers.Instance.GetSkillPicture(data.type);

    }
	public void FirstBtn()
	{
        firstPart.SetActive(false);
        secondPart.SetActive(true);
        thirdPart.SetActive(false);
    }
	public void SecondBtn()
	{
        firstPart.SetActive(false);
        secondPart.SetActive(false);
        thirdPart.SetActive(true);
    }
	public void ThirdBtn()
	{
        firstPart.SetActive(false);
        secondPart.SetActive(false);
        thirdPart.SetActive(false);
        activeBird.hasNewLevel = false;
        LevelPopup.SetActive(false);
        GuiContoler.Instance.CreateGraph(birdNum);
    }
	// Update is called once per frame
	void Update () {
		
	}
}
