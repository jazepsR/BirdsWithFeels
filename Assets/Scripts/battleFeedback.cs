using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class battleFeedback : MonoBehaviour {
    [HideInInspector]
    public feedBack fb;
    public void OnMouseEnter()
    {
        string fbText = SetFeedbackText();
        if (fbText == "")
        {
            Helpers.Instance.ShowTooltip("\nNo battle bonuses\n");
        }
        else
        {
            Helpers.Instance.ShowTooltip(fbText);
        }
        
    }

    public void OnMouseExit()
    {
        GuiContoler.Instance.tooltipText.transform.parent.gameObject.SetActive(false);
    }
    public string SetFeedbackText()
    {
        string text = "";
        if (GameLogic.Instance.Bird1Win(fb.PlayerEnemyBird, fb.birdScript))
            text += "Winning emotion: +40%\n"; 
        if (GameLogic.Instance.Bird1Win(fb.birdScript, fb.PlayerEnemyBird))
            text += "Losing emotion: +40%\n";
        if (Helpers.Instance.IsSuper(fb.birdScript.emotion))
            text += "Enemy bird has super emotion -10%\n";
        if (Helpers.Instance.IsSuper(fb.PlayerEnemyBird.emotion))
            text += "Player bird has super emotion +10%\n";
        if (fb.PlayerEnemyBird.levelRollBonus >= 1)
            text += "Player strength bonus +" + (fb.PlayerEnemyBird.levelRollBonus) * 10 + "%\n";
        if (fb.birdScript.levelRollBonus >= 1)
            text += "Enemy strength bonus -" +(fb.birdScript.levelRollBonus) * 10 + "%\n";
        if (fb.birdScript.PlayerRollBonus != 0)
            text += "Enemy affected by player birds " + (-10*fb.birdScript.PlayerRollBonus).ToString("+#;-#;0") + "%\n";
        if (fb.PlayerEnemyBird.PlayerRollBonus != 0)
            text += "Player affected by friendly birds " + (10*fb.PlayerEnemyBird.PlayerRollBonus).ToString("+#;-#;0") + "%\n";
        if(fb.PlayerEnemyBird.GroundRollBonus != 0)
            text += "Player ground bonus " + (10 * fb.PlayerEnemyBird.GroundRollBonus).ToString("+#;-#;0") + "%\n";
        return text;


    }
}
