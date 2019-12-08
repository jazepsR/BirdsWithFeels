using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public RectTransform rect;
    public float waitTime = 1.5f;
    public float speed = 0.5f;
    public float endPos = 332;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ScrollCredits());
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("mainMenu");
        }
    }
    IEnumerator ScrollCredits()
    {
        yield return new WaitForSecondsRealtime(waitTime);
        while(rect.position.y<endPos)
        {
            float posY = rect.position.y;
            posY += speed * Time.deltaTime;
            rect.position = new Vector3(rect.position.x, posY);
            yield return null;
        }
        yield return new WaitForSecondsRealtime(waitTime);
        SceneManager.LoadScene("mainMenu");
    }
}
