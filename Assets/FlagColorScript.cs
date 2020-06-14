using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagColorScript : MonoBehaviour
{
    public Color flagColor;

    

    // Start is called before the first frame update
    void Start()
    {
        
        if (flagColor==null)
        {
            flagColor = new Color(90, 30, 30);
        }

        int x = 20;
        flagColor = flagColor + new Color(Random.Range(-x, x), Random.Range(-x, x), Random.Range(-x, x));

        SpriteRenderer flagrenderer = GetComponentInChildren<SpriteRenderer>();
        flagrenderer.color = flagColor;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
