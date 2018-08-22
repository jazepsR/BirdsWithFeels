using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class firendLine : MonoBehaviour {
	//public GameObject LineObj;
	public List<GameObject> activeLines = new List<GameObject>();
	public GameObject lonelyParticleObj = null;
	public Color thick;
	public Color thin;
	public Color crush;
	public Color relationship;
    public LineRenderer lr;
	Bird birdScript;
    public static GameObject horizontalLine = null;
    public static GameObject verticalLine;
    public static GameObject diognalLineShort;
    public static GameObject diognalLineLong;
	public static GameObject lonelyParticles;
    public static GameObject cautiousParticles;
	// Use this for initialization
	void Start () {
		birdScript = GetComponent<Bird>(); 
        if(horizontalLine == null)
        {
            horizontalLine = Resources.Load<GameObject>("prefabs/lines/horizontal");
            verticalLine = Resources.Load<GameObject>("prefabs/lines/vertical");
            diognalLineShort = Resources.Load<GameObject>("prefabs/lines/diagShort");
            diognalLineLong =  Resources.Load<GameObject>("prefabs/lines/diagLong");
			lonelyParticles = Resources.Load<GameObject>("prefabs/lines/lonely_effect");
            cautiousParticles = Resources.Load<GameObject>("prefabs/lines/cautious_effect");
        }
               
	}
	
	// Update is called once per frame
	void Update () {
		
	}


	public void DrawLines()
	{
        int x = birdScript.x;
        int y = birdScript.y;
		if (x < 0 || y < 0)
			return;
		DrawLine(y, x + 1,horizontalLine);
		DrawLine(y, x - 1,horizontalLine);
		DrawLine(y + 1, x,verticalLine);
		DrawLine(y - 1, x,verticalLine);
		DrawLine(y + 1, x - 1, null);
		DrawLine(y - 1, x + 1, null);
		DrawLine(y + 1, x + 1, null);
		DrawLine(y - 1, x - 1, null);
		/*DrawLine(y + 1, x - 1, diognalLineShort);
		DrawLine(y - 1, x + 1, diognalLineShort);
		DrawLine(y + 1, x + 1, diognalLineLong);
		DrawLine(y - 1, x-1,diognalLineLong);*/
		CheckParticles();
		/*if (isLonely)
			lonelyParticleObj = Instantiate(lonelyParticles, birdScript.transform);*/

	}



	void DrawLine(int x, int y,GameObject line)
	{
		try
		{
			if (x < 0 || y < 0)
				return;
			if (Var.playerPos[y, x] != null)
			{

				if (line != null)
				{
					Vector3 pos = (birdScript.target + Var.playerPos[y, x].target) / 2f;
					Quaternion rot = line.transform.rotation;
					GameObject LineObj = Instantiate(line, pos, rot);
					activeLines.Add(LineObj);
					Var.playerPos[y, x].lines.activeLines.Add(LineObj);
				}
				Destroy(Var.playerPos[y, x].gameObject.GetComponent<firendLine>().lonelyParticleObj);
                //Deprecated code
                /*
			  
				if (isThick)
				{
					lr.startColor = thick;
					lr.endColor = thick;
				}
				else
				{
					lr.startColor = thin;
					lr.startColor = thin;
				}*/


                //Relationship stuff. May revisit later
                /*bool firstCrush = false;
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
				}*/






            }
		}
		catch {
			
		}

	}

	public void CheckParticles()
	{
		if(birdScript.x < 0 || birdScript.y < 0)
			return;
		if (Helpers.Instance.GetAdjacentBirds(birdScript).Count == 0 && lonelyParticleObj == null)
		{
			lonelyParticleObj = Instantiate(lonelyParticles, birdScript.transform);
			lonelyParticleObj.transform.localPosition = new Vector3(0.3f, 0, 0);
		}
    }


	public void RemoveLines()
	{
		foreach(GameObject line in activeLines)
		{		   
			Destroy(line);
		}
		activeLines.Clear();
		if (lonelyParticleObj != null)
			Destroy(lonelyParticleObj);
		try
		{
			foreach (Bird bird in Var.activeBirds)
			{
                if (bird != birdScript)
                {
                    bird.lines.CheckParticles();
                }
			}
		}
		catch
		{

		}
	}

}
