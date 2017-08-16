using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVisualSetup : MonoBehaviour {
    public List<GameObject> backgrounds;
	// Use this for initialization
	void Start () {
		for(int i = 0; i < backgrounds.Count; i++)
        {
            backgrounds[i].SetActive(i == Var.currentBG);
            
        }       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
