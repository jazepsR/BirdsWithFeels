using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiMap : MonoBehaviour {
	public bool inMap = false;
	public GameObject mapIcon;
	//public static GuiMap Instance { get; private set; }
	public Transform start;
	public Transform finish;
	public GameObject cup;
	public Transform nodes;
	public Text nextAreaInfo;
	public Image nextAdventureIcon;
	public GameObject trialObj;
//	[HideInInspector]
	public List<SpriteRenderer> nodeList;
	float dist;
	int count = 0;
	public void Awake()
	{
		//Instance = this;
		if (inMap)
			return;
		if (Var.isTutorial)
		{
			if (Var.map.Count == 0)
			{
				for (int i = 0; i < 6; i++)
				{
					MapBattleData BattleStuff = new MapBattleData();
					BattleStuff.emotionPercentage.Add(1);
					BattleStuff.emotionType.Add(Var.Em.Neutral);
					Var.map.Add(new BattleData(Var.Em.Neutral, false, new List<Var.Em>(), BattleStuff));
				}
				Var.map.Add(new BattleData(Var.Em.finish, false, new List<Var.Em>(), null));
				nextAreaInfo.text = "";

            }
            nextAreaInfo.gameObject.SetActive(false);
        }
        else if(Var.isEnding)
        {
            Var.map = new List<BattleData>(); 
            for (int i = 0; i < 7; i++)
            {
                MapBattleData BattleStuff = new MapBattleData();
                BattleStuff.emotionPercentage.Add(1);
                BattleStuff.emotionType.Add(Var.Em.Neutral);
                Var.map.Add(new BattleData(Var.Em.Neutral, false, new List<Var.Em>(), BattleStuff));
            }
            Var.map.Add(new BattleData(Var.Em.finish, false, new List<Var.Em>(), null));
            nextAreaInfo.text = "";
        }
		else
		{
			if (Var.map.Count == 0)
			{
				for (int i = 0; i < 2; i++)
				{
					MapBattleData BattleStuff = new MapBattleData();
					BattleStuff.emotionPercentage.Add(1);
					BattleStuff.emotionType.Add(Var.Em.Confident);
					Var.map.Add(new BattleData(Var.Em.Neutral, true, new List<Var.Em>() { Var.Em.Confident, Var.Em.Cautious, Var.Em.Solitary, Var.Em.Social }, BattleStuff, 1, new List<Bird.dir>() { Bird.dir.front, Bird.dir.top }, new List<Var.PowerUps>() { Var.PowerUps.dmg, Var.PowerUps.heal }));

					MapBattleData BattleStuff2 = new MapBattleData();
					BattleStuff2.emotionPercentage.Add(1);
					BattleStuff2.emotionType.Add(Var.Em.Cautious);
					Var.map.Add(new BattleData(Var.Em.Confident, true, new List<Var.Em>() { Var.Em.Confident }, BattleStuff2, 1, new List<Bird.dir>() { Bird.dir.front, Bird.dir.top }, new List<Var.PowerUps>() { Var.PowerUps.dmg, Var.PowerUps.heal }));


				}
				Var.map.Add(new BattleData(Var.Em.finish, false, new List<Var.Em>(), null));
			}
			if (inMap)
				return;
				foreach (MapSaveData data in Var.mapSaveData)
				{
                if (data.ID == Var.currentStageID)
                {
                    if (data.trialID == data.ID && trialObj != null)
                    {
                        trialObj.SetActive(false);
                    }
                }
                else
                {

                    foreach (MapSaveData targ in Var.mapSaveData)
                    {
                        if (targ.ID == data.trialID)
                        {
                            nextAdventureIcon.color = Helpers.Instance.GetEmotionColor(targ.emotion);
                            nextAreaInfo.text = targ.areaName;
                            nextAdventureIcon.GetComponent<ShowTooltip>().tooltipText = targ.areaName + " is the next big challenge. Main emotion: " + targ.emotion.ToString();
                            break;
                        }
                    }

                }
					
				}
					
			
		}
	}

	public void MoveMapBird(int pos)
	{
		//transform.localPosition = new Vector3(-578 + pos * 95,transform.localPosition.y,transform.localPosition.z);
		transform.position = new Vector3(start.position.x + dist * pos, transform.position.y, start.position.z);
		for(int i=0;i<nodeList.Count;i++)
		{
			if(pos>i)
			{
				Color color= nodeList[i].color;
				nodeList[i].color = new Color(color.r,color.g,color.b,0.4f);
			}
		}
	}



	public void CreateMap(List<BattleData> map)
	{
		Clear();
		if(map == null)
		{
			return;
		}
		if (inMap)
		{
			dist = 0.5f * Mathf.Abs(start.position.x - finish.position.x) / ((map.Count - 1));
		}
		else
		{
			dist = Mathf.Abs(start.position.x - finish.position.x) / ((map.Count - 1));
		}
		foreach (BattleData part in map)
		{
			if(part.type!= Var.Em.finish)
				DrawCircles(part.type , 22.5f);
		}
		GameObject cupObj = Instantiate(cup, finish.position,Quaternion.identity);
		cupObj.transform.parent = nodes;
		if (inMap)
		{
			//LeanTween.delayedCall(0.3f, CreateColor);
			cupObj.transform.localPosition = new Vector3(dist * count * 150, 0, 0);
			cupObj.transform.localScale = Vector3.one * 25;
		}
	}
	public void Clear()
	{
		count = 0;
		foreach (Transform child in nodes)
			Destroy(child.gameObject);
		nodeList.Clear();

	}
	void DrawCircles(Var.Em emotion, float scale = 30)
	{
		GameObject point = Instantiate(mapIcon, new Vector3(start.position.x + dist * count, start.position.y, start.position.z), Quaternion.identity,nodes);
		point.GetComponent<SpriteRenderer>().sprite = Helpers.Instance.GetEmotionIcon(emotion);
		if (!inMap)
			point.GetComponent<ShowTooltip>().tooltipText = Helpers.Instance.GetHexColor(emotion) + emotion.ToString() + "</color>";
		nodeList.Add(point.GetComponent<SpriteRenderer>());
		//point.transform.parent = nodes;
		//point.GetComponent<SpriteRenderer>().color = Helpers.Instance.GetEmotionColor(emotion);
		if(inMap)
		{ 
			point.transform.localPosition = new Vector3(count * dist * 150, 0, 0);
		}
		point.transform.localScale =Vector3.one* scale;
		count++;
		/*
LineRenderer lr =point.GetComponent<LineRenderer>();
		if (inMap)
		{
			lr.startColor = new Color(0, 0, 0, 0);
			lr.endColor = new Color(0, 0, 0, 0);
			lr.transform.localScale = Vector3.one * 30;
			//HACK, its super terrible
			point.transform.localPosition = new Vector3(count * dist*150, 0, 0);
		}else
		{
			point.transform.localScale =Vector3.one* 30f;
		}
		lr.sortingOrder = 55;
		lr.positionCount = 2;
		lr.SetPosition(0, new Vector3(start.position.x + dist * count, start.position.y, start.position.z));
		lr.SetPosition(1, new Vector3(start.position.x + dist * (count+1), start.position.y, start.position.z));*/
	}
	/*void CreateColor()
	{
		foreach(LineRenderer lr in nodes.GetComponentsInChildren<LineRenderer>())
		{
			lr.startColor = Color.white;
			lr.endColor = Color.black;

		}


	}*/
}
