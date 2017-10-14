using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emoParticleController : MonoBehaviour {

	public ParticleSystem particles_social;
	public ParticleSystem particles_lonely;
	public ParticleSystem particles_confident;
	public ParticleSystem particles_cautious;
	public int small;
	public int medium;
	public int large;

	// Use this for initialization
	void Start () {
 
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void emitParticles(Var.Em emotion, int intensity)
	{
		switch(emotion)
		{
			case Var.Em.Friendly:
				particles_social.Emit(getNumber(intensity));
				break;

			case Var.Em.Confident:
				particles_confident.Emit(getNumber(intensity));
				break;

			case Var.Em.Lonely:
				particles_lonely.Emit(getNumber(intensity));
				break;

			case Var.Em.Scared:
				particles_cautious.Emit(getNumber(intensity));
				break; 
		}
	}
	
	int getNumber(int intensity)
	{
		if(intensity==0)
		{
			return small;
		}
		if (intensity == 1)
		{
			return medium;
		}
		if (intensity == 2)
		{
			return large;
		}
		Debug.Log("Particle emitter parameter Intensity is not between 0-2. Oh no!");

		return 0;

	}
}
