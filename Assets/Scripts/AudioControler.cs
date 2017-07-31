using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioControler : MonoBehaviour {
    public static AudioControler Instance { get; private set; }
    public bool inBattle = false;
    [Header("Sound cilps")]
    public AudioClip applause;   
    public AudioClip click;
    public AudioClip pickupBird;
    public AudioClip dropBird;
    public AudioClip enemyMove;
    public AudioClip playerWin;
    public AudioClip mouseOverBird;
    public AudioClip expand;    
    public AudioClip enemyMouseover1;
    public AudioClip enemyMouseover2;
    public AudioClip paperSound;
    public AudioClip battleStart;
    public AudioClip newEmotion;
    [Header("Ambient sounds")]
    public AudioClip[] AmbientSounds;
    public AudioClip[] battleTracks;
    [Header("Audio sources")]
    public AudioSource mainAudioSource;
    public AudioSource ambientAudioSource;
    public AudioSource battleSource;
    public AudioSource musicSource;
    float musicVol;
    float battleVol;
    // Use this for initialization
    void Start ()
    {
       
        
        Instance = this;
        LeanTween.delayedCall(0.2f, StartSound);
        if (inBattle)
        {
            LeanTween.delayedCall(Random.Range(7, 20), AmbientControl);
            musicVol = musicSource.volume;
            battleVol = battleSource.volume;
            battleSource.volume = 0.0f;
        }

    }
    void battleVolumeToggle(float vol)
    {
        battleSource.volume = vol*battleVol;
        musicSource.volume = (1 - vol)*musicVol;
    }


    public void setBattleVolume(float vol)
    {
        if (vol != 0.0f)
            battleSource.PlayOneShot(battleTracks[Random.Range(0, battleTracks.Length)]);
        LeanTween.value(gameObject, battleVolumeToggle, battleSource.volume,vol, 2.6f);
    }
    void AmbientControl()
    {
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
        mainAudioSource.PlayOneShot(click);
    }
    public void EnemySound()
    {
        if (Helpers.Instance.RandomBool())
            mainAudioSource.PlayOneShot(enemyMouseover1);
        else
            mainAudioSource.PlayOneShot(enemyMouseover2);
    }
    public void PlayPaperSound()
    {
        mainAudioSource.PlayOneShot(paperSound);
    }
}
