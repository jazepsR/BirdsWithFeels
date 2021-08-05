using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class WinScreenData
{
    public EventScript.Character character;
    public GameObject birdObject;
    public Image birdFill;
}
public class WinScreen : MonoBehaviour
{
    public WinScreenData[] birdsOnWinScreenData;
    public static WinScreen Instance;

    public string TextWhenNormalTrial;
    public string TextWhenFirstTrial;
    public Sprite adventureComplete_normal;
    public Sprite adventureComplete_trial;
    public Image AdventureCompleteImage;
    public Text trialInfoText;
    public GameObject TrialInfoParent;

    public void Awake()
    {
        Instance = this;
    }

    public void SetupWinScreen(List<Bird> birdList, bool isTrial, bool isFirstTrial)
    {
        ResetWinScreen();
        PopulateWinScreen(birdList);
        SetupTrialWinscreen(isTrial, isFirstTrial);
    }

    private void PopulateWinScreen(List<Bird> birdList)
    {
        foreach (Bird bird in birdList)
        {
            EventScript.Character character = Helpers.Instance.GetCharEnum(bird);
            foreach (WinScreenData data in birdsOnWinScreenData)
            {
                if (data.character == character)
                {
                    data.birdObject.SetActive(true);
                  //  Debug.LogError(bird.charName + " conf: " + bird.data.confidence + " friend: " + bird.data.friendliness + " emotion: " + bird.emotion);
                    data.birdFill.color = Helpers.Instance.GetSoftEmotionColor(bird.emotion);
                }
            }
        }
    }

    void SetupTrialWinscreen(bool isTrial, bool isFirstTrial)
    {
        TrialInfoParent.SetActive(isTrial);

        if (isTrial)
        {
            AdventureCompleteImage.sprite = adventureComplete_trial;
        }
        else
        {
            AdventureCompleteImage.sprite = adventureComplete_normal;
            
        }

        if (isFirstTrial)
        {
            trialInfoText.text = TextWhenFirstTrial;
        }
        else
        {
            trialInfoText.text = TextWhenNormalTrial;
        }
    }

    private void ResetWinScreen()
    {
         foreach (WinScreenData data in birdsOnWinScreenData)
        {
            data.birdObject.SetActive(false);
        }
    }
}
