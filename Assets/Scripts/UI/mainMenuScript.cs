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
	public GameObject deleteSaveDialog;
	string toDelete = "debug";
	public static mainMenuScript Instance;
	// Use this for initialization
	void Awake()
	{
		Instance = this;
	}
	void Start()
    {
        Var.StartedNormally = true;
        titleColor = title.color;
        TweenForward();
		deleteSaveDialog.SetActive(false);
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
	public void DeleteSave(string name)
	{
		SaveLoad.DeleteSave(name);
		string path = Application.persistentDataPath + "/" +name+"/Terry.dat";
		System.IO.File.Delete(path);
		path = Application.persistentDataPath + "/" + name + "/Kim.dat";
		System.IO.File.Delete(path);
		path = Application.persistentDataPath + "/" + name + "/Alexander.dat";
		System.IO.File.Delete(path);
		path = Application.persistentDataPath + "/" + name + "/Rebecca.dat";
		System.IO.File.Delete(path);
		path = Application.persistentDataPath + "/" + name + "/Sophie.dat";
		System.IO.File.Delete(path);
		foreach (SaveSlot slot in FindObjectsOfType<SaveSlot>())
			slot.Refresh();

	}

	public void OpenDeleteDialog(string name)
	{
		deleteSaveDialog.SetActive(true);
		toDelete = name;

	}
	public void yesDelete()
	{
		DeleteSave(toDelete);
		CloseDeleteDialog();
	}
	public void CloseDeleteDialog()
	{
		deleteSaveDialog.SetActive(false);
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
		DeleteSave("debug");
    }
}

