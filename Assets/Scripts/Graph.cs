using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour {
    public Image graphArea;
    public Image heart;
    public Image prevHeart;
    public Transform canvas;
    public int graphSize = 220;
    int multiplier;
    public Material mat;
    public static Graph Instance { get; private set; }
    // Use this for initialization
    void Start()
    {
        multiplier = graphSize / 15;
        PlotFull(3, 4, -5, -6,heart);        
    }



     public void PlotFull(int prevX, int prevY,int currX,int currY,Image port)
    {

        Image preHeart = PlotPoint(prevX, prevY, prevHeart);
        Image tempHeart = PlotPoint(currX, currY, port);
        LineRenderer lr =preHeart.transform.gameObject.AddComponent<LineRenderer>();
        lr.SetPosition(0, preHeart.rectTransform.transform.position);
        lr.SetPosition(1, tempHeart.rectTransform.transform.position);
        lr.SetWidth(0.05f, 0.05f);
        lr.material = mat;     



    } 
    Image PlotPoint(int x,int y, Image obj)
    {
        Vector2 corner = graphArea.rectTransform.anchoredPosition;
        Rect size = graphArea.rectTransform.rect;
        Vector2 max = graphArea.rectTransform.anchorMax;
        Vector2 offset = graphArea.rectTransform.offsetMax;
        Image heartt =Instantiate(obj,graphArea.transform);
        heartt.rectTransform.transform.localPosition = new Vector3(-x*14, y*14, 0);
        heartt.rectTransform.transform.localScale = new Vector3(1, 1, 1);
        return heartt;     
    }
	// Update is called once per frame
	void Update () {
		
	}
}
