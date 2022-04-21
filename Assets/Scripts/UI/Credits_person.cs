using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Credits_person : MonoBehaviour
{
    public string ContinueButtonLine;
    Credits_special credits;
    private Animator animator;
    public Text buttonText;
    public Text progressText;

    // Start is called before the first frame update
    void Start()
    {
        if (ContinueButtonLine == null)
        {
            ContinueButtonLine = "next!";
        }
        if (buttonText != null)
        {
            buttonText.text = ContinueButtonLine;
        }


        animator = GetComponent<Animator>();
    }
    public Text GetProgressText()
    {
        if (progressText != null)
        {
            return progressText;
        }
        else
        {
            return null;
        }
    }

    public void ShowPerson(Credits_special aCredits)
    {
        credits = aCredits;
        animator.SetTrigger("show");
    }

    public void HidePerson()
    {
        animator.SetTrigger("hide");
    }

    public void ButtonClicked()
    {
        if (credits != null)
        {
            credits.ContinueButtonClicked(this);
        }
        else
        {
            print("invalid credits reference! Sorry :( ");
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
