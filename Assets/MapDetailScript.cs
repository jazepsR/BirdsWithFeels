using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDetailScript : MonoBehaviour
{
    private Animator anim;



    public enum AnimationEnum { idle, floating };
    public AnimationEnum UseThisAnim;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        anim = this.gameObject.GetComponent<Animator>();

        if (UseThisAnim == AnimationEnum.floating)
        {
            print("animation integer for float: " + (int)UseThisAnim);
        }
        if (anim != null)
        {
            anim.SetInteger("animation", (int)UseThisAnim);
        }
    }

    // Update is called once per frame

}
