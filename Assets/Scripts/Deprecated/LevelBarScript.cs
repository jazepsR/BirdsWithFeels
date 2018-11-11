using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBarScript : MonoBehaviour {
	public Image levelBar;
	int currentPoints;
	[HideInInspector]
	public int maxPoints;
	public Levels.type type;
	public bool isSecond;
	ShowTooltip tooltipScript;
	int id = -1;
	// Use this for initialization
	void Start () {
		LeanTween.cancel(id);
		tooltipScript = GetComponent<ShowTooltip>();
		gameObject.SetActive(Var.gameSettings.shownLevelTutorial);
	}
	public void ClearPoints()
	{
		levelBar.rectTransform.localScale = new Vector3(1, 0, 1);
		currentPoints = 0;
	}
	// Update is called once per frame
	void Update () {
		
	}
	public void AddPoints(Bird bird)
	{
		currentPoints++;
		Vector3 temp= levelBar.rectTransform.localScale;
		temp.y = currentPoints /(float) maxPoints;
		levelBar.rectTransform.localScale = temp;
		id =LeanTween.value(gameObject, (float val) => levelBar.rectTransform.localScale = new Vector3(temp.x, val, temp.z), (currentPoints - 1) / ((float)maxPoints), temp.y, 0.5f).setEaseOutQuad().id;
		if (maxPoints == currentPoints)
			Helpers.ApplyLevel(type, bird);
		SetText(bird);
	}
	public void SetText(Bird bird)
	{
		string color = Helpers.Instance.GetHexColor(Helpers.Instance.GetLevelEmotion(type));
		tooltipScript.tooltipText =color +Helpers.Instance.GetLevelEmotion(type).ToString() + " level</color>\n" + currentPoints + 
			" / " + maxPoints + " seeds collected\nCollect all seeds to become stronger!";
	}
}
