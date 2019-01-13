using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class EventExporter : EditorWindow
{

	string exportPath = "CSV/eventData";
	string dialogueExportPath = "CSV/dialogueData";
	string importPath = "";
	string dialogueImportPath = "";
	string eventImportPath = "";
	GameObject importedEventParent;
	GameObject importedDialogueParent;
	[MenuItem("Window/Event exporter")]
	public static void ShowWindow()
	{
		GetWindow<EventExporter>();
	}

	private void OnGUI()
	{

		GUILayout.Label("Event exporter", EditorStyles.boldLabel);
		exportPath = EditorGUILayout.TextField("Event export path", exportPath);
		if (GUILayout.Button("Export events data"))
		{
			EventDataExport.CreateEventExportFile();
			EventDataExport.SaveEvents("/" + exportPath);
		}

		dialogueExportPath = EditorGUILayout.TextField("Dialogue export path", dialogueExportPath);
		if (GUILayout.Button("Export dialogue data"))
		{
			EventDataExport.CreateDialogueExportFile();
			EventDataExport.SaveDialogues("/" + dialogueExportPath);
		}
		GUILayout.Label("Event importer", EditorStyles.boldLabel);

		eventImportPath= EditorGUILayout.TextField("Event import path", eventImportPath);
		if (GUILayout.Button("Import events data"))
		{
			if(eventImportPath != "")
				importedEventParent = new GameObject("Imported Events");
			EventDataExport.LoadEvents(importedEventParent.transform, eventImportPath);
		}
		dialogueImportPath = EditorGUILayout.TextField("Dialogue import path", dialogueImportPath);
		if (GUILayout.Button("Import dialog data"))
		{
			if (dialogueImportPath != "")
				importedDialogueParent = new GameObject("Imported Dialogues");
			EventDataExport.LoadDialogues(importedDialogueParent.transform, dialogueImportPath);
		}
	}
}
