using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager Instance { get; private set; }

	public List<AudioClip> sounds;

	public AudioSource soundPlayer;

	// Use this for initialization
	void Start () {
		
	}

	void Awake()
	{
		Instance = this;

		soundPlayer = GetComponent<AudioSource> ();
	}

	public void PlaySoundFX(string name)
	{
		// Try to find the sound!
		foreach (var fx in sounds) {
			if (fx.name.Contains (name)) {
				soundPlayer.PlayOneShot (fx);
				break;
			}
		}
	}
}
