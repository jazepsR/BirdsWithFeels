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
		string jsonString = "{events:[";
		foreach(SerializedEvent obj in serializedEvents.events)
		{
			jsonString += "{\"mainEvent\":" + obj.mainEvent + ",\"eventParts\":[";
			foreach(string part in obj.eventParts)
			{
				jsonString += part + ",";
			}
			jsonString = jsonString.Substring(0, jsonString.Length - 1);

			jsonString += " ],\"eventConsequence\":[";

			foreach (string part in obj.eventConsequence)
			{
				jsonString += part + ",";
			}
			jsonString = jsonString.Substring(0, jsonString.Length - 1);
			jsonString += "],\"afterEventDialog\":"+obj.afterEventDialog +"},";
		}
		jsonString = jsonString.Substring(0,jsonString.Length - 1);
		jsonString += "]}";
		File.WriteAllText(filePath, jsonString);
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
		foreach (EventScript eventObj in eventsToExport)
		{
			SerializedEvent ev = new SerializedEvent();
			ev.mainEvent = JsonUtility.ToJson(eventObj);
			List<EventPart> parts = new List<EventPart>(eventObj.GetComponentsInChildren<EventPart>(true));
			foreach (EventPart part in parts)
			{
				ev.eventParts.Add(JsonUtility.ToJson(part));
			}
			foreach (EventConsequence consequence in eventObj.options)
			{
				ev.eventConsequence.Add(JsonUtility.ToJson(consequence));
			}
			if (ev.afterEventDialog == null)
				ev.afterEventDialog = "none";
			else
				ev.afterEventDialog = eventObj.afterEventDialog.name;
			serializedEvents.events.Add(ev);
		}
		Debug.Log("Serialized " + serializedEvents.events.Count + " events!");
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
	public string mainEvent;
	public List<string> eventParts;
	public List<string> eventConsequence;
	public string afterEventDialog;

	public SerializedEvent()
	{
		eventParts = new List<string>();
		eventConsequence = new List<string>();
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
