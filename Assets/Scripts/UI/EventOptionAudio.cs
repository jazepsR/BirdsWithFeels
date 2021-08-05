using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventOptionAudio : MonoBehaviour, IPointerEnterHandler
{
    int ID = -1;
    // Start is called before the first frame update
    public void Setup(int ID)
    {
        this.ID = ID;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(ID!= -1 && AudioControler.Instance)
        {
            AudioControler.Instance.PlayOptionHoverSound(ID);
        }
    }
}
