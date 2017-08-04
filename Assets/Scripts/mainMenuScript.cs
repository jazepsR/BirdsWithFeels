using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mainMenuScript : MonoBehaviour {
    public Text title;
    public Color titleColor2;
    Color titleColor;
    public Button ContinueBtn;
    void Start()
    {
        titleColor = title.color;
        TweenForward();
        if (Var.activeBirds.Count<1)
            ContinueBtn.interactable = false;

    }
    void TweenForward()
    {
        LeanTween.textColor(title.rectTransform, titleColor2, 2f).setEase(LeanTweenType.easeInBack).setOnComplete(TweenBack);
    }

    void TweenBack()
    {
        LeanTween.textColor(title.rectTransform, titleColor, 2f).setEase(LeanTweenType.easeInBack).setOnComplete(TweenForward);
    }
	public void ContinueClick()
    {
        Var.fled = true;
        SceneManager.LoadScene("Map");
    }

    public void StartClick()
    {
        ResetGame();
        Var.isTutorial = true;
        SceneManager.LoadScene("newMain");
    }
    public void SecretStartClick()
    {
        //TODO: add secret scene
    }
    public void Quit()
    {
        Application.Quit();
    }


    public void ResetGame()
    {
    Var.mapSaveData = new List<MapSaveData>(); 
    Var.activeBirds = new List<Bird>();
    Var.availableBirds = new List<Bird>();
    Var.map = new List<BattleData>();
    ContinueBtn.interactable = false;
    }
}
