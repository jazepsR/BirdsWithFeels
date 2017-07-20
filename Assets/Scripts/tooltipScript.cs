using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tooltipScript : MonoBehaviour {
    public string tooltipText;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void OnMouseEnter()
    {
        Helpers.Instance.ShowTooltip(tooltipText);

    }
    void OnMouseExit()
    {
        Helpers.Instance.HideTooltip();

    }
}
