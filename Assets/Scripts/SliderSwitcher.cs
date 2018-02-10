using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

[RequireComponent(typeof(Slider))]
[RequireComponent(typeof(RectTransform))]
public class SliderSwitcher : MonoBehaviour
{

    private Slider _slider;
    private float _sliderSize;
    public float SliderScaler = 0.3f;
    public float offset = 0f;
    public Image fill;
    public Image pos1Indicator;
    public Image pos2Indicator;
    public Image neg1Indicator;
    public Image neg2Indicator;
    public Text num;
    public bool isConf;
    Color PositiveCol;
    Color NegativeCol;
    Color FadedCol;
    Color FadedPosCol;
    Color FadedNegCol;
    void Start()
    {
        GetColor();
        _slider = GetComponent<Slider>();
        FadedCol = new Color(0.6f, 0.6f, 0.6f, 0.4f);
    }

    void UpdateSliderSense()
    {
        if (_sliderSize == 0)
        {
            _sliderSize = GetComponent<RectTransform>().rect.width;
            _sliderSize = _sliderSize * SliderScaler / (_slider.maxValue - _slider.minValue);
        }

        _slider.fillRect.rotation = new Quaternion(0, 0, 0, 0);
        _slider.fillRect.pivot = new Vector2(0, _slider.fillRect.pivot.y);
        if (_slider.value > 0)
        {
            _slider.fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _sliderSize * _slider.value * SliderScaler);
        }
        else
        {
            _slider.fillRect.Rotate(0, 0, 180);
            _slider.fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, -1 * _sliderSize * _slider.value * SliderScaler);
        }
        _slider.fillRect.localPosition = new Vector3(offset, 0, 0);
    }

    void GetColor()
    {
        if (isConf)
        {
            NegativeCol = Helpers.Instance.GetEmotionColor(Var.Em.Confident);
            PositiveCol = Helpers.Instance.GetEmotionColor(Var.Em.Cautious);           
        }
        else
        {
            NegativeCol = Helpers.Instance.GetEmotionColor(Var.Em.Social);
            PositiveCol = Helpers.Instance.GetEmotionColor(Var.Em.Solitary);
        }
        FadedPosCol = new Color(PositiveCol.r*0.8f, PositiveCol.g * 0.8f, PositiveCol.b* 0.8f, 0.65f);
        FadedNegCol = new Color(NegativeCol.r * 0.8f, NegativeCol.g * 0.8f, NegativeCol.b * 0.8f, 0.65f);
        SetColors(FadedPosCol, FadedNegCol);

    }

    public void SetDist(int emotionStr,Bird bird)
    {
        try
        {
            if (PositiveCol == null)
            {
                GetColor();
            }
            SetIndicators(bird,emotionStr);
        emotionStr = -emotionStr;
        if (emotionStr > 0)
            LeanTween.color(fill.rectTransform, PositiveCol, 0.1f);           
        else
            LeanTween.color(fill.rectTransform, NegativeCol, 0.1f);
    
        float currentVal = _slider.value;        
        LeanTween.value(gameObject, SetVal, currentVal, emotionStr, 0.1f);
        num.text = Mathf.Abs(emotionStr).ToString();
           
       
    }catch(Exception ex)
    {
            print(ex.Message);
    }
      
    }
    void SetIndicators(Bird bird,int emotionStr)
    {
        try
        {
            SetColors(FadedPosCol, FadedNegCol);
            if (bird.battleCount >= bird.battlesToNextLVL)
            {
                setState(EmotionBarScript.state.available);
                bool hasPos1 = false;
                bool hasNeg1 = false;
                if (isConf)
                {
                    hasPos1 = Helpers.Instance.ListContainsLevel(Levels.type.Brave1, bird.levelList);
                    hasNeg1 = Helpers.Instance.ListContainsLevel(Levels.type.Scared1, bird.levelList);

                }
                else
                {
                    hasPos1 = Helpers.Instance.ListContainsLevel(Levels.type.Friend1, bird.levelList);
                    hasNeg1 = Helpers.Instance.ListContainsLevel(Levels.type.Lonely1, bird.levelList);
                }

                if (emotionStr >= 7)
                {
                    pos1Indicator.GetComponent<EmotionBarScript>().currentState = EmotionBarScript.state.completed;
                }
                if (emotionStr >= 10)
                {
                    pos2Indicator.GetComponent<EmotionBarScript>().currentState = EmotionBarScript.state.completed;
                }
                if (emotionStr <= -7)
                {
                    neg1Indicator.GetComponent<EmotionBarScript>().currentState = EmotionBarScript.state.completed;
                }
                if (emotionStr >= -10)
                {
                    neg2Indicator.GetComponent<EmotionBarScript>().currentState = EmotionBarScript.state.completed;
                }
                pos1Indicator.gameObject.SetActive(!hasPos1);
                pos2Indicator.gameObject.SetActive(hasPos1);
                neg1Indicator.gameObject.SetActive(!hasNeg1);
                neg2Indicator.gameObject.SetActive(hasNeg1);
            }
            else
            {
                SetColors(FadedCol, FadedCol);
                setState(EmotionBarScript.state.unavailable);
            }
        }
        catch { }
    }

    public void SetColors(Color posCol, Color negCol)
    {
        try
        {
            pos1Indicator.color = negCol;
            pos2Indicator.color = negCol;
            neg1Indicator.color = posCol;
            neg2Indicator.color = posCol;
        }
        catch { }
    }

    public void setState(EmotionBarScript.state state)
    {
        pos1Indicator.GetComponent<EmotionBarScript>().currentState = state;
        pos2Indicator.GetComponent<EmotionBarScript>().currentState = state;
        neg1Indicator.GetComponent<EmotionBarScript>().currentState = state;
        neg2Indicator.GetComponent<EmotionBarScript>().currentState = state;
    }

    public void SetVal(float val)
    {
        _slider.value = val;
        UpdateSliderSense();
        
    }
}
