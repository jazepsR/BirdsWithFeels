using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[System.Serializable]

public class AudioGroup
{
	public AudioClip[] clips;
	public bool usePitchVariation = false;
	public audioSourceType sourceType = audioSourceType.main;
	[Range(0, 2f)]
	public float volume=1f;
	[Range(0, 2f)]
	public float minPitch=1f;
	[Range(0, 2f)]
	public float maxPitch=1f;
	public void Play()
	{
      // Debug.LogError("Playing sound: "+clips[0].name );
		AudioControler.Instance.PlaySound(this);
	}
}
public enum audioSourceType { main,ambient,birdVoices,ui,particles, other, eventAudio,graphMusic, battleSource, musicSource };
public class AudioControler : MonoBehaviour {
    public static AudioControler Instance { get; private set; }
    public bool inBattle = false;
    [Header("Sound clips")]
    public AudioMixerSnapshot defaultSnapshot;
    public AudioGroup playerWin;
    public AudioGroup combatLose;
    public AudioGroup battleOver;
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
    public AudioGroup tileHighlightBirdHover;
    public AudioGroup tileHighlightBirdHoverSpecial;
    public AudioGroup confidentParticlePostBattle;
    public AudioGroup heartParticlePostBattle;
    [Header("Graph effects")]
    public AudioGroup smallGraphAppear;
    public AudioGroup smallGraphDisappear;
    public AudioGroup returnButton;
    public AudioGroup graphHighlight;
    public AudioGroup graphMove;
    public AudioGroup enterDangerZone;
    public AudioGroup exitDangerZone;
    public AudioGroup loseMHP;
    public AudioGroup collectEmoSeed;
    public AudioGroup emoBarFill;
    public AudioGroup iconMove;
    public AudioGroup birdInjuredPopup;
    [Header("Map effects")]
    public AudioGroup mapNodeClick;
    public AudioGroup buttonSoundMap;
    public AudioGroup restSound;
    public AudioGroup mapPanGrab;
    public AudioGroup mapPan;
    public AudioGroup mapPanRelease;
    public AudioGroup mouseOverLockedNode;
    public AudioGroup tutorialButtonClick;
    public AudioGroup mapSelectBtnClick;
    public AudioGroup startGameBtnClick;
    [Header("UI effects")]
    public AudioGroup hoverSounds;
    public AudioGroup speechBubbleAppear;
    public AudioGroup speechBubbleContinue;
    [Header("Main menu clips")]
    public AudioGroup mainMenuNewGameHighlight;
    public AudioGroup mainMenuLoadGameHighlight;
    public AudioGroup mainMenuFreeSaveBtnClick;
    public AudioGroup hoverOnMultipleChoiceBtn;
    public AudioGroup StartGameBtnClick;

    [Header("Powerup effects")]
    public AudioGroup powerTilePositive;
    public AudioGroup powerTileNegative;
    public AudioGroup powerTileHeart;
    public AudioGroup powerTileCombat;
    public AudioGroup powerTileShield;

    [Header("Ambient sounds")]
    public AudioGroup AmbientSounds;
    public AudioGroup battleTracks;
    [Header("Music")]
    public AudioClip defaultThinkingMusic;
    public AudioClip TrialThinkingMusic;
    public AudioClip bossThinkingMusic;
    public AudioClip levelCompleteMusic;
    public AudioClip introCutSceneMusic;
    //public AudioClip emoChartMusic;

