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
        
        ContinueBtn.interactable = SaveLoad.Load();

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
        SceneManager.LoadScene("NewMain");
    }
    public void SecretStartClick()
    {
        ResetGame();
        SceneManager.LoadScene("newMainDev");
    }
    public void Quit()
    {
        Application.Quit();
    }


    public void ResetGame()
    {
    SaveLoad.DeleteSave();
    Var.mapSaveData = new List<MapSaveData>(); 
    Var.activeBirds = new List<Bird>();
    Var.availableBirds = new List<Bird>();
    Var.map = new List<BattleData>();
    Var.isTutorial = false;
    ContinueBtn.interactable = false;
    }
}

