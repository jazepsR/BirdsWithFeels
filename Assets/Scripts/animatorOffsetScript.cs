using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatorOffsetScript : MonoBehaviour {

    Animator anime;
    public float offset;

	// Use this for initialization
	void Start () {
        anime = GetComponent<Animator>();
        if (offset == 0)
        {
            offset = Random.Range(0, 50)/10;
        }

        anime.SetFloat("offset", offset);
        //print( offset);
	}

}
