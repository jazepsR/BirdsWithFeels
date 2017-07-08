using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiEffects : MonoBehaviour {
    public float killTime=1.0f;
    public float scaleTo = 1.5f;
    public bool fadeOut;
	// Use this for initialization
	void Start () {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color col = sr.color;
        Vector3 scale = transform.localScale * scaleTo;
        LeanTween.scale(gameObject, scale, killTime).setEase(LeanTweenType.easeInCubic);
        if (fadeOut)
            LeanTween.color(gameObject, new Color(col.r, col.g, col.b, 0), killTime);
        Destroy(gameObject, killTime);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
