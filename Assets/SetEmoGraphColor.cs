using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetEmoGraphColor : MonoBehaviour
{
    public List<Image> ImagesToColor;
    public Color colorOne;
    public Color colorTwo;
    private bool colorOneActive;

    // Start is called before the first frame update
    void Start()
    {
        colorOneActive = true;
    }

    public void ToggleImageColor()
    {

        Color chosenColor;

        if (colorOneActive)
        {
            chosenColor = colorOne;
        }
        else
        {
            chosenColor = colorTwo;
        }

        

        for(int i = 0; i < ImagesToColor.Count; i++)
        {
            ImagesToColor[i].color = chosenColor;
        }
        colorOneActive = !colorOneActive;

    }
}
