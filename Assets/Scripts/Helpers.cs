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
    public Color level;
    [Header("Soft colors")]
    public Color softBrave;
    public Color softScared;
    public Color softFriendly;
    public Color softLonely;
    public string BraveHexColor;
    public string ScaredHexColor;
    public string LonelyHexColor;
    public string FriendlyHexColor;   
    GameObject heartBreak;
    GameObject heartGain;
    Sprite fullHeart;
    Sprite emptyHeart;
    public bool inMap;
    List<Image> heartsToFill = new List<Image>();
    public Transform relationshipDialogs;
    public void Awake()
    {
        heartBreak = Resources.Load<GameObject>("prefabs/heartBreak");
        heartGain = Resources.Load<GameObject>("prefabs/heartGain");
        fullHeart = Resources.Load<Sprite>("sprites/heart");
        emptyHeart = Resources.Load<Sprite>("sprites/emptyHeart");
        Instance = this;
    }
    public EventScript.Character GetCharEnum(Bird bird)
    {
        try
        {
            return (EventScript.Character)Enum.Parse(typeof(EventScript.Character), bird.charName);
        }
        catch
        {
            return EventScript.Character.None;
        }
    }
    public Bird GetBirdFromEnum(EventScript.Character ch, bool useAll = false)
    {
        List<Bird> birds;
        if (inMap || useAll)
            birds = Var.availableBirds;
        else
        {
            birds = new List<Bird>();
            birds.AddRange(FillPlayer.Instance.playerBirds);
        }
         

        if(ch == EventScript.Character.Random)
        {
            return birds[UnityEngine.Random.Range(0, birds.Count)];
        }
        foreach (Bird bird in birds)
        {
            if (bird.charName == ch.ToString())
                return bird;
        }
        return null;


    }
        public Var.Em GetOppositeEmotion(Var.Em type)
    {
        switch (type)
        {
            case Var.Em.Confident:
                return Var.Em.Scared;
            case Var.Em.Scared:
                return Var.Em.Confident;
            case Var.Em.Friendly:
                return Var.Em.Lonely;
            case Var.Em.Lonely:
                return Var.Em.Friendly;
            default:
                return Var.Em.Neutral;
        }
    }
    public GameObject GetEnemyVisual(Bird.dir dir, Var.Em emotion, fillEnemy.enemyType type)
    {
        int id = 0;
        if (dir == Bird.dir.top)
            id += 5;
        switch (emotion)
        {
            case Var.Em.Confident:
                id += 0;
                break;
            case Var.Em.Friendly:
                id += 1;
                break;
            case Var.Em.Lonely:
                id += 2;
                break;
            case Var.Em.Neutral:
                id += 3;
                break;
            case Var.Em.Scared:
                id += 4;
                break;
        }
        switch (type)
        {
            case fillEnemy.enemyType.drill:
                return Var.drillEnemySprites[id];
            case fillEnemy.enemyType.normal:
                return Var.enemySprites[id];
            case fillEnemy.enemyType.wizard:
                return Var.wizardEnemySprites[id];
        }
        return null;
        

    }
    public GameObject GetPortrait(EventScript.Character Char)
    {
        if (Char == EventScript.Character.Random)
            return GetBirdFromEnum(Char).portrait;
        else
        {
            return Resources.Load<GameObject>("prefabs/portrait_" + Char.ToString());
        }

    }
    public Var.Em RandomEmotion()
    {
        int rnd = UnityEngine.Random.Range(0, 5);
        switch (rnd)
        {
            case 0:
                return Var.Em.Neutral;
            case 1:
                return Var.Em.Scared;
            case 2:
                return Var.Em.Confident;
            case 3:
                return Var.Em.Lonely;
            case 4:
                return Var.Em.Friendly;
            default:
                return Var.Em.Neutral;
        }


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
    void FillHearts()
    {
        foreach(Image heart in heartsToFill)
        {
            heart.sprite = fullHeart;
        }
        heartsToFill = new List<Image>();
    }
    public void setHearts(Image[] hearts,int currentHP, int MaxHP, int prevRoundHealth = -1)
    {
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < currentHP)
            {
                hearts[i].gameObject.SetActive(true);
                if (i >= prevRoundHealth && prevRoundHealth != -1)
                {
                    GameObject breakObj = Instantiate(heartGain, hearts[i].transform.position, Quaternion.identity);
                    breakObj.transform.parent = hearts[i].transform;
                    breakObj.transform.localScale = Vector3.one;// * 0.5f;
                    heartsToFill.Add(hearts[i]);
                    LeanTween.delayedCall(0.46f, FillHearts);
                    Destroy(breakObj, 0.45f);
                }
                else
                {
                    hearts[i].sprite = fullHeart;
                    
                }
            } else
            {
                if (i < prevRoundHealth && prevRoundHealth != -1)
                {
                    GameObject breakObj = Instantiate(heartBreak, hearts[i].transform.position, Quaternion.identity);
                    breakObj.transform.parent = hearts[i].transform;
                    breakObj.transform.localScale = Vector3.one * 0.5f;
                    Destroy(breakObj, 0.45f);
                }
                
                if (i < MaxHP)
                {
                    hearts[i].sprite = emptyHeart;
                    hearts[i].gameObject.SetActive(true);
                }
                else
                {
                    hearts[i].gameObject.SetActive(false);
                }
                
            }
        }  
    }

    public String GetHexColor(Var.Em emotion)
    {
        switch (emotion)
        {
            case Var.Em.Friendly:
                return FriendlyHexColor;
            case Var.Em.Lonely:
                return LonelyHexColor;
            case Var.Em.Confident:
                return BraveHexColor;
            case Var.Em.Scared:
                return ScaredHexColor;
            case Var.Em.SuperFriendly:
                return FriendlyHexColor;
            case Var.Em.SuperLonely:
                return LonelyHexColor;
            case Var.Em.SuperConfident:
                return BraveHexColor;
            case Var.Em.SuperScared:
                return ScaredHexColor;
            case Var.Em.Neutral:
                return "<color=#000000FF>";
            default:
                return "<color=#000000FF>";
        }
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
    public Sprite GetHatSprite(string charName)
    {
        switch (charName)
        {
            case "Kim":
                return Var.hatSprites[1];
            case "Sophie":
                return Var.hatSprites[0];
            case "Terry":
                return Var.hatSprites[2];
            case "Rebecca":
                return Var.hatSprites[4];
            case "Alexander":
                return Var.hatSprites[3];
            default:
                return null;
        }
    }

    public string GetLevelTitle(Levels.type type)
    {
       
        switch (type)
        {
            case Levels.type.Alexander:
                return "Alexander";
            case Levels.type.Kim:
                return "Kim";
            case Levels.type.Rebecca:
                return "Rebecca";
            case Levels.type.Sophie:
                return "Sophie";
            case Levels.type.Terry:
                return "Terry";
            case Levels.type.Friend1:
                return "Healing touch";
            case Levels.type.Friend2:
                return "A great massage";
            case Levels.type.Brave1:
                return "Bird shield";
            case Levels.type.Brave2:
                return "Fake hearts";
            case Levels.type.Lonely1:
                return "Emo intensifier";
            case Levels.type.Lonely2:
                return "Time lord";
            case Levels.type.Scared1:
                return "Diagonal backstabber";
            case Levels.type.Scared2:
                return "Original backstabber";
            default:
                return "";
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
    public string ApplyTitle(Bird name, string title)
    {
        
        if (name == null)
            return title;
        if (title != "")
            return title.Replace("<name>", name.charName);
        else
            return name.charName;
    }
    public List<Bird> GetInactiveBirds()
    {
        List<Bird> inactiveBirds = new List<Bird>();
        foreach(Bird bird in Var.availableBirds)
        {
            bool isInactive = true;
            foreach(Bird activeBird in Var.activeBirds)
            {
                if (bird.charName == activeBird.charName)
                    isInactive = false;
            }
            if (isInactive)
                inactiveBirds.Add(bird);
        }
        return inactiveBirds;
    }
    public Sprite GetSkillPicture(Levels.type type)
    {
        switch (type)
        {
            case Levels.type.Brave1:
                return Var.skillIcons[0];
            case Levels.type.Brave2:
                return Var.skillIcons[1];
            case Levels.type.Friend1:
                return Var.skillIcons[6];
            case Levels.type.Friend2:
                return Var.skillIcons[7];
            case Levels.type.Lonely1:
                return Var.skillIcons[2];
            case Levels.type.Lonely2:
                return Var.skillIcons[3];
            case Levels.type.Scared1:
                return Var.skillIcons[4];
            case Levels.type.Scared2:
                return Var.skillIcons[5];
            default:
                return null;
        }
    }


    public string GetLevelUpText(string name, Levels.type type)
    {
        switch (type)
        {
            case Levels.type.Brave1:
                return "Successfully fending off two crows at once, <name> feels a surge of confidence flow through them. Nothing can stop <name> at this point! <name> promises to use their newfound confidence to shield their teammates from harm. ".Replace("<name>", name);
            case Levels.type.Brave2:
                return "There's no stopping <name> now! Nothing can hurt them! (As long as they think so, that is)".Replace("<name>", name);
            case Levels.type.Friend1:
                return "<name> smacks down the enemy! Surrounded by friends, <name> realizes they'd do almost anything for them. <name> promises themselves to help their team mates in any way possible".Replace("<name>", name);
            case Levels.type.Friend2:
                return "<name> has found a way to help their friends even more! Applying massage techniques passed down from the friendship gods, <name> can now fully heal team mates if given the time!".Replace("<name>", name);
            case Levels.type.Lonely1:
                return "Through deep soliditary meditation coupled with the rush of victory, <name> realizes that all emotions are just a result of circumstances. <name> realizes they can intensify the emotions of their teammates!".Replace("<name>", name);
            case Levels.type.Lonely2:
                return "All this alone time was a great chance for <name> to do some serious studying! Through careful observation, <name> has percieved that life is but a series of turns, which can be manipulated with the right tools. ".Replace("<name>", name);
            case Levels.type.Scared1:
                return "<name> realizes the depths of their incompetence. Were they ever fit for battle? They resolve to help the team with more cunning means - by weakening the enemy.  ".Replace("<name>", name);
            case Levels.type.Scared2:
                return "Direct combat is definetelly not for <name>! They prefers to attack his enemies from behind and let teammates finish the job! ".Replace("<name>", name);
            default:
                return "Error in level up text";        
        }

    }
    public string GetDeathText(Levels.type type, string name)
    {
        
        string deathTxt = "";
        switch (type)
        {
            case Levels.type.Brave1:
                deathTxt = "Despite their bravery, the enemies were too much for <name>. <name> hopes they will be strong enough to make it without him. ";
                break;
            case Levels.type.Brave2:
                deathTxt = "<name> was sure nothing could hurt them. Unfortunately, <name> realized this error too late... ";
                break;
            case Levels.type.Friend1:
                deathTxt = "<name> tired to help the team, but was not able to escape the danger. Who will take care of them now? ";
                break;
            case Levels.type.Friend2:
                deathTxt = "<name> was so close... If they had just made it back to base quicker, <name> would have made it all right. It's too late now. ";
                break;
            case Levels.type.Lonely1:
                deathTxt = "<name> kept to themselves during the last period of their life. Would they have survived with more friends? Questions the team will never stop asking themselves ";
                break;
            case Levels.type.Lonely2:
                deathTxt = "Alas, it is too late for <name> to turn back time now... ";
                break;
            case Levels.type.Scared1:
                deathTxt = "<name> wasn't cut out for fighting. And in the end, their clever tricks came up short. ";
                break;
            case Levels.type.Scared2:
                deathTxt = "For all their attempts do avoid fighting, violence eventually found <name>. ";
                break;
            default:
                deathTxt = "<name>'s young life was tragically cut short. Lets hope the rest of the team won't suffer a similar fate. ";
                break;
        }
        deathTxt += "\n" + Var.deathSignoffs[UnityEngine.Random.Range(0, Var.deathSignoffs.Length)];
        deathTxt = deathTxt.Replace("<name>", name);
        return deathTxt;




    }
    public string GetLVLProgress(Levels.type type, Bird bird)
    {
        switch (type)
        {
            case Levels.type.Brave2:

            default:
                return "";
        }
    }
    public string GetLVLRequirements(Levels.type type)
    {
        switch (type)
        {
            case Levels.type.Brave1:
                return "Win two fights at once";
            case Levels.type.Brave2:
                return "Win 5 fights in a row";
            case Levels.type.Friend1:
                return "gain + 5 or more social in one turn and and win a fight";
            case Levels.type.Friend2:
                return "Be close to a friendly bird and gain + 5 or more social in one turn";
            case Levels.type.Lonely1:
                return "No teammates to be seen in all four directions + diagonally! At the same time, Win a fight";
            case Levels.type.Lonely2:
                return "The bird is not used for two adventures in a row. Then, spend a fight all alone";
            case Levels.type.Scared1:
                return "Lose a fight while being close to a friendly bird winning a fight";
            case Levels.type.Scared2:
                return "Rest 3 turns in a row";
            default:
                return "Error in level up text";
        }

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
        treshold = 12;
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
            case Var.Em.finish:
                return level;
            case Var.Em.Random:
                return level;
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

    public Sprite GetLVLSprite(Levels.type type)
    {
        switch (type) {
            case Levels.type.Kim:
                return Var.startingLvlSprites[3];
            case Levels.type.Rebecca:
                return Var.startingLvlSprites[0];
            case Levels.type.Terry:
                return Var.startingLvlSprites[4];
            case Levels.type.Alexander:
                return Var.startingLvlSprites[1];
            case Levels.type.Sophie:
                return Var.startingLvlSprites[2];
            case Levels.type.Brave1:
                return Var.lvlSprites[1];
            case Levels.type.Brave2:
                return Var.lvlSprites[5];
            case Levels.type.Friend1:
                return Var.lvlSprites[0];
            case Levels.type.Friend2:
                return Var.lvlSprites[4];
            case Levels.type.Scared1:
                return Var.lvlSprites[2];
            case Levels.type.Scared2:
                return Var.lvlSprites[6];
            case Levels.type.Lonely1:
                return Var.lvlSprites[3];
            case Levels.type.Lonely2:
                return Var.lvlSprites[7];
            default:
                return null;


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
            case Bird.dir.top:
                return new Vector3(0, increment);
            default:
                return new Vector3(0, 0, 0);
        }
    }

    public void ShowTooltip(String text)
    {
        GuiContoler.Instance.tooltipText.transform.parent.gameObject.SetActive(true);
        GuiContoler.Instance.tooltipText.transform.parent.gameObject.GetComponent<Image>().enabled = false;
        GuiContoler.Instance.tooltipText.text = text;
        AudioControler.Instance.PlaySound(AudioControler.Instance.expand);       
        //LeanTween.delayedCall(0.05f, GuiContoler.Instance.tooltipText.transform.parent.gameObject.GetComponent<tooltipScript>().SetPos);
    }
    public void HideTooltip()
    {
        GuiContoler.Instance.tooltipText.transform.parent.gameObject.SetActive(false);
    }


    public void EmitEmotionParticles(Transform parent,Var.Em emotion, bool useText= true)
    {
        var emParticles = Instantiate(Var.emotionParticles, parent);
        var particleSys = emParticles.GetComponentInChildren<ParticleSystem>().colorOverLifetime;
        Gradient grad = new Gradient();
        Color col = GetEmotionColor(emotion);
        grad.SetKeys(new GradientColorKey[] { new GradientColorKey(col, 0.0f), new GradientColorKey(col, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) });
        particleSys.color = grad;
        Text text = emParticles.transform.Find("Text").GetComponent<Text>();
        text.enabled = useText;
        text.text = emotion.ToString().Replace("Super","");
        if (text.text == "finish")
            text.text = "Level up!";
        text.color = col;
       // LeanTween.moveLocalZ(text.gameObject, 55.0f, 1f);
       // LeanTween.scale(text.gameObject, Vector3.one * 2f, 1.2f);
     //   LeanTween.alphaText(text.rectTransform, 0.0f, 1.2f);
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
        //TODO: Make these values global
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
        return -3 + 2 * lonelyVal;

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
                return "Give all diagonal birds - X to all dice rolls  (both friendly and enemy birds)";
            case Levels.type.Scared2:
                return "Backstabbing. If in stealth, bird does not fight enemies. All enemies passing by bird gains -30% chance to win. Toggle stealth by clicking bird";
            case Levels.type.Lonely1:
                return "Emo Intensifier - All birds in the same column's emotional changes are increased by +2";
            case Levels.type.Lonely2:
                return " if resting, reroll all battles. 3 turn cooldown";
            case Levels.type.Alexander:
                return "Impressionable- bird gains +1 in the dominant feeling of an adjacent bird";                
            case Levels.type.Kim:
                return "Ground effects affect you twice as much";
            case Levels.type.Rebecca:
                return "Lose -2 confidence when resting";
            case Levels.type.Sophie:
                return "If resting, all adjacent birds recieve 10% chance to win fights";
            case Levels.type.Terry:
                return "All birds in the same row gain +1 confidence";
                
            default:
                return "Level not found error";
                
        }       
    }
}
