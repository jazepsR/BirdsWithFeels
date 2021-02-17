using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBarScript : MonoBehaviour {
	public Image levelBar;
	public GameObject heartIcon;
	public GameObject powerIcon;
	int currentPoints;
	[HideInInspector]
	public int maxPoints;
	public Levels.type type;
	public bool isSecond;
	ShowTooltip tooltipScript;
	public GameObject[] seedIndicators;
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
		AudioControler.Instance.PlaySound(AudioControler.Instance.emoBarFill);
		currentPoints++;
		Vector3 temp = levelBar.rectTransform.localScale;
		levelBar.rectTransform.localScale = temp;
		temp.y = currentPoints / (float)maxPoints;
		id = LeanTween.value(gameObject, (float val) => levelBar.rectTransform.localScale = new Vector3(temp.x, val, temp.z), (currentPoints - 1) / ((float)maxPoints), temp.y, 0.5f).setEaseOutQuad().setOnComplete(() =>
		{ AudioControler.Instance.PlaySound(AudioControler.Instance.levelUp);
		if (maxPoints == currentPoints)
		{
			LeanTween.delayedCall(Var.levelPopupDelay,()=> Helpers.ApplyLevel(bird));
		}
	}		
		).id;
		
		SetText(bird);
	}
	public void SetText(Bird bird, LevelDataScriptable data = null)
	{
		if (data != null)
		{
			heartIcon.SetActive(data.givesHeart);
			powerIcon.SetActive(data.givesPower);
			heartIcon.GetComponent<ShowTooltip>().tooltipText = bird.charName + " will gain more health!";
			powerIcon.GetComponent<ShowTooltip>().tooltipText = bird.charName + " will be stronger in confrontations with vultures";
		}
		tooltipScript.tooltipText = currentPoints + 
			" / " + maxPoints + " seeds collected\nCollect all seeds to become stronger!";
		for (int i = 0; i < seedIndicators.Length; i++)
		{
			seedIndicators[i].SetActive(i < maxPoints);
		}
	}
}
