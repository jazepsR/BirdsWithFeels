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
	public Text deleteSaveText;
    string toDelete = "debug";
    public static mainMenuScript Instance;
	bool isDelete = false;
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
    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
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

    public void OpenCredits()
    {
        SceneManager.LoadScene("Credits");
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
        AudioControler.Instance.fightButtonAppear.Play();
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
        if (System.IO.File.Exists(path))
            System.IO.File.Delete(path);
        path = Application.persistentDataPath + "/" + name + "/Kim.dat";
        if (System.IO.File.Exists(path))
            System.IO.File.Delete(path);
        path = Application.persistentDataPath + "/" + name + "/Alexander.dat";
        if (System.IO.File.Exists(path))
            System.IO.File.Delete(path);
        path = Application.persistentDataPath + "/" + name + "/Rebecca.dat";
        if (System.IO.File.Exists(path))
            System.IO.File.Delete(path);
        path = Application.persistentDataPath + "/" + name + "/Sophie.dat";
        if (System.IO.File.Exists(path))
            System.IO.File.Delete(path);
        foreach (SaveSlot slot in FindObjectsOfType<SaveSlot>())
            slot.Refresh();

    }

    public void OpenDeleteDialog(string name, bool isDelete)
    {
        deleteSaveDialog.SetActive(true);
        toDelete = name;
		if (isDelete)
			deleteSaveText.text = "Are you sure you want to delete the save?";
		else
			deleteSaveText.text = "Are you sure you want to overwrite this save? All data will be lost";


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
    public void LoadMap()
    {
        Var.gameSettings = new Settings(true);
        SceneManager.LoadScene("Map");
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
        Var.gameSettings = new Settings(false);
        DeleteSave("debug");
    }
}

