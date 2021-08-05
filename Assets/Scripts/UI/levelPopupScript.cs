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
	public Image secondImage_lineart;
	public Image secondImage_fill;
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
	LevelDataScriptable data;
	bool healthGiven = false;
	public Dialogue levelCapDialogue;
	// Use this for initialization
	void Start () {
		Instance = this;
		heart = Resources.Load<Sprite>("Sprites/heart");
		sword = Resources.Load<Sprite>("Sprites/swords");
		
		//Setup(FillPlayer.Instance.playerBirds[1], new LevelData(Levels.type.Brave1, Var.Em.Confident, Var.lvlSprites[2]));
	}
	
	public void Setup(Bird bird, LevelDataScriptable data)
	{
		GuiContoler.Instance.GraphBlocker.SetActive(true);
		healthGiven = false;
		this.data = data;
		activeBird = bird;
		firstPart.SetActive(true);
		secondPart.SetActive(false);
		thirdPart.SetActive(false);
		string name = activeBird.charName;
		title.text = data.screenTitle;
		LevelPopup.SetActive(true);
		skillIcon.sprite = data.levelUpIcon;
		//First part
		if (data.givesPower)
		{
			firstImage.sprite = sword;
			firstText.text = name + " gained +10% diplomatic might!";
		}

		//second part
		secondTextList = new List<string>();
		secondTextList.AddRange(Helpers.Instance.ApplyTitle(activeBird, data.LevelUpText).Split('&'));
		secondText.text = secondTextList[0];
		secondTextList.RemoveAt(0);
		secondImage_fill.sprite = Helpers.Instance.GetPortrait(Helpers.Instance.GetCharEnum(bird)).transform.Find("bg/bird_color").GetComponent<Image>().sprite;
		secondImage_fill.color = Helpers.Instance.GetEmotionColor(bird.emotion);
		secondImage_lineart.sprite = Helpers.Instance.GetPortrait(Helpers.Instance.GetCharEnum(bird)).transform.Find("bg/bird").GetComponent<Image>().sprite;
		// Third part
		//thirdImage.sprite = Helpers.Instance.GetSkillPicture(data.type);

	}
	public void FirstBtn()
	{
		if (secondTextList.Count == 0)
		{
			firstPart.SetActive(false);
			secondPart.SetActive(true);
			thirdPart.SetActive(false);
			title.text = "Getting better!";
			AudioControler.Instance.levelUpSwordSound.Play();
		}
		else
		{
			secondText.text = secondTextList[0];
			secondTextList.RemoveAt(0);
		}
	}
	public void SecondBtn()
	{
		if (data.givesHeart && !healthGiven)
		{
			activeBird.data.maxHealth++;
			activeBird.data.health++;
			ProgressGUI.Instance.PortraitClick(activeBird);
			firstImage.sprite = heart;
			firstText.text = "Gained +1 health!";
			activeBird.GainedLVLHealth = false;
			healthGiven = true;
			AudioControler.Instance.levelUpHeartSound.Play();
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
		GuiContoler.Instance.GraphBlocker.SetActive(false);
		firstPart.SetActive(false);
		secondPart.SetActive(false);
		thirdPart.SetActive(false);
		activeBird.hasNewLevel = false;
		

        Animator popupAnim = LevelPopup.GetComponent<Animator>();
        popupAnim.SetTrigger("despawn");
		if (LevelBarScript.Instance.levelUpAnimator)
		{
			LevelBarScript.Instance.levelUpAnimator.SetBool("isLevellingUp", false);
		}
		if (activeBird.data.level == Var.maxLevel)
		{
			ShowLevelCapTutorial();
		}
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
			//string[] texts = Helpers.Instance.GetLevelUpDialogs(data.type, Helpers.Instance.GetCharEnum(activeBird)).Split('&');
			//foreach (string text in texts)
			//	GuiContoler.Instance.ShowSpeechBubble(Tutorial.Instance.portraitPoint, text,activeBird.birdSounds.GetTalkGroup(activeBird.emotion));
		}
		
		
	}

	public void ShowLevelCapTutorial()
	{
		if (!Var.gameSettings.shownLevelCapTutorial && Var.gameSettings.shownLevelTutorial)
		{
			DialogueControl.Instance.CreateParticularDialog(levelCapDialogue);
			Var.gameSettings.shownLevelCapTutorial = true;
			SaveLoad.Save(false);
		}
	}
	// Update is called once per frame
	void Update () {
		
	}
}
