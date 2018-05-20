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
	// Use this for initialization
	void Start () {
		tooltipScript = GetComponent<ShowTooltip>();
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
        if (maxPoints == currentPoints)
            Helpers.ApplyLevel(type, bird);
		SetText(bird);
    }
	public void SetText(Bird bird)
	{
		string color = Helpers.Instance.GetHexColor(Helpers.Instance.GetLevelEmotion(type));
		tooltipScript.tooltipText =color+ Helpers.Instance.GetLevelTitle(type) + ":</color> " + currentPoints + " / " + maxPoints + " seeds collected\n" + Helpers.Instance.GetLVLInfoText(type);
	}
}
