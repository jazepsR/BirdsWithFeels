using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bossBattleVultureVisuals : MonoBehaviour
{
    public GameObject[] rightVultures;
    public GameObject[] leftVultures;

    public GameObject[] twoVulturekINGS;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < leftVultures.Length; i++)
        {
            leftVultures[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < rightVultures.Length; i++)
        {
            rightVultures[i].gameObject.SetActive(false);
        }


    }

    void setupLastBattle()
    {
        for (int i = 0; i < leftVultures.Length; i++)
        {
            leftVultures[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < rightVultures.Length; i++)
        {
            rightVultures[i].gameObject.SetActive(true);
        }
    }


    // Update is called once per frame
    void Update()
    {


if(        Input.GetKeyDown(KeyCode.A))
        {
            addVulturesToLeftSide();
        }

    }

    
    void addVulturesToLeftSide()
    {


        for (int i = 0; i < leftVultures.Length; i++)
        {
            if(leftVultures[i].gameObject.activeInHierarchy==false)
            {
                leftVultures[i].gameObject.SetActive(true);
                break;
            }
            
        }

        for (int i = 0; i < rightVultures.Length; i++)
        {
            if (rightVultures[i].gameObject.activeInHierarchy == true)
            {
                rightVultures[i].gameObject.SetActive(false);
                break;
            }

        }


    }

}
