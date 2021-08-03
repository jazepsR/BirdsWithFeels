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
    public void Awake()
    {
        Instance = this;
    }

    public void SetupWinScreen(List<Bird> birdList)
    {
        ResetWinScreen();
        PopulateWinScreen(birdList);
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
                    Debug.LogError(bird.charName + " conf: " + bird.data.confidence + " friend: " + bird.data.friendliness + " emotion: " + bird.emotion);
                    data.birdFill.color = Helpers.Instance.GetSoftEmotionColor(bird.emotion);
                }
            }
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
