using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

    public Button FightButton;    
    public GameObject friendlyBoost;
    public Transform boostHolder;
    private Vector3 screenPosition = Vector3.zero;
    public List<GameObject> battleResultHolders = new List<GameObject> ();       
    public static GameLogic Instance { get; private set; }
    bool prevstate = false;
    void Awake()
    {
        Instance = this;

        // Just some random stuff
        Application.targetFrameRate = 60;

    }
    void Start()
    {
        CanWeFight();
    }



    
    void OnShowFeedback_Enenmy(Vector2 index)
    {
        int lineY = (int)index.y;

        // What is the enemy?
        Bird enemy = Var.enemies[lineY];
        Bird user = Var.playerPos [(int)index.x, (int)index.y];

        bool userBirdWin = Bird1Win (user, enemy);
        bool enemyBirdWin = Bird1Win (enemy, user);

        // Hide all icons

        for (int i = 0; i < battleResultHolders[lineY].transform.childCount; i++)
            battleResultHolders[lineY].transform.GetChild (i).gameObject.SetActive(false);

        string iconStr = "";

        if(userBirdWin == enemyBirdWin)
            iconStr = "Coin";
        else if(userBirdWin && !enemyBirdWin)
            iconStr = "Win";
        else
            iconStr = "Lose";

        // Show the real icon
        battleResultHolders [lineY].transform.Find(iconStr).gameObject.SetActive(true);

        GameObject iconToShow = battleResultHolders [lineY];
        iconToShow.transform.localScale = Vector3.zero;

        LeanTween.scale (iconToShow, Vector3.one, 0.25f)
            .setEase(LeanTweenType.easeOutBack);

       //OnShowFeedback_Bird(index);
    }

    public class ExtraPower
    {
        public GameObject obj;
        public Vector2 indexA;
        public Vector2 indexB;
        public int extraFriendly = 0;
    }

    public List<ExtraPower> extraPowerList = new List<ExtraPower>();

    public void UpdateFeedback()
    {
        feedBack[] feedBack = FindObjectsOfType<feedBack>();
        //print("updateCall");
        foreach(feedBack fb in feedBack)
        {
            fb.RefreshFeedback();
            if (Var.isTutorial)
                fb.HighlightTutorialTiles();
        }

    }
    public bool CheckIfResting(Bird bird)
    {
        feedBack[] feedBack = FindObjectsOfType<feedBack>();
        if (bird.isHiding)
            return true;
        foreach (feedBack fb in feedBack)
        {
            if (!fb.CheckResting(bird))
                return false;
        }
        return true;
    }


    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y)? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

  /*  void OnShowFeedback_Bird(Vector2 index)
    {
        // Check what we can check
        int extraFriendly = Helpers.Instance.Findfirendlieness((int)index.x, (int)index.y);
        

//		Debug.Log ("extraFriendly:" + extraFriendly);

        if (extraFriendly >= 0) {
            // We have something to show!

        }
    }*/

    void OnHideFeedback_Enemy(int index)
    {
        GameObject iconToShow = battleResultHolders [index];
        LeanTween.cancel (iconToShow);

        LeanTween.scale (iconToShow, Vector3.zero, 0.25f)
            .setEase (LeanTweenType.easeInBack);
    }


    public void OnClearFeedbackQuick()
    {
        // Hide all holders!
        for(int i = 0;i<5;i++){
            OnHideFeedback_Enemy (i);
        }

        for(int i=0;i<extraPowerList.Count;i++){

            Destroy (extraPowerList [i].obj);
            extraPowerList.RemoveAt (i);
            i--;
        }
    }

    public float GetBonus(Bird playerBird, Bird enemyBird)
    {
        float winBonus = 0.0f;
        winBonus = winBonus + playerBird.getBonus() - enemyBird.getBonus();


        if (Bird1Win(playerBird, enemyBird))
            winBonus += 4;

        if (Bird1Win(enemyBird, playerBird))
            winBonus -= 4;      
        
        return 0.5f + winBonus * 0.1f;
    }

    public int Fight(Bird playerBird, Bird enemyBird)
    {
        float winBonus = GetBonus(playerBird, enemyBird);
        /* int playerRoll = Random.Range(0, DiceSize) + (int)winBonus;
         int enemyroll = Random.Range(0, DiceSize);*/
        if (Var.isTutorial)
        {
            if (winBonus == 0.5f && Tutorial.Instance.CurrentPos == 1)
            {
                winBonus = 0.0f;
            }
            else
            {
                if (winBonus >= 0.5f)
                {
                    winBonus = 1.1f;
                }
                else
                {
                    winBonus = 0f;
                }
            }
        }
       


        if (winBonus>Random.Range(0f,1f))
        {
            //Win
            
            playerBird.consecutiveFightsWon++;
            playerBird.winsInOneFight++;
            if (playerBird.wonLastBattle == 0 || playerBird.wonLastBattle == 2)
                playerBird.wonLastBattle = 2;
            else
                playerBird.wonLastBattle = 1;
            return +1;
        }else
        {           
            //lose
            
            playerBird.consecutiveFightsWon = 0;
            if (playerBird.wonLastBattle == 1 || playerBird.wonLastBattle == 2)
                playerBird.wonLastBattle = 2;
            else
                playerBird.wonLastBattle = 0;       
            return -1;            
        }

        //Add confidence!!

    }

    public bool Bird1Win(Bird Bird1, Bird Bird2)
    {
        Var.Em em1 = Bird1.emotion;
        Var.Em em2 = Bird2.emotion;
        //TODO: discuss super rules
        if ((em1 == Var.Em.Friendly || em1 == Var.Em.SuperFriendly) && em2 == Var.Em.Neutral)
            return true;
        if ((em1 == Var.Em.Confident || em1 == Var.Em.SuperConfident) && em2 == Var.Em.Neutral)
            return true;
        if ((em1 == Var.Em.Lonely || em1 == Var.Em.SuperLonely)&& em2 == Var.Em.Neutral)
            return true;
        if ((em1 == Var.Em.Scared || em1 == Var.Em.SuperScared) && em2 == Var.Em.Neutral)
            return true;

        if (em1 == Var.Em.Friendly && em2 == Var.Em.Confident)
            return true;
        if (em1 == Var.Em.Confident && em2 == Var.Em.Lonely)
            return true;
        if (em1 == Var.Em.Lonely && em2 == Var.Em.Scared)
            return true;
        if (em1 == Var.Em.Scared && em2 == Var.Em.Friendly)
            return true;

        if (em1 == Var.Em.SuperFriendly && em2 == Var.Em.Confident)
            return true;
        if (em1 == Var.Em.SuperConfident && em2 == Var.Em.Lonely)
            return true;
        if (em1 == Var.Em.SuperLonely && em2 == Var.Em.Scared)
            return true;
        if (em1 == Var.Em.SuperScared && em2 == Var.Em.Friendly)
            return true;

        if (em1 == Var.Em.SuperFriendly && em2 == Var.Em.SuperConfident)
            return true;
        if (em1 == Var.Em.SuperConfident && em2 == Var.Em.SuperLonely)
            return true;
        if (em1 == Var.Em.SuperLonely && em2 == Var.Em.SuperScared)
            return true;
        if (em1 == Var.Em.SuperScared && em2 == Var.Em.SuperFriendly)
            return true;
        /* TODO: Discuss this
        if (em1 == Var.Em.SuperConfident && em2 == Var.Em.Scared)
            return true;
        if (em1 == Var.Em.SuperLonely && em2 == Var.Em.Friendly)
            return true;
        if (em1 == Var.Em.SuperScared && em2 == Var.Em.Confident)
            return true;
        if (em1 == Var.Em.SuperFriendly && em2 == Var.Em.Lonely)
            return true;
            */




        return false;
    }




    public void CanWeFight()
    {
        bool canFight = true;
        feedBack[] feedBack = FindObjectsOfType<feedBack>();
        ShowTooltip fightTooltip = FightButton.gameObject.GetComponent<ShowTooltip>();
        string tooltipText = "";
        tooltipText = "";
        foreach (Bird bird in FillPlayer.Instance.playerBirds)
        {
            if (bird.gameObject.activeSelf && bird.x <= -1)
            {
                canFight = false;
                tooltipText = "All your birds must be placed on the grid!";
                break;
            }
        }
        foreach (feedBack fb in feedBack)
        {
            if (fb.isMain)
            {
                if (!fb.CheckOpponent())
                {
                    canFight = false;
                    tooltipText = "You must fight all the enemies!";
                    break;
                }
            }
        }
		if (Var.isTutorial && canFight && prevstate != canFight)
			Tutorial.Instance.ShowTutorialBeforeBattleText(Tutorial.Instance.CurrentPos);
		if (GuiContoler.Instance.speechBubbleObj.activeSelf)
        {
            canFight = false;
            tooltipText = "Fight availabe after the dialogue is completed";
        }
        fightTooltip.tooltipText = tooltipText;
        if (Var.Infight)
            canFight = false;
        //FightButton.gameObject.SetActive(canFight);  
        FightButton.GetComponent<Animator>().SetBool("active", canFight);
        FightButton.interactable = canFight;
       
        if (GuiContoler.Instance.speechBubbleObj.activeSelf)
            prevstate = true;
        else
            prevstate = canFight;
    }
    
}
