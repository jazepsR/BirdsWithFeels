using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour {
    public Image graphArea;
    public Image heart;
    public GameObject prevHeart;
    public Transform canvas;
    public int graphSize = 220;
    int multiplier;
    public Material mat;
    public static Graph Instance { get; private set; }
    
    // Use this for initialization
    void Start()
    {
        Instance = this;
        multiplier = graphSize / 15;
        
         
    }



     public void PlotFull(int prevX, int prevY,int currX,int currY,GameObject port)
    {
        
        GameObject preHeart = PlotPoint(prevX, prevY, prevHeart);
        GameObject tempHeart = PlotPoint(currX, currY, port);
        LineRenderer lr =preHeart.transform.gameObject.AddComponent<LineRenderer>();
		lr.SetPosition(0, new Vector3(preHeart.transform.position.x,preHeart.transform.position.y,0));
		lr.SetPosition(1, new Vector3(tempHeart.transform.position.x,tempHeart.transform.position.y,0));
        lr.SetWidth(0.045f, 0.045f);     
        lr.textureMode = LineTextureMode.Tile;
		lr.widthMultiplier = 3f;

        lr.material = mat;     



    } 
    GameObject PlotPoint(int x,int y, GameObject obj)
    {
        Vector2 corner = graphArea.rectTransform.anchoredPosition;
        Rect size = graphArea.rectTransform.rect;
        Vector2 max = graphArea.rectTransform.anchorMax;
        Vector2 offset = graphArea.rectTransform.offsetMax;
        GameObject heartt =Instantiate(obj,graphArea.transform);
        heartt.transform.localScale = new Vector3(0.45f, 0.45f, 0.45f);
        heartt.transform.localPosition = new Vector3(-x*21, y*21, 0);

        

        Canvas dummy = heartt.AddComponent<Canvas> ();
		dummy.overrideSorting = true;
		dummy.sortingOrder = 11;

        return heartt;     
    }
	// Update is called once per frame
	void Update () {
		
	}
}
