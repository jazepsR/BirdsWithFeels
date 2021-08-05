using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class firendLine : MonoBehaviour {
	//public GameObject LineObj;
	[SerializeField]
	public List<FriendLineObject> activeLines = new List<FriendLineObject>();
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
		RemoveLines();
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
					FriendLineObject LineObj = new FriendLineObject(Instantiate(line, pos, rot), birdScript, Var.playerPos[y, x]);
					activeLines.Add(LineObj);
					Var.playerPos[y, x].lines.activeLines.Add(LineObj);
                    AudioControler.Instance.PlaySound(AudioControler.Instance.createLines);
					//Debug.Log("drawing line from: " + birdScript.charName + " to: " + Var.playerPos[y, x].charName);
                }
               if (Var.playerPos[y, x].indicator && !Var.Infight)
                {
                    List<Bird> birds = Helpers.Instance.GetAdjacentBirds(Var.playerPos[y, x]);

					Var.Em emo1 = (birds.Count == 0 ? Var.Em.Solitary : Var.Em.Social);


					if (emo1 == Var.Em.Social)
                    {
                        emo1 = (Helpers.Instance.getFriendState(Var.playerPos[y, x]) == Helpers.friendState.diagonal ? Var.Em.Neutral : Var.Em.Social);
                    }
                    
					
					Var.Em emo2 = Var.playerPos[y, x].fighting ? Var.Em.Neutral : Var.Em.Cautious;
					//Debug.Log("active lines count " + activeLines.Count + "friendline play pos for" + Var.playerPos[y, x].charName + " emo1 " + emo1 + "emo2: " + emo2);
					Var.playerPos[y, x].indicator.SetEmotions(emo1, emo2);
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
			//lonelyParticleObj = Instantiate(lonelyParticles, birdScript.transform);
            AudioControler.Instance.PlaySound(AudioControler.Instance.SolitaryAppear);
			//lonelyParticleObj.transform.localPosition = new Vector3(0.3f, 0, 0);
		}
        if (birdScript.indicator && !Var.Infight)
        {
            Var.Em emo1 = (Helpers.Instance.GetAdjacentBirds(birdScript).Count == 0 ? Var.Em.Solitary : Var.Em.Social);
            Var.Em emo2 = birdScript.fighting ? Var.Em.Neutral : Var.Em.Cautious;

			if (emo1 == Var.Em.Social)
			{
				emo1 = Helpers.Instance.getFriendState(birdScript) == Helpers.friendState.diagonal ? Var.Em.Neutral : Var.Em.Social; 
			}
			//Debug.Log("active lines count " + activeLines.Count + "particle play pos for" + birdScript.charName + " emo1 " + emo1 + "emo2: " + emo2);
			birdScript.indicator.SetEmotions(emo1, emo2);
        }


    }


	public void RemoveLines()
	{
		foreach (Bird bird in Var.activeBirds)
		{

			for(int i = bird.lines.activeLines.Count - 1; i >= 0; i--)
			{
				if(bird.lines.activeLines[i].isConnectedToBird(birdScript))
				{
					Destroy(bird.lines.activeLines[i].line);
					bird.lines.activeLines.Remove(bird.lines.activeLines[i]);
                }
			}
		}
		//Debug.LogError(birdScript.charName + " clearing lines!");
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
[System.Serializable]
public class FriendLineObject
{
	public GameObject line;
	public Bird firstBird;
	public Bird secondBird;

	public FriendLineObject(GameObject obj, Bird bird1, Bird bird2)
    {
		line = obj;
		firstBird = bird1;
		secondBird = bird2;
    }
	public bool isConnectedToBird(Bird bird)
    {
		if(bird.charName == firstBird.charName || bird.charName == secondBird.charName)
        {
			return true;
        }
        else
        {
			return false;
        }
    }

}
