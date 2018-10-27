using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class MapDataExporter : EditorWindow
{
	string exportPath = "CSV/mapData";
	string importPath = "";
	[MenuItem("Window/Map data exporter")]
	public static void ShowWindow()
	{
		GetWindow<MapDataExporter>();
	}


	private void OnGUI()
	{

		GUILayout.Label("Map node editor", EditorStyles.boldLabel);

		exportPath = EditorGUILayout.TextField("Export path", exportPath);
		if (GUILayout.Button("Export map data"))
		{
			ExcelExport.CreateExportTable();
			List<MapIcon> icons = FindObjectsOfType<MapIcon>().ToList<MapIcon>();
			var result = icons.OrderBy(a => a.ID);
			foreach (MapIcon icon in result)
				ExcelExport.AddMapNode(icon);
			ExcelExport.Save("/" + exportPath + ".csv");
		}
		if (GUILayout.Button("Select data file"))
		{
			importPath = EditorUtility.OpenFilePanel("Select data file", Application.dataPath, "csv");
		}
		if(importPath == "")
			GUILayout.Label("no data file selected!", EditorStyles.boldLabel);
		else
			GUILayout.Label("file \""+importPath+"\" selected", EditorStyles.boldLabel);
		if (GUILayout.Button("Import map data"))
		{
			if(importPath!= "")
				ExcelExport.ImportExcelData();
		}
	}
}
