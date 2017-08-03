using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollow : MonoBehaviour {
    public Transform target;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(target!= null)
            transform.position =new Vector3(target.position.x,target.position.y, 0);
        else
            transform.position = new Vector3(-10000,-10000);
    }
}
