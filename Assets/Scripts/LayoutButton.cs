using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class LayoutButton : MonoBehaviour
{
    public Vector2 index = new Vector2(0, 0);  
    Bird currentBird = null;
    public bool hasBird;

    void OnTriggerEnter2D(Collider2D other)
    {
        BirdEnter(other);
    }
 

    void OnTriggerStay2D(Collider2D other)
    {
        BirdEnter(other);
    }
    void BirdEnter(Collider2D other)
    {
        if (other.tag == "feet" &&  Input.GetMouseButtonUp(0))
        {
            Bird birdObj = other.transform.parent.GetComponent<Bird>();
            if (birdObj.gameObject == Var.selectedBird)
            {
                if (currentBird != null && currentBird != birdObj)
                {
                    currentBird.target = birdObj.target;
                    /* Vector3 tempHome = currentBird.home;
                     currentBird.home = birdObj.home;
                     birdObj.home = tempHome;*/
                    LeanTween.move(currentBird.gameObject, new Vector3(currentBird.target.x, currentBird.target.y, 0), 0.5f).setEase(LeanTweenType.easeOutBack);
                }
                Debug.Log("CollisionDetected!");
                birdObj.target = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
                currentBird = birdObj;
                Var.playerPos[(int)index.x, (int)index.y] = currentBird;
                currentBird.ReleseBird((int)index.x, (int)index.y);
                GameLogic.Instance.CanWeFight();
            }         
        }
        
    }
    void OnTriggerExit2D(Collider2D other)
    {
        currentBird = null;
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
       /* if (Input.GetMouseButtonUp(0) && currentBird != null)
        {
            Var.playerPos[(int)index.y, (int)index.x] = currentBird;
            if (currentBird.gameObject == Var.selectedBird)
            {
                currentBird.ReleseBird((int)index.y, (int)index.x);
                GameLogic.Instance.CanWeFight();
            }
        }*/
     
    }
}
