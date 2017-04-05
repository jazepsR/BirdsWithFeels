using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class LayoutButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	public Vector2 index = new Vector2(0,0);
	private Vector2 nopeVec = new Vector2 (-1, -1);

	// Use this for initialization
	void Start () {
		
	}

	public void OnPointerUp(PointerEventData eventData)
	{
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		GameLogic.Instance.ReDragBird (index,transform);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameLogic.Instance.dropVector = nopeVec;
		GameLogic.Instance.currentTile = null;

    }

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameLogic.Instance.dropVector = index;
		GameLogic.Instance.currentTile = gameObject;

		// Show some stats of the bird!
		GameLogic.Instance.ShowStatsOnHover(index);
	}
}
