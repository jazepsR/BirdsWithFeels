using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mapPan : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public AudioClip mapSwoosh;
	public float mouseSensitivity;
	private Vector3 lastPosition;
	public Vector2 screenSize;
	public float zoomFactor = 1;
	public float maxScale = 2;
	public float minScale = 0.5f;
	public float maxX;
	public float maxy;
	public RectTransform map;
	Rect maxPos;
	void Start()
	{
		maxPos = map.rect;
	}
	void Update()
	{
		if (GuiContoler.Instance.speechBubbleObj.activeSelf)
			return;
		if (Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			Vector3 targetpos = transform.position;
			Vector3 objPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
			float newScale =  Mathf.Clamp(transform.localScale.x + mouseWheel * zoomFactor, minScale, maxScale);
			float RelativeScale =newScale/transform.localScale.x; 
			Vector3 thing = targetpos - objPos;            
			Vector3 finalPos = (thing * RelativeScale) + objPos;
			
			transform.localScale = new Vector3(newScale,newScale,newScale);
			transform.position = finalPos;            
		}
		if (MapControler.Instance.canMove)
		{
			if (Input.GetMouseButtonDown(0))
			{
				lastPosition = Input.mousePosition;
				AudioControler.Instance.PlaySound(mapSwoosh);
				MapControler.Instance.HideSelectionMenu();
				MapControler.Instance.SelectedIcon = null;
				MapControler.Instance.startLvlBtn.gameObject.SetActive(false);
			}

			if (Input.GetMouseButton(0))
			{

				var delta = Input.mousePosition - lastPosition;
				transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
				lastPosition = Input.mousePosition;

				//float x = Mathf.Clamp(transform.localPosition.x, maxPos.xMin + screenSize.x / 2, maxPos.xMax - screenSize.x / 2 - 1100);//- 551f/0.66f);// *map.localScale.x;
				//float y = Mathf.Clamp(transform.localPosition.y, maxPos.yMin + screenSize.y / 2 + 1140, 30);// maxPos.yMax - screenSize.y / 2 - 1140);// *map.localScale.y;
				// transform.localPosition = new Vector3(x, y, transform.localPosition.z);
			}
		}
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		//MapControler.Instance.canMove = true;
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		//MapControler.Instance.canMove = false;
	}
}
