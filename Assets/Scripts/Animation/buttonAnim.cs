using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class buttonAnim : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Animator anim;
    
	// Use this for initialization
	void Start () {
        if (anim == null)
        {
            anim = GetComponent<Animator>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if(GetComponent<Button>() == null || GetComponent<Button>().interactable)
            anim.SetBool("onhover", true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        anim.SetBool("onhover", false);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        anim.SetTrigger("click");
    }
    }
