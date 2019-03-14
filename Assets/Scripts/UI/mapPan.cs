using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mapPan : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public float mouseSensitivity;
	private Vector3 lastPosition;
	public Vector2 screenSize;
	public float moveSoundDist = 25;
	public float zoomFactor = 1;
	public float maxScale = 2;
	public float minScale = 0.5f;
	public float minY;
	public float maxY;
    public float minX;
    public RectTransform map;
	//[HideInInspector]
	public Transform activeFog;
	public static mapPan Instance;
	Vector2 lastSoundPosition;
	Vector3 startingScale;
	Rect maxPos;
	void Awake()
	{
		startingScale = transform.localScale;
		Instance = this;
	}
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
			if (activeFog != null && activeFog.gameObject.activeSelf)
				activeFog.localScale =transform.localScale/startingScale.x;
		}
		if (MapControler.Instance.canMove)
		{
			if (Input.GetMouseButtonDown(0))
			{
				lastPosition = Input.mousePosition;
				AudioControler.Instance.mapPanGrab.Play();
				MapControler.Instance.HideSelectionMenu();
				MapControler.Instance.SelectedIcon = null;
				MapControler.Instance.startLvlBtn.gameObject.SetActive(false);
				lastSoundPosition = lastPosition;
			}

			if (Input.GetMouseButton(0))
			{
				float horizontalExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
				var delta = Input.mousePosition - lastPosition;
				if (activeFog != null && activeFog.gameObject.activeSelf && Camera.main.transform.position.x + horizontalExtent > activeFog.transform.position.x)
				{
					delta.x = Mathf.Max(0, delta.x);
				}
				if(Vector2.Distance(Input.mousePosition, lastSoundPosition)> moveSoundDist)
				{
					lastSoundPosition = Input.mousePosition;
					AudioControler.Instance.mapPan.Play();

				}
				transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
				Vector3 temp = transform.position;
				temp.y = Mathf.Clamp(temp.y, minY * transform.localScale.y / startingScale.y, maxY*transform.localScale.y/startingScale.y);
                temp.x = Mathf.Min(temp.x, minX * transform.localScale.x / startingScale.x);
				transform.position = temp;
				lastPosition = Input.mousePosition;
				//float x = Mathf.Clamp(transform.localPosition.x, maxPos.xMin + screenSize.x / 2, maxPos.xMax - screenSize.x / 2 - 1100);//- 551f/0.66f);// *map.localScale.x;
				//float y = Mathf.Clamp(transform.localPosition.y, maxPos.yMin + screenSize.y / 2 + 1140, 30);// maxPos.yMax - screenSize.y / 2 - 1140);// *map.localScale.y;
				// transform.localPosition = new Vector3(x, y, transform.localPosition.z);
			}
			if(Input.GetMouseButtonUp(0))
			{
				AudioControler.Instance.mapPanRelease.Play();
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
