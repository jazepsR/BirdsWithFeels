using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour {

	// What is the class of birds?
	public class BirdExampleClass
	{
		public int id = -1;
	}

	public static GameLogic Instance { get; private set; }

	[HideInInspector]
	public Vector2 dropVector;
	[HideInInspector]
	public Vector3 touchStartPosition;

	private Vector3 dragPosition;
	private Vector3 mouseOffset;

	public Image dragImage = null;
	private bool dragingBird = false;

	[HideInInspector]
	public int[,] board = new int[5, 3];

	void Awake()
	{
		Instance = this;

		// Just some random stuff
		Application.targetFrameRate = 60;
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
			if (dropVector.x != -1) {
				// Can we drop the bird here?
				board[(int)dropVector.y,(int)dropVector.x] = 1;	// Put bird id here?
			} else {
				
				// Cancel action
				dragImage.sprite = null;
			}

			dragingBird = false;
		}
	}

	// For now!
	public void OnDragBird(BirdButton info)
	{
		// Dont start this if finger not moved a bit up !
		dragingBird = true;

		dragImage.sprite = info.src.sprite;

		mouseOffset = new Vector3(0,0,10f);
	}
}
