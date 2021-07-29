using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BandageSwitcher : MonoBehaviour
{
    [SerializeField]
    public BandageSpriteData[] bandageSprites;

    // Start is called before the first frame update
    public void SetBandages(bool isHurt)
    {
        foreach(BandageSpriteData data in bandageSprites)
        {
            if(data.bandagedRenderer && data.healthySprite && data.bandagedSprite)
            {
                data.bandagedRenderer.sprite = isHurt ? data.bandagedSprite : data.healthySprite;
            }
        }
    }
}

[System.Serializable]
public class BandageSpriteData
{
    public Sprite healthySprite;
    public Sprite bandagedSprite;
    public SpriteRenderer bandagedRenderer;
}
