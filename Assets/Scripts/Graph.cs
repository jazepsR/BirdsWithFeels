using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour {
    public Image graphArea;
    public Image heart;
    public GameObject prevHeart;
    public Transform canvas;
    public int graphSize = 275;
    int multiplier;   
    public static Graph Instance { get; private set; }
    public List<GameObject> portraits;
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
        Vector3 secondPos = new Vector3(-bird.friendliness * 22.2f, bird.confidence * 22.2f, 0);
       // new Vector3(tempHeart.transform.position.x, tempHeart.transform.position.y, 0);
        portraitScript.StartGraph(secondPos);       
    }
    


    GameObject PlotPoint(int x,int y, GameObject obj, bool isPortrait, Bird bird=null )
    {
        Vector2 corner = graphArea.rectTransform.anchoredPosition;
        Rect size = graphArea.rectTransform.rect;
        Vector2 max = graphArea.rectTransform.anchorMax;
        Vector2 offset = graphArea.rectTransform.offsetMax;
        GameObject heartt =Instantiate(obj,graphArea.transform);
        if (isPortrait)
        {
            heartt.transform.Find("BirdName").GetComponent<Text>().text = bird.charName;
            portraits.Add(heartt);
            if (bird.prevEmotion != bird.emotion)
            {
                heartt.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.prevEmotion);
                LeanTween.color(heartt.transform.Find("bird_color").GetComponent<Image>().rectTransform, Helpers.Instance.GetEmotionColor(bird.emotion), 1.35f);
            }else
            {
                heartt.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
            }

        }
        heartt.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        heartt.transform.localPosition = new Vector3(-x*22.2f, y*22.2f, 0);

        

        Canvas dummy = heartt.AddComponent<Canvas> ();
		dummy.overrideSorting = true;
		dummy.sortingOrder = 210;

        return heartt;     
    }
	// Update is called once per frame
	void Update () {
		
	}
}
