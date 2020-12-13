using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphPortraitScript : MonoBehaviour {
	
	LineRenderer lr;
	Vector3 firstPos;
	Vector3 intercept;
	Vector3 finish;
	Text activeText;
	Var.Em targetEmotion;
	bool inDangerZone = false;
	float factor = 16f;
	Graph parent;
	float delayBetweenTicks = 0.2f;
	float lastTickTime = 0;
	bool shouldHaveSound = true;
	void Starter () {
		try
		{
			lr = gameObject.AddComponent<LineRenderer>();
			lr.material = Resources.Load<Material>("mat");
			//lr.sortingOrder = 120;
			lr.sortingOrder = 3;
			lr.sortingLayerName = "Front";
			lr.SetWidth(0.045f, 0.045f);
			lr.textureMode = LineTextureMode.Tile;
			lr.widthMultiplier = 3f;
			firstPos = new Vector3(transform.position.x, transform.position.y, 0);
			lr.SetPosition(0, firstPos);
			lr.SetPosition(1, firstPos);
			if (Mathf.Abs(transform.localPosition.x / factor) > 12 || Mathf.Abs(transform.localPosition.y / factor) > 12)
				inDangerZone = true;
			else
				inDangerZone = false;
			print("pos:" + transform.localPosition);
            if (GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().SetBool("dangerzone", inDangerZone);
            }
			LeanTween.value(gameObject, MovePoint, transform.localPosition, finish, 1.35f);
		}
		catch
		{
			Debug.Log("graph starter error");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (parent != null && parent.dangerZoneHighlight != null)
			parent.dangerZoneHighlight.transform.position = transform.position;
		if ((Mathf.Abs(transform.localPosition.x / factor) >= 12 || Mathf.Abs(transform.localPosition.y / factor) >= 12) && !inDangerZone)
		{
			inDangerZone = true;
			if (GuiContoler.Instance.dangerFollowHighlight.gameObject.activeSelf)
			{
				AudioControler.Instance.enterDangerZone.Play();
			}
            if (GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().SetBool("dangerzone", inDangerZone);
            }
		}
		if ((Mathf.Abs(transform.localPosition.x / factor) < 12 && Mathf.Abs(transform.localPosition.y / factor) < 12) && inDangerZone)
		{
			inDangerZone = false;
			if (GuiContoler.Instance.dangerFollowHighlight.gameObject.activeSelf)
			{
				AudioControler.Instance.exitDangerZone.Play();
			}
			if (GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().SetBool("dangerzone", inDangerZone);
            }
		}

	}

	public void StartGraph(Vector3 target, Var.Em targetEmotion, Bird bird, Graph parent, bool shouldHaveSound = false)
	{
		this.parent = parent;
		LeanTween.delayedCall(0.5f, Starter);
		this.shouldHaveSound = shouldHaveSound;
		GuiContoler.Instance.clearSmallGraph();
		this.targetEmotion = targetEmotion;
		ShowTooltip info =gameObject.AddComponent<ShowTooltip>();
		finish = target * factor;
		if (Mathf.Abs(transform.localPosition.x / factor) > 12 || Mathf.Abs(transform.localPosition.y / factor) > 12)
		{
			info.tooltipText = "This bird is outside their <b>comfort zone</b>! Get them back in before they lose all mental HP and <b>take damage</b>!";
		}
		else if (Var.Infight)
		{
			info.tooltipText = "";
			try
			{
                //TODO: restore functionality
				//info.tooltipText = GuiContoler.Instance.CreateEmotionChangeText(bird).Substring(1);
			}
			catch { }
			if (info.tooltipText == "")
				info.tooltipText = Helpers.Instance.GetStatInfo((int)target.y, -(int)target.x);
		}
		else
			info.tooltipText = Helpers.Instance.GetStatInfo((int)target.y, -(int)target.x);    
			
	}
	void ResumeMovement()
	{
		try
		{
			targetEmotion = Var.Em.finish;
			LeanTween.value(gameObject, MovePoint, transform.localPosition, finish, 1.35f);
		}
		catch { }
	}

  
	public void MovePoint(Vector3 pos)
	{
		
		if (reachedTarget(-pos.x/16f, pos.y/16f))
		{
			LeanTween.pause(gameObject);
            if (GetComponent<Animator>() != null)
            {
                GetComponent<Animator>().SetTrigger("NewEmotion");
            }
			activeText = Instantiate(Resources.Load<GameObject>("prefabs/emotionText"), transform).GetComponent<Text>();
			activeText.color = Helpers.Instance.GetEmotionColor(targetEmotion);
			activeText.rectTransform.localScale = Vector3.one;
			//heartt.transform.Find("bird_color").GetComponent<Image>().color = Helpers.Instance.GetEmotionColor(bird.emotion);
			//LeanTween.scale(activeText.gameObject.GetComponent<RectTransform>(), Vector3.one * 1.7f, 0.2f).setEase(LeanTweenType.linear).setOnComplete(scaleDownText);
			LeanTween.color(transform.Find("bird_color").GetComponent<Image>().rectTransform, Helpers.Instance.GetEmotionColor(targetEmotion), 0.7f).setEaseInBack();                                
			LeanTween.delayedCall(0.7f,ResumeMovement);
		}else
		{
			transform.localPosition = new Vector3(pos.x, pos.y, 0);
			lr.SetPosition(1, (new Vector3(transform.position.x, transform.position.y, 0)));
			if(lastTickTime + delayBetweenTicks < Time.timeSinceLevelLoad && shouldHaveSound)
			{
				lastTickTime = Time.timeSinceLevelLoad;
				AudioControler.Instance.PlaySound(AudioControler.Instance.graphMove);
			}
		}
	   
	}

	public void scaleDownText()
	{
		LeanTween.scale(activeText.gameObject.GetComponent<RectTransform>(), Vector3.one * 1.3f, 0.5f).setEase(LeanTweenType.easeInBack);
		LeanTween.textAlpha(activeText.rectTransform, 0.0f, 0.8f);
	}
	bool reachedTarget(float friendliness, float confidence)
	{
		Var.Em emotion = Var.Em.Neutral;
		if (Mathf.Abs((float)confidence) > Mathf.Abs((float)friendliness))
		{
			if (confidence > 0)
			{

				//Confident
				if (confidence >= Var.lvl1-0.15f)
					emotion = Var.Em.Confident;

			}
			else
			{
				//Scared
				if (confidence <= -Var.lvl1+0.15f)
					emotion = Var.Em.Cautious;
			}

		}
		else
		{
			//Friendly or lonely
			if (friendliness > 0)
			{

				//friendly				
				if (friendliness >= Var.lvl1-0.15f)
					emotion = Var.Em.Social;
			}
			else
			{
				//Lonely				
				if (friendliness <= -Var.lvl1+0.15f)
					emotion = Var.Em.Solitary;
			}

		}
		return emotion.Equals(targetEmotion);
	}
}
