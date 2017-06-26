using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class feedBack : MonoBehaviour {
    public TextMesh feedBackText;
	// Use this for initialization
	void Start () {
        feedBackText.transform.localScale = Vector3.zero;
	}
	
	public void ShowFeedback(float value)
    {
        feedBackText.gameObject.SetActive(true);
        float colorIndex = (value + 4.0f) / 8;

        Color textCol = Color.Lerp(Color.red, Color.green, colorIndex);
        LeanTween.color(feedBackText.gameObject, textCol, 0.2f);
        LeanTween.scale(feedBackText.gameObject, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutBack);
        feedBackText.text = ((int)value).ToString("+#;-#;0");
    }
    public void HideFeedBack()
    {
        LeanTween.scale(feedBackText.gameObject, Vector3.zero, 0.3f).setEase(LeanTweenType.easeInOutBack);

    }
}
