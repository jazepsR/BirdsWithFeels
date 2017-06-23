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
        if (other.tag == "feet") {
            Bird birdObj = other.transform.parent.GetComponent<Bird>();            
                
                /*if (currentBird != null)
                {
                    currentBird.target = currentBird.home;
                }*/

                birdObj.target = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
                currentBird = birdObj;               
                Debug.Log("etered)");
            }
        
    }

    void OnTriggerExit2D(Collider2D other)
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
    }


    void Update()
    {
        hasBird = (currentBird != null);
        if (Input.GetMouseButtonUp(0) && currentBird != null)
        {
            Var.playerPos[(int)index.y, (int)index.x] = currentBird;
            if (currentBird.gameObject == Var.selectedBird)
            {
                currentBird.ReleseBird((int)index.y, (int)index.x);
                GameLogic.Instance.CanWeFight();
            }
        }
     
    }
}
