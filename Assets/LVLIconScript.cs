using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVLIconScript : MonoBehaviour {
    [HideInInspector]
    public string textToDsiplay;
	// Use this for initialization
	void Start () {
		
	}
	
	public void ShowTooltip()
    {
        GuiContoler.Instance.tooltipText.transform.parent.gameObject.SetActive(true);
        GuiContoler.Instance.tooltipText.text = textToDsiplay;
        var screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f; //distance of the plane from the camera        
        GuiContoler.Instance.tooltipText.transform.parent.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

    }
}
