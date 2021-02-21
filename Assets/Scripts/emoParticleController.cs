using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class emoParticleController : MonoBehaviour {

	public ParticleSystem particles_social;
	public ParticleSystem particles_lonely;
	public ParticleSystem particles_confident;
	public ParticleSystem particles_cautious;
	public ParticleSystem particles_shield;
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
			case Var.Em.Social:
                if(particles_social)
				    particles_social.Emit(getNumber(intensity));
                else
                    Debug.LogError("Particle reference missing!");
                break;

			case Var.Em.Confident:
                if(particles_confident)                
				    particles_confident.Emit(getNumber(intensity));
                else
                    Debug.LogError("Particle reference missing!");
                break;

			case Var.Em.Solitary:
                if(particles_lonely)
				    particles_lonely.Emit(getNumber(intensity));
                else
                    Debug.LogError("Particle reference missing!");
                break;

			case Var.Em.Cautious:
                if (particles_cautious)
                    particles_cautious.Emit(getNumber(intensity));
                else
                    Debug.LogError("Particle reference missing!");
				break;

			case Var.Em.Shield:
				if (particles_shield)
					particles_shield.Emit(getNumber(intensity));
				else
					Debug.LogError("Particle reference missing!");
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
