using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

	public GameObject birdPlaygroundHolder;
	public Button FightButton;

	public GameObject friendlyBoost;
	public Transform boostHolder;

	[HideInInspector]
	public Vector2 dropVector = new Vector2(-1,-1);
	[HideInInspector]
	public Vector3 touchStartPosition;
    private Bird draggedBird = null;
	private Vector3 dragPosition;
	private Vector3 mouseOffset;
	private Vector3 screenPosition = Vector3.zero;
    
	public List<GameObject> battleResultHolders = new List<GameObject> ();

	public GameObject dragImage = null;

	private bool dragingBird;

	void OnClearDragObject()
	{
		dragImage.SetActive (false);

		if (dragImage.transform.childCount > 0) {
			Destroy(dragImage.transform.GetChild(0).gameObject);
		}
	}

    [HideInInspector]
	public GameObject currentTile = null;

	public static GameLogic Instance { get; private set; }

	void Awake()
	{
		Instance = this;

		// Just some random stuff
		Application.targetFrameRate = 60;

		// Do some dynamci stuff
	}
	void Start()
	{
		CanWeFight ();
	}

	void OnCancelDrag()
	{
		// Place bird back to the button?
		LeanTween.move(dragImage,draggedBird.transform.position,0.25f)
			.setEase(LeanTweenType.easeOutSine)
			.setOnComplete(OnClearDragObject);

		LeanTween.delayedCall (0.25f, () => {
			draggedBird.gameObject.SetActive (true);	
			draggedBird = null;
		});
	}

	void OnDropDrag()
	{
		// Drop bird on tile
		Var.playerPos [(int)dropVector.x, (int)dropVector.y] = draggedBird;

		GameObject birdPlace = Instantiate (draggedBird.birdPrefab, currentTile.transform, false);
		birdPlace.transform.position = dragImage.transform.GetChild(0).position;

		// Check bird offset when move to final position?

		// Move the bird to the spot and when done - show the actual
		LeanTween.moveLocal (birdPlace, Vector3.zero, 0.25f)
			.setEase (LeanTweenType.easeOutSine);

		OnClearDragObject ();
		draggedBird = null;

		// Check the fight scene!
		CanWeFight ();

		// Show some feedback about the birds?
		OnShowFeedback_Enenmy(dropVector);
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

	// Check who will make our bird frienlyer?!
	List<ExtraPower> GetFriendlyBirds(int x,int y)
	{
		List<ExtraPower> birdList = new List<ExtraPower> ();
		ExtraPower dummy = null;

		int maxY = Var.playerPos.GetLength(1)-1;

		if (y + 1 <= maxY && Var.playerPos [x, y + 1] != null) {
			dummy = new ExtraPower ();
			dummy.extraFriendly = 2;
			dummy.indexA = new Vector2 (x,y+1);
			dummy.obj = OnGetArrayVisualHolder (new Vector2 (x, y + 1));
			birdList.Add (dummy);
		}
		if (y - 1 >= 0 && Var.playerPos [x, y - 1] != null) {
			dummy = new ExtraPower ();
			dummy.extraFriendly = 2;
			dummy.indexA = new Vector2 (x,y-1);
			dummy.obj = OnGetArrayVisualHolder (new Vector2 (x, y - 1));
			birdList.Add (dummy);
		}

		// The Sides
		int maxX = Var.playerPos.GetLength(0)-1;

		if (x + 1 <= maxX && y + 1 <= maxY && Var.playerPos [x + 1, y + 1] != null) {
			dummy = new ExtraPower ();
			dummy.extraFriendly = 1;
			dummy.indexA = new Vector2 (x + 1,y + 1);
			dummy.obj = OnGetArrayVisualHolder (new Vector2 (x + 1, y + 1));
			birdList.Add (dummy);
		}
		if (x - 1 >= 0 && y + 1 <= maxY && Var.playerPos [x - 1, y + 1] != null) {
			dummy = new ExtraPower ();
			dummy.extraFriendly = 1;
			dummy.indexA = new Vector2 (x - 1,y + 1);
			dummy.obj = OnGetArrayVisualHolder (new Vector2 (x - 1, y + 1));
			birdList.Add (dummy);
		}
		if (x + 1 <= maxX && y - 1 >= 0 && Var.playerPos [x + 1, y - 1] != null) {
			dummy = new ExtraPower ();
			dummy.extraFriendly = 1;
			dummy.indexA = new Vector2 (x + 1,y - 1);
			dummy.obj = OnGetArrayVisualHolder (new Vector2 (x + 1, y - 1));
			birdList.Add (dummy);
		}
		if (x - 1 >= 0 && y - 1 >= 0 && Var.playerPos [x - 1, y - 1] != null) {
			dummy = new ExtraPower ();
			dummy.extraFriendly = 1;
			dummy.indexA = new Vector2 (x - 1,y - 1);
			dummy.obj = OnGetArrayVisualHolder (new Vector2 (x - 1, y - 1));
			birdList.Add (dummy);
		}

		return birdList;
	}

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
		GameObject centerObj = OnGetArrayVisualHolder (index);

//		Debug.Log ("extraFriendly:" + extraFriendly);

		if (extraFriendly >= 0) {
			// We have something to show!
			List<ExtraPower> listOfObj = GetFriendlyBirds((int)index.x,(int)index.y);

			// Now show what we will show!
			foreach(var birdAround in listOfObj){

				if (birdAround == null)
					continue;

				// Find the middle position between points!
				Vector3 centrPos = Vector3.Lerp(centerObj.transform.position, birdAround.obj.transform.position,0.5f);
				GameObject bonus = Instantiate (friendlyBoost, boostHolder, false);
				bonus.transform.position = centrPos;
				bonus.transform.localScale = Vector3.zero;

				// Whats the extra value!
				if (birdAround.extraFriendly == 1)
					bonus.GetComponent<Image> ().color = new Color32 (108, 140, 214, 255);
                else
                    bonus.GetComponent<Image>().color = new Color32(55, 183, 184, 255);//super friendly

                float angle = AngleBetweenVector2 (birdAround.indexA, index);

				bonus.transform.eulerAngles = new Vector3 (0, 0, -angle);

				bonus.SetActive (true);

				// And add number how much did we get from bird around!
//				bonus.GetComponentInChildren<Text>(true).text = "+"+extraFriendly;

				// Show it up!
				LeanTween.scale(bonus,Vector3.one,0.25f)
					.setEase(LeanTweenType.easeOutBack);

				ExtraPower dummyExtra = new ExtraPower ();
				dummyExtra.extraFriendly = birdAround.extraFriendly;
				dummyExtra.indexA = birdAround.indexA;
				dummyExtra.indexB = index;
				dummyExtra.obj = bonus;

				// Check if we dont have such info already?


				// Add it to the list
				extraPowerList.Add (dummyExtra);
			}
		}
	}

	void OnHideFeedback_Enemy(int index)
	{
		GameObject iconToShow = battleResultHolders [index];
		LeanTween.cancel (iconToShow);

		LeanTween.scale (iconToShow, Vector3.zero, 0.25f)
			.setEase (LeanTweenType.easeInBack);
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton (0)) {
			if (dragingBird) {
				// We drag the action point!
				Vector3 currentScreenSpace = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPosition.z);
				//convert screen position to world position with offset changes.
				dragPosition = Camera.main.ScreenToWorldPoint (currentScreenSpace) + mouseOffset;

				dragImage.transform.position = dragPosition;
			}
		} else if (Input.GetMouseButtonUp (0)) {

			if (dragingBird) {
				if (dropVector.x != -1) {

					// Can we place the bird here?
					if (CanWePlaceBird (dropVector)) {
						birdsPlaced += 1;
						OnDropDrag ();
					} else {

						// Should we replace it?
						if (MakeBirdSwitch (dropVector)) {
							// Check the fight scene!
							OnDropDrag();
						} else {
							OnCancelDrag ();
						}
					}

				} else {
					OnCancelDrag ();
				}

				dragingBird = false;
			}
		}
	}

	// For now!
	public void OnDragBird(Bird info)
	{
		if (draggedBird != null)
			return;

		// Reset values?
		dropVector = new Vector2 (-1, -1);

		Instantiate(info.birdPrefab,dragImage.transform,false);

		// Little helper
		screenPosition = Camera.main.WorldToScreenPoint(info.gameObject.transform.position);
		mouseOffset = info.gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));

		// Our active stuff
		draggedBird = info;

		// Dont start this if finger not moved a bit up !
		dragingBird = true;

		// Disable the gameobject
		draggedBird.gameObject.SetActive (false);

		// Show the actual drag image
		dragImage.SetActive (true);
	}

	// Quick restart feature
	public List<GameObject> birdButtons = new List<GameObject>();
	public List<GameObject> playgroundBirds = new List<GameObject> ();

	public void OnRestartGame()
	{
		// Reset the rulles
		birdsPlaced = 0;

		foreach (var but in birdButtons)
			but.SetActive (true);

		foreach (var bird in playgroundBirds) {
			if (bird.transform.childCount > 0) {
				Destroy(bird.transform.GetChild(0).gameObject);
			}
		}

		CanWeFight ();

		// NobodyGetsHurt!
		OnClearFeedbackQuick ();
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



	public int Fight(Bird playerBird, Bird enemyBird)
	{
		if (Bird1Win(playerBird, enemyBird))
		{
            playerBird.confidence += Var.confWinFight;
            return +1;
            
		}
		if (Bird1Win(enemyBird, playerBird))
		{
            playerBird.confidence += Var.confLoseFight;
			return -1;
		}
        float val = Random.Range(0f, 1f);
        if (val > 0.5f)
		{
            playerBird.confidence += Var.confWinFight;
            return +1;
		}else
		{
            playerBird.confidence += Var.confLoseFight;
            return -1;
		}

	}

	bool Bird1Win(Bird Bird1, Bird Bird2)
	{
		Var.Em em1 = Bird1.emotion;
		Var.Em em2 = Bird2.emotion;


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

	// - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
	// Check the rulles

	int birdsPlaced = 0;

	void CanWeFight()
	{
		// Do we complete all the rulles?
		if (birdsPlaced >= 3) {
			FightButton.interactable = true;
		} else {
			FightButton.interactable = false;
		}
	}

	public GameObject OnGetArrayVisualHolder(Vector2 index,bool birdObj = false)
	{
		foreach (var holder in playgroundBirds) {
			if (holder.GetComponent<LayoutButton> ().index == index) {
				if (birdObj)
					return holder.transform.GetChild (0).gameObject;
				else
					return holder;
			}
		}

		return null;
	}

	GameObject OnGetBirdButton(Bird info)
	{
		foreach (var button in birdButtons) {
			if (button.GetComponent<Bird> () == info)
				return button;
		}

		return null;
	}

	bool MakeBirdSwitch(Vector2 index)
	{
		Bird returnHome = Var.playerPos [(int)index.x, (int)dropVector.y];

		if (returnHome) {
			// How do we know where was this bird? - Or drop to the replacement bird?
			// Create the pref and move it to button - when complete the move - show the button and destroy the pref
			GameObject pre = Instantiate(returnHome.birdPrefab,birdPlaygroundHolder.transform,false);
			GameObject theHolder = OnGetArrayVisualHolder (index);
			pre.transform.position = theHolder.transform.position;

			// Remove
			Destroy(theHolder.transform.GetChild (0).gameObject);

			// To what position should the bird fly?
			GameObject buttonHolder = OnGetBirdButton(returnHome);
			LeanTween.move (pre, buttonHolder.transform.position, 0.25f)
				.setEase (LeanTweenType.easeOutSine)
				.setOnComplete(()=>{
					buttonHolder.SetActive(true);
					Destroy(pre);
				});

			return true;
		}

		return false;
	}

	bool CanWePlaceBird(Vector2 index)
	{
		// Check if no other bird is in this row!
		for(int x=0;x<3;x++){
			if (Var.playerPos [x, (int)dropVector.y] != null) {
				return false;
			}
		}

		// Do we have any oppenent in the opposite direction
		if (!Var.enemies [(int)dropVector.y].inUse)
			return false;

		if (Var.playerPos [(int)index.x, (int)dropVector.y] == null)
			return true;
		
		return false;
	}

	public void ShowStatsOnHover(Vector2 index)
	{
		Bird birdToMove = Var.playerPos [(int)index.x, (int)dropVector.y];

		if (!birdToMove)
			return;

		birdToMove.showText ();
        GuiContoler.Instance.PortraitControl(birdToMove.portraitOrder,birdToMove.emotion);
	}

	public void ReDragBird(Vector2 index, Transform holder)
	{
		// Do we have any bird here?
		Bird birdToMove = Var.playerPos [(int)index.x, (int)dropVector.y];

		if (!birdToMove)
			return;

		// Clear feedback!
		OnHideFeedback_Enemy((int)index.y);

		// Clear extras
		for(int i=0;i<extraPowerList.Count;i++){
			if (extraPowerList [i].indexA == index || extraPowerList [i].indexB == index) {
				Destroy (extraPowerList [i].obj);
				extraPowerList.RemoveAt (i);
				i--;
			}
		}

		birdsPlaced--;

		// Clear the stuff
		Var.playerPos [(int)index.x, (int)dropVector.y] = null;

		// If we fail - we drop it back!
		dropVector = index;

		// The usual stuff
		GameObject def = Instantiate(birdToMove.birdPrefab,dragImage.transform,false);
		def.transform.localPosition = Vector3.zero;

		// Remove the holder from holder :D
		Destroy(holder.GetChild(0).gameObject);

		dragImage.transform.position = holder.position;

		// Little helper
		screenPosition = Camera.main.WorldToScreenPoint(dragImage.transform.position);
		mouseOffset = dragImage.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));

		// Our active stuff
		draggedBird = birdToMove;

		// Dont start this if finger not moved a bit up !
		dragingBird = true;

		// Disable the gameobject
		draggedBird.gameObject.SetActive (false);

		// Show the actual drag image
		dragImage.SetActive (true);

		// Show stats of this bird?
//		birdToMove.showText();
	}
}
