using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWebpage : MonoBehaviour
{
    public string socialMediaUrl;

    public void OpenWebpage()
    {
        if (socialMediaUrl != null)
        {
            Application.OpenURL(socialMediaUrl);
        }
        else
        {
            print("Invalid URL supplied! Sorry");
        }
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
