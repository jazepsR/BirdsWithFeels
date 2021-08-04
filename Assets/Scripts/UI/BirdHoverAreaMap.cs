using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdHoverAreaMap : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
   

    private void OnMouseExit()
    {
        if (!EventController.Instance.eventObject.activeSelf)
        {
            MapControler.Instance.charInfoAnim.SetBool("hide", true);
            MapControler.Instance.charInfoAnim.SetBool("show", false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
