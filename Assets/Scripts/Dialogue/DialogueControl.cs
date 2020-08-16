using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueControl : MonoBehaviour {
    List<Dialogue> dialogs;
    public AudioGroup kingAudioGroup;
    [HideInInspector]
    public List<Dialogue> relationshipDialogs = new List<Dialogue>();
    public Dialogue testDialog;
    public Transform portraitPoint;
    public static DialogueControl Instance { get; private set; }
    public List<Transform> areaDialogues;
    List<EventScript.Character> acceptableNpcs = new List<EventScript.Character>() {EventScript.Character.The_Vulture_King};//, EventScript.Character.player };
	public Transform anyAreaDialogues;
	[Range(0f, 1f)]
	public float dialogueFrequency = 0.2f;
	[Range(0f, 1f)]
	public float relationshipDialogueFrequency = 1f;
	public List<Bird> speakers = new List<Bird>();
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

        speakers = new List<Bird>();
        if (acceptableNpcs.Contains(dialog.speakers[0]) && dialog.location == Dialogue.Location.battle)
        {
            speakers.Add(null);
          // Debug.LogError("Addded firsrt speaker: " + dialog.speakers[0] + " count: " + speakers.Count);
        }
        else
        {
            Bird dialogueBird = Helpers.Instance.GetBirdFromEnum(dialog.speakers[0]);
            if(dialogueBird == null)
            {
                dialogueBird = Var.activeBirds[0];
            }
            speakers.Add(dialogueBird);
            //Debug.LogError("Addded firsrt speaker: " + dialogueBird.charName + " count: " + speakers.Count);
        }
        LeanTween.delayedCall(0.2f, () => CreateDialogue(dialog));

	}

	public void TryDialogue(Dialogue.Location location, EventScript.Character Char = EventScript.Character.None)
	{

        if (Var.isTutorial || Var.isEnding || (GuiContoler.Instance.winBanner != null
        && GuiContoler.Instance.winBanner.activeSelf) || Var.currentStageID == 100 || Var.gameSettings.shownBattlePlanningTutorial == false)
        {
            return;
        }
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
			if (dialogue.location == location && canCreate&& AllBirdsInScene && !alreadySeen && !dialogueBird.data.injured)
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
            if (dialogue.canUseRandomBirds && !acceptableNpcs.Contains(dialogue.speakers[i]))
            {
                Bird bird = Helpers.Instance.GetBirdFromEnum(dialogue.speakers[i]);
                /*if(dialogue.speakers[i-1]== EventScript.Character.the_Vulture_King)
                {
                    speakers.Add(new Bird("King",3,3));
                }
                else*/
                if(bird == null)
                {
                    speakers.Add(Var.activeBirds[i]);
                    // Debug.LogError("speaker count: " + speakers.Count + " added " + Var.activeBirds[i - 1].charName);
                }
                else
                {
                    speakers.Add(bird);
                }
            }
            else
            {

                int j = 0;
                EventScript.Character dialogBird = dialogue.speakers[i];
                if (acceptableNpcs.Contains(dialogBird) && dialogue.location == Dialogue.Location.battle)
                { 
                    speakers.Add(null);
                  // Debug.LogError("NPC! speaker count: " + speakers.Count + " added " + dialogBird);

                }
			else
			{
				while (true)
				{
					Bird enumBird = Helpers.Instance.GetBirdFromEnum(dialogBird);
					if (!speakers.Contains(enumBird))
					{
						speakers.Add(enumBird);
                     //   Debug.LogError("ENUM speaker count: " + speakers.Count + " added " + enumBird);
                            break;
					}
					j++;
					if (j > 1000)
					{
						//Debug.LogError("couldnt add dialogue speaker to list");
						speakers.Add(Var.activeBirds[0]);
						break;
					}

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
					case EventScript.Character.The_Vulture_King:
						GuiContoler.Instance.boss.SetActive(true);
						GuiContoler.Instance.ShowSpeechBubble(GuiContoler.Instance.kingMouth.transform, partData.text,kingAudioGroup,false);
						break;
					case EventScript.Character.player:
						GuiContoler.Instance.ShowSpeechBubble(GuiContoler.Instance.playerMouth.transform, partData.text,kingAudioGroup);
						break;
					default:
						break;

				}


			}
			else
			{
               //Debug.LogError("speaker count: " + speakers.Count + " ID: " + partData.speakerID);
				Bird activeBird = speakers[partData.speakerID];
				if (activeBird != null)
					activeBird.Speak(partData.text);
			}
		}
	}

	void CreateGraphDialogue(Dialogue dialogue)
	{
		AudioGroup birdTalk = AudioControler.Instance.GetBirdSoundGroup("default").GetTalkGroup(Var.Em.Neutral);
		foreach (DialoguePart partData in dialogue.dialogueParts)
		{
			GuiContoler.Instance.ShowSpeechBubble(portraitPoint,partData.text, birdTalk);
		}
	}
}
