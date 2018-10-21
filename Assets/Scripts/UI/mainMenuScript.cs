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
    public cutScene cutsceneScript;
	public SaveSlot saveSlotTemplate;
	public Transform saveSlotParent;
	public GameObject buttonPanel;
	public GameObject saveSlotPanel;
	// Use this for initialization
	void Start()
    {
        Var.StartedNormally = true;
        titleColor = title.color;
        TweenForward();       
        
        ContinueBtn.interactable = SaveLoad.Load();
		buttonPanel.SetActive(true);
		saveSlotPanel.SetActive(false);
	}
	public void OpenSaveSlots(bool isNewGame)
	{
		foreach (Transform slot in saveSlotParent)
			Destroy(slot.gameObject);
		for (int i = 0; i < 3; i++)
		{
			SaveSlot slot = Instantiate(saveSlotTemplate, saveSlotParent);
			slot.Setup(isNewGame, "Save"+(i+1).ToString());
		}
		buttonPanel.SetActive(false);
		saveSlotPanel.SetActive(true);

	}
	public void CloseSaveSlots()
	{
		buttonPanel.SetActive(true);
		saveSlotPanel.SetActive(false);
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
		SaveLoad.Save();
        SceneManager.LoadScene("Map");
    }

    public void StartClick()
    {
        ResetGame();
        Var.isTutorial = true;
        cutsceneScript.StartCutscene();
        //SceneManager.LoadScene("NewMain");
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
	public void DeleteSave()
	{
		SaveLoad.DeleteSave();
		string path = Application.persistentDataPath + "/Terry.dat";
		System.IO.File.Delete(path);
		path = Application.persistentDataPath + "/Kim.dat";
		System.IO.File.Delete(path);
		path = Application.persistentDataPath + "/Alexander.dat";
		System.IO.File.Delete(path);
		path = Application.persistentDataPath + "/Rebecca.dat";
		System.IO.File.Delete(path);
		path = Application.persistentDataPath + "/Sophie.dat";
		System.IO.File.Delete(path);


	}

	public void ResetGame()
    {
        SaveLoad.DeleteSave();
        Var.currentWeek = -1;
        Var.currentBG = 0;
        Var.currentStageID = -1;
        GuiContoler.mapPos = 0;
        Var.mapSaveData = new List<MapSaveData>();
		Var.timedEvents = new List<TimedEventData>();
        Var.activeBirds = new List<Bird>();
        Var.availableBirds = new List<Bird>();
        Var.map = new List<BattleData>();
        Var.shownEvents = new List<string>();
        Var.shownDialogs = new List<string>();
        Var.isTutorial = false;
        Var.isBoss = false;
        ContinueBtn.interactable = false;
        Var.tutorialCompleted = false;
        Var.gameSettings = new Settings();
		DeleteSave();
    }
}

