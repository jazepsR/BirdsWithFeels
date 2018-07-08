using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class cutScene : MonoBehaviour {
    public Transform panToPoint;
    public List<cutscenePart> parts;
    public GameObject mainMenu;
    public GameObject textBox;
    public Text cutsceneText;
    public Image clickImage;
    bool canClick = false;
    int currentArea = 0;
    int currentCount = 0;
    public GameObject startBtn;
	public bool isEnd = false;
	// Use this for initialization
    void Start()
    {


    }

	public void StartCutscene()
    {
        cutsceneText.text = parts[0].cutsceneTexts[0];
		if (isEnd)
		{
			startTexts();
		}
		else
		{
			LeanTween.move(mainMenu, panToPoint, 4f).setOnComplete(startTexts).setEase(LeanTweenType.easeOutQuad);
		}

    }
    void Update()
    {
        if (!canClick)
            return;
        if (Input.GetMouseButtonDown(0))
        {

            if (currentCount+1 < parts[currentArea].cutsceneTexts.Count)
            {
                currentCount++;
                cutsceneText.text = parts[currentArea].cutsceneTexts[currentCount];
                
            }
            else
            {
                if (currentArea+1 < parts.Count)
                {
                    currentArea++;
                    currentCount = 0;
                    cutsceneText.text = parts[currentArea].cutsceneTexts[currentCount];
                    clickImage.sprite = parts[currentArea].image;
                    clickImage.color = Color.white;
                }else
                {
					if (isEnd)
					{
						SceneManager.LoadScene("mainMenu");
					}
					else
					{
						startBtn.SetActive(true);
						textBox.SetActive(false);
						canClick = false;
					}
                }
            }
        }
    }
    
    public void startTexts()
    {

        canClick = true;

    }
	public void StartLevel()
	{
		SceneManager.LoadScene("NewMain");
	}
}
