using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueControl : MonoBehaviour {
    List<Dialogue> dialogs;
    public List<Dialogue> relationshipDialogs = new List<Dialogue>();
    public Transform portraitPoint;
    public static DialogueControl Instance { get; private set; }
    public List<Transform> areaDialogues;
    public Transform anyAreaDialogues;
    [Range(0f, 1f)]
    public float dialogueFrequency = 0.2f;
    [Range(0f, 1f)]
    public float relationshipDialogueFrequency = 1f;
    // Use this for initialization
    public void GetRelationshipDialogs()
    {
        relationshipDialogs = new List<Dialogue>();
        foreach(Bird bird in Var.availableBirds)
        {
            if(bird.relationshipDialogs!= null)
                relationshipDialogs.AddRange(bird.relationshipDialogs);
        }

    }
    void Awake() {
        Instance = this;
        try
        {
            dialogs = new List<Dialogue>(transform.GetComponentsInChildren<Dialogue>());
            dialogs.AddRange(anyAreaDialogues.GetComponentsInChildren<Dialogue>());
            dialogs.AddRange(areaDialogues[Var.currentBG].GetComponentsInChildren<Dialogue>());
        }
        catch { }
    }
	
	public void TryDialogue(Dialogue.Location location, EventScript.Character Char = EventScript.Character.None)
    {
        if (Random.Range(0, 1.0f) > dialogueFrequency)
            return;
        for(int i =0; i<100; i++)
        {
            

            Dialogue dialogue = dialogs[Random.Range(0, dialogs.Count)];
            if (Random.Range(0, 1.0f) < relationshipDialogueFrequency && relationshipDialogs.Count>0)
                dialogue = relationshipDialogs[Random.Range(0, relationshipDialogs.Count)];
            Bird dialogueBird = Helpers.Instance.GetBirdFromEnum(dialogue.dialogueParts[0].speaker);
            if (dialogue.location == location && ConditionCheck.CheckCondition(dialogue.condition,dialogueBird))
            {
                if (Char == EventScript.Character.None || Char == dialogue.dialogueParts[0].speaker)
                {
                    CreateDialogue(dialogue);
                    break;
                }
            }


        }



    }
    public void CreateDialogue(Dialogue dialogue)
    {
        if (dialogue.location == Dialogue.Location.battle || dialogue.location == Dialogue.Location.map)
            CreateBattleDialogue(dialogue);
        if (dialogue.location == Dialogue.Location.graph)
            CreateGraphDialogue(dialogue);
    }   
    
    void CreateBattleDialogue(Dialogue dialogue)
    {
        foreach (DialoguePart partData in dialogue.dialogueParts)
        {
           Bird activeBird = Helpers.Instance.GetBirdFromEnum(partData.speaker);
           if(activeBird!=null)
            activeBird.Speak(partData.text);
        }
    }

    void CreateGraphDialogue(Dialogue dialogue)
    {
        foreach (DialoguePart partData in dialogue.dialogueParts)
        {
            GuiContoler.Instance.ShowSpeechBubble(portraitPoint, partData.text);
        }
    }
}
