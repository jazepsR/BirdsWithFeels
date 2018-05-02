using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelBarScript : MonoBehaviour {
    public Image levelBar;
    int currentPoints;
    [HideInInspector]
    public int maxPoints;
    public Levels.type type;
	// Use this for initialization
	void Start () {
		
	}
	public void ClearPoints()
    {
        levelBar.rectTransform.localScale = new Vector3(1, 0, 1);
        currentPoints = 0;
    }
	// Update is called once per frame
	void Update () {
		
	}
    public void AddPoints(Bird bird)
    {
        currentPoints++;
        Vector3 temp= levelBar.rectTransform.localScale;
        temp.y = currentPoints /(float) maxPoints;
        levelBar.rectTransform.localScale = temp;
        if (maxPoints == currentPoints)
            Helpers.ApplyLevel(type, bird);

    }
}
