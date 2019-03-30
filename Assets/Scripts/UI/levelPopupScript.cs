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
	public Dialogue firstLevelUpDialog;
	public Image skillIcon;
	[HideInInspector]
	public List<string> secondTextList;
	Sprite heart;
	Sprite sword;
	Bird activeBird;
	LevelData data;
	// Use this for initialization
	void Start () {
		Instance = this;
		heart = Resources.Load<Sprite>("Sprites/heart");
		sword = Resources.Load<Sprite>("Sprites/swords");
		//Setup(FillPlayer.Instance.playerBirds[1], new LevelData(Levels.type.Brave1, Var.Em.Confident, Var.lvlSprites[2]));
	}
	
	public void Setup(Bird bird, LevelData data)
	{
		this.data = data;
		activeBird = bird;
		firstPart.SetActive(true);
		secondPart.SetActive(false);
		thirdPart.SetActive(false);
		string name = activeBird.charName;
		title.text = "Character developement!";
		LevelPopup.SetActive(true);
		skillIcon.sprite = Helpers.Instance.GetLVLSprite(data.type);
		//First part
		//if (data.emotion == Var.Em.Scared || data.emotion == Var.Em.Lonely)
		{
			firstImage.sprite = sword;
			firstText.text = name + " gained +10% combat strength!";
		}

		//second part
		secondTextList = new List<string>();
		secondTextList.AddRange(Helpers.Instance.GetLevelUpText(name, data.type).Split('&'));
		secondText.text = secondTextList[0];
		secondTextList.RemoveAt(0);   
		secondImage.sprite = Helpers.Instance.GetSkillPicture(data.type);
		// Third part
		thirdText.text = Helpers.Instance.GetLVLInfoText(data.type);
		thirdImage.sprite = Helpers.Instance.GetSkillPicture(data.type);

	}
	public void FirstBtn()
	{
		if (secondTextList.Count == 0)
		{
			firstPart.SetActive(false);
			secondPart.SetActive(true);
			thirdPart.SetActive(false);
			title.text = "Getting stronger!";
		}
		else
		{
			secondText.text = secondTextList[0];
			secondTextList.RemoveAt(0);
		}	
	}
	public void SecondBtn()
	{
		if (activeBird.GainedLVLHealth)
		{
			activeBird.data.maxHealth++;
			activeBird.data.health++;
			ProgressGUI.Instance.PortraitClick(activeBird);
			firstImage.sprite = heart;
			firstText.text = "First level in a new emotion!\nGained +1 health!";
			activeBird.GainedLVLHealth = false;
		}
		else
		{
            ThirdBtn();
			/*title.text = "New skill: " + Helpers.Instance.GetLevelTitle(data.type);
			firstPart.SetActive(false);
			secondPart.SetActive(false);
			thirdPart.SetActive(true);
			//activeBird.levelList.Add(data);
			ProgressGUI.Instance.PortraitClick(activeBird);*/
		}
	}
	public void ThirdBtn()
	{
		firstPart.SetActive(false);
		secondPart.SetActive(false);
		thirdPart.SetActive(false);
		activeBird.hasNewLevel = false;
		LevelPopup.SetActive(false);
		if (DebugMenu.Instance.debugMenu.activeSelf)
			return;
		try
		{
			GuiContoler.Instance.CreateGraph(GuiContoler.Instance.currentGraph);
		}
		catch
		{
			Debug.LogError(" error in createGraph at levelPopupScript.");
		}
		
		if (!Var.gameSettings.shownFirstLevelUp)
		{
			DialogueControl.Instance.CreateParticularDialog(firstLevelUpDialog);
			Var.gameSettings.shownFirstLevelUp = true;
		}else
		{
			string[] texts = Helpers.Instance.GetLevelUpDialogs(data.type, Helpers.Instance.GetCharEnum(activeBird)).Split('&');
			foreach (string text in texts)
				GuiContoler.Instance.ShowSpeechBubble(Tutorial.Instance.portraitPoint, text,activeBird.birdSounds.birdTalk);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
