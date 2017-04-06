using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiMap : MonoBehaviour {
  

    public void MoveMapBird(int pos)
    {
        
        transform.localPosition = new Vector3(-578 + pos * 95,transform.localPosition.y,transform.localPosition.z);

    }
}
