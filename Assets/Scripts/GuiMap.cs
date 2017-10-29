using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiMap : MonoBehaviour {
	public bool inMap = false;
	public GameObject mapIcon;
	public static GuiMap Instance { get; private set; }
	public Transform start;
	public Transform finish;
	public GameObject cup;
	public Transform nodes;
	public Text nextAreaInfo;
	float dist;
	int count = 0;
	void Awake()
	{
		Instance = this;
		if (inMap)
			return;
		if (Var.isTutorial)
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
		else
		{
			if (Var.map.Count == 0)
			{
				for (int i = 0; i < 2; i++)
				{
					MapBattleData BattleStuff = new MapBattleData();
					BattleStuff.emotionPercentage.Add(1);
					BattleStuff.emotionType.Add(Var.Em.Neutral);
					Var.map.Add(new BattleData(Var.Em.Neutral, true, new List<Var.Em>() { Var.Em.Confident, Var.Em.Scared, Var.Em.Lonely, Var.Em.Friendly }, BattleStuff, 1, new List<Bird.dir>() { Bird.dir.front, Bird.dir.top }, new List<Var.PowerUps>() { Var.PowerUps.dmg, Var.PowerUps.heal }));

					MapBattleData BattleStuff2 = new MapBattleData();
					BattleStuff2.emotionPercentage.Add(1);
					BattleStuff2.emotionType.Add(Var.Em.Confident);
					Var.map.Add(new BattleData(Var.Em.Confident, true, new List<Var.Em>() { Var.Em.Confident }, BattleStuff2, 1, new List<Bird.dir>() { Bird.dir.front, Bird.dir.top }, new List<Var.PowerUps>() { Var.PowerUps.dmg, Var.PowerUps.heal }));


				}
				Var.map.Add(new BattleData(Var.Em.finish, false, new List<Var.Em>(), null));
			}
			
			string nextAreasText = "Next Adventure:";
			foreach (MapSaveData data in Var.mapSaveData)
			{
				if (data.ID == Var.currentStageID)
				{
					foreach (int id in data.targets)
					{
						foreach (MapSaveData targ in Var.mapSaveData)
						{
							if (targ.ID == id)
							{
								if (nextAreasText == "Next Adventure:")
									nextAreasText += "\n";
								else
									nextAreasText += " or\n";
								nextAreasText += Helpers.Instance.GetHexColor(targ.emotion) + targ.emotion.ToString() + "</color>";
							}
						}
					}
				}
			}
			if (nextAreasText == "Next Adventure:")
				nextAreaInfo.text = "";
			else
				nextAreaInfo.text = nextAreasText;
		}
	}

	public void MoveMapBird(int pos)
	{
		//transform.localPosition = new Vector3(-578 + pos * 95,transform.localPosition.y,transform.localPosition.z);
		transform.position = new Vector3(start.position.x + dist * pos-0.05f, transform.position.y, start.position.z);
	}



	public void CreateMap(List<BattleData> map)
	{
		Clear();
		dist = Mathf.Abs(start.position.x - finish.position.x)/((map.Count-1));
		foreach (BattleData part in map)
		{
			if(part.type!= Var.Em.finish)
				DrawCircles(part.type);
		}
		GameObject cupObj =  Instantiate(cup, finish.position,Quaternion.identity);
		cupObj.transform.parent = nodes;
		print("created map");
		if (inMap)
			LeanTween.delayedCall(0.3f, CreateColor);
	}
	public void Clear()
	{
		count = 0;
		foreach (Transform child in nodes)
			Destroy(child.gameObject);

	}
	void DrawCircles(Var.Em emotion)
	{       
		GameObject point = Instantiate(mapIcon, new Vector3(start.position.x + dist * count, start.position.y, start.position.z), Quaternion.identity);
		point.transform.parent = nodes;
		point.GetComponent<SpriteRenderer>().color = Helpers.Instance.GetEmotionColor(emotion);
		LineRenderer lr =point.GetComponent<LineRenderer>();
		if (inMap)
		{
			lr.startColor = new Color(0, 0, 0, 0);
			lr.endColor = new Color(0, 0, 0, 0);
		}
		lr.sortingOrder = 55;
		lr.SetPosition(0, new Vector3(start.position.x + dist * count, start.position.y, start.position.z));
		lr.SetPosition(1, new Vector3(start.position.x + dist * (count+1), start.position.y, start.position.z));
		count++;        
	}
	void CreateColor()
	{
		foreach(LineRenderer lr in nodes.GetComponentsInChildren<LineRenderer>())
		{
			lr.startColor = Color.white;
			lr.endColor = Color.black;

		}


	}
}
