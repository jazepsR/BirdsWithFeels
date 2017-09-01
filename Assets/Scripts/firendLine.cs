using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class firendLine : MonoBehaviour {
	public GameObject LineObj;
	public List<GameObject> activeLines = new List<GameObject>();
	public Color thick;
	public Color thin;
	public Color crush;
	public Color relationship;
    public LineRenderer lr;
	Bird birdScript;
	// Use this for initialization
	void Start () {
		birdScript = GetComponent<Bird>();        
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void DrawLines(int x, int y)
	{
		if (x < 0 || y < 0)
			return;
		DrawLine(y, x + 1,true);
		DrawLine(y, x - 1,true);
		DrawLine(y + 1, x + 1,false);
		DrawLine(y + 1, x,true);
		DrawLine(y + 1, x - 1,false);
		DrawLine(y - 1, x + 1,false);
		DrawLine(y - 1, x,true);
		DrawLine(y - 1, x-1,false);
	}



	void DrawLine(int x, int y,bool isThick)
	{
		try
		{
			if (x < 0 || y < 0)
				return;
			if (Var.playerPos[y, x] != null)
			{
				
				GameObject line = Instantiate(LineObj);
				LineRenderer lr = line.GetComponent<LineRenderer>();
				lr.sortingOrder = 0;
				lr.SetPosition(0, birdScript.target);
				lr.SetPosition(1, Var.playerPos[y, x].target);
			  
				if (isThick)
				{
					lr.startColor = thick;
					lr.endColor = thick;
				}
				else
				{
					lr.startColor = thin;
					lr.startColor = thin;
				}
				bool firstCrush = false;
				bool secondCrush = false;
				if (birdScript.relationshipBird != null && birdScript.relationshipBird.charName == Var.playerPos[y, x].charName)
				{
					lr.startColor = crush;
					firstCrush = true;
				}
				if (Var.playerPos[y, x].relationshipBird != null && Var.playerPos[y, x].relationshipBird.charName == birdScript.charName)
				{
					lr.endColor = crush;
					firstCrush = true;
				}


				if ( firstCrush && secondCrush)
				{
					lr.startColor = relationship;
					lr.endColor = relationship;                   
				}
				activeLines.Add(line);
				Var.playerPos[y, x].lines.activeLines.Add(line);





			}
		}
		catch {
			
		}

	}




	public void RemoveLines()
	{
		foreach(GameObject line in activeLines)
		{
		   
			Destroy(line);
		}
		activeLines.Clear();


	}

}
