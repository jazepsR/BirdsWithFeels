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
	public float factor = 2f;
	public bool isSmall;
	public LevelBarScript levelBar;
	public Image dangerZoneHighlight;
	public GameObject dangerZoneParent;
	public Image lockImage;
	bool afterBattle = false;
	private float seedMoveTime = 0.4f;
	// Use this for initialization
	void Start()
	{
		if (!isSmall)
			Instance = this;
		if (isSmall && !Var.tutorialCompleted)
			graphArea.transform.parent.gameObject.SetActive(false);
		//Sprite sp = Resources.Load<Sprite>("Icons/NewIcons_1");
		multiplier = graphSize / 15;
		CheckEmotionLock();
	}

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.J) && Var.cheatsEnabled && !isSmall)
		{
			var level = Helpers.Instance.levels[Mathf.Min(Helpers.Instance.levels.Count - 1,
			   Var.activeBirds[0].data.level - 1)];

			foreach (LevelBits bit in level.levelBits)
			{
				if (!Var.activeBirds[0].data.recievedSeeds.Contains(bit.name))
				{
					CollectSeed(Var.activeBirds[0], bit);
					break;
				}

			}
		}
	}
	public void CheckEmotionLock()
	{
		if (isSmall && lockImage != null && Var.freezeEmotions)
		{
			lockImage.gameObject.SetActive(true);
		}
		else if (lockImage != null)
		{
			lockImage.gameObject.SetActive(false);
		}

	}
	public void PlotFull(Bird bird, bool afterBattle, bool shouldHaveSound = false)
	{
		if (!GuiContoler.Instance.inMap && GuiContoler.Instance.winBanner.activeSelf)
			return;
		if (bird.data.health <= 0)
		{
			return;
		}
		if (isSmall && Var.freezeEmotions)
			return;
		if (dangerZoneParent)
		{
			dangerZoneParent.SetActive(Var.gameSettings.useMHP);
		}
		this.afterBattle = afterBattle;
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
		{
			tempHeart = PlotPoint(bird.prevFriend, bird.prevConf, bird.portraitTiny, true, bird);
			GraphPortraitScript portraitScript = tempHeart.transform.gameObject.AddComponent<GraphPortraitScript>();
			Vector3 secondPos = new Vector3(-bird.data.friendliness, bird.data.confidence, 0);
			Var.Em emotion = bird.emotion;
			if (bird.prevEmotion == bird.emotion)
				emotion = Var.Em.finish;

			//Debug.LogError("plot full "+bird.charName + " prevconf: " + bird.prevConf + " conf: " + bird.data.confidence);
			//Debug.LogError(bird.charName + " prevfriend: " + bird.prevFriend + " friend: " + bird.data.friendliness);

			portraitScript.StartGraph(secondPos, emotion, bird, this, shouldHaveSound);
		}
		if ((GuiContoler.Instance.currentGraph == 3 && afterBattle && !GuiContoler.Instance.inMap) || !Var.gameSettings.shownLevelTutorial)
		{
			Debug.Log("not going to create seeds! current graph: "+ GuiContoler.Instance.currentGraph);
		}
		else
		{
			CreateLevelSeeds(bird, afterBattle);

		//	Debug.LogError("going to create seeds! current graph: " + GuiContoler.Instance.currentGraph);
		}

	}

	GameObject PlotPoint(int x, int y, GameObject obj, bool isPortrait, Bird bird = null)
	{
		Vector2 corner = graphArea.rectTransform.anchoredPosition;
		Rect size = graphArea.rectTransform.rect;
		Vector2 max = graphArea.rectTransform.anchorMax;
		Vector2 offset = graphArea.rectTransform.offsetMax;
		GameObject heartt = Instantiate(obj, graphParent.transform);
		if (isPortrait)
		{
			if (isSmall)
			{
				heartt.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
				ShowTooltip info = heartt.gameObject.AddComponent<ShowTooltip>();
				info.tooltipText = "<b>" + Helpers.Instance.GetStatInfo(y, x) + "</b>";
			}
			else
			{
				//heartt.transform.Find("BirdName").GetComponent<Text>().text = bird.charName;
				heartt.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.prevEmotion);
				ShowTooltip info = heartt.gameObject.AddComponent<ShowTooltip>();
				info.tooltipText = "<b>" + Helpers.Instance.GetStatInfo(y, x) + "</b>";
				//heartt.transform.Find("bg").Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.prevEmotion);
			}
			portraits.Add(heartt);
			Canvas dummy = heartt.AddComponent<Canvas>();
			dummy.overrideSorting = true;
			dummy.sortingLayerName = "Front";
			dummy.sortingOrder = 10;
			heartt.AddComponent<GraphicRaycaster>();
		}
		if (isSmall)
			heartt.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
		else
			heartt.transform.localScale = new Vector3(1f, 1f, 1f);
		heartt.transform.localPosition = new Vector3(-x * factor, y * factor, 0);
		return heartt;
	}
	void CreateLevelSeeds(Bird bird, bool afterBattle)
	{
		
		Debug.Log("SEEDS ARE BEING CREATED");
		float factor = isSmall ? 8 : 18;
		if (!isSmall)
		{
			SetupLevelBar(bird);
		}
		if (bird.data.level >= Var.maxLevel)
		{
			return;
		}
		LevelDataScriptable level = Helpers.Instance.levels[Mathf.Min(Helpers.Instance.levels.Count - 1,
		bird.data.level - 1)];

		foreach (LevelBits bit in level.levelBits)
		{
			if (!bird.data.recievedSeeds.Contains(bit.name))
			{

				GameObject toInstantiate = Helpers.Instance.seed;
				//if (bird.emotion == bit.emotion)
				//toInstantiate = Helpers.Instance.seed;
				GameObject obj = Instantiate(toInstantiate, graphParent.transform);
				obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(-factor * bit.social, factor * bit.conf, 0);
				obj.name = bit.name;
				obj.transform.localScale = Vector3.one * factor / 16f;
				obj.GetComponent<ShowTooltip>().tooltipText = Helpers.Instance.GetStatInfo(bit.conf, bit.social);

				//obj.GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bit.emotion);

			}
			else if (!isSmall)
			{
				levelBar.AddPoints(bird);
			}
		}
	}
	public void CheckIfCollectedSeed(Bird bird)
	{
		Debug.Log("SEEDS ARE BEING CHECKED IF COLLECTED");
		GuiContoler.Instance.canChangeGraph = true;
		if (graphParent.transform.childCount == 0)
			return;
		if (!afterBattle)
			return;
		if (bird.seedCollectedInRound)
			return;
		if (bird.data.injured)
			return;
		if (Var.isEnding || Var.isTutorial || Var.gameSettings.shownLevelTutorial == false)
			return; 
		if (bird.data.level >= Var.maxLevel)
		{
			return;
		}
		LevelDataScriptable level = Helpers.Instance.levels[Mathf.Min(Helpers.Instance.levels.Count - 1, bird.data.level - 1)];
		foreach (LevelBits bit in level.levelBits)
		{
			if (Vector2.Distance(new Vector2(bird.data.friendliness, bird.data.confidence), new Vector2(bit.social, bit.conf)) <= 3
					&& !isSmall && !bird.data.recievedSeeds.Contains(bit.name) && !bird.seedCollectedInRound)
			{
				CollectSeed(bird, bit);
				bird.seedCollectedInRound = true;
			}
		}
	}
	public void CollectSeed(Bird bird, LevelBits bit)
	{
		if(bird.data.level >= Var.maxLevel)
        {
			return;
        }
		Debug.Log("SEEDS ARE BEING COLLECTED");
		foreach (Transform seed in graphParent.transform)
		{
			if(seed.name == bit.name)
            {
				Destroy(seed.gameObject);
            }
		}

		AudioControler.Instance.PlaySound(AudioControler.Instance.collectEmoSeed);
		GameObject toInstantiate = Helpers.Instance.seed;
		GameObject obj = Instantiate(toInstantiate, graphParent.transform);
		obj.GetComponent<RectTransform>().anchoredPosition = new Vector3(-factor * bit.social, factor * bit.conf, 0);
		obj.name = bit.name;
		obj.transform.localScale = Vector3.one * factor / 16f;
		obj.GetComponent<ShowTooltip>().tooltipText = Helpers.Instance.GetStatInfo(bit.conf, bit.social);
		Instantiate(Helpers.Instance.pickupExplosion, obj.transform.position, Quaternion.identity, obj.transform.parent);
		LeanTween.delayedCall(1.7f, () => levelBar.AddPoints(bird));
		obj.GetComponent<Image>().color = Color.yellow;
		bird.data.recievedSeeds.Add(bit.name);
		LeanTween.delayedCall(1f, () => LeanTween.move(obj, levelBar.transform.position, seedMoveTime).setEaseOutBack().setOnComplete(() =>
		{
			LeanTween.value(gameObject, (float scale) => obj.transform.localScale = Vector3.one * scale, obj.transform.localScale.x, 0, 0.4f).
			setEaseOutBack().setOnComplete(() => Destroy(obj));
			Instantiate(Helpers.Instance.pickupExplosion, obj.transform.position, Quaternion.identity, obj.transform.parent);
		}
		));
		GuiContoler.Instance.canChangeGraph = false;
		if (GuiContoler.Instance.graphInteractTweenID != -1)
		{
			LeanTween.cancel(GuiContoler.Instance.graphInteractTweenID);
		}
		LeanTween.delayedCall(2.1f, () => GuiContoler.Instance.canChangeGraph = true);
	}
		
	public void SetupLevelBar(Bird bird)
	{
		levelBar.ClearPoints();
		if (bird.data.level >= Var.maxLevel)
		{
			levelBar.gameObject.SetActive(false);
		}
		else
		{
			levelBar.gameObject.SetActive(true);
			LevelDataScriptable level = Helpers.Instance.levels[Mathf.Min(Helpers.Instance.levels.Count - 1,
				 bird.data.level - 1)];
			levelBar.maxPoints = level.seedsNeeded;
			levelBar.SetText(bird, level);
		}
	}

}
