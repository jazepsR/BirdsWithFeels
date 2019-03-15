using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DebugMenu : MonoBehaviour {

	public static DebugMenu Instance;
	// Use this for initialization
	public Text currentBird;
	public GameObject debugMenu;
    public static bool cameraControl = false;
    public Toggle cameraToggle;
    private Vector3 mousePos = new Vector3(-1,-1,-1);
    private System.DateTime resetTime;
    private float secondsWaitTime = 200;
    private bool resetGame = true;

    
    private void Update()
    {
        if (resetGame && Vector3.Distance(Input.mousePosition, mousePos) > 50f)
        {
            resetTime = System.DateTime.Now.AddSeconds(secondsWaitTime);
            mousePos = Input.mousePosition;
        }
        if(System.DateTime.Compare(System.DateTime.Now,resetTime)>0)
        {
            SaveLoad.DeleteSave(Var.currentSaveSlot);
            SceneManager.LoadScene("mainMenu");
        }
    }

    void Awake()
	{
		Instance = this;
        mousePos = Input.mousePosition;
        resetTime = System.DateTime.Now.AddSeconds(secondsWaitTime);
    }
	public void OpenDebug()
	{
		try
		{
			currentBird.text = "Current bird: " + Var.selectedBird.GetComponent<Bird>().charName;
		}
		catch
		{
			currentBird.text = "<color=#FF0000>Select bird first!</color>";
		}
        cameraToggle.isOn = cameraControl;
		debugMenu.gameObject.SetActive(true);
	}
	public void DeleteSave()
	{
		SaveLoad.DeleteSave("debug");
		string path = Application.persistentDataPath + "/debug/Terry.dat";
		System.IO.File.Delete(path);
		path = Application.persistentDataPath + "/debug/Kim.dat";
		System.IO.File.Delete(path);
		path = Application.persistentDataPath + "/debug/Alexander.dat";
		System.IO.File.Delete(path);
		path = Application.persistentDataPath + "/debug/Rebecca.dat";
		System.IO.File.Delete(path);
		path = Application.persistentDataPath + "/debug/Sophie.dat";
		System.IO.File.Delete(path);


	}
	public void AddLevel(int level)
	{
		try
		{
			Helpers.ApplyLevel((Levels.type)level, Var.selectedBird.GetComponent<Bird>());
		}
		catch
		{ }
	}
    public void ToggleCamControls(bool enabled)
    {
        cameraControl = enabled;
        cameraToggle.isOn = enabled;
    }
}
