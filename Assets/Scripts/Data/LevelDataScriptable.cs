using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData")]
public class LevelDataScriptable : ScriptableObject
{
    public List<LevelBits> levelBits;
    public bool givesHeart = true;
    public bool givesPower = true;
    public string screenTitle;
    [TextArea(5, 10)]
    public string firstLevelUpText;
    [TextArea(5, 10)]
    public string secondLevelUpText;
    public Sprite levelUpImage;
    public Sprite levelUpImage2;
    public string birdTitle;
}
