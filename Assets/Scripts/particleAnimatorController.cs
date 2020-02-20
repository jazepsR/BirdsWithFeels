using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class particleAnimatorController : MonoBehaviour {

	public ParticleSystem particles;


	// Use this for initialization
	void Start () {
		if (particles == null)
		{
			particles = GetComponent<ParticleSystem>();
		}
		particles.Stop();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

public void play()
	{
		particles.Play();
	}
	public void stop()
	{
		particles.Stop(false);
	}
}
