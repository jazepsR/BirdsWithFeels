using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class AudioGroup
{
	public AudioClip[] clips;
	public bool usePitchVariation = false;
	public audioSourceType sourceType = audioSourceType.main;
	[Range(0, 2f)]
	public float minPitch=1f;
	[Range(0, 2f)]
	public float maxPitch=1f;
	public void Play()
	{
		AudioControler.Instance.PlaySound(this);
	}
}
public enum audioSourceType { main,ambient,birdVoices,ui,particles, other};
public class AudioControler : MonoBehaviour {
	public static AudioControler Instance { get; private set; }
	public bool inBattle = false;
	[Header("Sound clips")]
	public AudioGroup playerWin;
	public AudioGroup combatLose;
	public AudioGroup birdTalk;
	public AudioGroup pickupBird;
	public AudioGroup dropBird;
	public AudioGroup newEmotion;
	public AudioGroup levelUp;
	public AudioGroup enemyRun;
	public AudioGroup considerSound;
	public AudioGroup fightCloudSound;
	public AudioGroup createLines;
	public AudioGroup SolitaryAppear;
	public AudioGroup BirdSitDown;
	public AudioGroup SocialInfoAppear;
	public AudioGroup SolitaryInfoAppear;
	public AudioGroup clicks;
	public AudioGroup fightButtonAppear;
    public AudioGroup fightButtonClick;
	public AudioGroup paperSound;
	public AudioGroup notebookOpen;
	public AudioGroup notebookClose;
	public AudioGroup notebookLeft;
	public AudioGroup notebookRight;
	public AudioGroup rockMouseover;
	public AudioGroup effectTileMouseover;
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
    public AudioGroup speechBubbleAppear;
    public AudioGroup speechBubbleContinue;
    [Header("Powerup effects")]
	public AudioGroup powerTilePositive;
	public AudioGroup powerTileNegative;
	public AudioGroup powerTileHeart;
	public AudioGroup powerTileCombat;

	[Header("Ambient sounds")]
	public AudioGroup AmbientSounds;
	public AudioGroup battleTracks;
	[Header("Audio sources")]
	public AudioSource mainAudioSource;
	public AudioSource ambientAudioSource;
	public AudioSource battleSource;
	public AudioSource musicSource;
	public AudioSource UiEffects;
	public AudioSource birdVoices;
	public AudioSource otherEffects;
	public AudioSource particleSounds;

	[Header("Additional sounds (not planned in docment")]
	public AudioGroup conflictWin;
	public AudioGroup applause;
	public AudioGroup mouseOverBird;
	public AudioGroup enterLevelUiArea;
	public AudioGroup showTooltip;
	public AudioGroup enemyMouseover;
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
	}

    private void Start()
    {
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
        if(UiEffects)
            UiEffects.volume = defaultSoundVol;
        if(birdVoices)
            birdVoices.volume = defaultSoundVol;
        if(otherEffects)
            otherEffects.volume = defaultSoundVol;
        if(particleSounds)
            particleSounds.volume = defaultSoundVol;

    }
public void PlaySoundWithPitch(AudioClip clip, audioSourceType sourceType, int pitchRange=0)
	{
		//PitchRange range = pitchRanges[System.Math.Min(pitchRanges.Length - 1, pitchRange)];
		//mainAudioSource.pitch = Random.Range(range.minPitch, range.maxPitch);
		GetAudioSource(sourceType).PlayOneShot(clip);       
	}

	public void PlaySound(AudioClip clip, audioSourceType sourceType)
	{
		GetAudioSource(sourceType).pitch = 1f;
		GetAudioSource(sourceType).PlayOneShot(clip);
	}
	public void PlaySound(AudioGroup group)
	{
		AudioSource source = GetAudioSource(group.sourceType);
		if (group.usePitchVariation)
			source.pitch = Random.Range(group.minPitch, group.maxPitch);
		else
			source.pitch = 1f;
		if(group.clips.Length ==0)
		{
			Debug.LogError("Audio clip has no sound assigned!");
			return;
		}
		source.PlayOneShot(group.clips[Random.Range(0, group.clips.Length)]);
	}
	public AudioSource GetAudioSource(audioSourceType sourceType)
	{
		switch(sourceType)
		{
			case audioSourceType.ui:
				return UiEffects;
			case audioSourceType.particles:
				return particleSounds;
			case audioSourceType.birdVoices:
			   return birdVoices;
			case audioSourceType.main:
				return mainAudioSource;
			case audioSourceType.other:
				return otherEffects;
			case audioSourceType.ambient:
				return ambientAudioSource;
			default:
				return mainAudioSource;
		}
	}
	public void PlayRandomSound(AudioClip[] clips, audioSourceType sourceType)
	{
		PlaySound(clips[Random.Range(0, clips.Length)],sourceType);
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
			PlaySound(birdTalk);
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
			PlaySound(battleTracks);
		LeanTween.value(gameObject, battleVolumeToggle, battleSource.volume,vol, 2.6f);
	}
	void AmbientControl()
	{
		if (!inBattle || ambientAudioSource== null)
			return;        
		PlaySound(AmbientSounds);
		if(Helpers.Instance.RandomBool())
			PlaySound(AmbientSounds);        
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
		PlaySound(enemyMouseover);
	}
	public void PlayPaperSound()
	{
		PlaySound(paperSound);
	}
}
