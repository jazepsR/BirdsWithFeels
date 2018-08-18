using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjuryIndicator : MonoBehaviour {
	public Bird bird;
	public TextMesh number;
	Animator anim;
	int prevInjury=4;
	// Use this for initialization
	void Start () {
		if (bird == null)
			gameObject.SetActive(false);
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {


		gameObject.SetActive(bird.data.injured);
		number.text = bird.data.TurnsInjured.ToString();
		if (prevInjury != bird.data.TurnsInjured)
			anim.SetTrigger("Increment");
		prevInjury = bird.data.TurnsInjured;

	}
}
