using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Helpers : MonoBehaviour {
    public static Helpers Instance { get; private set; }
    [Header("Colors")]
    public Color neutral;
    public Color brave;
    public Color scared;
    public Color friendly;
    public Color lonely;
    [Header("Soft colors")]
    public Color softBrave;
    public Color softScared;
    public Color softFriendly;
    public Color softLonely;
      
    public void Awake()
    {
       
        Instance = this;
    }
    public bool RandomBool()
    {
        if (UnityEngine.Random.Range(0f, 1f) > 0.5f)
            return true;
        else
            return false;
    }
    public string GetName(bool isMale)
    {
        if (isMale)
            return Var.maleNames[UnityEngine.Random.Range(0, Var.maleNames.Length)];
        else
            return Var.femaleNames[UnityEngine.Random.Range(0, Var.femaleNames.Length)];
    }
    //Neutral means weak to all
    public Var.Em GetWeakness(Var.Em Emotion)
    {
        switch (Emotion)
        {
            case Var.Em.Neutral:
                return Var.Em.Neutral;
            case Var.Em.Lonely:
                return Var.Em.Confident;
            case Var.Em.SuperLonely:
                return Var.Em.Confident;
            case Var.Em.Friendly:
                return Var.Em.Scared;
            case Var.Em.SuperFriendly:
                return Var.Em.Scared;
            case Var.Em.Confident:
                return Var.Em.Friendly;
            case Var.Em.SuperConfident:
                return Var.Em.Friendly;
            case Var.Em.Scared:
                return Var.Em.Lonely;
            case Var.Em.SuperScared:
                return Var.Em.Lonely;
            case Var.Em.finish:
                return Var.Em.Neutral;
            default:
                return Var.Em.Neutral;
        }
    }

    //Neutral means weak to all
    public Var.Em GetStenght(Var.Em Emotion)
    {
        switch (Emotion)
        {
            case Var.Em.Neutral:
                return Var.Em.Neutral;
            case Var.Em.Lonely:
                return Var.Em.Scared;
            case Var.Em.SuperLonely:
                return Var.Em.Scared;
            case Var.Em.Friendly:
                return Var.Em.Confident;
            case Var.Em.SuperFriendly:
                return Var.Em.Confident;
            case Var.Em.Confident:
                return Var.Em.Lonely;
            case Var.Em.SuperConfident:
                return Var.Em.Lonely;
            case Var.Em.Scared:
                return Var.Em.Friendly;
            case Var.Em.SuperScared:
                return Var.Em.Friendly;
            case Var.Em.finish:
                return Var.Em.Neutral;
            default:
                return Var.Em.Neutral;
        }
    }
    public List<Bird> GetAdjacentBirds(Bird bird)
    {
        int x = bird.x;
        int y = bird.y;
        List<Bird> list = new List<Bird>();
        int sizeY = Var.playerPos.GetLength(1) - 1;
        int sizeX = Var.playerPos.GetLength(0) - 1;
        if (y + 1 <= sizeY && Var.playerPos[x, y + 1] != null)
            list.Add(Var.playerPos[x, y + 1]);
        if (y - 1 >= 0 && Var.playerPos[x, y - 1] != null)
            list.Add(Var.playerPos[x, y - 1]);
        if (x + 1 <= sizeX && Var.playerPos[x + 1, y] != null)
            list.Add(Var.playerPos[x + 1, y]);
        if (x - 1 >= 0 && Var.playerPos[x - 1, y] != null)
            list.Add(Var.playerPos[x - 1, y]);
        if (y + 1 <= sizeY && x + 1 <= sizeX && Var.playerPos[x + 1, y + 1] != null)
            list.Add(Var.playerPos[x + 1, y + 1]);
        if (y + 1 <= sizeY && x - 1 >= 0 && Var.playerPos[x - 1, y + 1] != null)
            list.Add(Var.playerPos[x - 1, y + 1]);
        if (y - 1 >= 0 && x + 1 <= sizeX && Var.playerPos[x + 1, y - 1] != null)
            list.Add(Var.playerPos[x + 1, y - 1]);
        if (y - 1 >= 0 && x - 1 >= 0 && Var.playerPos[x - 1, y - 1] != null)
            list.Add(Var.playerPos[x - 1, y - 1]);
        return list;
    }
    public string ApplyTitle(string name, string title)
    {
        if (title != "")
            return title.Replace("<name>", name);
        else
            return name;
    }

    public bool ListContainsLevel(Levels.type level, List<LevelData> list)
    {
        if (list != null)
        {
            foreach (LevelData data in list)
            {
                if (data.type == level)
                    return true;
            }
            return false;
        }else
        {
            return false;
        }
    }
    public bool ListContainsEmotion(Var.Em emotion, List<LevelData> list)
    {
        if (list != null)
        {
            foreach (LevelData data in list)
            {
                if (data.emotion == emotion)
                    return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }
    public void NormalizeStats(Bird bird)
    {
        int treshold = Var.lvl2 - 1;
        //Super starts at 10
        if (bird.level > 1)
        {
            treshold = 12;
        }
        if (bird.confidence > treshold)
            bird.confidence = treshold;
        if (bird.confidence < -treshold)
            bird.confidence = -treshold;
        if (bird.friendliness > treshold)
            bird.friendliness = treshold;
        if (bird.friendliness < -treshold)
            bird.friendliness = -treshold;
    }

    public Color GetEmotionColor(Var.Em emotion)
    {
        switch (emotion)
        {
            case Var.Em.Neutral:
                return neutral;
            case Var.Em.Friendly:
                return friendly;
            case Var.Em.Lonely:
                return lonely;
            case Var.Em.Confident:
                return brave;
            case Var.Em.Scared:
                return scared;
            case Var.Em.SuperFriendly:
                return friendly;
            case Var.Em.SuperLonely:
                return lonely;
            case Var.Em.SuperConfident:
                return brave;
            case Var.Em.SuperScared:
                return scared;
            default:
                return neutral;
        }
    }
    public Color GetSoftEmotionColor(Var.Em emotion)
    {
        switch (emotion)
        {
            case Var.Em.Neutral:
                return neutral;
            case Var.Em.Friendly:
                return softFriendly;
            case Var.Em.Lonely:
                return softLonely;
            case Var.Em.Confident:
                return softBrave;
            case Var.Em.Scared:
                return softScared;
            case Var.Em.SuperFriendly:
                return softFriendly;
            case Var.Em.SuperLonely:
                return softLonely;
            case Var.Em.SuperConfident:
                return softBrave;
            case Var.Em.SuperScared:
                return softScared;
            default:
                return neutral;
        }
    }
    public string GetLVLTitle(Levels.type lvl)
    {
        switch (lvl)
        {
            case Levels.type.Brave1:
                return "<name> the protector";
            case Levels.type.Brave2:
                return "<name> the invicible";
            case Levels.type.Friend1:
                return "<name> the healer";                
            case Levels.type.Friend2:
                return "Doctor <name>";
            case Levels.type.Lonely1:
                return "Apprentice <name>";
            case Levels.type.Lonely2:
                return "Time lord <name>";
            case Levels.type.Scared1:
                return "Cunning <name>";
            case Levels.type.Scared2:
                return "<name> the rogue";
            default:
                return "";
        }
    }
    public Vector3 dirToVector(Bird.dir dir)
    {
        float increment = 0.7f;
        switch (dir)
        {
            case Bird.dir.front:
                return new Vector3(increment, 0);
            case Bird.dir.bottom:
                return new Vector3(0, -increment);
            case Bird.dir.top:
                return new Vector3(0, increment);
            default:
                return new Vector3(0, 0, 0);
        }
    }

    public void ShowTooltip(String text)
    {
        GuiContoler.Instance.tooltipText.transform.parent.gameObject.SetActive(true);
        GuiContoler.Instance.tooltipText.text = text;
        var screenPoint = Input.mousePosition;
        screenPoint.z = 10.0f;
        GuiContoler.Instance.tooltipText.transform.parent.transform.position = Camera.main.ScreenToWorldPoint(screenPoint);

    }


    public void EmitEmotionParticles(Transform parent,Var.Em emotion)
    {
        var emParticles = Instantiate(Var.emotionParticles, parent);
        var particleSys = emParticles.GetComponent<ParticleSystem>().colorOverLifetime;
        Gradient grad = new Gradient();
        Color col = GetEmotionColor(emotion);
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(col, 0.0f), new GradientColorKey(col, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
        particleSys.color = grad;
        Text text = emParticles.transform.Find("Text").GetComponent<Text>();
        text.text = emotion.ToString().Replace("Super","");
        text.color = col;
        LeanTween.moveLocalZ(text.gameObject, 55.0f, 1f);
        LeanTween.scale(text.gameObject, Vector3.one * 2f, 1.2f);
        LeanTween.alphaText(text.rectTransform, 0.0f, 1.2f);
        Destroy(emParticles, 1.7f);
    }


    public float RandGaussian(float stdDev, float mean)
    {
        float u1 = UnityEngine.Random.Range(0.0f, 1.0f);
        float u2 = UnityEngine.Random.Range(0.0f, 1.0f);
        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        return  mean + stdDev * randStdNormal;
    }
    public bool IsSuper(Var.Em emotion)
    {
        if (emotion == Var.Em.SuperFriendly || emotion == Var.Em.SuperConfident || emotion == Var.Em.SuperLonely || emotion == Var.Em.SuperFriendly)
            return true;
        else
            return false;
    }

    public int Findfirendlieness(Bird bird)
    {        
        int x = 0;
        int y = 0;
        for (int i = 0; i < Var.playerPos.GetLength(0); i++)
        {
            for (int j = 0; j < Var.playerPos.GetLength(1); j++)
            {

                if (Var.playerPos[i, j] == bird)
                {
                    x = i;
                    y = j;
                    break;
                }
            }
        }
   
        int sizeY = Var.playerPos.GetLength(1)-1;
        int sizeX = Var.playerPos.GetLength(0)-1;
        int lonelyVal = 0;
        if(y+1<= sizeY && Var.playerPos[x, y + 1] != null)
        {
            lonelyVal += 2;
        }
        if (y - 1 >=0 && Var.playerPos[x, y - 1] != null)
        {
            lonelyVal += 2;
        }
        if (x + 1 <= sizeX && Var.playerPos[x+1, y] != null)
        {
            lonelyVal += 2;
        }
        if (x - 1 >= 0 && Var.playerPos[x-1, y ] != null)
        {
            lonelyVal += 2;
        }
        if (y+1<=sizeY && x+1<= sizeX && Var.playerPos[x+1, y + 1] != null)
        {
            lonelyVal += 1;
        }
        if (y + 1 <= sizeY && x - 1 >= 0 && Var.playerPos[x - 1, y + 1] != null)
        {
            lonelyVal += 1;
        }
        if (y - 1 >= 0 && x + 1 <= sizeX && Var.playerPos[x + 1, y - 1] != null)
        {
            lonelyVal += 1;
        }
        if (y - 1 >= 0 && x - 1 >= 0 && Var.playerPos[x - 1, y - 1] != null)
        {
            lonelyVal += 1;
        }
        return -2 + 2 * lonelyVal;

    }

    //Used to apply stat changes based on the emotion. first number is friendliness, second number is confidence
    public Vector2 ApplyEmotion(Vector2 currentStats, Var.Em emotion, int magnitude = 1)
    {
        if (emotion == Var.Em.Confident || emotion == Var.Em.SuperConfident)
            return new Vector2(currentStats.x, currentStats.y + magnitude);
        if (emotion == Var.Em.Scared || emotion == Var.Em.SuperScared)
            return new Vector2(currentStats.x, currentStats.y - magnitude);
        if (emotion == Var.Em.Friendly || emotion == Var.Em.SuperFriendly)
            return new Vector2(currentStats.x + magnitude, currentStats.y );
        if (emotion == Var.Em.Lonely || emotion == Var.Em.SuperLonely)
            return new Vector2(currentStats.x - magnitude, currentStats.y);
        return currentStats;
    }


    public string GetLVLInfoText(Levels.type level)
    {
        switch (level)
        {
            case Levels.type.Brave1:
                return "Prevent a random close bird from losing health if they lose their fight. 3 turn cooldown";
            case Levels.type.Brave2:
                return "If feeling confident - when taking damage, -2 confidence instead of -1 heart";
            case Levels.type.Friend1:
                return "Once per turn, if resting, give a random close bird +1 hearts";
            case Levels.type.Friend2:
                return "After an adventure fully heal one bird of your choice";
            case Levels.type.Scared1:
                return "Backstabbing. If in stealth, bird does not fight enemies. All enemies passing by bird gains -30% chance to win. Toggle stealth by clicking bird";
            case Levels.type.Scared2:
                return "Give all diagonal birds - X to all dice rolls  (both friendly and enemy birds)";
            case Levels.type.Lonely1:
                return "All birds in the same column gain +1 loneliness";
            case Levels.type.Lonely2:
                return " if resting, reroll all battles. 3 turn cooldown";
            case Levels.type.Toby:
                return "Impressionable- bird gains +1 in the dominant feeling of an adjacent bird";                
            case Levels.type.Kim:
                return "Ground effects affect you twice as much";
            case Levels.type.Rebecca:
                return "Lose -2 confidence when resting";
            case Levels.type.Tova:
                return "If resting, all adjacent birds recieve 10% chance to win fights";
            case Levels.type.Terry:
                return "All birds in the same column gain +1 confidence";
                
            default:
                return "Level not found error";
                
        }       
    }
}
