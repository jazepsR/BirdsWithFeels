using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class LayoutButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler 
{
	public Vector2 index = new Vector2(0,0);
	private Vector2 nopeVec = new Vector2 (-1, -1);

	// Use this for initialization
	void Start () {
		
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameLogic.Instance.dropVector = nopeVec;
        GameLogic.Instance.currentTileImg = null;

    }

	public void OnPointerEnter(PointerEventData eventData)
	{
		GameLogic.Instance.dropVector = index;
        GameLogic.Instance.currentTileImg = GetComponent<Image>();
	}
}
