using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapControler : MonoBehaviour {
	public static MapControler Instance { get; private set; }
	[HideInInspector]
	public bool canFight= false;
    [HideInInspector]
    public bool canHeal = false;
    GameObject healTrail;
    public Text title;
    public Transform centerPos;
    public GameObject SelectionMenu;
    public Text SelectionText;
    public Text SelectionTitle;
    public GameObject selectionTiles;
    public Button startLvlBtn;
    public float scaleTime = 0.35f;
    [HideInInspector]
    public MapIcon SelectedIcon;    
    void Awake()
	{
        Instance = this;
    }
	// Use this for initialization
	void Start () {
        //SaveLoad.Save();
        canHeal = false;
        foreach(Bird bird in Var.activeBirds)
        {
            if(Helpers.Instance.ListContainsLevel(Levels.type.Friend2, bird.levelList) &&!Var.fled)
            {
                healTrail = Instantiate(Resources.Load("MouseHealParticle"), Input.mousePosition, Quaternion.identity) as GameObject;
                canHeal = true;
                title.text = bird.charName + " can heal one of your birds!";
            }
        }
        //ProgressGUI.Instance.PortraitClick(Var.availableBirds[0]);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (canHeal)
        {
            healTrail.transform.position = Camera.main.ScreenToWorldPoint( Input.mousePosition);
        }else
        {
            Destroy(healTrail);
        }
	}
	public void CanLoadBattle()
	{
		canFight = true;
		for(int i = 0; i < 3; i++)
		{
			if(Var.playerPos[i,0]== null)
			{
				canFight = false;
				break;
			}
		}
		if (canFight)
		{
			MapIcon[] icons = FindObjectsOfType<MapIcon>();
			foreach(MapIcon icon in icons)
			{
				//icon.LockedIcon.SetActive(false);
			}
		}
        startLvlBtn.interactable = canFight;
		
	}
    public void StartLevel()
    {
        if (SelectedIcon != null)
        {
            AudioControler.Instance.ClickSound();
            SelectedIcon.LoadBattleScene();
            
        }
    }


    public void HideSelectionMenu()
    {
        LeanTween.scale(MapControler.Instance.SelectionMenu, Vector3.zero, MapControler.Instance.scaleTime).setEase(LeanTweenType.easeInBack);
        LeanTween.scale(MapControler.Instance.selectionTiles, Vector3.zero, MapControler.Instance.scaleTime).setEase(LeanTweenType.easeInBack);
        MapControler.Instance.ScaleSelectedBirds(MapControler.Instance.scaleTime, Vector3.zero);
    }

    
    public void ScaleSelectedBirds(float time, Vector3 to)
    {
        for (int i = 0; i < 3; i++)
        {
            if(Var.playerPos[i, 0]!= null)
            {
                LeanTween.scale(Var.playerPos[i, 0].gameObject, to, time).setEase(LeanTweenType.easeOutBack);
            }
        }
    }
}
