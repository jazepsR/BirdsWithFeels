using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class levelElementFill : MonoBehaviour {
    public Text Description;
    public Image icon;
	// Use this for initialization
	void Start () {
		
	}
	public void FillLevel(LevelData data)
    {
        icon.sprite = data.LVLIcon;
        Description.text ="<b>"+ Helpers.Instance.GetLevelTitle(data.type)+"</b>\n"+ Helpers.Instance.GetLVLInfoText(data.type);
    }
}
