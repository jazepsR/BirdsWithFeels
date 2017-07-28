﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite Competed;
    public Sprite Default;
	public string LevelName;
	public Sprite skillImage;    
	public string SkillText;
	public string ConditionText;
	public string LoreText;
	public Text LevelNameHolder;
	public Image SkillImageHolder;
	public Text SkillTextHolder;
	public Text ConditionTextHolder;
	public Text LoreTextHolder;
    public Var.Em emotion;
    public Levels.type level;
    [HideInInspector]
    public bool isLocked = false;
    Color defaultColor;
    [HideInInspector]
    public Image myImage;
    public GameObject lockObj;
	// Use this for initialization
	void Start () {
        myImage = GetComponent<Image>();
        defaultColor = Helpers.Instance.GetEmotionColor(emotion);
        myImage.color = defaultColor;
        myImage.sprite = Default;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
        ProgressGUI.Instance.skillArea.SetActive(true);
        if (!isLocked)
        {
            LevelNameHolder.text = LevelName;
            if (myImage.sprite.Equals(Competed))
                LevelNameHolder.text += " - Completed";
            SkillTextHolder.text = Helpers.Instance.GetLVLInfoText(level);
            ConditionTextHolder.text = "Requirements:\n" + Helpers.Instance.GetLVLRequirements(level);
            LoreTextHolder.text = LoreText;
            SkillImageHolder.sprite = Helpers.Instance.GetSkillPicture(level);
            SkillImageHolder.gameObject.SetActive(true);
        }
	}
    public void OnPointerExit(PointerEventData eventData)
    {
        LevelNameHolder.text = "";
        SkillTextHolder.text = "";
        ConditionTextHolder.text = "";
        LoreTextHolder.text = "";
        SkillImageHolder.gameObject.SetActive(false);

    }

    public void Lock()
    {
        isLocked = true;
        if (lockObj != null)
            lockObj.SetActive(true);
        myImage.color = Color.black;
    }
    public void Unlock()
    {
        isLocked = false;
        if(lockObj!= null)
            lockObj.SetActive(false);
        myImage.color = defaultColor;
    }
}
