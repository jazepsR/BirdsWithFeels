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
	public Toggle freezeToggle;
	private bool UIHidden;

	[SerializeField]
	private CanvasGroup[] CanvasGroupsToHideWhenDebugging;
	[SerializeField]
	private GameObject[] GameObjectsToHideWhenDebugging;

	private void Update()
    {
        if (resetGame && Vector3.Distance(Input.mousePosition, mousePos) > 50f)
        {
            resetTime = System.DateTime.Now.AddSeconds(secondsWaitTime);
            mousePos = Input.mousePosition;
        }
      /*  if(System.DateTime.Compare(System.DateTime.Now,resetTime)>0)
        {
            SaveLoad.DeleteSave(Var.currentSaveSlot);
            SceneManager.LoadScene("mainMenu");
        }*/
    }

    void Awake()
	{
		Instance = this;
        mousePos = Input.mousePosition;
        resetTime = System.DateTime.Now.AddSeconds(secondsWaitTime);
    }
	public void OpenDebug()
	{
		if (!Var.cheatsEnabled)
		{
			return;
		}
		try
		{
			currentBird.text = "Current bird: " + Var.selectedBird.GetComponent<Bird>().charName;
		}
		catch
		{
			currentBird.text = "<color=#FF0000>Select bird first!</color>";
		}
        cameraToggle.isOn = cameraControl;
		freezeToggle.isOn = Var.freezeEmotions;
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
			Helpers.ApplyLevel(Var.selectedBird.GetComponent<Bird>());
		}
		catch
		{ }
	}

	public void AddHP()
	{
		if (Var.selectedBird)
		{
			//Var.selectedBird.GetComponent<Bird>().data.injured = false;
			//Var.selectedBird.GetComponent<Bird>().data.TurnsInjured = 0;
			Var.selectedBird.GetComponent<Bird>().ChageHealth(1);
			UpdateBirdStats();
		}
	}

	public void LoseHP()
	{
		if (Var.selectedBird)
		{
			Var.selectedBird.GetComponent<Bird>().ChageHealth(-1);
			UpdateBirdStats();
		}
	}


	public void IncreaseConfidence()
	{
		if(Var.selectedBird)
		{
			Var.selectedBird.GetComponent<Bird>().prevConf++;
		Var.selectedBird.GetComponent<Bird>().data.confidence++;
		UpdateBirdStats();
		}

	}

	public void DecreaseConfidence()
	{
		if(Var.selectedBird)
		{
		Var.selectedBird.GetComponent<Bird>().prevConf--;
		Var.selectedBird.GetComponent<Bird>().data.confidence--;
		UpdateBirdStats();
			}
	}

	public void IncreaseSocial()
	{
	if(Var.selectedBird)
		{
		Var.selectedBird.GetComponent<Bird>().prevFriend++;
		Var.selectedBird.GetComponent<Bird>().data.friendliness++;
		UpdateBirdStats();
			}
	}

	public void DecreaseSocial()
	{
	if(Var.selectedBird)
		{
		Var.selectedBird.GetComponent<Bird>().prevFriend--;
		Var.selectedBird.GetComponent<Bird>().data.friendliness--;
		UpdateBirdStats();
			}
	}
	private void UpdateBirdStats()
	{
		Var.selectedBird.GetComponent<Bird>().SetEmotion();
		GuiContoler.Instance.clearSmallGraph();
		GuiContoler.Instance.smallGraph.PlotFull(Var.selectedBird.GetComponent<Bird>(),false);
	}

    public void ToggleCamControls(bool enabled)
    {
        cameraControl = enabled;
        cameraToggle.isOn = enabled;
    }

	public void ToogleFreeze(bool enabled)
	{
		Var.freezeEmotions = enabled;
		freezeToggle.isOn = enabled;
		Graph[] graphs = FindObjectsOfType<Graph>();
		foreach(Graph graph in graphs)
		{
			graph.CheckEmotionLock();
		}
	}

	public void ToggleHideUI()
    {
		UIHidden = !UIHidden;

		float alpha = 1;
        if (UIHidden)
        {
			alpha = 0;
        }

		for(int i = 0; i < CanvasGroupsToHideWhenDebugging.Length; i++)
        {
			CanvasGroupsToHideWhenDebugging[i].alpha = alpha;
        }

		for (int i = 0; i < GameObjectsToHideWhenDebugging.Length; i++)
		{
			GameObjectsToHideWhenDebugging[i].SetActive(!UIHidden);
		}

	}
}
