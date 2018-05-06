using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public Sprite Completed;
	public Sprite Default;
	//public string LevelName;
	public Sprite skillImage;    
	public string SkillText;
	public string ConditionText;
	public string LoreText;
	public Text LevelNameHolder;
	public Image SkillImageHolder;
	public Text SkillTextHolder;
	public Text ConditionTextHolder;
	public Text LoreTextHolder;
	public Var.Em emotion;
	public Levels.type level;
	[HideInInspector]
	public bool isLocked = false;
	[HideInInspector]
	public Color defaultColor;
	public bool isSmall = false;
	Animator anim;
    public LevelBarScript levelBar;
    public LevelBits[] levelBits;
	// Use this for initialization
	void Start () {
		defaultColor = Helpers.Instance.GetSoftEmotionColor(emotion);
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		//Color col = Helpers.Instance.GetEmotionColor(emotion);
		AudioControler.Instance.PlaySound(AudioControler.Instance.expand);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		//TODO: add pointerColors 
	}
	/*public void OnPointerExit(PointerEventData eventData)
	{
		LevelNameHolder.text = "";
		SkillTextHolder.text = "";
		ConditionTextHolder.text = "";
		LoreTextHolder.text = "";
		SkillImageHolder.gameObject.SetActive(false);

	}*/
    void SetLevelBits(Bird bird)
    {
        if (levelBar == null)
            return;
        float factor = 16f;
        levelBar.maxPoints = levelBits.Length;
        levelBar.ClearPoints();
        foreach(LevelBits bit in levelBits)
        {
            if (!bird.recievedSeeds.Contains(bit.name))
            {
                GameObject toInstantiate = Helpers.Instance.seedFar;
                if(bird.emotion == bit.emotion)
                    toInstantiate = Helpers.Instance.seed;
                GameObject obj = Instantiate(toInstantiate,Graph.Instance.graphParent.transform);
                obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(-factor * bit.social, factor * bit.conf, 0);
                obj.name = bit.name;
                obj.GetComponent<ShowTooltip>().tooltipText = "Social: " + bit.social + "\nConfidence: " + bit.conf;
                //if(bird.friendliness == bit.social && bird.confidence == bit.conf)
				if(Vector2.Distance(new Vector2(bird.friendliness,bird.confidence),new Vector2(bit.social,bit.conf))<=2)
				{
                    LeanTween.delayedCall(1.7f, ()=>levelBar.AddPoints(bird));
                    obj.GetComponent<Image>().color = Color.yellow;
                    bird.recievedSeeds.Add(bit.name);
                    LeanTween.delayedCall(1f, () => LeanTween.move(obj, levelBar.transform.position, 0.7f).setEaseOutBack().setOnComplete(() => Destroy(obj)));
                }
                //obj.GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bit.emotion);

            }
            else
            {
                levelBar.AddPoints(bird);
            }
        }

    }
	public void SetAnimator(Bird bird, bool isFinal)
	{
		int exitement = 1;
		if (Helpers.Instance.ListContainsLevel(level, bird.levelList))
		{
			exitement = 0;
		}

		if (bird.bannedLevels == emotion)
			exitement = 0;

		float emotionRequirement = 7;
		if (level.ToString().Contains("2"))
		{
			emotionRequirement = 10;
			if (!Helpers.Instance.ListContainsEmotion(emotion, bird.levelList))
				exitement = 0;

		}
		if (Var.isTutorial || isFinal || !Var.gameSettings.shownLevelTutorial)
			exitement = 0;
		if (bird.emotion == emotion && Helpers.Instance.GetEmotionValue(bird, emotion) >= emotionRequirement && exitement != 0)
		{
			ProgressGUI.Instance.CanLevel = true;
			ProgressGUI.Instance.ConditionsBG.color = Helpers.Instance.GetSoftEmotionColor(emotion);
			exitement = 3;
			ProgressGUI.Instance.levelStuffText.text = "<b>" + Helpers.Instance.GetLevelTitle(level) +
				"</b> available!<b>\nRequirements:</b>\n" + Helpers.Instance.GetLVLRequirements(level);
		}
		else if(Helpers.Instance.GetEmotionValue(bird, emotion) >= emotionRequirement-2 && exitement!=0)
		{
			if(emotion == Var.Em.Cautious && bird.confidence <= -emotionRequirement +2)
				exitement = 2;
			if (emotion == Var.Em.Solitary && bird.friendliness <= -emotionRequirement + 2)
				exitement = 2;
			if (emotion == Var.Em.Social || emotion == Var.Em.Confident)
				exitement = 2;
		}
		anim.SetInteger("excitement", exitement);
        if(!isFinal)
            SetLevelBits(bird);
    }
}
