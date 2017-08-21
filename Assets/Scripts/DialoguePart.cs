using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialoguePart : MonoBehaviour {
    //public enum Speaker { Tova, Terry, Kim, Rebecca, Toby , Random};
    public int speakerID = 0;
    [TextArea(3, 10)]
    public string text;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
