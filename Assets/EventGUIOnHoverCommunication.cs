using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventGUIOnHoverCommunication : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public EventController EventController;

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventController.SetIsHovering(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventController.SetIsHovering(false);
    }
    public void OnMouseEnter()
    {
        
    }

    public void OnMouseExit()
    {
       
    }
}
