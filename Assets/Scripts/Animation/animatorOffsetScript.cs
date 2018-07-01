using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatorOffsetScript : MonoBehaviour {

    Animator anime;
         float offset;

	// Use this for initialization
	void Start () {
        anime = GetComponent<Animator>();
       
            offset = Random.Range(0.0f, 1.0f);
        

        anime.SetFloat("offset", offset);
        //print( offset);
	}

}
