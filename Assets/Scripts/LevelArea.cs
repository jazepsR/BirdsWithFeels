using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelArea : MonoBehaviour, IPointerEnterHandler
{
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
    Image myImage;
    public GameObject lockObj;
	// Use this for initialization
	void Awake () {
        myImage = GetComponent<Image>();
        defaultColor = new Color(1, 1, 1, 0.4f);
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
            SkillTextHolder.text = SkillText;
            ConditionTextHolder.text = "Requirements:\n" + ConditionText;
            LoreTextHolder.text = LoreText;
        }
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
