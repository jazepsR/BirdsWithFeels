using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVLIconScript : MonoBehaviour {
    [HideInInspector]
    public string textToDsiplay;
	// Use this for initialization
	void Start () {
		
	}
	
	public void ShowTooltip()
    {
        Helpers.Instance.ShowTooltip(textToDsiplay);

    }
}
