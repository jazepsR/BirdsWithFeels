using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour {
	public Image graphArea;
	public Image heart;
	public GameObject prevHeart;
	public GameObject graphParent;
	public int graphSize = 275;
	int multiplier;   
	public static Graph Instance { get; private set; }
	[HideInInspector]
	public List<GameObject> portraits;
	public float factor =2f;
	public bool isSmall;
	public LevelBarScript socialBar;
	public LevelBarScript solitaryBar;
	public LevelBarScript confidenceBar;
	public LevelBarScript cautiousBar;
	public Image dangerZoneHighlight;
	public Image lockImage;
	// Use this for initialization
	void Start()
	{
		if(!isSmall)
			Instance = this;
		if (isSmall && !Var.tutorialCompleted)
			graphArea.transform.parent.gameObject.SetActive(false);
		//Sprite sp = Resources.Load<Sprite>("Icons/NewIcons_1");
		multiplier = graphSize / 15;
		CheckEmotionLock();
	}
	public void CheckEmotionLock()
	{
		if(isSmall && lockImage!= null && Var.freezeEmotions)
		{
			lockImage.gameObject.SetActive(true);
		}
			else if(lockImage!= null)
		{
			lockImage.gameObject.SetActive(false);			
		}

	}
	public void PlotFull(Bird bird,bool afterBattle)
	{
		if (!GuiContoler.Instance.inMap && GuiContoler.Instance.winBanner.activeSelf)
			return;
		if (bird.data.health <= 0)
			return;
		if(isSmall && Var.freezeEmotions)
			return;
		//GameObject preHeart = PlotPoint(bird.prevFriend, bird.prevConf, prevHeart,false);
		GameObject tempHeart;
		if (isSmall)
		{
			tempHeart = PlotPoint(bird.prevFriend, bird.prevConf, bird.portraitTiny, true, bird);
			if (Time.timeSinceLevelLoad > 0.5f && GuiContoler.Instance.inMap)
				AudioControler.Instance.smallGraphAppear.Play();
			try
			{
				dangerZoneHighlight.transform.position = tempHeart.transform.position;
			}
			catch { }
		}
		else
			tempHeart = PlotPoint(bird.prevFriend, bird.prevConf, bird.portrait, true, bird);
		if (!isSmall)
		{		
			GraphPortraitScript portraitScript = tempHeart.transform.gameObject.AddComponent<GraphPortraitScript>();
			Vector3 secondPos = new Vector3(-bird.data.friendliness, bird.data.confidence, 0);
			Var.Em emotion = bird.emotion;
			if (bird.prevEmotion == bird.emotion)
				emotion = Var.Em.finish;
			portraitScript.StartGraph(secondPos, emotion, bird,this);
		}
		if ((GuiContoler.Instance.currentGraph == 3 && afterBattle) || !Var.gameSettings.shownLevelTutorial)
		{
		}
		else
			CreateLevelSeeds(bird, afterBattle);
		
	}
	
	GameObject PlotPoint(int x,int y, GameObject obj, bool isPortrait, Bird bird=null )
	{
		Vector2 corner = graphArea.rectTransform.anchoredPosition;
		Rect size = graphArea.rectTransform.rect;
		Vector2 max = graphArea.rectTransform.anchorMax;
		Vector2 offset = graphArea.rectTransform.offsetMax;
		GameObject heartt =Instantiate(obj,graphParent.transform);
		if (isPortrait)
		{
			if (isSmall)
			{
				heartt.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
				ShowTooltip info = heartt.gameObject.AddComponent<ShowTooltip>();
				info.tooltipText = "<b>" +Helpers.Instance.GetStatInfo(y,x) + "</b>";
			}
			else
			{
				heartt.transform.Find("BirdName").GetComponent<Text>().text = bird.charName;
				heartt.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.prevEmotion);
			}
			portraits.Add(heartt);
			Canvas dummy = heartt.AddComponent<Canvas>();
			dummy.overrideSorting = true;
			dummy.sortingLayerName = "Front";
			dummy.sortingOrder = 10;
			heartt.AddComponent<GraphicRaycaster>();
		}
		if(isSmall)
			heartt.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
		else
			heartt.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
		heartt.transform.localPosition = new Vector3(-x*factor, y*factor, 0);
		return heartt;     
	}
	void CreateLevelSeeds(Bird bird,bool afterBattle)
	{
		float factor =8f;
		if(!isSmall)
		{
			factor = 16;
			SetupLevelBar(Var.Em.Cautious, Helpers.Instance.ListContainsLevel(Levels.type.Scared1, bird.data.levelList),bird);
			SetupLevelBar(Var.Em.Confident, Helpers.Instance.ListContainsLevel(Levels.type.Brave1, bird.data.levelList), bird);
			SetupLevelBar(Var.Em.Social, Helpers.Instance.ListContainsLevel(Levels.type.Friend1, bird.data.levelList), bird);
			SetupLevelBar(Var.Em.Solitary, Helpers.Instance.ListContainsLevel(Levels.type.Lonely1, bird.data.levelList), bird);

		}
		foreach (LevelBits bit in Helpers.Instance.levelBits)
		{
			if (bird.data.bannedLevels != bit.emotion && Helpers.Instance.ListContainsEmotion(bit.emotion, bird.data.levelList) == bit.isSecond)
			{
				if (!bird.data.recievedSeeds.Contains(bit.name))
				{

					GameObject toInstantiate = Helpers.Instance.seedFar;
					if (bird.emotion == bit.emotion)
						toInstantiate = Helpers.Instance.seed;
					GameObject obj = Instantiate(toInstantiate, graphParent.transform);
					obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(-factor * bit.social, factor * bit.conf, 0);
					obj.name = bit.name;
					obj.transform.localScale = Vector3.one * factor / 16f;
					obj.GetComponent<ShowTooltip>().tooltipText = Helpers.Instance.GetStatInfo(bit.conf, bit.social);
					if (Vector2.Distance(new Vector2(bird.data.friendliness, bird.data.confidence), new Vector2(bit.social, bit.conf)) <= 3
						&& !isSmall && afterBattle)
					{
						Instantiate(Helpers.Instance.pickupExplosion, obj.transform.position, Quaternion.identity, obj.transform.parent);
						LeanTween.delayedCall(1.7f, () => GetLevelBar(bit.emotion).AddPoints(bird));
						obj.GetComponent<Image>().color = Color.yellow;
						bird.data.recievedSeeds.Add(bit.name);

						LeanTween.delayedCall(1f, () => LeanTween.move(obj, GetLevelBar(bit.emotion).transform.position, 0.7f).setEaseOutBack().setOnComplete(() =>
						{
							LeanTween.value(gameObject, (float scale) => obj.transform.localScale = Vector3.one * scale, obj.transform.localScale.x, 0, 0.4f).
							setEaseOutBack().setOnComplete(() => Destroy(obj));
							Instantiate(Helpers.Instance.pickupExplosion, obj.transform.position, Quaternion.identity, obj.transform.parent);
						}
						));						
						GuiContoler.Instance.canChangeGraph = false;
						LeanTween.delayedCall(2.1f, () => GuiContoler.Instance.canChangeGraph = true);
					}
					//obj.GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bit.emotion);

				}
				else if (!isSmall)
				{
					GetLevelBar(bit.emotion).AddPoints(bird);
				}
			}
		}
	}

	LevelBarScript GetLevelBar(Var.Em emotion)
	{
		switch(emotion)
		{
			case Var.Em.Cautious:
				return cautiousBar;				
			case Var.Em.Confident:
				return confidenceBar;
			case Var.Em.Social:
				return socialBar;
			case Var.Em.Solitary:
				return solitaryBar;
			default:
				return null;


		}
	}
	void SetupLevelBar(Var.Em emotion, bool isSecond,Bird bird)
	{
		GetLevelBar(emotion).ClearPoints();
		int count = 0;
		GetLevelBar(emotion).isSecond = isSecond;
		foreach (LevelBits levelBit in Helpers.Instance.levelBits)
		{
			if (levelBit.emotion == emotion && levelBit.isSecond == isSecond)
				count++;
		}
		GetLevelBar(emotion).maxPoints = count;
		GetLevelBar(emotion).SetText(bird);
	}

}
