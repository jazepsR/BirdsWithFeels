﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(RectTransform))]
public class SliderSwitcher : MonoBehaviour
{

    private Slider _slider;
    private float _sliderSize;
    public float SliderScaler = 0.3f;
    public float offset = 0f;
    public Image fill;
    public Text num;
    public bool isConf;
    Color PositiveCol;
    Color NegativeCol;
    void Start()
    {
        if (isConf)
        {
            PositiveCol = Helpers.Instance.GetEmotionColor(Var.Em.Confident);
            NegativeCol = Helpers.Instance.GetEmotionColor(Var.Em.Scared);
        }else
        {
            PositiveCol = Helpers.Instance.GetEmotionColor(Var.Em.Friendly);
            NegativeCol = Helpers.Instance.GetEmotionColor(Var.Em.Lonely);
        }
        
        _slider = GetComponent<Slider>();
    }
    
    void UpdateSliderSense()
    {
        if (_sliderSize == 0)
        {
            _sliderSize = GetComponent<RectTransform>().rect.width;
            _sliderSize = _sliderSize*SliderScaler / (_slider.maxValue - _slider.minValue);
        }

        _slider.fillRect.rotation = new Quaternion(0, 0, 0, 0);
        _slider.fillRect.pivot = new Vector2(0, _slider.fillRect.pivot.y);
        if (_slider.value > 0)
        {
            _slider.fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _sliderSize * _slider.value*SliderScaler);
        }
        else
        {
            _slider.fillRect.Rotate(0, 0, 180);
            _slider.fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, -1 * _sliderSize * _slider.value*SliderScaler);
        }
        _slider.fillRect.localPosition = new Vector3(offset, 0, 0);
    }


    public void SetDist(int emotionStr)
    {
        
        if (emotionStr > 0)
            LeanTween.color(fill.rectTransform, PositiveCol, 0.1f);           
        else
            LeanTween.color(fill.rectTransform, NegativeCol, 0.1f);
        float currentVal = _slider.value;
        num.text = emotionStr.ToString();
        LeanTween.value(gameObject, SetVal, currentVal, (float)emotionStr, 0.1f);
       // LeanTween.value(slider.gameObject, setSliderVal, (float)prevFame, (float)Variables.playerStats.fame, 0.1f * fameGain / 2);
    }

    public void SetVal(float val)
    {
        _slider.value = val;
        UpdateSliderSense();
        
    }
}
