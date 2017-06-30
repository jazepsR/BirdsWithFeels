using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapIcon : MonoBehaviour {
    public GameObject CompleteIcon;
    public Var.Em type;
    public int birdLVL = 1;
    public int length = 1;
    public MapIcon[] targets;
    LineRenderer lr;
    public bool completed = false;
    public bool hasObstacles;
    public bool available;
    Image sr;
    public int ID;
    MapSaveData mySaveData;
    // Use this for initialization
    void Start() {
        LoadSaveData();
        sr = GetComponent<Image>();
        sr.color = Helpers.Instance.GetEmotionColor(type);        
        GetComponent<Button>().interactable = available;        
        CompleteIcon.SetActive(completed);
        lr = GetComponent<LineRenderer>();
        int i = 0;
        lr.numPositions = targets.Length * 2;
        if (targets.Length>0)
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
        if (available)
        {
            Var.map.Clear();
            for (int i = 0; i < length; i++)
            {
                AddStageToLevel(type);              
            }
            Var.map.Add(new BattleData(Var.Em.finish,hasObstacles));
            Var.currentStageID = ID;
            SceneManager.LoadScene("NewMain");
        }

    }

    void AddStageToLevel(Var.Em emotion)
    {
        List<Var.Em> emotions = new List<Var.Em> { Var.Em.Confident, Var.Em.Friendly, Var.Em.Lonely, Var.Em.Scared, Var.Em.Neutral };
        emotions.Remove(emotion);
        if (Random.Range(0f, 1f) < 0.7f)
        {
            Var.map.Add(new BattleData(emotion,hasObstacles));
        }else
        {
            Var.map.Add(new BattleData(emotions[Random.Range(0, 4)],hasObstacles));
        }
    }
}
