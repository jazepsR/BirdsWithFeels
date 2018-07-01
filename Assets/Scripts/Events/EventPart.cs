using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPart : MonoBehaviour {
    public int speakerId = 0;
    [TextArea(3, 10)]
    public string text;
    public bool useCustomPic = false;
    public Sprite customPic;

}
