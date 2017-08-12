using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite Completed;
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
    [HideInInspector]
    public Color defaultColor;
    [HideInInspector]
    public Image myImage;
    public bool isSmall = false;
	// Use this for initialization
	void Start () {
        myImage = GetComponent<Image>();
        defaultColor = Helpers.Instance.GetSoftEmotionColor(emotion);
        myImage.color = defaultColor;
        myImage.sprite = Default;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
        Color col = Helpers.Instance.GetEmotionColor(emotion);
        myImage.color = new Color(col.r,col.g,col.b,0.5f);
        Debug.Log("enterd!");
        ProgressGUI.Instance.skillBG.color = Helpers.Instance.GetSoftEmotionColor(emotion);
        ProgressGUI.Instance.skillArea.SetActive(true);        
        LevelNameHolder.text = LevelName;
        if (myImage.sprite.Equals(Completed))
            LevelNameHolder.text += " - Completed";
        SkillTextHolder.text = "Ability\n" +Helpers.Instance.GetLVLInfoText(level);
        ConditionTextHolder.text = "Requirements:\n" + Helpers.Instance.GetLVLRequirements(level);
        LoreTextHolder.text = LoreText;
        SkillImageHolder.sprite = Helpers.Instance.GetSkillPicture(level);
        SkillImageHolder.gameObject.SetActive(true);
        AudioControler.Instance.PlaySound(AudioControler.Instance.expand);
	}
    public void OnPointerExit(PointerEventData eventData)
    {
        myImage.color = defaultColor;
    }
    /*public void OnPointerExit(PointerEventData eventData)
    {
        LevelNameHolder.text = "";
        SkillTextHolder.text = "";
        ConditionTextHolder.text = "";
        LoreTextHolder.text = "";
        SkillImageHolder.gameObject.SetActive(false);

    }*/

    public void Lock()
    {
        isLocked = true;       
        myImage.color = Color.black;
    }
    public void Unlock()
    {
        isLocked = false;       
        myImage.color = defaultColor;
    }
}