    [Header("Audio sources")]
    public AudioSource mainAudioSource;
    public AudioSource ambientAudioSource;
    public AudioSource battleSource;
    public AudioSource musicSource;
    public AudioSource UiEffects;
    public AudioSource birdVoices;
    public AudioSource otherEffects;
    public AudioSource particleSounds;
    public AudioSource emoGraphSource;
    [Header("Individual bird sounds")]
    public AudioGroup VultureDialogueSounds;
    public BirdSound TerrySounds;
    public BirdSound RebeccaSounds;
    public BirdSound AlexSound;
    public BirdSound KimSound;
    public BirdSound SophieSound;
    public BirdSound DefaultBirdSound;
    [Header("Bird event sounds")]
    public List<EventAudio> eventCharacterAudio;
    [Header("Additional sounds (not planned in docment")]
    public AudioGroup conflictWin;
    public AudioGroup applause;
    public AudioGroup enterLevelUiArea;
    public AudioGroup showTooltip;
    public AudioGroup enemyMouseover;
    [HideInInspector]
    public float defaultMusicVol, defaultSoundVol;
    float pitch = 1;
    public float lastSoundTickTime = 0;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        LeanTween.delayedCall(0.2f, StartSound);
        if (inBattle)
        {
            LeanTween.delayedCall(UnityEngine.Random.Range(7, 20), AmbientControl);
            battleSource.volume = 0.0f;
        }
        defaultSoundVol = PlayerPrefs.GetFloat("soundVol", 1);
        defaultMusicVol = PlayerPrefs.GetFloat("musicVol", 1);

    }

    private void Start()
    {        
        if (Var.ambientSounds != null)
        {
            AmbientSounds = Var.ambientSounds;
        }
        if (mainMenuScript.Instance == null)
        {
          
            if (inBattle)
            {

                AudioControler.Instance.ActivateMusicSource(audioSourceType.musicSource);
                Debug.Log("hi");
                if (Var.isBoss)
                {
                    musicSource.clip = bossThinkingMusic;
                }
                else if (Var.freezeEmotions)
				{
                    musicSource.clip = TrialThinkingMusic;
                    
				}
                else
                {
                    musicSource.clip = defaultThinkingMusic;
                }
            }
        }
        musicSource.Play();
        SetSoundVol();

    }
    public void PlayStartGameHover()
    {
        PlaySound(mainMenuNewGameHighlight);
    }
    public void PlayLoadGameHover()
    {
        PlaySound(mainMenuLoadGameHighlight);
    }
    public void PlayTutorialBtnClick()
    {
        PlaySound(tutorialButtonClick);
    }
    public void PlayIntroCutSceneMusic()
    {
        //LeanTween.value(gameObject, battleVolumeToggle, battleSource.volume, 1, 1f);
        musicSource.clip = introCutSceneMusic;
        musicSource.loop = true;
        musicSource.Play();
    }
    public BirdSound GetBirdSoundGroup(string charName)
    {
        charName = charName.ToLower();
        switch (charName)
        {
            case "terry":
                return TerrySounds;
            case "kim":
                return KimSound;
            case "rebecca":
                return RebeccaSounds;
            case "alex":
                return AlexSound;
            case "alexander":
                return AlexSound;
            case "sophie":
                return SophieSound;
            default:
                return DefaultBirdSound;
        }
    }

    public EventAudio GetEventAudio(EventScript.Character character)
    {
        foreach(EventAudio evAudio in eventCharacterAudio)
        {
            if (evAudio.character == character)
            {
                return evAudio;
            }
        }
        switch (character)
        {
            case EventScript.Character.Terry:
                return TerrySounds.eventAudio;
            case EventScript.Character.Kim:
                return KimSound.eventAudio;
            case EventScript.Character.Rebecca:
                return RebeccaSounds.eventAudio;
            case EventScript.Character.Alexander:
                return AlexSound.eventAudio;
            case EventScript.Character.Sophie:
                return SophieSound.eventAudio;
            default:
                return DefaultBirdSound.eventAudio;
        }
    }



    private void SetAudioSnapshot()
	{
		if(Var.snapshot==null)
		{
			defaultSnapshot.TransitionTo(0.2f);
		}
		else
		{
			Var.snapshot.TransitionTo(0.2f);
		}
	}
    public void SetSoundVol()
	{
        if (Time.timeSinceLevelLoad > lastSoundTickTime + 0.3f)
        {
            ClickSound();
            lastSoundTickTime = Time.timeSinceLevelLoad;
        }
		mainAudioSource.volume = defaultSoundVol;
		if(ambientAudioSource)
			ambientAudioSource.volume = defaultSoundVol;
		if(battleSource)
			battleSource.volume = defaultMusicVol;
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
        Debug.LogError("Playing sound: " + clip.name);
		GetAudioSource(sourceType).pitch = 1f;
		GetAudioSource(sourceType).PlayOneShot(clip);
	}
	public void PlaySound(EventAudio eventSound)
	{
       // Debug.LogError(eventSound.sylables[0].name);
		if(eventSound.sylables.Length == 0)
		{
			return;
		}
       eventTalk = StartCoroutine(PlayNextBirdTalk(eventSound));      
	}
    private Coroutine eventTalk = null;
    private IEnumerator PlayNextBirdTalk(EventAudio eventAudio)
    {
        AudioSource source = GetAudioSource(audioSourceType.birdVoices);
        source.Stop();
       // LeanTween.value(gameObject, (float val) => source.volume = val, 0f, 1f, 0.3f);
        source.pitch = 1f;
        source.clip = (eventAudio.sylables[UnityEngine.Random.Range(0, eventAudio.sylables.Length)]);
        /*if(eventSound.startPoints.Length != 0)
		{
			int number= Random.Range(0,eventSound.startPoints.Length);
			Debug.Log("numbre: "+ number);
			source.time = eventSound.startPoints[number];

		}*/
        source.PlayOneShot(source.clip,eventAudio.volume);
        yield return new WaitForSeconds(source.clip.length + 0.15f);
        eventTalk = StartCoroutine(PlayNextBirdTalk(eventAudio));
    }
	public void FadeOutBirdTalk()
    {
        if (eventTalk != null)
        {
            StopCoroutine(eventTalk);
        }
	}
	public void PlaySound(AudioGroup group)
	{
		AudioSource source = GetAudioSource(group.sourceType);

		if (group.usePitchVariation)
			source.pitch = UnityEngine.Random.Range(group.minPitch, group.maxPitch);
		else
			source.pitch = 1f;
		if(group.clips.Length ==0)
		{
			return;
		}
		source.PlayOneShot(group.clips[UnityEngine.Random.Range(0, group.clips.Length)],group.volume);
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
            case audioSourceType.eventAudio:
                return EventController.Instance.eventAudioSource;
            case audioSourceType.graphMusic:
                return emoGraphSource;
            case audioSourceType.battleSource:
                return battleSource;
            case audioSourceType.musicSource:
                return musicSource;
			default:
				return mainAudioSource;
		}
	}
	public void PlayRandomSound(AudioClip[] clips, audioSourceType sourceType)
	{
		PlaySound(clips[UnityEngine.Random.Range(0, clips.Length)],sourceType);
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

	void battleVolumeToggle(float vol)
	{
		battleSource.volume = vol*defaultSoundVol;
		musicSource.volume = (1 - vol)*defaultMusicVol;
	}

    public void PlayWinMusic()
    { 
        //LeanTween.value(gameObject, battleVolumeToggle, battleSource.volume, 1, 1f);
        musicSource.clip = levelCompleteMusic;
        //musicSource.loop = true; //loops victory music
        AudioControler.Instance.ActivateMusicSource(audioSourceType.musicSource);
        musicSource.Play();
    }

    public void ActivateMusicSource(audioSourceType sourceToActivate)
	{
        
       // Debug.LogError("activate source: " + sourceToActivate);
		if (sourceToActivate == audioSourceType.battleSource)
			PlaySound(battleTracks);
        StartCoroutine(ToggleMusicSource(sourceToActivate, 0.5f));
	}

    private IEnumerator ToggleMusicSource(audioSourceType sourceToActivate, float lerpTime)
    {
        List<AudioSource> audioSources = new List<AudioSource>();
        audioSources.Add(GetAudioSource(audioSourceType.battleSource));
        audioSources.Add(GetAudioSource(audioSourceType.graphMusic));
        audioSources.Add(GetAudioSource(audioSourceType.musicSource));

        AudioSource activeSource = GetAudioSource(sourceToActivate);
        audioSources.Remove(activeSource);
        activeSource.Play();
        float t = 0;
        while(t<1)
        {
            t += Time.deltaTime / lerpTime;
            activeSource.volume = Mathf.Max(activeSource.volume, t);
            foreach(AudioSource source in audioSources)
            {
                source.volume = Mathf.Min(source.volume, 1 - t);
            }
            yield return null;
        }
        foreach (AudioSource source in audioSources)
        {
            source.Stop();
        }
    }

    void AmbientControl()
	{
		if (!inBattle || ambientAudioSource== null)
			return;        
		PlaySound(AmbientSounds);     
		LeanTween.delayedCall(UnityEngine.Random.Range(20, 45), AmbientControl);
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
