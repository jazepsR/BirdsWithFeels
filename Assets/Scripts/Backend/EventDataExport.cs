using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System;

public class EventDataExport : MonoBehaviour {
	static SerializedEvents serializedEvents;
	static SerializedDialogues serializedDialogues;
	public static void SaveEvents(string path)
	{
		string filePath = getPath() + path;
		Directory.CreateDirectory(filePath);
		foreach (SerializedEvent obj in serializedEvents.events)
		{			
			string jsonString = JsonUtility.ToJson(obj);
			FileStream fs =File.Create(filePath + "/" + obj.evName + ".json");
			fs.Close();
			File.WriteAllText(filePath + "/" + obj.evName + ".json", jsonString);

		}
	}

	public static void SaveDialogues(string path)
	{
		string filePath = getPath() + path;
		string jsonString = "{dialogues:[";
		foreach (SerializedDialogue obj in serializedDialogues.dialogues)
		{
			jsonString += "{\"mainDialogue\":" + obj.mainDialogue + ",\"dialogueParts\":[";
			foreach (string part in obj.dialogueParts)
			{
				jsonString += part + ",";
			}
			jsonString = jsonString.Substring(0, jsonString.Length - 1);
			jsonString += "]},";
		}
		jsonString = jsonString.Substring(0, jsonString.Length - 1);
		jsonString += "]}";
		File.WriteAllText(filePath, jsonString);
	}

	public static void CreateDialogueExportFile()
	{
		Debug.Log("started serizalizing dialogues!");
		List<Dialogue> eventsToExport = new List<Dialogue>(FindObjectsOfType<Dialogue>());
		serializedDialogues = new SerializedDialogues();
		foreach(Dialogue dialogueObj in eventsToExport)
		{
			SerializedDialogue dialog = new SerializedDialogue();
			dialog.mainDialogue = JsonUtility.ToJson(dialogueObj);
			List<DialoguePart> parts = new List<DialoguePart>(dialogueObj.GetComponentsInChildren<DialoguePart>(true));
			foreach(DialoguePart part in parts)
			{
				dialog.dialogueParts.Add(JsonUtility.ToJson(part));
			}
			serializedDialogues.dialogues.Add(dialog);
		}
		Debug.Log("Serialized " + serializedDialogues.dialogues.Count + " dialogues!");
	}
	public static void CreateEventExportFile()
	{
		Debug.Log("started serizalizing events!");
		List<EventScript> eventsToExport = new List<EventScript>(FindObjectsOfType<EventScript>());
		serializedEvents = new SerializedEvents();
		foreach (EventScript ev in eventsToExport)
		{
			SerializedEvent eventObj = new SerializedEvent(ev.speakers,
				ev.condition,
				ev.magnitude,
				ev.targetEmotion,
				ev.heading,
				ev.canShowMultipleTimes,
				ev.name);
			
			List<EventPart> parts = new List<EventPart>(ev.GetComponentsInChildren<EventPart>(true));
			foreach (EventPart part in parts)
			{
				eventObj.eventParts.Add(new SerializedEventPart(part.speakerId,part.text,part.useCustomPic,part.customPic));
			}
			foreach (EventConsequence con in ev.options)
			{
				eventObj.eventConsequence.Add(new SerializedEventConsequence(con.consequenceType1, con.consequenceType2, con.consequenceType3, con.magnitude1,
					con.magnitude2, con.magnitude3, con.useAutoExplanation, con.AfterImage, con.icon, con.selectionTooltip, con.conclusionHeading, con.conclusionText));
			}
			if (ev.afterEventDialog == null)
				eventObj.afterEventDialog = "none";
			else
				eventObj.afterEventDialog = ev.afterEventDialog.name;
			serializedEvents.events.Add(eventObj);
		}
		Debug.Log("Serialized " + serializedEvents.events.Count + " events!");
	}
	public static void LoadEvents(Transform parent, string path)
	{

		string fullPath = ExcelExport.getPath()+ "/"+ "CSV/eventData" + ".json";
		if (!File.Exists(fullPath))
		{			
			Debug.LogError("file not found in path!");
			return;
		}

		string allText = File.ReadAllText(fullPath);
		SerializedEvent ev= JsonUtility.FromJson<SerializedEvent>(allText);
		CreateLoadedEvent(ev, parent);

	}
	public static void CreateLoadedEvent(SerializedEvent evData, Transform parent)
	{
		GameObject evObj = new GameObject(evData.evName);
		evObj.transform.parent = parent;
		EventScript evScript = evObj.AddComponent<EventScript>();
		evData.GetEventScript(evScript);
		//Generate event parts
		GameObject parts = new GameObject(evData.evName);
		parts.transform.parent = evObj.transform.parent;
		foreach (SerializedEventPart part in evData.eventParts)
		{
			GameObject partObj = new GameObject("part");
			partObj.transform.parent = parts.transform.parent;
			EventPart evPart = evObj.AddComponent<EventPart>();
			part.GetEventPart(evPart);		
		}
		//Generate event consequences
		evScript.options = new EventConsequence[evData.eventConsequence.Count];
		int i = 0;
		foreach (SerializedEventConsequence part in evData.eventConsequence)
		{
			GameObject partObj = new GameObject("consequence");
			partObj.transform.parent = evObj.transform;
			EventConsequence evPart = evObj.AddComponent<EventConsequence>();
			part.GetEventConsequence(evPart);
			evScript.options[i] = evPart;
			i++;
		}

	}
	// Following method is used to retrive the relative path as device platform
	private static string getPath()
	{
#if UNITY_EDITOR
		return Application.dataPath;
#elif UNITY_ANDROID
		return Application.persistentDataPath;
#elif UNITY_IPHONE
		return Application.persistentDataPath+"/";
#else
		return Application.dataPath + "/";
#endif
	}
}

