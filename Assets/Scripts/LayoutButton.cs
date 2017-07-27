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
    Color rowColor = Color.clear;
    Color columnColor = Color.clear;
    Color baseColor;
    public Color highlightColor;
   // [HideInInspector]
    public int ConfBonus = 0;
   // [HideInInspector]
    public int FriendBonus = 0;
    public int RollBonus = 0;
    public int PlayerRollBonus = 0;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
        baseColor = sr.color;
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
    public void SetColor(Color color,bool isRow)
    {        
            //Clear == null
            if (isRow)
            {
                rowColor = color;
                if (columnColor.Equals(Color.clear))
                {
                    LeanTween.color(gameObject, color, 0.3f);
                    baseColor = color;
                }
                else
                {
                    Color col = (columnColor + color) / 2;
                    LeanTween.color(gameObject, col, 0.3f);
                    baseColor = col;
                }

            }
            else
            {
                columnColor = color;
                if (rowColor.Equals(Color.clear))
                {
                    LeanTween.color(gameObject, color, 0.3f);
                    baseColor = color;
                }
                else
                {
                    Color col = (rowColor + color) / 2;
                    LeanTween.color(gameObject, col, 0.3f);
                    baseColor = col;
                }
            }
        
        
    }
    public void ResetColor(bool isRow)
    {        
            if (isRow)
            {
                //Clear == null
                rowColor = Color.clear;
                if (columnColor.Equals(Color.clear))
                {
                    LeanTween.color(gameObject, defaultColor, 0.3f);
                    baseColor = defaultColor;
                }
                else
                {
                    LeanTween.color(gameObject, columnColor, 0.3f);
                    baseColor = columnColor;
                }
            }
            else
            {
                //Clear == null
                columnColor = Color.clear;
                if (rowColor.Equals(Color.clear))
                {
                    LeanTween.color(gameObject, defaultColor, 0.3f);
                    baseColor = defaultColor;
                }
                else
                {
                    LeanTween.color(gameObject, rowColor, 0.3f);
                    baseColor = rowColor;
                }
            }
        
        

    }

   

    public void ApplyPower(Bird birdObj)
    {
        //TODO: is this a ground ability?
        
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
        birdObj.confBoos += ConfBonus;// *birdObj.groundMultiplier;
        birdObj.friendBoost += FriendBonus;// *birdObj.groundMultiplier;
        birdObj.GroundRollBonus += RollBonus;
        birdObj.PlayerRollBonus += PlayerRollBonus;
    }

    public void Reset()
    {
        swapBird = null;
        currentBird = null;
        baseColor = defaultColor;
        sr.color = defaultColor;
    }
    public void fullReset()
    {
        //TODO: implement full reset
        Reset();

    }
    void OnTriggerExit2D(Collider2D other)
    {
        LeanTween.color(gameObject, baseColor, 0.3f);
        Bird tempBird = other.transform.parent.GetComponent<Bird>();
        if (tempBird.dragged)
        {
            //tempBird.target = tempBird.home;
            if (swapBird == null)
            {                
                currentBird = null;
            }
            else
            {
                swapBird = null;

            }
        }
    }     


    void Update()
    {
       
        hasBird = (currentBird != null);        
        if (Input.GetMouseButtonUp(0) && currentBird != null )
        {
            if (currentBird.dragged)
            {
                if ((int)index.x == -1)
                {
                    currentBird.target = currentBird.home;
                    currentBird.ReleseBird((int)index.x, (int)index.y);
                    currentBird = null;
                    swapBird = null;
                    return;
                }
                Var.playerPos[(int)index.x, (int)index.y] = currentBird;
                LeanTween.color(gameObject, baseColor, 0.3f);                
                currentBird.target = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
                ApplyPower(currentBird);
                currentBird.ReleseBird((int)index.x, (int)index.y);
                if (!inMap)
                {                    
                    GameLogic.Instance.CanWeFight();                    
                }
                else
                {
                    MapControler.Instance.CanLoadBattle();
                }

            }
            if(swapBird != null)
            {

                Var.playerPos[(int)index.x, (int)index.y] = swapBird;               
                if (swapBird.y != -1 )
                {
                    ObstacleGenerator.Instance.tiles[swapBird.y * 4 + swapBird.x].currentBird = currentBird;
                    currentBird.target = swapBird.target;                    
                    Var.playerPos[swapBird.x, swapBird.y] = currentBird;
                }else
                {
                    currentBird.target = currentBird.home;
                    print("swapped home");
                }             
                currentBird.OnLevelPickup();
                currentBird.ReleseBird(swapBird.x, swapBird.y);
                currentBird = swapBird;
                currentBird.target = new Vector3(transform.position.x, transform.position.y + 0.5f, 0);
               // currentBird.res
                ApplyPower(currentBird);
                currentBird.ReleseBird((int)index.x, (int)index.y);
                if (!inMap)
                {
                    GameLogic.Instance.CanWeFight();                    
                }
                else
                {
                    MapControler.Instance.CanLoadBattle();
                }
            }
            swapBird = null;
            if (index.x == -1)
            {
                currentBird = null;
                swapBird = null;
            }
        }
     
    }
}
