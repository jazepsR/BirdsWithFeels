using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class emoReportBit : MonoBehaviour
{
    public Text changeNumber;
    public Image emoIcon;
    public Text changeText;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetEmoBit(string number, Var.Em emo, string changeText)
    {
        changeNumber.text = number;
        changeNumber.color = Helpers.Instance.GetEmotionColor(emo);
        emoIcon.sprite = Helpers.Instance.GetEmotionIcon(emo,false);
        this.changeText.text = changeText;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
