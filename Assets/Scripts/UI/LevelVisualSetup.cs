using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelVisualSetup : MonoBehaviour {
    public List<GameObject> backgrounds;
    public List<Color> tileColors;
    public static LevelVisualSetup Instance { get; private set; }
    public bool isDebug = false;
    public int debugSelection = 0;
    Transform  progressAnim;
    // Use this for initialization
    void Awake()
    {
        Instance = this;
		for(int i = 0; i < backgrounds.Count; i++)
        {
            if (isDebug)
                Var.currentBG = debugSelection;

              
          /*  if(Var.currentBG==0) //Seb hates the swamp
            {
                Var.currentBG=1;
            }   */      
              
            backgrounds[i].SetActive(i == Var.currentBG);

            if (i == Var.currentBG && Var.currentBackgroundProgressAnim != -1)
            {
                progressAnim = backgrounds[i].transform.Find("ProgressAnimator");
                if (progressAnim == null)
                {
                    Debug.LogError("error: unable to find a progress animator");
                    return;
                }
                try
                {
                    progressAnim.GetComponent<Animator>().SetInteger("ProgressInt", Var.currentBackgroundProgressAnim);
                }
                catch {
                    Debug.LogError("error: setting background default to 0");
                    progressAnim.GetComponent<Animator>().SetInteger("ProgressInt", 0);
                    
                }
            }
        }       
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
