﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class EventExporter : EditorWindow
{

	string exportPath = "CSV/eventData";
	string dialogueExportPath = "CSV/dialogueData";
	string importPath = "";
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
			EventDataExport.SaveEvents("/" + exportPath + ".json");
		}

		dialogueExportPath = EditorGUILayout.TextField("Dialogue export path", dialogueExportPath);
		if (GUILayout.Button("Export dialogue data"))
		{
			EventDataExport.CreateDialogueExportFile();
			EventDataExport.SaveDialogues("/" + dialogueExportPath + ".json");
		}
	}
}