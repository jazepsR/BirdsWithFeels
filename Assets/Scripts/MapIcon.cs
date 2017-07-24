using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MapIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject CompleteIcon;
    public GameObject LockedIcon;
    public int ID;
    public bool completed = false;
    public bool available;
    [Header("Level configuration")]
    public Var.Em type;
    public int birdLVL = 1;
    public int length = 1;
    public bool hasTopEnemyRow = true;
    public bool hasFrontEnemyRow = true;
    public bool hasBottomEnemyRow = true;
    [Header("Tile Configuration")]
    public bool hasObstacles;
    public bool hasScaredPowerUps;
    public bool hasFirendlyPowerUps;
    public bool hasConfidentPowerUps;
    public bool hasLonelyPwerUps;
    public bool hasHealthPowerUps;
    public bool hasDMGPowerUps;
    [HideInInspector]
    public MapIcon[] targets;
    MapSaveData mySaveData;
    LineRenderer lr;
    Image sr;
    // Use this for initialization
    void Start()
    {
        LoadSaveData();
        sr = GetComponent<Image>();
        sr.color = Helpers.Instance.GetEmotionColor(type);
        GetComponent<Button>().interactable = available;
        CompleteIcon.SetActive(completed);
        LockedIcon.SetActive(false);
        if (available)
            LockedIcon.SetActive(true);
        lr = GetComponent<LineRenderer>();
        int i = 0;
        lr.positionCount = targets.Length * 2;
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
        LockedIcon.SetActive(false);
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
    public void LoadBattleScene()
    {
        if (available && MapControler.Instance.canFight)
        {
            Var.fled = false;
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

        if (Random.Range(0f, 1f) < 0.8f)
        {
            Var.map.Add(new BattleData(emotion, hasObstacles, EmPowerList(), birdLVL, CreateDirList(), PowerList()));
        }
        else
        {
            Var.map.Add(new BattleData(emotions[Random.Range(0, 4)], hasObstacles, EmPowerList(), birdLVL, CreateDirList(), PowerList()));
        }
    }

    List<Bird.dir> CreateDirList()
    {
        List<Bird.dir> dirList = new List<Bird.dir>();
        if (hasFrontEnemyRow)
            dirList.Add(Bird.dir.front);
        if (hasTopEnemyRow)
            dirList.Add(Bird.dir.top);
        if (hasBottomEnemyRow)
            dirList.Add(Bird.dir.bottom);
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



    public void OnPointerEnter(PointerEventData eventData)
    {
       


        Helpers.Instance.ShowTooltip(ToString());
    }
   
    public void OnPointerExit(PointerEventData eventData)
    {
        Helpers.Instance.HideTooltip();
    }
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
        stageInfo += "\nMain emotion: " + type;
        stageInfo += "\nEnemies attack form the ";
        if (hasFrontEnemyRow)
            stageInfo += "front ";
        if (hasTopEnemyRow)
            stageInfo += "top ";
        if (hasBottomEnemyRow)
            stageInfo += "back";
        if (hasObstacles)
            stageInfo += "\nHas obstacles";
        if (hasHealthPowerUps)
            stageInfo += "\nHas healing tiles";
        if (hasDMGPowerUps)
            stageInfo += "\nHas bonus damage tiles";
        return stageInfo;
    }

}






