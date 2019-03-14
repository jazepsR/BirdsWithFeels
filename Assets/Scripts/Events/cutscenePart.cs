using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutscenePart : MonoBehaviour {
    public Sprite image;
    [TextArea(2,10)]
    public List<string> cutsceneTexts;
	// Use this for initialization

	public cutscenePart(Sprite image, string cutsceneTexts)
	{
		this.image = image;
		string[] texts = cutsceneTexts.Split('&');
		this.cutsceneTexts = new List<string>(texts);
	}
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
