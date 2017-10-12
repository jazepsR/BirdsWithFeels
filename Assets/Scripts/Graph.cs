using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour {
	public Image graphArea;
	public Image heart;
	public GameObject prevHeart;
    public GameObject graphParent;
	public Transform canvas;
	public int graphSize = 275;
	int multiplier;   
	public static Graph Instance { get; private set; }
	public List<GameObject> portraits;
    float factor = 22.4f;
	// Use this for initialization
	void Start()
	{
		Instance = this;
		Sprite sp = Resources.Load<Sprite>("Icons/NewIcons_1");
		multiplier = graphSize / 15;
	}
    
	 public void PlotFull(Bird bird)
	{
		if (bird.health <= 0)
			return;
		GameObject preHeart = PlotPoint(bird.prevFriend, bird.prevConf, prevHeart,false);
		GameObject tempHeart = PlotPoint(bird.prevFriend, bird.prevConf, bird.portrait,true,bird);        
		GraphPortraitScript portraitScript = tempHeart.transform.gameObject.AddComponent<GraphPortraitScript>();       
        Vector3 secondPos = new Vector3(-bird.friendliness, bird.confidence, 0);
        Var.Em emotion = bird.emotion;
        if (bird.prevEmotion == bird.emotion)
            emotion = Var.Em.finish;
		portraitScript.StartGraph(secondPos,emotion);       
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
			heartt.transform.Find("BirdName").GetComponent<Text>().text = bird.charName;
			portraits.Add(heartt);
            heartt.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.prevEmotion);
            Canvas dummy = heartt.AddComponent<Canvas>();
            dummy.overrideSorting = true;
            dummy.sortingLayerName = "Front";
            dummy.sortingOrder = 10;
            heartt.AddComponent<GraphicRaycaster>();
        }
		heartt.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
		heartt.transform.localPosition = new Vector3(-x*factor, y*factor, 0);
		return heartt;     
	}
	// Update is called once per frame
	void Update () {
		
	}
}
