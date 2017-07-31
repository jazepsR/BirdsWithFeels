using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class tooltipScript : MonoBehaviour {   
    public void SetPos()
    {
        gameObject.GetComponent<Image>().enabled = true;
        gameObject.GetComponentInChildren<Text>().color = Color.black;
        var pos = transform.localPosition;
        RectTransform me = gameObject.GetComponent<RectTransform>();

        var distPastX = me.anchoredPosition.x + me.sizeDelta.x - 1280;
        if (distPastX > 0)
        {
            pos = new Vector3(pos.x - distPastX, pos.y, pos.z);
            print("did x");
        }
        var distPastY = me.anchoredPosition.y + me.sizeDelta.y - 800;
        if (distPastY > 0)
        {
            pos = new Vector3(pos.x, pos.y - distPastY, pos.z);
            print("did y");
        }

        transform.localPosition = pos;   
    }
    
}
