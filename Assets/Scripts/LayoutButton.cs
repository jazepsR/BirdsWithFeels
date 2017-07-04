using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class LayoutButton : MonoBehaviour
{
    public Vector2 index = new Vector2(0, 0);  
    Bird currentBird = null;
    Bird swapBird = null;
    public bool hasBird;
    [HideInInspector]
    public bool isActive = true;
    [HideInInspector]
    public powerTile power = null;
    public bool inMap = false;
    SpriteRenderer sr;
    Color defaultColor;
    public Color highlightColor;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isActive && other.transform.parent.GetComponent<Bird>().dragged)
        {
            LeanTween.color(gameObject, highlightColor, 0.3f);
            if (currentBird == null)
                currentBird = other.transform.parent.GetComponent<Bird>();
            else
                swapBird = other.transform.parent.GetComponent<Bird>();

        }
    }
 

    void OnTriggerStay2D(Collider2D other)
    {
       // if (isActive)
            //BirdEnter(other);
    }
    /*void BirdEnter(Collider2D other)
    {
        if (other.tag == "feet" &&  Input.GetMouseButtonUp(0))
        {
            LeanTween.color(gameObject, defaultColor, 0.3f);
            Bird birdObj = other.transform.parent.GetComponent<Bird>();
            if (birdObj.gameObject == Var.selectedBird)
            {
                if (currentBird != null && currentBird != birdObj)
                {
                    currentBird.target = birdObj.target;                   
                    LeanTween.move(currentBird.gameObject, new Vector3(currentBird.target.x, currentBird.target.y, 0), 0.5f).setEase(LeanTweenType.easeOutBack);
                }
                Debug.Log("CollisionDetected!");
                birdObj.target = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
                currentBird = birdObj;
                Var.playerPos[(int)index.x, (int)index.y] = currentBird;                
                if (!inMap)
                {
                    GameLogic.Instance.CanWeFight();
                    applyPower(birdObj);
                }else
                {
                    MapControler.Instance.CanLoadBattle();
                }
                currentBird.ReleseBird((int)index.x, (int)index.y);
            }         
        }        
    }*/

    void applyPower(Bird birdObj)
    {
        if (power != null)
        {
            try
            {
                power.ApplyPower(birdObj);
            }
            catch
            {
                Debug.Log("failed to apply power");
            }
        }
    }
    public void Reset()
    {
        swapBird = null;
        currentBird = null;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        LeanTween.color(gameObject, defaultColor, 0.3f);
        Bird tempBird = other.transform.parent.GetComponent<Bird>();
        if (tempBird.dragged)
        {
            if (swapBird == null)
                currentBird = null;
            else
                swapBird = null;
        }
    }       /* void OnTriggerExit2D(Collider2D other)
         {
             if (other.tag == "feet")
             {
                 Bird birdObj = other.transform.parent.GetComponent<Bird>();
                 birdObj.target = birdObj.home;
                 if (currentBird != null)
                 {
                     Var.playerPos[(int)index.y, (int)index.x] = null;
                 }
                 currentBird = null;          
                 Debug.Log("Exited");       
             }
         }*/


    void Update()
    {
        hasBird = (currentBird != null);
        
        //hasBird = currentBird.dragged;
        if (Input.GetMouseButtonUp(0) && currentBird != null )
        {
            if (currentBird.dragged)
            {
                Var.playerPos[(int)index.x, (int)index.y] = currentBird;
                LeanTween.color(gameObject, defaultColor, 0.3f);                
                currentBird.target = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
                currentBird.ReleseBird((int)index.x, (int)index.y);
                if (!inMap)
                {
                    GameLogic.Instance.CanWeFight();
                    applyPower(currentBird);
                }
                else
                {
                    MapControler.Instance.CanLoadBattle();
                }

            }
            if(swapBird != null)
            {
                Var.playerPos[(int)index.x, (int)index.y] = swapBird;
                currentBird.target = swapBird.target;
                if (swapBird.y != -1)
                {
                    ObstacleGenerator.Instance.tiles[swapBird.y * 4 + swapBird.x].currentBird = currentBird;
                    Var.playerPos[swapBird.x, swapBird.y] = currentBird;
                }
                currentBird.ReleseBird(swapBird.x, swapBird.y);
                currentBird = swapBird;
                currentBird.target = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
                currentBird.ReleseBird((int)index.x, (int)index.y);
                if (!inMap)
                {
                    GameLogic.Instance.CanWeFight();
                    applyPower(currentBird);
                }
                else
                {
                    MapControler.Instance.CanLoadBattle();
                }
            }
            swapBird = null;
        }
     
    }
}
