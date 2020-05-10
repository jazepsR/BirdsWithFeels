using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closePopup : MonoBehaviour {
	
	// Use this for initialization
	void Start () {
		
	}
	
	public void Hide()
	{
		gameObject.SetActive(false);
        GuiContoler.Instance.GraphBlocker.SetActive(false);
	}
	public void ShowSmallGraph()
	{
		GuiContoler.Instance.GraphBlocker.SetActive(false);
		GuiContoler.Instance.smallGraph.graphArea.transform.parent.gameObject.SetActive(true);
	}
}
