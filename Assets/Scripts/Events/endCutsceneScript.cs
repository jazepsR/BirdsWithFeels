using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endCutsceneScript : MonoBehaviour {
	public cutScene endCutscene;
	public ending endingScript;
	// Use this for initialization
	void Start () {
		endingScript.BuildCutscene(endCutscene);
		endCutscene.StartCutscene();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
