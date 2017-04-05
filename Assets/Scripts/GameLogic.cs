using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

	public GameObject birdPlaygroundHolder;
		
	[HideInInspector]
	public Vector2 dropVector = new Vector2(-1,-1);
	[HideInInspector]
	public Vector3 touchStartPosition;
    private Bird draggedBird = null;
	private Vector3 dragPosition;
	private Vector3 mouseOffset;

	public GameObject dragImage = null;

	private bool _dragingBird;
	[HideInInspector]
	public bool dragingBird
	{
		set
		{
			dragImage.SetActive(value);

			if (value == false) {
				if (dragImage.transform.childCount > 0) {
					Destroy(dragImage.transform.GetChild(0).gameObject);
				}
			}

			_dragingBird = value;
		}

		get { return _dragingBird; }
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
		/*for (int i = 0; i < 4; i++)
		{
			Bird a = new Bird("Alice", (int)Random.Range(-15, 15), (int)Random.Range(-15, 15));
			Bird b = new Bird("Bob", (int)Random.Range(-15, 15), (int)Random.Range(-15, 15));
			Debug.Log(Fight(a, b).charName + " won!");
		}*/
	}
	
	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButton (0)) {
			if (dragingBird) {
				// We drag the action point!
				Vector3 currentScreenSpace = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10f);
				//convert screen position to world position with offset changes.
				dragPosition = Camera.main.ScreenToWorldPoint (currentScreenSpace) + mouseOffset;

				dragImage.transform.position = dragPosition;
			}
		} else if (Input.GetMouseButtonUp (0)) {

			if (dragingBird) {
				if (dropVector.x != -1) {
					// Drop bird on tile
					Var.playerPos[(int)dropVector.x,(int)dropVector.y] = draggedBird;
//
//					if (currentTileImg != null)
//						currentTileImg.sprite = draggedBird.src.sprite;

					Instantiate (draggedBird.birdPrefab, currentTile.transform, false);

				} else {
					// Place bird back to the button?
					draggedBird.gameObject.SetActive (true);

					// Cancel action
					draggedBird = null;

					// Clear the created prefab

				}

				dragingBird = false;
			}
		}
	}

	// For now!
	public void OnDragBird(Bird info)
	{
		// This will change to prefab?
//		dragImage.GetComponent<Image>().sprite = info.src.sprite;
		Instantiate(info.birdPrefab,dragImage.transform,false);

		// Little helper
		mouseOffset = new Vector3(0,0,10f);

		// Our active stuff
		draggedBird = info;

		// Dont start this if finger not moved a bit up !
		dragingBird = true;

		// Disable the gameobject
		draggedBird.gameObject.SetActive (false);
	}

	// Quick restart feature
	public List<GameObject> birdButtons = new List<GameObject>();
	public List<GameObject> playgroundBirds = new List<GameObject> ();

	public void OnRestartGame()
	{
		foreach (var but in birdButtons)
			but.SetActive (true);

		foreach (var bird in playgroundBirds) {
			if (bird.transform.childCount > 0) {
				Debug.Log ("bird.transform.childCount:" + bird.transform.childCount);
				Destroy(bird.transform.GetChild(0).gameObject);
			}
				
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
}
