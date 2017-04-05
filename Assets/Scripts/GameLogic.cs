using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

	public GameObject birdPlaygroundHolder;
	public Button FightButton;
		
	[HideInInspector]
	public Vector2 dropVector = new Vector2(-1,-1);
	[HideInInspector]
	public Vector3 touchStartPosition;
    private Bird draggedBird = null;
	private Vector3 dragPosition;
	private Vector3 mouseOffset;
	private Vector3 screenPosition = Vector3.zero;
    

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
		birdPlace.transform.position = dragImage.transform.position;

		// Move the bird to the spot and when done - show the actual
		LeanTween.moveLocal (birdPlace, Vector3.zero, 0.25f)
			.setEase (LeanTweenType.easeOutSine);

		OnClearDragObject ();
		draggedBird = null;

		// Check the fight scene!
		CanWeFight ();
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

	GameObject OnGetArrayVisualHolder(Vector2 index)
	{
		foreach (var holder in playgroundBirds) {
			if (holder.GetComponent<LayoutButton> ().index == index)
				return holder;
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
	}

	public void ReDragBird(Vector2 index, Transform holder)
	{
		// Do we have any bird here?
		Bird birdToMove = Var.playerPos [(int)index.x, (int)dropVector.y];

		if (!birdToMove)
			return;

		birdsPlaced--;

		// Clear the stuff
		Var.playerPos [(int)index.x, (int)dropVector.y] = null;

		// If we fail - we drop it back!
		dropVector = index;

		// Remove the holder from holder :D
		Destroy(holder.GetChild(0).gameObject);

		// The usual stuff
		Instantiate(birdToMove.birdPrefab,dragImage.transform,false);

		// Little helper
		screenPosition = Camera.main.WorldToScreenPoint(holder.position);
		mouseOffset = holder.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPosition.z));

		// Our active stuff
		draggedBird = birdToMove;

		// Dont start this if finger not moved a bit up !
		dragingBird = true;

		// Disable the gameobject
		draggedBird.gameObject.SetActive (false);

		// Show the actual drag image
		dragImage.SetActive (true);

		// Show stats of this bird?
		birdToMove.showText();
	}
}
