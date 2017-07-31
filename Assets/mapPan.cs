﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mapPan : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float mouseSensitivity;
    private Vector3 lastPosition;
    public Vector2 screenSize;
    public bool canMove = true;
    public RectTransform map;
    Rect maxPos;
    void Start()
    {
        maxPos = map.rect;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canMove && Var.selectedBird == null)
        {
            lastPosition = Input.mousePosition;
            MapControler.Instance.HideSelectionMenu();
            MapControler.Instance.SelectedIcon = null;
            MapControler.Instance.startLvlBtn.gameObject.SetActive(false);
        }

        if (Input.GetMouseButton(0) && canMove && Var.selectedBird == null)
        {
            var delta = Input.mousePosition - lastPosition;
            transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
            float x = Mathf.Clamp(transform.localPosition.x, maxPos.xMin+screenSize.x/2, maxPos.xMax-screenSize.x/2);
            float y = Mathf.Clamp(transform.localPosition.y, maxPos.yMin+screenSize.y/2, maxPos.yMax-screenSize.y/2);
            transform.localPosition = new Vector3(x, y, transform.localPosition.z);
            lastPosition = Input.mousePosition;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        canMove = true;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        canMove = false;
    }
}
