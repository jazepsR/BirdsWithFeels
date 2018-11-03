using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

public class SaveSlot : MonoBehaviour {
	public Text lastDateText;
	public Text lastPlayedText;
	public Color emptySlotColor;
	public Color fullSlotColor;
	public GameObject activeContents;
	public GameObject emptyContents;
	bool isNewGame = false;
	string saveSlot;
	bool saveExtists = false;
	Image background;
	// Use this for initialization
	void Start () {
		
	}
	public void Setup(bool isNewGame, string saveSlot)
	{
		this.isNewGame = isNewGame;
		this.saveSlot = saveSlot;
		background = GetComponent<Image>();
		Refresh();
	}
		public void DeleteSave()
	{
		Debug.Log("CLICKED DELETE!");
		mainMenuScript.Instance.OpenDeleteDialog(saveSlot);
	}
	public void Refresh()
	{
		saveExtists = File.Exists(Application.persistentDataPath + "/" + saveSlot + "/saveGame.dat");
		activeContents.SetActive(saveExtists);
		emptyContents.SetActive(!saveExtists);
		Image bg = GetComponent<Image>();
		if (saveExtists)
		{
			lastDateText.text = "Last played: " + File.GetLastAccessTime(Application.persistentDataPath + "/" + saveSlot + "/saveGame.dat").ToShortDateString();
			background.color = fullSlotColor;
			if (isNewGame)
				bg.color = Color.white;
			else
			{
				bg.color = new Color(1, 0.78f, 0);
			}
		}else
		{
			background.color = emptySlotColor;
			if (isNewGame)
				bg.color = new Color(1, 0.78f, 0);
			else
			{
				bg.color = Color.white;
				GetComponent<Button>().interactable = false;
			}
		}

	}
	public void Select()
	{
		Var.currentSaveSlot = saveSlot;
		mainMenuScript mainMenu = FindObjectOfType<mainMenuScript>();
		if (isNewGame)
		{
			mainMenu.StartClick();
		}
		else if(saveExtists)
		{			
			mainMenu.ContinueClick();
		}else
		{
			mainMenu.StartClick();
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
}
