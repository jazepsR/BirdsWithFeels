using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public RectTransform rect;
    public float waitTime;
    public float speed;
    public float FastSpeed;
    public float endPos;
    float acceleration =1f;

    float currentSpeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = speed;

        StartCoroutine(ScrollCredits());
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("mainMenu");
        }


        if (Input.GetMouseButton(0))
        {
            if(currentSpeed<FastSpeed)
            { currentSpeed = currentSpeed + acceleration; }
            
            
        }
        else
        {
            if(currentSpeed>speed)
            {
                currentSpeed = currentSpeed - acceleration * 5;
            }
            else
            {
                currentSpeed = speed;
            }

        }
    }
    IEnumerator ScrollCredits()
    {
        yield return new WaitForSecondsRealtime(waitTime);
        while(rect.position.y<endPos)
        {
            float posY = rect.position.y;
            posY += currentSpeed * Time.deltaTime;
            rect.position = new Vector3(rect.position.x, posY);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(waitTime);
        
        SceneManager.LoadScene("Stats");
    }
}
