using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundInAnimation : MonoBehaviour
{
    public AudioGroup soundToPlay;
    // Start is called before the first frame update
    public void PlaySound()
    {
        AudioControler.Instance.PlaySound(soundToPlay);
    }
}
