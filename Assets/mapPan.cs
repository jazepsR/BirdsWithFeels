using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class mapPan : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float mouseSensitivity;
    private Vector3 lastPosition;
    public bool canMove = true;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && canMove && Var.selectedBird == null)
        {
            lastPosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0) && canMove && Var.selectedBird == null)
        {
            var delta = Input.mousePosition - lastPosition;
            transform.Translate(delta.x * mouseSensitivity, delta.y * mouseSensitivity, 0);
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
