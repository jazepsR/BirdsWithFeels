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
		if(fb.PlayerEnemyBird.emotion == Var.Em.Neutral && fb.birdScript.emotion != Var.Em.Neutral)
			text += "Emotion against neurtal: -40%\n";
		if (fb.PlayerEnemyBird.emotion == fb.birdScript.emotion && fb.birdScript .emotion != Var.Em.Neutral)
			text += "Winning emotion: +40%\n";
		if (fb.PlayerEnemyBird.emotion ==Helpers.Instance.GetOppositeEmotion(fb.birdScript.emotion))
				text += "Losing emotion: -40%\n";
		if (fb.PlayerEnemyBird.data.levelRollBonus >= 1)
            text += "Player strength bonus +" + (fb.PlayerEnemyBird.data.levelRollBonus) * 10 + "%\n";
        if (fb.birdScript.data.levelRollBonus >= 1)
            text += "Enemy strength bonus -" +(fb.birdScript.data.levelRollBonus) * 10 + "%\n";
        if (fb.birdScript.PlayerRollBonus != 0)
            text += "Enemy affected by player birds " + (-10*fb.birdScript.PlayerRollBonus).ToString("+#;-#;0") + "%\n";
        if (fb.PlayerEnemyBird.PlayerRollBonus != 0)
            text += "Player affected by friendly birds " + (10*fb.PlayerEnemyBird.PlayerRollBonus).ToString("+#;-#;0") + "%\n";
        if(fb.PlayerEnemyBird.GroundRollBonus != 0)
            text += "Player ground bonus " + (10 * fb.PlayerEnemyBird.GroundRollBonus).ToString("+#;-#;0") + "%\n";
        return text;


    }
}
