using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

	public Button FightButton;
    int DiceSize = 10;
    public GameObject friendlyBoost;
	public Transform boostHolder;
	private Vector3 screenPosition = Vector3.zero;
	public List<GameObject> battleResultHolders = new List<GameObject> ();   
	private bool dragingBird;    
	public static GameLogic Instance { get; private set; }

	void Awake()
	{
		Instance = this;

		// Just some random stuff
		Application.targetFrameRate = 60;

	}
	void Start()
	{
		CanWeFight ();
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

		OnShowFeedback_Bird (index);
	}

	public class ExtraPower
	{
		public GameObject obj;
		public Vector2 indexA;
		public Vector2 indexB;
		public int extraFriendly = 0;
	}

	public List<ExtraPower> extraPowerList = new List<ExtraPower>();




	private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
	{
		Vector2 diference = vec2 - vec1;
		float sign = (vec2.y < vec1.y)? -1.0f : 1.0f;
		return Vector2.Angle(Vector2.right, diference) * sign;
	}

	void OnShowFeedback_Bird(Vector2 index)
	{
		// Check what we can check
		int extraFriendly = Helpers.Instance.Findfirendlieness((int)index.x, (int)index.y);
		

//		Debug.Log ("extraFriendly:" + extraFriendly);

		if (extraFriendly >= 0) {
			// We have something to show!

		}
	}

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
        return winBonus;
    }

	public int Fight(Bird playerBird, Bird enemyBird)
	{
        float winBonus = GetBonus(playerBird, enemyBird);
        int playerRoll = Random.Range(0, DiceSize) + (int)winBonus;
        int enemyroll = Random.Range(0, DiceSize);



        if (playerRoll> enemyroll)
        {
            //Win
            playerBird.confidence += Var.confWinFight;
            return +1;
        }else
        {
            if(playerRoll < enemyroll)
            {
                //lose
                playerBird.confidence += Var.confLoseFight;
                return -1;
            }else
            {
                //tie
               return Fight(playerBird, enemyBird);
            }
        }

        //Add confidence!!

	}

	bool Bird1Win(Bird Bird1, Bird Bird2)
	{
		Var.Em em1 = Bird1.emotion;
		Var.Em em2 = Bird2.emotion;

        if (em1 == Var.Em.Friendly && em2 == Var.Em.Neutral)
            return true;
        if (em1 == Var.Em.Confident && em2 == Var.Em.Neutral)
            return true;
        if (em1 == Var.Em.Lonely && em2 == Var.Em.Neutral)
            return true;
        if (em1 == Var.Em.Scared && em2 == Var.Em.Neutral)
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

		if (em1 == Var.Em.SuperConfident && em2 == Var.Em.Scared)
			return true;
		if (em1 == Var.Em.SuperLonely && em2 == Var.Em.Friendly)
			return true;
		if (em1 == Var.Em.SuperScared && em2 == Var.Em.Confident)
			return true;
		if (em1 == Var.Em.SuperFriendly && em2 == Var.Em.Lonely)
			return true;





		return false;
	}




    public void CanWeFight()
	{
        int birdsPlaced = 0;
        foreach(Bird bird in Var.playerPos)
        {
            if(bird != null)
                birdsPlaced++;
        }        
		if (birdsPlaced >= 3) {
			FightButton.interactable = true;
		} else {
			FightButton.interactable = false;
		}
	}
    
}
