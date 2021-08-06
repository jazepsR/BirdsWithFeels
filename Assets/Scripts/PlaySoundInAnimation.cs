using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundInAnimation : MonoBehaviour
{
    public AudioGroup soundToPlay;
    private Renderer rend= null;
    public bool checkRenderer = true;
    private void Start()
    {
        rend = GetComponent<Renderer>();
    }
    // Start is called before the first frame update
    public void PlaySound()
    {
        if (!checkRenderer || (rend != null && rend.isVisible))
        {
            AudioControler.Instance.PlaySound(soundToPlay);
        }
    }
}
