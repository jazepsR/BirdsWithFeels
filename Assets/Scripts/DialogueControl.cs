using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueControl : MonoBehaviour {
	List<Dialogue> dialogs;
	[HideInInspector]
	public List<Dialogue> relationshipDialogs = new List<Dialogue>();
    public Dialogue testDialog;
	public Transform portraitPoint;
	public static DialogueControl Instance { get; private set; }
	public List<Transform> areaDialogues;
	List<EventScript.Character> acceptableNpcs= new List<EventScript.Character>() { EventScript.Character.the_Vulture_King, EventScript.Character.player };
	public Transform anyAreaDialogues;
	[Range(0f, 1f)]
	public float dialogueFrequency = 0.2f;
	[Range(0f, 1f)]
	public float relationshipDialogueFrequency = 1f;
	List<Bird> speakers;
	Dialogue afterEventDialog= null;
	// Use this for initialization
	/*public void GetRelationshipDialogs()
	{
		relationshipDialogs = new List<Dialogue>();
		foreach(Bird bird in Var.availableBirds)
		{
			if(bird.relationshipDialogs!= null)
				relationshipDialogs.AddRange(bird.relationshipDialogs);
		}

	}*/
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
	void Start()
    {
        if(testDialog != null)
        {
            CreateParticularDialog(testDialog);
        }

    }

    public void CreateParticularDialog(Dialogue dialog)
    {
        Bird dialogueBird = Helpers.Instance.GetBirdFromEnum(dialog.speakers[0]);
        speakers = new List<Bird>();
        speakers.Add(dialogueBird);
        CreateDialogue(dialog);

    }

	public void TryDialogue(Dialogue.Location location, EventScript.Character Char = EventScript.Character.None)
	{

		if (Var.isTutorial ||(GuiContoler.Instance.winBanner!= null && GuiContoler.Instance.winBanner.activeSelf ))
			return;
		if (Random.Range(0, 1.0f) > dialogueFrequency)
			return;
		for(int i =0; i<100; i++)
		{
			

			Dialogue dialogue = dialogs[Random.Range(0, dialogs.Count)];
			if (Random.Range(0, 1.0f) < relationshipDialogueFrequency && relationshipDialogs.Count>0)
				dialogue = relationshipDialogs[Random.Range(0, relationshipDialogs.Count)];
			Bird dialogueBird = Helpers.Instance.GetBirdFromEnum(dialogue.speakers[0]);
			speakers = new List<Bird>();
			speakers.Add(dialogueBird);
			bool AllBirdsInScene = true;
			foreach(EventScript.Character dialogBird in dialogue.speakers)
			{
				if(Helpers.Instance.GetBirdFromEnum(dialogBird) == null)
				{
					if (acceptableNpcs.Contains(dialogBird) && dialogue.location == Dialogue.Location.battle)
					{

					}else
					{
						AllBirdsInScene = false;
						break;
					}                    
				}
			}
			bool canCreate = ConditionCheck.CheckCondition(dialogue.condition, dialogueBird, dialogue.targetEmotion, dialogue.magnitude);
			bool alreadySeen = Var.shownDialogs.Contains(dialogue.dialogueParts[0].text);
			if (dialogue.location == location && canCreate&& AllBirdsInScene && !alreadySeen && !dialogueBird.injured)
			{
				if (Char == EventScript.Character.None || Char == dialogue.speakers[0])
				{
					CreateDialogue(dialogue);
					break;
				}
			}            
		}
	}

	void Update()
	{
		if(afterEventDialog !=null)
		{
			if(!EventController.Instance.eventObject.activeSelf)
			{
				CreateDialogue(afterEventDialog);
				afterEventDialog = null;
			}
		}
	}
	public void CreateDialogue(Dialogue dialogue)
	{
		if(EventController.Instance.eventObject.activeSelf)
		{
			afterEventDialog = dialogue;
			return;
		}

		if (!dialogue.canShowMultipleTimes)
		{           
			Var.shownDialogs.Add(dialogue.dialogueParts[0].text);
		}
		
		for(int i=1;i<dialogue.speakers.Count;i++)
		{
			int j = 0;
			EventScript.Character dialogBird = dialogue.speakers[i];
            if (acceptableNpcs.Contains(dialogBird) && dialogue.location == Dialogue.Location.battle)
                speakers.Add(null);
            else
            {
                while (true)
                {
                    Bird enumBird = Helpers.Instance.GetBirdFromEnum(dialogBird);
                    if (!speakers.Contains(enumBird))
                    {
                        speakers.Add(enumBird);
                        break;
                    }
                    j++;
                    if (j > 1000)
                    {
                        Debug.LogError("couldnt add dialogue speaker to list");
                        break;
                    }

                }
            }
		}
		if (dialogue.location == Dialogue.Location.battle) 
			CreateBattleDialogue(dialogue);
        if (dialogue.location == Dialogue.Location.map)
            CreateMapDialogue(dialogue);
		if (dialogue.location == Dialogue.Location.graph)
			CreateGraphDialogue(dialogue);
	}   
	
	void CreateMapDialogue(Dialogue dialogue)
	{
		foreach (DialoguePart partData in dialogue.dialogueParts)
		{
		   Bird activeBird = speakers[partData.speakerID];
		   if(activeBird!=null)
			activeBird.Speak(partData.text);
		}
	}
    void CreateBattleDialogue(Dialogue dialogue)
    {
        foreach (DialoguePart partData in dialogue.dialogueParts)
        {
            if (acceptableNpcs.Contains(dialogue.speakers[partData.speakerID]))
            {
                switch (dialogue.speakers[partData.speakerID])
                {
                    case EventScript.Character.the_Vulture_King:
                        GuiContoler.Instance.boss.SetActive(true);
                        GuiContoler.Instance.ShowSpeechBubble(GuiContoler.Instance.kingMouth.transform, partData.text);
                        
                        break;
                    case EventScript.Character.player:
                        GuiContoler.Instance.ShowSpeechBubble(GuiContoler.Instance.playerMouth.transform, partData.text);
                        break;
                    default:
                        break;

                }


            }
            else
            {
                Bird activeBird = speakers[partData.speakerID];
                if (activeBird != null)
                    activeBird.Speak(partData.text);
            }
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
