using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiMap : MonoBehaviour {

    public GameObject mapIcon;
    public static GuiMap Instance { get; private set; }
    public Transform start;
    public Transform finish;
    public GameObject cup;
    float dist;
    int count = 0;
    void Awake()
    {
        
        Instance = this;
    }

    public void MoveMapBird(int pos)
    {
        //transform.localPosition = new Vector3(-578 + pos * 95,transform.localPosition.y,transform.localPosition.z);
        transform.position = new Vector3(start.position.x + dist * pos, start.position.y+0.3f, start.position.z);
    }



    public void CreateMap()
    {
        dist = Mathf.Abs(start.position.x - finish.position.x)/(Var.map.Count*3);
        foreach (BattleData part in Var.map)
        {
            DrawCircles(part.type);
        }
        Instantiate(cup, finish.position,Quaternion.identity);
        MoveMapBird(2);
    }

    void DrawCircles(Var.Em emotion)
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject point = Instantiate(mapIcon, new Vector3(start.position.x + dist * count, start.position.y, start.position.z), Quaternion.identity);
            point.GetComponent<SpriteRenderer>().color = Helpers.Instance.GetEmotionColor(emotion);
            LineRenderer lr =point.GetComponent<LineRenderer>();
            lr.SetPosition(0, new Vector3(start.position.x + dist * count, start.position.y, start.position.z));
            lr.SetPosition(1, new Vector3(start.position.x + dist * (count+1), start.position.y, start.position.z));
            count++;
        }
    }
}
