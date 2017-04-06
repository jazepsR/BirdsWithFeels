using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAction : MonoBehaviour 
{
	public GameObject CloudA;
	public GameObject CloudB;
	public GameObject CloudC;

	public GameObject birdHolder;
	public GameObject enemyHolder;

	public Transform cloudHolder;

	public ParticleSystem particleA;
	public ParticleSystem particleB;

	public static BattleAction Instance { get; private set; }

	private List<BattleInfo> battleList = new List<BattleInfo>();

	public List<Vector3> battleCenterPoints = new List<Vector3>();

	public class floatingCloud
	{
		public Transform entity;

		// The move part * * * * * * * * * * * * * * *
		public bool shouldMove = false;

		public float moveTimer = 3f;

		public float moveStrenghtX = 0f;
		public float startX = 0f;

		public int moveDir = 0;
		public float startY = 0f;
		public float moveStrength = 1f;

		// The rotate part!* * * * * * * * * * * * * *
		public bool shouldRotate = false;

		public float rotateStart = 0f;
		public float rotateFinish = 0f;

		public float currentMin = 0f;
		public float currentMax = 0f;
		public float currentTime = 0f;

		public float startTime = 0f;
		public float durration = 1f;

		public float finalValue = 0f;

		public float startCounter = 0f;
	}

	public List<floatingCloud> floatingClouds = new List<floatingCloud> ();

	public List<floatingCloud> birdsFloat = new List<floatingCloud>();

	// Use this for initialization
	void Start () {
		
	}

//	public Class
	
	// Update is called once per frame
	void Update () {
		// Move the clouds!

		foreach (var ent in floatingClouds) {
			if (ent.moveStrenghtX != 0f) {
				ent.entity.localPosition = new Vector3 (
					ent.startX - (float)Mathf.Sin((Time.time - ent.startCounter)*ent.moveTimer) * ent.moveStrenghtX,
					ent.entity.localPosition.y,
					ent.entity.localPosition.z);
			}

			if (ent.moveStrength != 0f) {

				ent.entity.localPosition = new Vector3 (
					ent.entity.localPosition.x,
					ent.startY + ((float)Mathf.Sin ((Time.time - ent.startCounter)*(ent.moveTimer+1f)) * ent.moveStrength),
					ent.entity.localPosition.z);
			}
		}

		foreach (var ent in birdsFloat) {
			if (ent.moveStrenghtX != 0f) {
				ent.entity.localPosition = new Vector3 (
					ent.startX - (float)Mathf.Sin((Time.time - ent.startCounter)*ent.moveTimer+2f) * ent.moveStrenghtX,
					ent.entity.localPosition.y,
					ent.entity.localPosition.z);
			}

			if (ent.moveStrength != 0f) {

				ent.entity.localPosition = new Vector3 (
					ent.entity.localPosition.x,
					ent.startY + ((float)Mathf.Sin ((Time.time - ent.startCounter)*(ent.moveTimer+2f)) * ent.moveStrength),
					ent.entity.localPosition.z);
			}
		}

	}

	void Awake() {
		Instance = this;

		CloudA.transform.localScale = Vector3.zero;
		CloudB.transform.localScale = Vector3.zero;
		CloudC.transform.localScale = Vector3.zero;

		// Wazap!
		floatingCloud dummySimpleEnt = new floatingCloud();

		dummySimpleEnt.entity = CloudA.transform;
		dummySimpleEnt.shouldMove = true;

		float randomValue = Random.Range (3f, 6f);
		dummySimpleEnt.moveTimer = Random.Range (3f, 4f);

		dummySimpleEnt.moveStrength = randomValue;
		dummySimpleEnt.startY = dummySimpleEnt.entity.localPosition.y;

		dummySimpleEnt.startX = dummySimpleEnt.entity.localPosition.x;
		dummySimpleEnt.moveStrenghtX = randomValue * 2f;

		dummySimpleEnt.startCounter = Time.time;

		floatingClouds.Add (dummySimpleEnt);

		// - - - - - 
		dummySimpleEnt = new floatingCloud();

		dummySimpleEnt.entity = CloudB.transform;
		dummySimpleEnt.shouldMove = true;

		randomValue = Random.Range (3f, 6f);
		dummySimpleEnt.moveTimer = Random.Range (3f, 4f);

		dummySimpleEnt.moveStrength = randomValue;
		dummySimpleEnt.startY = dummySimpleEnt.entity.localPosition.y;

		dummySimpleEnt.startX = dummySimpleEnt.entity.localPosition.x;
		dummySimpleEnt.moveStrenghtX = randomValue * 2f;

		dummySimpleEnt.startCounter = Time.time;

		floatingClouds.Add (dummySimpleEnt);

		// - - - - - 
		dummySimpleEnt = new floatingCloud();

		dummySimpleEnt.entity = CloudC.transform;
		dummySimpleEnt.shouldMove = true;

		randomValue = Random.Range (3f, 6f);
		dummySimpleEnt.moveTimer = Random.Range (3f, 4f);

		dummySimpleEnt.moveStrength = randomValue;
		dummySimpleEnt.startY = dummySimpleEnt.entity.localPosition.y;

		dummySimpleEnt.startX = dummySimpleEnt.entity.localPosition.x;
		dummySimpleEnt.moveStrenghtX = randomValue * 2f;

		dummySimpleEnt.startCounter = Time.time;

		floatingClouds.Add (dummySimpleEnt);
	}

	private Transform activeBird;
	private Transform activeEnemy;

	public void OnStartBattle()
	{
		// Clear stuff!!!
		birdsFloat = new List<floatingCloud> ();

		// Run trought all the birds!
		BattleInfo activeBattle = battleList [0];
		battleList.RemoveAt (0);

		activeBird = activeBattle.bird.transform;
		activeEnemy = activeBattle.enemy.transform;

		// Move the bird in
		Vector3 birdPosition = new Vector3 (
			                       battleCenterPoints [activeBattle.lineY].x - 20f,
			                       battleCenterPoints [activeBattle.lineY].y,
			                       battleCenterPoints [activeBattle.lineY].z);
		
		LeanTween.moveLocal (activeBattle.bird, birdPosition, 0.5f)
			.setOnComplete(OnShowClouds);

		// Move the enemy in
		Vector3 enemyPosition = new Vector3 (
			battleCenterPoints [activeBattle.lineY].x + 20f,
			battleCenterPoints [activeBattle.lineY].y,
			battleCenterPoints [activeBattle.lineY].z);
		
		LeanTween.moveLocal (activeBattle.enemy, enemyPosition, 0.5f);

		// Center clouds where should they be!
		cloudHolder.localPosition = new Vector3(
			battleCenterPoints [activeBattle.lineY].x,
			battleCenterPoints [activeBattle.lineY].y - 30f,0f);

		LeanTween.delayedCall (1f, OnEndFight);
	}

	void OnShowClouds()
	{
		// When the collide - show battle clouids!
		LeanTween.scale(CloudA,Vector3.one,0.25f).setEase(LeanTweenType.easeOutBack);
		LeanTween.scale(CloudB,Vector3.one,0.25f).setEase(LeanTweenType.easeOutBack);
		LeanTween.scale(CloudC,Vector3.one,0.25f).setEase(LeanTweenType.easeOutBack);

		OnStartBirdFight ();
	}

	void OnClearStuff()
	{
		foreach (Transform child in birdHolder.transform) {
			GameObject.Destroy(child.gameObject);
		}

		foreach (Transform child in enemyHolder.transform) {
			GameObject.Destroy(child.gameObject);
		}

		// Return to main action!
		GuiContoler.Instance.OnCompletedBattleVisual ();
	}

	void OnStartBirdFight()
	{
		floatingCloud dummySimpleEnt = new floatingCloud();

		dummySimpleEnt.entity = activeBird;
		dummySimpleEnt.shouldMove = true;

		float randomValue = Random.Range (3f, 6f);
		dummySimpleEnt.moveTimer = Random.Range (3f, 4f);

		dummySimpleEnt.moveStrength = randomValue * 2f;
		dummySimpleEnt.startY = dummySimpleEnt.entity.localPosition.y;

		dummySimpleEnt.startX = dummySimpleEnt.entity.localPosition.x;
		dummySimpleEnt.moveStrenghtX = randomValue * 2f;

		dummySimpleEnt.startCounter = Time.time;

		birdsFloat.Add (dummySimpleEnt);

		// Rotate by some angle!
		activeBird.eulerAngles = new Vector3(0,0,20f);

		// Hello
		dummySimpleEnt = new floatingCloud();

		dummySimpleEnt.entity = activeEnemy;
		dummySimpleEnt.shouldMove = true;

		randomValue = Random.Range (3f, 6f);
		dummySimpleEnt.moveTimer = Random.Range (3f, 4f);

		dummySimpleEnt.moveStrength = randomValue * 2f;
		dummySimpleEnt.startY = dummySimpleEnt.entity.localPosition.y;

		dummySimpleEnt.startX = dummySimpleEnt.entity.localPosition.x;
		dummySimpleEnt.moveStrenghtX = randomValue * 2f;

		dummySimpleEnt.startCounter = Time.time;

		birdsFloat.Add (dummySimpleEnt);

		// Rotate by some angle!
		activeEnemy.eulerAngles = new Vector3(0,0,-20f);

		// Play some particles babY!
		particleA.Play();
		particleB.Play ();
	}

	void OnEndFight()
	{
		// Clear the clouds!
		LeanTween.scale(CloudA,Vector3.zero,0.25f).setEase(LeanTweenType.easeInBack);
		LeanTween.scale(CloudB,Vector3.zero,0.25f).setEase(LeanTweenType.easeInBack);
		LeanTween.scale(CloudC,Vector3.zero,0.25f).setEase(LeanTweenType.easeInBack);

		// Set win loose anim!!!
		birdsFloat = new List<floatingCloud> ();

		activeEnemy.eulerAngles = Vector3.zero;
		activeBird.eulerAngles = Vector3.zero;



		if (battleList.Count > 0) {
			LeanTween.delayedCall (0.25f, OnStartBattle);
		} else {
			// Show the battle screen!
			LeanTween.delayedCall(0.5f,OnClearStuff);
		}
	}

	public class BattleInfo
	{
		public GameObject bird;
		public GameObject enemy;
		public bool birdWin = false;
		public int lineY = 0;
	}

	public void AddBattleInfo(GameObject birdPref, GameObject enemyPref, int fightResult, int lineY)
	{
		GameObject playerCopy = Instantiate (birdPref, birdHolder.transform, true);
		birdPref.SetActive (false);

		GameObject enemyCopy = Instantiate (enemyPref, enemyHolder.transform, true);
		enemyPref.SetActive (false);

		BattleInfo dummy = new BattleInfo ();
		dummy.bird = playerCopy;
		dummy.enemy = enemyCopy;

		// How do we know who win!
		if (fightResult > 0)
			dummy.birdWin = true;
		else
			dummy.birdWin = false;

		dummy.lineY = lineY;

		// Add to the battle list
		battleList.Add(dummy);
	}

	// Check if we have any next battle!

}
