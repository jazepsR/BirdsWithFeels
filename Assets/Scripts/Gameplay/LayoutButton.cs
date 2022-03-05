﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class LayoutButton : MonoBehaviour
{
	public Vector2 index = new Vector2(0, 0);
	public Bird currentBird = null;
	public Bird swapBird = null;
	public bool hasBird;
	[HideInInspector]
	public bool isActive = true;
	[HideInInspector]
	public powerTile power = null;
	public bool inMap = false;
	SpriteRenderer sr;
	[HideInInspector]
	public Color defaultColor;
	Color rowColor = Color.clear;
	Color columnColor = Color.clear;
	[HideInInspector]
	public Color baseColor;
	public Color highlightColor;
	public List<Color> tileColors;
	// [HideInInspector]
	public int ConfBonus = 0;
	// [HideInInspector]
	public int FriendBonus = 0;
	public int RollBonus = 0;
	public int PlayerRollBonus = 0;
	public bool isInfluenced = false;
	GameObject selectionEffect;
	void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
		tileColors = new List<Color>();
		try
		{
			if (LevelVisualSetup.Instance.isDebug)
				sr.color = LevelVisualSetup.Instance.tileColors[LevelVisualSetup.Instance.debugSelection];
			else
				sr.color = LevelVisualSetup.Instance.tileColors[Var.currentBG];
			defaultColor = sr.color;
			baseColor = sr.color;
		}
		catch
		{
			defaultColor = sr.color;
			baseColor = sr.color;
		}
	}
	void OnTriggerEnter2D(Collider2D other)
	{
	    Debug.Log("entered!");
		if (isActive && other.transform.parent.GetComponent<Bird>().dragged)
		{
			if (selectionEffect == null && index.x >= 0)
				selectionEffect = Instantiate(Helpers.Instance.selectionEffectGround, transform);
			LeanTween.color(gameObject, highlightColor, 0.3f);
			if (currentBird == null)
				currentBird = other.transform.parent.GetComponent<Bird>();
			else
				swapBird = other.transform.parent.GetComponent<Bird>();
            AudioControler.Instance.PlaySound(AudioControler.Instance.tileHighlightBirdHover);
		}
	}

	

	void OnMouseOver()
	{
		if (!Var.isDragControls && Input.GetMouseButtonDown(0) && Var.clickedBird != null && isActive)
		{
			if (selectionEffect == null && index.x >= 0)
				selectionEffect = Instantiate(Helpers.Instance.selectionEffectGround, transform);
			LeanTween.color(gameObject, highlightColor, 0.3f);
			if (currentBird == null)
				currentBird = Var.clickedBird;
			else
				swapBird = Var.clickedBird;
			Debug.Log("Clicked: " + Var.clickedBird.charName + " current: " + currentBird.charName);
			Var.clickedBird.dragged = true;
			Var.clickedBird = null;
			foreach (LayoutButton btn in ObstacleGenerator.Instance.tiles)
			{
				DestroySelectionEffect();
				btn.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
			}
		}
	}


	public void ShowHighlight(float duration = -1)
	{
		if (selectionEffect == null && index.x >= 0)
			selectionEffect = Instantiate(Helpers.Instance.highlightGroundEffect, transform);
		if (duration != -1)
		{
			LeanTween.delayedCall(duration, DestroySelectionEffect);
		}
	}

	public void AddColor(Color color)
	{
		if (!tileColors.Contains(color))
			tileColors.Add(color);
		SetColor();
	}
	void SetColor()
	{

		baseColor = new Color();
		foreach (Color col in tileColors)
			baseColor += col / tileColors.Count;
		sr.color = baseColor;
		LeanTween.color(gameObject, baseColor, 0.7f);
	}
	public void RemoveColor(Color color)
	{
		if (tileColors.Contains(color))
			tileColors.Remove(color);
		if (tileColors.Count > 0)
			SetColor();
		else
		{
			sr.color = defaultColor;
			baseColor = defaultColor;
			LeanTween.color(gameObject, baseColor, 0.3f);
		}

	}



	public void ApplyPower(Bird birdObj)
	{
		//TODO: is this a ground ability?

		if (power != null)
		{
			try
			{
				power.ApplyPower(birdObj);
				Debug.Log("applypowerbirdobj");
			}
			catch
			{
				Debug.Log("failed to apply power");
			}
			if (birdObj.groundMultiplier > 1)
				birdObj.GroundBonus.SetActive(true);
		}
		birdObj.isInfluenced = isInfluenced;
		//birdObj.GroundRollBonus = RollBonus;
		birdObj.PlayerRollBonus = PlayerRollBonus;
		// print(birdObj.charName+" apply power playerRollBonus: " + PlayerRollBonus + " groundRollBonus: " + RollBonus);
		if (birdObj.groundMultiplier > 1 && (ConfBonus != 0 || FriendBonus != 0 || RollBonus != 0))
			birdObj.GroundBonus.SetActive(true);
	}

	public void Reset()
	{
		swapBird = null;
		currentBird = null;
		power = null;
		isInfluenced = false;
		ConfBonus = 0;
		FriendBonus = 0;
		RollBonus = 0;
		PlayerRollBonus = 0;
		baseColor = defaultColor;
		sr.color = defaultColor;
		tileColors = new List<Color>();
	}
	public void fullReset()
	{
		//TODO: implement full reset
		Reset();

	}
	void OnTriggerExit2D(Collider2D other)
	{
		LeanTween.color(gameObject, baseColor, 0.3f);
		Bird tempBird = other.transform.parent.GetComponent<Bird>();
		if (tempBird.dragged)
		{
			if (selectionEffect != null)
			{
				selectionEffect.GetComponent<Animator>().SetTrigger("disappear");
				LeanTween.delayedCall(0.25f, () =>
				{
					if (selectionEffect != null)
						Destroy(selectionEffect);
				});
			}
			//tempBird.target = tempBird.home;
			if (swapBird == null)
			{
				currentBird = null;
			}
			else
			{
				swapBird = null;

			}
		}

	}
	void DestroySelectionEffect()
	{
		if (selectionEffect != null)
		{
			selectionEffect.GetComponent<Animator>().SetTrigger("disappear");
			LeanTween.delayedCall(0.2f, () =>
			{
				if (selectionEffect != null)
					Destroy(selectionEffect);
			});
		}
	}

	void Update()
	{
		//detect feet/resting collison in tile if current bird == null to prevent fake social lines
		// check to see location of birds are the same in a tile to address overlapping?
		  //if matches position/tile check to see what the current bird is on the tile and then remove the glitched bird that isnt in current

		hasBird = (currentBird != null);
		if (Input.GetMouseButtonUp(0))
			DestroySelectionEffect();

		if (currentBird != null && currentBird.dragged && selectionEffect == null && index.x >= 0)
			selectionEffect = Instantiate(Helpers.Instance.selectionEffectGround, transform);
		if (Input.GetMouseButtonUp(0) && currentBird != null)
		{
			if (currentBird.dragged)
			{
				DestroySelectionEffect();

				if ((int)index.x == -1)
				{
					currentBird.target = currentBird.home;
					currentBird.ReleaseBird((int)index.x, (int)index.y);
					currentBird = null;
					swapBird = null;
					return;
				}
				Var.playerPos[(int)index.x, (int)index.y] = currentBird;
				LeanTween.color(gameObject, baseColor, 0.3f);
				currentBird.target = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
				ApplyPower(currentBird);
				currentBird.ReleaseBird((int)index.x, (int)index.y);

				Debug.Log("tile x: " + (int)index.x + " bird X" + currentBird.x + " tile y: " + (int)index.y + "bird Y: " + currentBird.y);

				foreach (Bird bird in FillPlayer.Instance.playerBirds)
                {
					//Debug.Log(bird.target);
					if (bird != currentBird && bird.target == currentBird.target)
					{
						bird.target = bird.home;
						currentBird.target = currentBird.home;
						bird.ReleaseBird((int)index.x, (int)index.y);
						currentBird.ReleaseBird((int)index.x, (int)index.y);
						Var.playerPos[(int)index.x, (int)index.y] = null;
						currentBird.lines.RemoveLines();
						bird.lines.RemoveLines();
						currentBird = null;
						swapBird = null;
					}

					//if (((int)index.x == bird.x && (int)index.y == bird.y) && currentBird != null)
					//{
						//Debug.Log("I glitched out - CURRENT BIRD NULL BUG - LAYOUT BUTTON CLASS");
						//currentBird = bird;
					//	currentBird.ReleaseBird((int)index.x, (int)index.y);

					//}
				}


			}
			if (swapBird != null)
			{

				Var.playerPos[(int)index.x, (int)index.y] = swapBird;
				if (swapBird.y != -1)
				{
					ObstacleGenerator.Instance.tiles[swapBird.y * 4 + swapBird.x].currentBird = currentBird;
					currentBird.target = swapBird.target;
					Var.playerPos[swapBird.x, swapBird.y] = currentBird;
				}
				else
				{
					currentBird.target = currentBird.home;
					print("swapped home");
				}

				currentBird.OnLevelPickup();
				currentBird.ReleaseBird(swapBird.x, swapBird.y);
				currentBird.GroundRollBonus = 0;
				currentBird = swapBird;
				currentBird.target = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
				foreach(Bird bird in Var.activeBirds)
                {
					bird.ResetCanGrab();
                }
				// currentBird.res
				ApplyPower(currentBird);
				currentBird.ReleaseBird((int)index.x, (int)index.y);
				if (!inMap)
				{
					GameLogic.Instance.CanWeFight();
				}
				else
				{
					MapControler.Instance.CanLoadBattle();
				}
			}
			swapBird = null;
			if (index.x == -1)
			{
				currentBird = null;
				swapBird = null;
			}

			if(currentBird == null)
			{
				Debug.Log("hello");
			}
			if (!inMap)
			{
				GameLogic.Instance.CanWeFight();
			}
			else
			{
				MapControler.Instance.CanLoadBattle();
			}

		}
		
	}
}

