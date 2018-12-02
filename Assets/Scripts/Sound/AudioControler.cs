using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControler : MonoBehaviour {
	public static AudioControler Instance { get; private set; }
	public bool inBattle = false;
	[Header("Sound cilps")]
	public AudioClip applause;   
	public AudioClip click;
    public AudioClip mapNodeClick;
    public AudioClip fightButtonClick;
	public AudioClip pickupBird;
	public AudioClip dropBird;
	public AudioClip enemyMove;
	public AudioClip playerWin;
	public AudioClip mouseOverBird;
	public AudioClip expand;    
	public AudioClip enemyMouseover1;
	public AudioClip enemyMouseover2;
	public AudioClip paperSound;
	public AudioClip newEmotion;
	public AudioClip[] birdTalk;
    [Header("Particle sounds")]
    public AudioClip createLines;
    public AudioClip SolitaryAppear;
    public AudioClip BirdSitDown;
    public AudioClip SocialInfoAppear;
    public AudioClip SolitaryInfoAppear;
    [Header("combat sounds")]
    public AudioClip conflictWin;
    public AudioClip conflictLose;
    public AudioClip enemyRun;
    public AudioClip considerSound;
    public AudioClip fightCloudSound;
    [Header("Ambient sounds")]
	public AudioClip[] AmbientSounds;
	public AudioClip[] battleTracks;
	[Header("Audio sources")]
	public AudioSource mainAudioSource;
	public AudioSource ambientAudioSource;
	public AudioSource battleSource;
	public AudioSource musicSource;
	[HideInInspector]
	public float defaultMusicVol, defaultSoundVol;
	float pitch = 1;    
	// Use this for initialization
	void Awake ()
	{      
		Instance = this;
		LeanTween.delayedCall(0.2f, StartSound);
		if (inBattle)
		{
			LeanTween.delayedCall(Random.Range(7, 20), AmbientControl);
			battleSource.volume = 0.0f;
		}
		defaultSoundVol = PlayerPrefs.GetFloat("soundVol", 1);
		defaultMusicVol = PlayerPrefs.GetFloat("musicVol", 1);
		SetSoundVol();
	}   



	public void SetSoundVol()
	{
		mainAudioSource.volume = defaultSoundVol;
		if(ambientAudioSource)
			ambientAudioSource.volume = defaultSoundVol;
		if(battleSource)
			battleSource.volume = defaultSoundVol;
		if(musicSource)
			musicSource.volume = defaultMusicVol;
	}
	public void PlaySoundWithPitch(AudioClip clip)
	{
		mainAudioSource.pitch = Random.Range(0.6f, 1.6f);
		mainAudioSource.PlayOneShot(clip);       
	}

	public void PlaySound(AudioClip clip)
	{
		mainAudioSource.pitch = 1f;
		mainAudioSource.PlayOneShot(clip);
	}

	public void SetSoundVolume(float vol)
	{
		defaultSoundVol = vol;
		SetSoundVol();
	}

	public void SetMusicVolume(float vol)
	{
		defaultMusicVol = vol;
		SetSoundVol();
	}
	public void SaveVolumeSettings()
	{
		PlayerPrefs.SetFloat("soundVol", defaultSoundVol);
		PlayerPrefs.SetFloat("musicVol", defaultMusicVol);
		PlayerPrefs.Save();
		SetSoundVol();
	}

	public void PlayVoice()
	{
		try
		{
			PlaySoundWithPitch(birdTalk[Random.Range(0, birdTalk.Length)]);
		}
		catch {
			print("ddd");
		}
	}


	void battleVolumeToggle(float vol)
	{
		battleSource.volume = vol*defaultSoundVol;
		musicSource.volume = (1 - vol)*defaultMusicVol;
	}


	public void setBattleVolume(float vol)
	{
		if (vol != 0.0f)
			battleSource.PlayOneShot(battleTracks[Random.Range(0, battleTracks.Length)]);
		LeanTween.value(gameObject, battleVolumeToggle, battleSource.volume,vol, 2.6f);
	}
	void AmbientControl()
	{
		if (!inBattle || ambientAudioSource== null)
			return;        
		ambientAudioSource.PlayOneShot(AmbientSounds[Random.Range(0, AmbientSounds.Length)]);
		if(Helpers.Instance.RandomBool())
			ambientAudioSource.PlayOneShot(AmbientSounds[Random.Range(0, AmbientSounds.Length)]);        
		LeanTween.delayedCall(Random.Range(7, 45), AmbientControl);
	}




	void StartSound()
	{
		mainAudioSource.volume = 1.0f;
	}
	// Update is called once per frame
	void Update () {
		
	}
	public void ClickSound()
	{
		PlaySound(click);
	}
	public void EnemySound()
	{
		if (Helpers.Instance.RandomBool())
			PlaySoundWithPitch(enemyMouseover1);
		else
			PlaySoundWithPitch(enemyMouseover2);
	}
	public void PlayPaperSound()
	{
		PlaySound(paperSound);
	}
}
