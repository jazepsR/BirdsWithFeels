using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundInAnimation : MonoBehaviour
{
    public AudioGroup soundToPlay;
    private Renderer renderer= null;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
    }
    // Start is called before the first frame update
    public void PlaySound()
    {
        if (renderer != null && renderer.isVisible)
        {
            AudioControler.Instance.PlaySound(soundToPlay);
        }
    }
}
