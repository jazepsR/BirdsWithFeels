using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipScript : MonoBehaviour {
	static int minRelationship = -10;
	static int maxRelationship = 15;
	static int treshold = 8;
	static int likeGain = 9;
	static int normGain = 4;
	static int dislikeGain = 4;
	static int decayLose = 1;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	public static void applyRelationship(Bird bird)
	{
		List<Bird> closeBirds = Helpers.Instance.GetAdjacentBirds(bird);
		foreach (Bird closeBird in closeBirds)
		{
			var key = Helpers.Instance.GetCharEnum(closeBird);
			if (closeBird.emotion == bird.preferredEmotion)
			{
				bird.relationships[key] = bird.relationships[key] + likeGain;
			}
			else
			{
				if (closeBird.emotion == Helpers.Instance.GetOppositeEmotion(bird.preferredEmotion))
				{
					bird.relationships[key] = bird.relationships[key] + dislikeGain;
				}
				else
				{
					bird.relationships[key] = bird.relationships[key] + normGain;
				}
			}
		}
		weakenRelationship(bird);
		Bird relationshipBird = null;
		int currentTreshold = treshold;
        var keys = new List<EventScript.Character>(bird.relationships.Keys);
        foreach (EventScript.Character birdFriend in keys)
		{
			if(bird.relationships[birdFriend]>= treshold)
			{
				relationshipBird = Helpers.Instance.GetBirdFromEnum(birdFriend);
				currentTreshold = bird.relationships[birdFriend];
                //Debug.LogError("Relationship alert!");
			}
		}
		bird.relationshipBird = relationshipBird;

        string text = "";
        foreach (KeyValuePair<EventScript.Character, int> kvp in bird.relationships)
        {
            //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
            text += string.Format("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
        }
        print(text);
	}
	static void weakenRelationship(Bird bird)
	{
        var keys = new List<EventScript.Character>(bird.relationships.Keys);
        foreach (EventScript.Character birdFriend in keys)
		{
			bird.relationships[birdFriend] = (int)Mathf.Clamp( bird.relationships[birdFriend] - decayLose,minRelationship,maxRelationship);

		}

	}


}
