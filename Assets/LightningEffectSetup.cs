using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightningEffectSetup : MonoBehaviour
{
    public GameObject BirdEffect;
    public Image emotionIndicator;
    public SpriteRenderer sr_glowEffect;
    public GameObject glowEffect_wizard;
    private ShowTooltip tooltipScript;
    [SerializeField]
    private GameObject forcefield;
    [SerializeField]
    private ParticleSystem ps;

    private void SetupReferences()
    {
    
        if (emotionIndicator != null)
        {
            tooltipScript = emotionIndicator.GetComponent<ShowTooltip>();
        }
    }

    public void SetupGlowEffect( Var.Em emotion, Bird affectedBird,Bird enemy)
    {
        SetupReferences();

        if (BirdEffect != null)
        {
            sr_glowEffect.color = Helpers.Instance.GetEmotionColor(emotion); 
            BirdEffect.transform.position = affectedBird.target;
            emotionIndicator.sprite = Helpers.Instance.GetEmotionIcon(emotion,false);
            tooltipScript.tooltipText = affectedBird.charName + " will become more " + emotion.ToString() + " from facing the wizard vulture";
            glowEffect_wizard.transform.position = enemy.target;
            glowEffect_wizard.GetComponent<SpriteRenderer>().color = Helpers.Instance.GetEmotionColor(emotion);
            forcefield.transform.position = affectedBird.transform.position;
            var PsMain = ps.main;
            PsMain.startColor= Helpers.Instance.GetEmotionColor(emotion);

        }
    }
}
