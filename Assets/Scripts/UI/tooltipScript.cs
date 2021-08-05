using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tooltipScript : MonoBehaviour { 
	
	public Vector3 offset;
	//TODO: get screensize parametrically
	public Vector2 ScreenSize;
	public void LateUpdate()
	{
		SetPos();
	}  
	public void SetPos()
	{
		var screenPoint = Input.mousePosition + offset;
		screenPoint.z = 10.0f;
		GuiContoler.Instance.tooltipText.color = new Color(0, 0, 0, 0);
		GuiContoler.Instance.tooltipText.transform.parent.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
		gameObject.GetComponent<Image>().enabled = true;
		gameObject.GetComponentInChildren<Text>().color = Color.black;
		var pos = transform.localPosition;
		RectTransform me = gameObject.GetComponent<RectTransform>();

		var distPastX = me.anchoredPosition.x + me.sizeDelta.x - ScreenSize.x;
		if (distPastX > 0)
		{
			pos = new Vector3(pos.x - distPastX, pos.y, pos.z);
			//print("did x");
		}
		var distPastY = me.anchoredPosition.y + me.sizeDelta.y - ScreenSize.y;
		if (distPastY > 0)
		{
			pos = new Vector3(pos.x, pos.y - distPastY, pos.z);
			//print("did y");
		}

		transform.localPosition = pos;   
	}

	public void DoShake()
	{
		transform.parent.parent.GetComponent<Animator>().SetTrigger("newline");
	}    
}
