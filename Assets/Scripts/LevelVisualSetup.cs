using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVisualSetup : MonoBehaviour {
    public List<GameObject> backgrounds;
    public List<Color> tileColors;
    public static LevelVisualSetup Instance { get; private set; }
    public bool isDebug = false;
    public int debugSelection = 0;
    // Use this for initialization
    void Awake()
    {
        Instance = this;
    }
    void Start () {
        
		for(int i = 0; i < backgrounds.Count; i++)
        {
            if(isDebug)
                backgrounds[i].SetActive(i == debugSelection);
            else
                backgrounds[i].SetActive(i == Var.currentBG);
        }       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
