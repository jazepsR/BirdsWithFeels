using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapIcon : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject CompleteIcon;
    public GameObject LockedIcon;
    public int ID;
    public bool completed = false;
    public string levelName;
    public bool available;
    [Header("Level configuration")]
    public Var.Em type;
    public int birdLVL = 1;
    public int length = 1;
    public int minEnemies = 3;
    public int maxEnemies = 4;
    public bool hasTopEnemyRow = true;
    public bool hasFrontEnemyRow = true;
    [Range(0.0f, 1.0f)]
    public float mainEmotionSpawnRate = 0.8f;   
    [Header("Tile Configuration")]
    public bool hasObstacles;
    public bool hasScaredPowerUps;
    public bool hasFirendlyPowerUps;
    public bool hasConfidentPowerUps;
    public bool hasLonelyPwerUps;
    public bool hasHealthPowerUps;
    public bool hasDMGPowerUps;
   // [HideInInspector]
    public MapIcon[] targets;
    MapSaveData mySaveData;
    LineRenderer lr;
    Image sr;
    bool active = false;
    Vector3 offset;    
    // Use this for initialization
    void Start()
    {
     
        offset = transform.position- transform.parent.position;
        LoadSaveData();
        sr = GetComponent<Image>();
        sr.color = Helpers.Instance.GetEmotionColor(type);
        //GetComponent<Button>().interactable = available;
        CompleteIcon.SetActive(completed);
        LockedIcon.SetActive(!available);        
        lr = GetComponent<LineRenderer>();
        if (!CheckTargetsAvailable() && available)
            LeanTween.delayedCall(0.1f, mapBtnClick);
    }
    void Update()
    {
        if(active)
        {
            transform.parent.position = transform.position - offset;
            print("active!!");
        }
        int i = 0;
        lr.positionCount = targets.Length * 2;
        lr.sortingOrder = 9;
        if (targets.Length > 0)
        {
            foreach (MapIcon pos in targets)
            {

                lr.SetPosition(i++, pos.gameObject.transform.position);
                lr.SetPosition(i++, transform.position);
            }
        }
        if (!completed)
        {
            lr.endColor = Color.gray;
            lr.startColor = Color.gray;
        }
    }
    public void RemoveLock()
    {
       // LockedIcon.SetActive(false);
    }
    void LoadSaveData()
    {
        bool SaveDataCreated = false;
        foreach (MapSaveData data in Var.mapSaveData)
        {
            if (data.ID == ID)
            {
                SaveDataCreated = true;
                break;
            }
        }


        if (!SaveDataCreated)
        {
            List<int> targIDs = new List<int>();
            foreach (MapIcon targ in targets)
            {
                targIDs.Add(targ.ID);
            }
            mySaveData = new MapSaveData(completed, available, ID, targIDs);
            Var.mapSaveData.Add(mySaveData);
        }
        else
        {
            foreach (MapSaveData data in Var.mapSaveData)
            {
                if (data.ID == ID)
                {
                    completed = data.completed;
                    available = data.available;
                    break;
                }

            }
        }
    }


    public void mapBtnClick()
    {
        try
        {
            AudioControler.Instance.ClickSound();
        }
        catch { }
        active = true;
        MapControler.Instance.SelectedIcon = null;
        MapControler.Instance.startLvlBtn.gameObject.SetActive(false);
        MapControler.Instance.SelectionMenu.transform.localScale = Vector3.zero;
        MapControler.Instance.selectionTiles.transform.localScale = Vector3.zero;
        MapControler.Instance.ScaleSelectedBirds(0, Vector3.zero);
        LeanTween.move(transform.parent.gameObject, MapControler.Instance.centerPos.position+(transform.parent.transform.position-transform.position), 0.8f).setEase(LeanTweenType.easeInBack).setOnComplete(ShowAreaDetails);
    }

    public void ShowAreaDetails()
    {
        active = false;      
        MapControler.Instance.SelectionMenu.SetActive(true);
        MapControler.Instance.selectionTiles.SetActive(true);
        MapControler.Instance.SelectionTitle.text = levelName + " details";
        MapControler.Instance.SelectionText.text = ToString();
        AudioControler.Instance.ClickSound();
        LeanTween.scale(MapControler.Instance.SelectionMenu, Vector3.one, MapControler.Instance.scaleTime).setEase(LeanTweenType.easeOutBack);
        if (available)
        {
            LeanTween.scale(MapControler.Instance.selectionTiles, Vector3.one, MapControler.Instance.scaleTime).setEase(LeanTweenType.easeOutBack);
            MapControler.Instance.ScaleSelectedBirds(MapControler.Instance.scaleTime, Vector3.one * 0.25f);
            MapControler.Instance.SelectedIcon = this;
            MapControler.Instance.startLvlBtn.gameObject.SetActive(true);
            
        }
    }
    bool CheckTargetsAvailable()
    {
        bool targetAvailable = false;
        foreach(MapIcon target in targets)
        {
            if (target.available)
                targetAvailable = true;
        }
        return targetAvailable;
    }


    public void LoadBattleScene()
    {
        
        if (available && MapControler.Instance.canFight)
        {
            Var.fled = false;
            Var.isTutorial = false;
            Var.map.Clear();
            for (int i = 0; i < length; i++)
            {
                AddStageToLevel(type);
            }
            Var.map.Add(new BattleData(Var.Em.finish, hasObstacles, new List<Var.Em>()));
            Var.currentStageID = ID;

            Var.activeBirds = new List<Bird>();
            for (int i = 0; i < 3; i++)
            {
                Var.activeBirds.Add(Var.playerPos[i, 0]);
            }

            SceneManager.LoadScene("NewMain");
        }

    }

    void AddStageToLevel(Var.Em emotion)
    {
        List<Var.Em> emotions = new List<Var.Em> { Var.Em.Confident, Var.Em.Friendly, Var.Em.Lonely, Var.Em.Scared, Var.Em.Neutral };
        emotions.Remove(emotion);
        BattleData data;
        if (Random.Range(0f, 1f) < mainEmotionSpawnRate)        
            data = new BattleData(emotion, hasObstacles, EmPowerList(), birdLVL, CreateDirList(), PowerList());
          
        else
           data = new BattleData(emotions[Random.Range(0, 4)], hasObstacles, EmPowerList(), birdLVL, CreateDirList(), PowerList());
        data.maxEnemies = maxEnemies;
        data.minEnemies = minEnemies;
        Var.map.Add(data);
    }

    List<Bird.dir> CreateDirList()
    {
        List<Bird.dir> dirList = new List<Bird.dir>();
        if (hasFrontEnemyRow)
            dirList.Add(Bird.dir.front);
        if (hasTopEnemyRow)
            dirList.Add(Bird.dir.top);       
        return dirList;
    }

    List<Var.Em> EmPowerList()
    {
        List<Var.Em> list = new List<Var.Em>();
        if (hasFirendlyPowerUps)
            list.Add(Var.Em.Friendly);
        if (hasConfidentPowerUps)
            list.Add(Var.Em.Lonely);
        if (hasLonelyPwerUps)
            list.Add(Var.Em.Lonely);
        if (hasScaredPowerUps)
            list.Add(Var.Em.Scared);
        return list;
    }
    List<Var.PowerUps> PowerList()
    {
        List<Var.PowerUps> list = new List<Var.PowerUps>();
        if (hasDMGPowerUps)
            list.Add(Var.PowerUps.dmg);
        if (hasHealthPowerUps)
            list.Add(Var.PowerUps.heal);
        return list;
    }



    /*public void OnPointerEnter(PointerEventData eventData)
    {
       


        Helpers.Instance.ShowTooltip(ToString());
    }
   
    public void OnPointerExit(PointerEventData eventData)
    {
        Helpers.Instance.HideTooltip();
    }*/
    public override string ToString()
    {
        string stageInfo = "";
        string stageState = "not available";
        if (completed)
            stageState = "completed";
        if (available)
            stageState = "available";
        stageInfo += "Stage " + (ID + 1) + ": " + stageState;
        stageInfo += "\nLength " + length;
        stageInfo += "\nMain emotion: " +Helpers.Instance.GetHexColor(type)+ type + "</color>. Weak to: " +
            Helpers.Instance.GetHexColor(Helpers.Instance.GetWeakness(type)) + Helpers.Instance.GetWeakness(type) + "</color>.";
        stageInfo += "\nEnemies attack from the: ";
        if (hasFrontEnemyRow)
            stageInfo += "\u2022Front ";
        if (hasTopEnemyRow)
            stageInfo += "\u2022Top ";      
        if (hasObstacles)
            stageInfo += "\nHas obstacles";
        if (hasHealthPowerUps)
            stageInfo += "\nHas healing tiles";
        if (hasDMGPowerUps)
            stageInfo += "\nHas bonus damage tiles";
        return stageInfo;
    }

}






