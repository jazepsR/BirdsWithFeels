using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class webpageOpener : MonoBehaviour {

	public string webpageLink;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void OpenWebpage()
	{
		Application.OpenURL(webpageLink);
	}
}
