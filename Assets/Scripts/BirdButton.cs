using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BirdButton : MonoBehaviour , IPointerDownHandler
{
	public Image src;

	public void OnPointerDown(PointerEventData eventData)
	{
		GameLogic.Instance.OnDragBird (this);
	}
}
