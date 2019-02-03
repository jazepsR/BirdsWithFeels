using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AudioGroup
{
	public AudioClip[] clips;
	public bool usePitchVariation = false;
	[Range(0, 2f)]
	public float minPitch=1f;
	[Range(0, 2f)]
	public float maxPitch=1f;
	public void Play()
	{
		AudioControler.Instance.PlaySound(this);
	}
}
public class AudioControler : MonoBehaviour {
	public static AudioControler Instance { get; private set; }
	public bool inBattle = false;
	[Header("Sound clips")]
	public AudioClip playerWin;
	public AudioClip combatLose;
	public AudioClip[] birdTalk;
	public AudioClip pickupBird;
	public AudioClip dropBird;
	public AudioClip newEmotion;
	public AudioClip levelUp;
	public AudioClip enemyRun;
	public AudioClip considerSound;
	public AudioClip fightCloudSound;
	public AudioClip createLines;
	public AudioClip SolitaryAppear;
	public AudioGroup BirdSitDown;
	public AudioClip SocialInfoAppear;
	public AudioClip SolitaryInfoAppear;
	public AudioGroup clicks;
	public AudioClip fightButtonClick;
	public AudioClip paperSound;
	public AudioClip[] notebookOpen;
	public AudioClip[] notebookClose;
	public AudioClip[] notebookLeft;
	public AudioClip[] notebookRight;
	public AudioClip[] rockMouseover;
	public AudioClip[] effectTileMouseover;
	public AudioGroup birdSelect;
	[Header("Graph effects")]
	public AudioGroup smallGraphAppear;
	public AudioGroup smallGraphDisappear;
	public AudioGroup returnButton;
	public AudioGroup graphHighlight;
	public AudioGroup graphMove;
	[Header("Map effects")]
	public AudioGroup mapNodeClick;
	public AudioGroup buttonSoundMap;
	public AudioGroup restSound;
	public AudioGroup mapPanGrab;
	public AudioGroup mapPan;
	public AudioGroup mapPanRelease;
	public AudioGroup mouseOverLockedNode;
	[Header("UI effects")]
	public AudioGroup hoverSounds;
	[Header("Powerup effects")]
	public AudioGroup powerTilePositive;
	public AudioGroup powerTileNegative;
	public AudioGroup powerTileHeart;
	public AudioGroup powerTileCombat;

	[Header("Ambient sounds")]
	public AudioClip[] AmbientSounds;
	public AudioClip[] battleTracks;
	[Header("Audio sources")]
	public AudioSource mainAudioSource;
	public AudioSource ambientAudioSource;
	public AudioSource battleSource;
	public AudioSource musicSource;

	[Header("Additional sounds (not planned in docment")]
	public AudioClip conflictWin;
	public AudioClip applause;
	public AudioClip mouseOverBird;
	public AudioClip[] expand;    
	public AudioClip enemyMouseover1;
	public AudioClip enemyMouseover2;
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
		ClickSound();
		mainAudioSource.volume = defaultSoundVol;
		if(ambientAudioSource)
			ambientAudioSource.volume = defaultSoundVol;
		if(battleSource)
			battleSource.volume = defaultSoundVol;
		if(musicSource)
			musicSource.volume = defaultMusicVol;
	}
	public void PlaySoundWithPitch(AudioClip clip, int pitchRange=0)
	{
		//PitchRange range = pitchRanges[System.Math.Min(pitchRanges.Length - 1, pitchRange)];
		//mainAudioSource.pitch = Random.Range(range.minPitch, range.maxPitch);
		mainAudioSource.PlayOneShot(clip);       
	}

	public void PlaySound(AudioClip clip)
	{
		mainAudioSource.pitch = 1f;
		mainAudioSource.PlayOneShot(clip);
	}
	public void PlaySound(AudioGroup group)
	{
		if (group.usePitchVariation)
			mainAudioSource.pitch = Random.Range(group.minPitch, group.maxPitch);
		else
			mainAudioSource.pitch = 1f;
		if(group.clips.Length ==0)
		{
			Debug.LogError("Audio clip has no sound assigned!");
			return;
		}
		mainAudioSource.PlayOneShot(group.clips[Random.Range(0, group.clips.Length)]);
	}
	public void PlayRandomSound(AudioClip[] clips)
	{
		PlaySound(clips[Random.Range(0, clips.Length)]);
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
		clicks.Play();
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
