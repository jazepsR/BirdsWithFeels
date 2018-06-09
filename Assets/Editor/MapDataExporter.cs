using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapDataExporter : EditorWindow
{
	string exportPath = "CSV/mapData";
	[MenuItem("Window/Map data exporter")]
	public static void ShowWindow()
	{
		GetWindow<MapDataExporter>();
	}


	private void OnGUI()
	{

		GUILayout.Label("Tile editor", EditorStyles.boldLabel);

		exportPath = EditorGUILayout.TextField("Export path", exportPath);
		if (GUILayout.Button("Export map data"))
		{
			ExcelExport.CreateExportTable();
			foreach (MapIcon icon in FindObjectsOfType<MapIcon>())
				ExcelExport.AddMapNode(icon);
			ExcelExport.Save("/"+exportPath+".csv");
		}
	}
}