[System.Serializable]
public class SerializedEvents
{
	public List<SerializedEvent> events;
	public SerializedEvents()
	{
		events = new List<SerializedEvent>();
	}
}

[System.Serializable]
public class SerializedEvent
{
	public List<EventScript.Character> speakers;
	public ConditionCheck.Condition condition;
	public int magnitude = 0;
	public Var.Em targetEmotion;
	public string heading;
	public bool canShowMultipleTimes = false;
	public List<SerializedEventPart> eventParts;
	public List<SerializedEventConsequence> eventConsequence;
	public string afterEventDialog;
	public string evName;
	public SerializedEvent(List<EventScript.Character> speakers,ConditionCheck.Condition condition,int magnitude, Var.Em targetEmotion,
		string heading, bool canShowMultipleTimes,string evName)
	{
		this.speakers = speakers;
		this.condition = condition;
		this.magnitude = magnitude;
		this.targetEmotion = targetEmotion;
		this.heading = heading;
		this.canShowMultipleTimes = canShowMultipleTimes;
		this.evName = evName;
		eventParts = new List<SerializedEventPart>();
		eventConsequence = new List<SerializedEventConsequence>();
	}
	public EventScript GetEventScript(EventScript ev)
	{
		ev.speakers = speakers;
		ev.condition = condition;
		ev.magnitude = magnitude;
		ev.targetEmotion = targetEmotion;
		ev.heading = heading;
		ev.canShowMultipleTimes = canShowMultipleTimes;
		ev.parts = new List<EventPart>();
		ev.options = new EventConsequence[0];
		return ev;
	}
}
[System.Serializable]
public class SerializedEventPart
{

	public int speakerId = 0;
	public string text;
	public bool useCustomPic = false;
	public Sprite customPic;

	public SerializedEventPart(int speakerId, string text, bool useCustomPic, Sprite customPic)
	{
		this.speakerId = speakerId;
		this.text = text;
		this.useCustomPic = useCustomPic;
		this.customPic = customPic;
	}
	public EventPart GetEventPart(EventPart ep)
	{
		ep.speakerId = speakerId;
		ep.text = text;
		ep.useCustomPic = useCustomPic;
		ep.customPic = customPic;
		return ep;
	}

}
[Serializable]
public class SerializedEventConsequence
{
	public ConsequenceType consequenceType1 = ConsequenceType.Nothing;
	public int magnitude1;
	public ConsequenceType consequenceType2 = ConsequenceType.Nothing;
	public int magnitude2;
	public ConsequenceType consequenceType3 = ConsequenceType.Nothing;
	public int magnitude3;
	public bool useAutoExplanation;
	public Sprite AfterImage = null;
	public Sprite icon;
	public string selectionText;
	public string selectionTooltip;
	public string conclusionHeading;
	public string conclusionText;

	public SerializedEventConsequence(ConsequenceType consequenceType1, ConsequenceType consequenceType2, ConsequenceType consequenceType3,
		int magnitude1, int magnitude2, int magnitude3, bool useAutoExplanation, Sprite AfterImage, Sprite icon, string selectionTooltip,
		string conclusionHeading, string conclusionText)
	{
		this.consequenceType1 = consequenceType1;
		this.consequenceType2 = consequenceType2;
		this.consequenceType3 = consequenceType3;
		this.magnitude2 = magnitude2;
		this.magnitude3 = magnitude3;
		this.useAutoExplanation = useAutoExplanation;
		this.AfterImage = AfterImage;
		this.icon = icon;
		this.selectionTooltip = selectionTooltip;
		this.conclusionHeading = conclusionHeading;
		this.conclusionText = conclusionText;
	}
	public EventConsequence GetEventConsequence(EventConsequence ec)
	{
		ec.consequenceType1 = consequenceType1;
		ec.consequenceType2 = consequenceType2;
		ec.consequenceType3 = consequenceType3;
		ec.magnitude2 = magnitude2;
		ec.magnitude3 = magnitude3;
		ec.useAutoExplanation = useAutoExplanation;
		ec.AfterImage = AfterImage;
		ec.icon = icon;
		ec.selectionTooltip = selectionTooltip;
		ec.conclusionHeading = conclusionHeading;
		ec.conclusionText = conclusionText;
		return ec;
	}

}
[System.Serializable]
public class SerializedDialogues
{
	public List<SerializedDialogue> dialogues;
	public SerializedDialogues()
	{
		dialogues = new List<SerializedDialogue>();
	}
}

[System.Serializable]
public class SerializedDialogue
{
	public string mainDialogue;
	public List<string> dialogueParts;

	public SerializedDialogue()
	{
		dialogueParts = new List<string>();
	}
}
